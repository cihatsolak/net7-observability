namespace Logging.Shared;

public static class Logging
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogging => (hostBuilderContext, loggerConfiguration) =>
    {
        loggerConfiguration
        // appsettings.json
        //.Filter.ByExcluding(Matching.FromSource("Microsoft")) 
        //.Filter.ByExcluding(Matching.FromSource("System"))
        //.Filter.ByIncludingOnly(Matching.FromSource("Microsoft.Hosting.Lifetime"))
        //.MinimumLevel.Information()
        .Enrich.With(new ThreadIdEnricher())
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .Enrich.WithProperty("Environment", hostBuilderContext.HostingEnvironment.EnvironmentName)
        .Enrich.WithProperty("ApplicationName", hostBuilderContext.HostingEnvironment.ApplicationName);

        string elasticSearchBaseUrl = hostBuilderContext.Configuration.GetSection("ElasticSearch")["BaseUrl"];
        string userName = hostBuilderContext.Configuration.GetSection("ElasticSearch")["UserName"];
        string password = hostBuilderContext.Configuration.GetSection("ElasticSearch")["Password"];
        string indexName = hostBuilderContext.Configuration.GetSection("ElasticSearch")["IndexName"];

        loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchBaseUrl))
        {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8, //elastic search version
            IndexFormat = $"{indexName}-{hostBuilderContext.HostingEnvironment.EnvironmentName}-logs-" + "{0:yyy.MM.dd}",
            ModifyConnectionSettings = connection => connection.BasicAuthentication(userName, password),
            CustomFormatter = new ElasticsearchJsonFormatter()
        });
    };
}