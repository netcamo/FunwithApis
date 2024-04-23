using HumanLanguages;

namespace FunWithAPIs.Translations
{
    public static class TranslationsEndpoints
    {

        private const string SpecificTranslationsRoute = "/translations/{currentLanguageIsoCodeString}/";
        public static RouteGroupBuilder GroupTranslationsApisV1(this RouteGroupBuilder group)
        {

            group.MapGet(SpecificTranslationsRoute, async (string currentLanguageIsoCodeString = "en-US") =>
            {
                var currentLanguageIsoCode = HumanHelper.CreateLanguageIsoCode(currentLanguageIsoCodeString);
                var transalations = await SpecificTranslations(currentLanguageIsoCode.LanguageId);
                return Results.Ok(transalations.ToDictionary(t => t.Key, t =>
                {
                    if (t.Value is null)
                    {
                        return t.Key;
                    }
                    if (t.Value.TryGetValue(currentLanguageIsoCode.LanguageLocaleVariationCode, out string? localeValue) && !string.IsNullOrWhiteSpace(localeValue))
                    {
                        return localeValue;
                    }
                    if (t.Value.TryGetValue(LanguageLocaleVariationCode.Default, out string? defaultValue) && !string.IsNullOrWhiteSpace(defaultValue))
                    {
                        return defaultValue;
                    }
                    return t.Key;
                }));
            })
            .Produces<Dictionary<string, string>>(StatusCodes.Status200OK);
            return group;
        }

        private static async Task<Dictionary<string, Dictionary<LanguageLocaleVariationCode, string>?>> SpecificTranslations(LanguageId currentLangCode)
        {
            return await Task.FromResult(Translations.TranslationsDictionary.ToDictionary(d => d.Key.ToString(), d => d.Value.TryGetValue(currentLangCode, out var value) ? value : null));
        }
    }
}
