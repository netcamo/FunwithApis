using HumanLanguages;

namespace FunWithAPIs.Translations
{
    public static class TranslationsEndpoints
    {

        private const string SpecificTranslationsRoute = "/translations/{currentLanguageIsoCode}/";
        public static RouteGroupBuilder GroupTranslationsApisV1(this RouteGroupBuilder group)
        {

            group.MapGet(SpecificTranslationsRoute, async (string currentLanguageIsoCode = "en-US") =>
            {
                var lang = currentLanguageIsoCode.Split('-');
                LanguageIsoCode currentLangCode = Enum.Parse<LanguageIsoCode>(lang.First());
                var transalations = await SpecificTranslations(currentLangCode);
                if (lang.Length == 1)
                {
                    return Results.Ok(transalations.ToDictionary(d => d.Key, d => d.Value.FirstOrDefault(dd => dd.Key == LanguageVariationIsoCode.Default).Value));
                }
                return Results.Ok(transalations.ToDictionary(d => d.Key, d =>
                {
                    if (Enum.TryParse<LanguageVariationIsoCode>(lang.Last(), out LanguageVariationIsoCode languageVariationIsoCode))
                    {
                        var languageVariationTransalation = d.Value.FirstOrDefault(dd => dd.Key == languageVariationIsoCode).Value;
                        if (!string.IsNullOrEmpty(languageVariationTransalation))
                        {
                            return languageVariationTransalation;
                        }
                    }
                    return d.Value.FirstOrDefault(dd => dd.Key == LanguageVariationIsoCode.Default).Value;
                }));
            })
            .Produces<Dictionary<string, string>>(StatusCodes.Status200OK);
            return group;
        }

        private static async Task<Dictionary<string, Dictionary<LanguageVariationIsoCode, string>?>> SpecificTranslations(LanguageIsoCode currentLangCode)
        {
            return await Task.FromResult(HumanLanguages.Translations.TranslationsDictionary.ToDictionary(d => d.Key.ToString(),
                        d => d.Value.TryGetValue(currentLangCode, out Dictionary<LanguageVariationIsoCode, string>? value) ? value : null
                    ));
        }
    }
}
