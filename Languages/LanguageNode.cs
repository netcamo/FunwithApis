namespace FunWithAPIs.Languages
{
    public sealed record LanguageNode(bool IsSelected, Dictionary<string, string> LanguageIsoCodesWithLocales);
}
