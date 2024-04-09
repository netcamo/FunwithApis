namespace FunWithAPIs.Translations
{
    public sealed class TranslationsConfigurer : IApiConfigurer
    {
        public void AddEndpoints(WebApplication app)
        {
            app.MapGroup(IApiConfigurer.RouteForVersion1)
                .GroupTranslationsApisV1()
                .WithTags(IApiConfigurer.RouteForVersion1);
        }
    }
}
