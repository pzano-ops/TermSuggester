using TermSuggester.Core.Services;

namespace TermSuggester.Tests.UnitTests.Services;


[TestFixture]
public class TermSuggesterWithMatcherTests : TermSuggesterContractTests
{
    protected override ITermSuggester CreateSuggester()
        => new TermSuggesterWithMatcher();

    // Ajouter ici des tests spécifiques à l'implémentation Matcher
    // Par exemple, comportements et edge cases spécifiques, verification de performances etc...
    // Les tests de "base" sont effectués dans la classe parente qui teste le contrat de l'interface, voir TermSuggesterContractTests.cs
}