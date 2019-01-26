using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Logging;
using MidnightLizard.KafkaConnect.ElasticSearch.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidnightLizard.KafkaConnect.ElasticSearch.Services
{
    public enum QueueStatus
    {
        Stopped = 0,
        Running = 1,
        Paused = 2
    }

    public interface IMessagingQueue
    {
        Task BeginProcessing(CancellationToken token);
        bool CheckStatus();
        Task PauseProcessing();
        Task ResumeProcessing(CancellationToken token);
    }

    public class MessagingQueue : IMessagingQueue
    {
        protected QueueStatus queueStatus = QueueStatus.Stopped;
        protected Message<string, string> lastConsumedEvent;
        protected Message<string, string> lastConsumedRequest;
        protected readonly TimeSpan timeout = TimeSpan.FromSeconds(1);
        protected List<TopicPartition> assignedEventsPartitions;
        protected readonly ILogger<MessagingQueue> logger;
        protected readonly KafkaConfig kafkaConfig;
        private readonly IMessageHandler eventHandler;
        protected Consumer<string, string> eventsConsumer;
        protected CancellationToken cancellationToken;
        protected TaskCompletionSource<bool> queuePausingCompleted;
        private int errorCount = 0;
        private readonly int maxErrorCount = 10;

        public MessagingQueue(
            ILogger<MessagingQueue> logger,
            KafkaConfig config,
            IMessageHandler eventHandler)
        {
            this.logger = logger;
            this.kafkaConfig = config;
            this.eventHandler = eventHandler;
        }

        public bool CheckStatus()
        {
            if (this.queueStatus == QueueStatus.Running || this.errorCount < this.maxErrorCount)
            {
                switch (this.queueStatus)
                {
                    case QueueStatus.Paused:
                        this.ResumeProcessing(this.cancellationToken);
                        break;
                    case QueueStatus.Stopped:
                        this.BeginProcessing(this.cancellationToken);
                        break;
                }
                return true;
            }
            return false;
        }

        public async Task PauseProcessing()
        {
            this.queuePausingCompleted = new TaskCompletionSource<bool>();
            this.queueStatus = QueueStatus.Paused;
            await this.queuePausingCompleted.Task;
        }

        public async Task ResumeProcessing(CancellationToken token)
        {
            this.queueStatus = QueueStatus.Stopped;
            await this.BeginProcessing(token);
        }

        public async Task BeginProcessing(CancellationToken token)
        {
            if (this.cancellationToken != token)
            {
                this.cancellationToken = token;
                token.Register(async () =>
                {
                    while (this.queueStatus == QueueStatus.Running)
                    {
                        await Task.Delay(this.timeout);
                    }
                });
            }

            if (this.queueStatus == QueueStatus.Stopped)
            {
                try
                {
                    this.queueStatus = QueueStatus.Running;
                    using (var eventsConsumer = new Consumer<string, string>(
                        this.kafkaConfig.KAFKA_CONSUMER_CONFIG,
                        new StringDeserializer(Encoding.UTF8),
                        new StringDeserializer(Encoding.UTF8)))
                    {
                        this.eventsConsumer = eventsConsumer;

                        this.eventsConsumer.Subscribe(this.kafkaConfig.KAFKA_TOPICS);

                        while (this.queueStatus == QueueStatus.Running && !this.cancellationToken.IsCancellationRequested)
                        {
                            if (this.eventsConsumer.Consume(out var @event, this.timeout))
                            {
                                await this.HandleMessage(@event);
                                await this.eventsConsumer.CommitAsync(@event);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Failed to consume new messages");
                    this.errorCount++;
                }
                finally
                {
                    switch (this.queueStatus)
                    {
                        case QueueStatus.Paused:
                            this.queuePausingCompleted?.SetResult(true);
                            break;
                        case QueueStatus.Running:
                        case QueueStatus.Stopped:
                        default:
                            this.queueStatus = QueueStatus.Stopped;
                            break;
                    }
                }
            }
        }

        protected async Task HandleMessage(Message<string, string> kafkaMessage)
        {
            if (kafkaMessage.Error.HasError)
            {
                this.logger.LogError($"Failed to consume [{kafkaMessage.Value ?? "message"}] with reason: {kafkaMessage.Error.Reason}");
            }
            else
            {
                this.logger.LogWarning($"Starting handling event [{kafkaMessage.Key}]");
                await this.eventHandler.HandleMessage(kafkaMessage.Key, kafkaMessage.Value);
                this.logger.LogWarning($"Event [{kafkaMessage.Key}] handled");
            }
        }
    }
}