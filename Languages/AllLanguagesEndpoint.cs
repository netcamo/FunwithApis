using HumanLanguages;

namespace FunWithAPIs.Languages
{
    public static class AllLanguagesEndpoint
    {
        private const string AllLanguagesRoute = "/allLanguages/{currentCultureIsoCode}/";
        public static RouteGroupBuilder GroupLanguagesApisV1(this RouteGroupBuilder group)
        {
            group.MapGet(AllLanguagesRoute, async (string currentCultureIsoCode = "en-US") =>
            {
                var currentLanguageIsoCode = currentCultureIsoCode.Split('-').First();

                LanguageIsoCode currentLangCode = (Enum.TryParse(currentLanguageIsoCode, true, out LanguageIsoCode langCode)) ? langCode : LanguageIsoCode.en;

                return Results.Ok(await GetAllLanguages(currentLangCode));
            })
            .Produces<Dictionary<LanguageIsoCode, LanguageNode>>(StatusCodes.Status200OK);

            return group;
        }

        private static async Task<Dictionary<LanguageIsoCode, LanguageNode>> GetAllLanguages(LanguageIsoCode currentLangCode)
        {
            return await Task.FromResult(HumanLanguages.Languages.LanguagePropertiesDictionary.ToDictionary(l => l.Key, l =>
            {
                var languageProperties = HumanLanguages.Languages.LanguagePropertiesDictionary[l.Key];
                var localName = languageProperties.LanguageNames[l.Key];
                var nameInCurrentLanguage = languageProperties.LanguageNames[currentLangCode];
                return new LanguageNode(
                    IsSelected: currentLangCode == l.Key,
                    DisplayName: (localName == nameInCurrentLanguage || string.IsNullOrWhiteSpace(nameInCurrentLanguage) ? localName : $"{localName} ({nameInCurrentLanguage})"));
            }));
        }
    }
}
