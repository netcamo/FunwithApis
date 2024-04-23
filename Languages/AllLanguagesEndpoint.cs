using HumanLanguages;

namespace FunWithAPIs.Languages
{
    public static class AllLanguagesEndpoint
    {
        private const string AllLanguagesRoute = "/allLanguages/{currentCultureIsoCodeString}/";
        public static RouteGroupBuilder GroupLanguagesApisV1(this RouteGroupBuilder group)
        {
            group.MapGet(AllLanguagesRoute, async (string currentCultureIsoCodeString = "en-US") =>
            {
                var currentCultureIsoCode = HumanHelper.CreateLanguageIsoCode(currentCultureIsoCodeString);
                return Results.Ok(await GetAllLanguages(currentCultureIsoCode.LanguageId));
            })
            .Produces<Dictionary<LanguageIsoCode, LanguageNode>>(StatusCodes.Status200OK);

            return group;
        }

        private static async Task<Dictionary<LanguageId, LanguageNode>> GetAllLanguages(LanguageId currentLanguageId)
        {
            return await Task.FromResult(HumanLanguages.Languages.LanguagePropertiesDictionary.ToDictionary(l => l.Key, l =>
            {
                var languageProperties = HumanLanguages.Languages.LanguagePropertiesDictionary[l.Key];
                var localName = languageProperties.LanguageNames[l.Key];
                var nameInCurrentLanguage = languageProperties.LanguageNames[currentLanguageId];
                var languageNameInBothLanguages = (localName == nameInCurrentLanguage || string.IsNullOrWhiteSpace(nameInCurrentLanguage) ? localName : $"{localName} ({nameInCurrentLanguage})");
                var languageIsoCodesWIthLocales = new Dictionary<string, string>() { { l.Key.ToString(), languageNameInBothLanguages } };

                return new LanguageNode(
                    IsSelected: currentLanguageId == l.Key,
                    LanguageIsoCodesWithLocales: languageIsoCodesWIthLocales.Union(languageProperties.VariationNativeNames.ToDictionary(v => $"{l.Key}-{v.Key}", v => $"{languageNameInBothLanguages} - {v.Value}" )).ToDictionary());
            }));
        }
    }
}
