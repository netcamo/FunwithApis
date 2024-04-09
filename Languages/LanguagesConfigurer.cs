namespace FunWithAPIs.Languages
{
    public sealed class LanguagesConfigurer : IApiConfigurer
    {
        public void AddEndpoints(WebApplication app)
        {
            app.MapGroup(IApiConfigurer.RouteForVersion1)
                .GroupLanguagesApisV1()
                .WithTags(IApiConfigurer.RouteForVersion1);
        }
    }
}
