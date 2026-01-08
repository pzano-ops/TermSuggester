namespace TermSuggester.Core.Tools;

/// <summary>
/// Compute similarity score. (following guidelines)
/// </summary>
public static class SimilarityCalculator
{
    public static int GetDifferenceScore(string input, string match)
    {
        if(match.Length < input.Length) // ignore cases where input is longer than suggested term (match) : worst score possible, eventually ignored in the logic later
            return int.MaxValue;

        var bestScore = int.MaxValue; // lower is better matching (count differences)
        
        for (var matchIndex = 0; matchIndex < (match.Length - input.Length) + 1; matchIndex++) // check each section of a match term, compared to the input
        {
            var currentScore = 0;
            for (var inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                if (input[inputIndex] != match[matchIndex + inputIndex])
                {
                    currentScore++;
                }
            }
            
            if (currentScore == 0) //Perfect matching
                return 0;
            
            if (currentScore < bestScore) //Update the best score 
            {
                bestScore = currentScore;
            }
        }

        return bestScore;
    }
}