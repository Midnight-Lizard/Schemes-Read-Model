using Elasticsearch.Net;
using MidnightLizard.KafkaConnect.ElasticSearch.Configuration;
using System;
using System.Threading.Tasks;

namespace MidnightLizard.KafkaConnect.ElasticSearch.Services
{
    public interface IElasticSearchAccessor
    {
        Task DeleteDocument(string docId);
        Task UpsertDocument(string docId, string doc);
    }

    public class ElasticSearchAccessor : IElasticSearchAccessor
    {
        private readonly ElasticSearchConfig elasticSearchConfig;
        private readonly IElasticLowLevelClient elasticClient;

        public ElasticSearchAccessor(ElasticSearchConfig elasticSearchConfig)
        {
            this.elasticSearchConfig = elasticSearchConfig;
            this.elasticClient = this.CreateElasticClient();
        }

        protected virtual IElasticLowLevelClient CreateElasticClient()
        {
            var settings = new ConnectionConfiguration(
                new Uri(this.elasticSearchConfig.ELASTIC_SEARCH_CLIENT_URL))
                .RequestTimeout(TimeSpan.FromSeconds(this.elasticSearchConfig.ELASTIC_SEARCH_REQUEST_TIMEOUT_SEC));

            return new ElasticLowLevelClient(settings);
        }

        private void HandleErrors(StringResponse result)
        {
            if (!result.Success)
            {
                if (result.OriginalException is null)
                {
                    result.TryGetServerError(out var serverError);
                    if (serverError != null)
                    {
                        throw new ApplicationException(serverError.Error.Reason);
                    }
                    throw new ApplicationException("Unknown ES server error");
                }
                throw result.OriginalException;
            }
        }

        public async Task DeleteDocument(string docId)
        {
            var result = await this.elasticClient.DeleteAsync<StringResponse>(
                 this.elasticSearchConfig.ELASTIC_SEARCH_INDEX_NAME,
                 this.elasticSearchConfig.ELASTIC_SEARCH_INDEX_TYPE_NAME,
                 docId);
            this.HandleErrors(result);
        }

        public async Task UpsertDocument(string docId, string doc)
        {
            var postData = PostData.String($"{{ doc: {doc}, \"doc_as_upsert\" : true }}");
            var result = await elasticClient.UpdateAsync<StringResponse>(
                 this.elasticSearchConfig.ELASTIC_SEARCH_INDEX_NAME,
                 this.elasticSearchConfig.ELASTIC_SEARCH_INDEX_TYPE_NAME,
                 docId, postData);
        }
    }
}
