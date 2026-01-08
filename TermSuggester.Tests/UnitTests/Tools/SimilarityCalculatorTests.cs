using TermSuggester.Core.Tools;

namespace TermSuggester.Tests.UnitTests.Tools;

[TestFixture]
public class SimilarityCalculatorTests
{
    #region Core Behaviour
    
    [Test]
    public void GetDifferenceScore_PerfectMatch_ReturnsZero()
    {
        var input = "gros";
        var match = "gros";
        var score = SimilarityCalculator.GetDifferenceScore(input, match);

        Assert.That(score, Is.EqualTo(0));
    }

    [Test]
    [TestCase("gros", "gras", 1)]
    [TestCase("gros", "agressif", 1)]
    [TestCase("gros", "graisse", 2)]
    public void GetDifferenceScore_PartialMatch_ReturnsCorrectScore(string input, string match, int expectedScore)
    {
        var score = SimilarityCalculator.GetDifferenceScore(input, match);
        Assert.That(score, Is.EqualTo(expectedScore));
    }
    
    #endregion
    
    #region EdgeCases 
    
    [Test]
    public void GetDifferenceScore_ShorterMatch_ReturnsMaxValue()
    {
        // input plus long que match 
        var score = SimilarityCalculator.GetDifferenceScore("gros", "gro");
        Assert.That(score, Is.EqualTo(int.MaxValue));
    }
    
    [Test]
    public void GetDifferenceScore_EmptyMatch_ReturnsMaxValue()
    {
        // input plus long que match vide
        var score = SimilarityCalculator.GetDifferenceScore("gros", "");
        Assert.That(score, Is.EqualTo(int.MaxValue));
    }

    [Test]
    public void GetDifferenceScore_BothEmpty_ReturnsZero()
    {
        var score = SimilarityCalculator.GetDifferenceScore("", "");
        Assert.That(score, Is.EqualTo(0));
    }
    
    #endregion
    
    #region Complex Scenario
   
    [TestCase("abc", "xabcx", 0)] // parfait au milieu
    [TestCase("abc", "xabc", 0)]  // parfait à la fin
    [TestCase("abc", "abcx", 0)]  // parfait au début
    [TestCase("test", "atestb", 0)] // entouré
    [TestCase("ab", "xaybz", 1)]  // meilleur = 'ab' vs 'xa' ou 'ay' = 1 diff
    public void GetDifferenceScore_SlidingWindow_FindsBestPosition(
        string input, 
        string match, 
        int expectedScore)
    {
        var score = SimilarityCalculator.GetDifferenceScore(input, match);
        Assert.That(score, Is.EqualTo(expectedScore));
    }
    
    #endregion
}