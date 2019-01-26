using Elasticsearch.Net;
using System.Threading.Tasks;

namespace MidnightLizard.KafkaConnect.ElasticSearch.Services
{
    public interface IMessageHandler
    {
        Task HandleMessage(string messageKey, string messageJsonString);
    }

    public class MessageHandler : IMessageHandler
    {
        private readonly IElasticSearchAccessor elasticSearchAccessor;

        public MessageHandler(IElasticSearchAccessor elasticSearchAccessor)
        {
            this.elasticSearchAccessor = elasticSearchAccessor;
        }

        public async Task HandleMessage(string messageKey, string messageJsonString)
        {
            if (messageJsonString is null)
            {
                await this.elasticSearchAccessor.DeleteDocument(messageKey);
            }
            else
            {
                await this.elasticSearchAccessor.UpsertDocument(messageKey, messageJsonString);
            }
        }
    }
}
