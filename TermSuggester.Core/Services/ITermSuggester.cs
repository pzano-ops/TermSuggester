namespace TermSuggester.Core.Services;

public interface ITermSuggester
{
    IReadOnlyList<string> Suggest(string inputTerm, IEnumerable<string> terms, int maxSuggestions);
}