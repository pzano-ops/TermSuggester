// See https://aka.ms/new-console-template for more information

using TermSuggester.Core.Services;

namespace TermSuggester;

//Structure pour des tests simples
internal record TestTermSet(string Input, IReadOnlyList<string> Terms, int MaxResults);


internal static class Program
{
    //mise en forme console pour tester les suggester
    private static void RunTest(ITermSuggester suggester, TestTermSet test)
    {
        Console.WriteLine($"[{suggester.GetType().Name}] Input: \"{test.Input}\"");
        foreach (var r in suggester.Suggest(test.Input, test.Terms, test.MaxResults))
            Console.WriteLine($"  - {r}");
        Console.WriteLine();
    }
    
    private static void Main()
    {
        var suggesters = new ITermSuggester[]
        {
            new TermSuggesterDictionary(),
            new TermSuggesterWithMatcher()
        };

        var tests = new[]
        {
            new TestTermSet("gros", new[] { "gros", "gras", "graisse", "agressif", "go", "ros", "gro" }, 4),
            new TestTermSet("chat", new[] { "chut", "char", "chats", "achat", "rat" }, 3),
            new TestTermSet("car",  new[] { "bar", "cat", "cap", "can" }, 10),
            new TestTermSet("bonjour", new[] { "bon", "bonjour", "bonsoir", "jour" }, 5)
        };

        foreach (var suggester in suggesters)
        foreach (var test in tests)
            RunTest(suggester, test);
    }
}