namespace MidnightLizard.KafkaConnect.ElasticSearch.Configuration
{
    public class ElasticSearchConfig
    {
        public string ELASTIC_SEARCH_CLIENT_URL { get; set; }

        public string ELASTIC_SEARCH_INDEX_NAME { get; set; }
        public string ELASTIC_SEARCH_INDEX_TYPE_NAME { get; set; }

        public int ELASTIC_SEARCH_REQUEST_TIMEOUT_SEC { get; set; } = 60;
    }
}
