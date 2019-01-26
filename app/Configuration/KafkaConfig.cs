using System.Collections.Generic;

namespace MidnightLizard.KafkaConnect.ElasticSearch.Configuration
{
    public class KafkaConfig
    {
        public Dictionary<string, object> KAFKA_CONSUMER_CONFIG { get; set; }
        public string[] KAFKA_TOPICS { get; set; }
    }
}