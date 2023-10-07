namespace Logging.Shared.Enrichers
{
    public class RequestContentLengthEnricher : ILogEventEnricher
    {
        private readonly HttpContext _httpContext;

        public RequestContentLengthEnricher(IServiceProvider serviceProvider)
        {
            _httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "RequestContentLength", _httpContext.Request.ContentLength));
        }
    }
}
