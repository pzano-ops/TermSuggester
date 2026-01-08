using TermSuggester.Core.Tools;

namespace TermSuggester.Core.Services;

/// <summary>
/// 1st version (intuitive one)
/// Suggester : orchestrator for the exercice logic. No computation 
/// </summary>
public class TermSuggesterDictionary : ITermSuggester
{
    public IReadOnlyList<string> Suggest(string inputTerm, IEnumerable<string> terms, int maxSuggestions)
    {
        var suggestionRanking = new Dictionary<int, List<string>>();

        foreach (var matchTerm in terms)
        {
            var ranking = SimilarityCalculator.GetDifferenceScore(inputTerm, matchTerm);
            
            //Ignore edge cases (0% matching) 
            if (ranking == int.MaxValue || ranking >= inputTerm.Length)
                continue;
            
            if(!suggestionRanking.ContainsKey(ranking)) 
                suggestionRanking.Add(ranking, []);
            
            suggestionRanking[ranking].Add(matchTerm);
        }
        
        return suggestionRanking
            .OrderBy(kvp => kvp.Key)                 
            .SelectMany(kvp => kvp.Value
                .OrderBy(s => s.Length) //Correspond dans notre contexte à la chaine la plus proche en longueur (car on ignore les termes plus petit que l'input comparé). on pourrait calculer le delta pour etre plus generaliste. 
                .ThenBy(s => s)) 
            .Take(maxSuggestions)
            .ToList();
    }
}