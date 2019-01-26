using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MidnightLizard.KafkaConnect.ElasticSearch.Configuration;
using MidnightLizard.KafkaConnect.ElasticSearch.Services;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MidnightLizard.KafkaConnect.ElasticSearch
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ElasticSearchConfig>(x =>
            {
                var esConfig = new ElasticSearchConfig();
                this.Configuration.Bind(esConfig);
                return esConfig;
            });

            services.AddSingleton<KafkaConfig>(x => new KafkaConfig
            {
                KAFKA_CONSUMER_CONFIG = JsonConvert
                    .DeserializeObject<Dictionary<string, object>>(
                        this.Configuration.GetValue<string>(
                            nameof(KafkaConfig.KAFKA_CONSUMER_CONFIG))),

                KAFKA_TOPICS = JsonConvert
                    .DeserializeObject<string[]>(
                        this.Configuration.GetValue<string>(
                            nameof(KafkaConfig.KAFKA_TOPICS))),
            });

            services.AddSingleton<IElasticSearchAccessor, ElasticSearchAccessor>();
            services.AddSingleton<IMessageHandler, MessageHandler>();
            services.AddSingleton<IMessagingQueue, MessagingQueue>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }
    }
}
