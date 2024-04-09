namespace FunWithAPIs
{
    public interface IApiConfigurer
    {
        const string RouteForVersion1 = "v1";
        const string RouteForVersion2 = "v2";
        void AddEndpoints(WebApplication app);
    }
}
