using TermSuggester.Core.Tools;

namespace TermSuggester.Core.Services;

/// <summary>
/// Version avec une struct intermédiaire pour stocker les données de comparaison
/// </summary>
public class TermSuggesterWithMatcher : ITermSuggester
{
    public IReadOnlyList<string> Suggest(string inputTerm, IEnumerable<string> terms, int maxSuggestions)
    {
        var suggestionRanking = new List<TermMatch>();

        foreach (var matchTerm in terms)
        {
            var ranking = SimilarityCalculator.GetDifferenceScore(inputTerm, matchTerm);
            
            if (ranking == int.MaxValue || ranking >= inputTerm.Length) 
                continue;
            
            suggestionRanking.Add(
                new TermMatch(matchTerm, ranking, Math.Abs(matchTerm.Length - inputTerm.Length))
            );
        }

        return suggestionRanking
            .OrderBy(match => match.DifferenceScore)
            .ThenBy(match => match.LengthDelta)
            .ThenBy(match => match.Term)
            .Take(maxSuggestions)
            .Select(match => match.Term)
            .ToList();
    }
}

internal readonly struct TermMatch(string term, int differenceScore, int lengthDelta)
{
    public readonly string Term = term;
    public  readonly int DifferenceScore = differenceScore; 
    public readonly int LengthDelta = lengthDelta;
}