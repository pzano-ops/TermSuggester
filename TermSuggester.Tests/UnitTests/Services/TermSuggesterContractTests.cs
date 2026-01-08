using TermSuggester.Core.Services;

namespace TermSuggester.Tests.UnitTests.Services;

// Tests du contrat (comportements obligatoires de ITermSuggester)
[TestFixture]
public abstract class TermSuggesterContractTests
{
    protected abstract ITermSuggester CreateSuggester();

    #region Guidelines

    [Test]
    public void Suggest_ExampleFromRequirements_ReturnsCorrectResults()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "gros", "gras", "graisse", "agressif", "go", "ros", "gro" };
        
        var results = suggester.Suggest("gros", terms, 2);
        
        Assert.That(results, Has.Count.EqualTo(2));
        Assert.That(results[0], Is.EqualTo("gros"));
        Assert.That(results[1], Is.EqualTo("gras")); // plus court qu'agressif
    }

    #endregion

    #region Core Behaviour 

    [Test]
    public void Suggest_ExactMatch_ReturnsExactMatchFirst()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "chien", "chat", "chaton" };
        
        var results = suggester.Suggest("chat", terms, 3);
        
        Assert.That(results[0], Is.EqualTo("chat"));
    }

    [Test]
    public void Suggest_MaxSuggestionsRespected_ReturnsCorrectCount()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "test", "tests", "testing", "tester", "tested" };
        
        var results = suggester.Suggest("test", terms, 3);
        
        Assert.That(results, Has.Count.EqualTo(3));
    }

    [Test]
    public void Suggest_NoMatchingTerms_ReturnsEmptyList()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "a", "b", "c" }; // tous trop courts pour "test"
        
        var results = suggester.Suggest("test", terms, 5);
        
        Assert.That(results, Is.Empty);
    }

    #endregion

    #region Sorting rules (score -> lenght -> alphabetical)

    [Test]
    public void Suggest_SameScore_ShorterTermFirst()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "agressif", "gras" }; // même score (1) pour "gros"
        
        var results = suggester.Suggest("gros", terms, 2);
        
        Assert.That(results, Is.EqualTo(new[]
        {
            "gras",
            "agressif",
        }));
    }

    [Test]
    public void Suggest_SameScoreAndLength_AlphabeticalOrder()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "bbcd", "cbcd", "dbcd" }; // même longueur
        
        var results = suggester.Suggest("abcd", terms, 3);
        
        // Tous ont même score et longueur -> ordre alpha
        Assert.That(results, Is.EqualTo(new[]
        {
            "bbcd",
            "cbcd",
            "dbcd"
        }));
    }

    [Test]
    public void Suggest_DifferentScores_BestScoreFirst()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "graisse", "gras", "gros" }; 
        // gros: 0, gras: 1, graisse: 2
        
        var results = suggester.Suggest("gros", terms, 3);
        
        Assert.That(results, Is.EqualTo(new[]
        {
            "gros",
            "gras",
            "graisse"
        }));
    }

    #endregion

    #region Edge cases

    [Test]
    public void Suggest_EmptyTermsList_ReturnsEmpty()
    {
        var suggester = CreateSuggester();
        var terms = Array.Empty<string>();
        
        var results = suggester.Suggest("test", terms, 5);
        
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void Suggest_MaxSuggestionsZero_ReturnsEmpty()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "test", "testing" };
        
        var results = suggester.Suggest("test", terms, 0);
        
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void Suggest_MoreResultsThanMax_OnlyReturnsMax()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "test", "tests", "testing", "tester", "tested" };
        
        var results = suggester.Suggest("test", terms, 2);
        
        Assert.That(results, Has.Count.EqualTo(2));
    }

    [Test]
    public void Suggest_TermsShorterThanInput_Ignored()
    {
        var suggester = CreateSuggester();
        var terms = new[] { "go", "ros", "gro", "gros" }; // 3 trop courts
        
        var results = suggester.Suggest("gros", terms, 5);
        
        Assert.That(results, Has.Count.EqualTo(1));
        Assert.That(results[0], Is.EqualTo("gros"));
    }

    #endregion

    #region Complex Scenario

    [Test]
    public void Suggest_ComplexScenario_AppliesAllRules()
    {
        var suggester = CreateSuggester();
        
        //Verification de l'ordre : score, puis longueur et alphabetique. Edge cases à ignorer.
        var terms = new[] 
        { 
            "dotnet",      // 0 diff
            "dotnetA",     // 0 diff, plus long +1, alpha 1
            "dotnetB",     // 0 diff, plus long +1, alpha 2
            "dotnetABC",   // 0 diff, plus long +2
            
            "aotnet",      // 1 diff, même longueur, alpha 1
            "botnet",      // 1 diff, même longueur, alpha 2
            "aotnet2",     // 1 diff, plus long +1 
            
            "dobnep",      // 2 diffs
            
            "dot",         // trop court (ignoré)
            ""             // vide (ignoré)
        };

        terms.Shuffle(); // melange la liste pour ne pas biaser le tri
        var results = suggester.Suggest("dotnet", terms, 10);

        Assert.That(results, Is.EqualTo(new[]
        {
            "dotnet",
            "dotnetA",
            "dotnetB",
            "dotnetABC",
            "aotnet",
            "botnet",
            "aotnet2",
            "dobnep"
        }));
    }

    #endregion
}