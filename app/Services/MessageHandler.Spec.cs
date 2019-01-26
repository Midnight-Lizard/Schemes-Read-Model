using Elasticsearch.Net;
using MidnightLizard.Testing.Utilities;
using NSubstitute;

namespace MidnightLizard.KafkaConnect.ElasticSearch.Services
{
    public class MessageHandlerSpecs
    {
        private readonly IMessageHandler messageHandler;
        private readonly IElasticSearchAccessor elasticSearchAccessor;

        public MessageHandlerSpecs()
        {
            this.elasticSearchAccessor = Substitute.For<IElasticSearchAccessor>();
            this.messageHandler = new MessageHandler(this.elasticSearchAccessor);
        }

        public class HandleTransportEventSpec : MessageHandlerSpecs
        {
            [It(nameof(MessageHandler.HandleMessage))]
            public async void Should_delete_document_on_tombstone_event()
            {
                var key = "key";
                await this.messageHandler.HandleMessage(key, null);

                await this.elasticSearchAccessor.Received(1).DeleteDocument(key);
            }

            [It(nameof(MessageHandler.HandleMessage))]
            public async void Should_upsert_document_on_valid_event()
            {
                string key = "key", message = "message";
                await this.messageHandler.HandleMessage(key, message);

                await this.elasticSearchAccessor.Received(1).UpsertDocument(key, message);
            }
        }
    }
}
