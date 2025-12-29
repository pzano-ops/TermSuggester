# TermSuggester

Console .NET pour suggérer des termes proches d’un input.

## Structure

- `Suggester/` : interface `ITermSuggester` et deux implémentations
- `Program.cs` : tests rapides, plusieurs cas disponibles
- `SimilarityCalculator.cs` : calcul du score similarité entre deux mots

## Exemple d'usage

```csharp
var suggester = new TermSuggesterWithMatcher();
var results = suggester.Suggest("gros", new[] { "gros", "gras", "graisse" }, 4);
Console.WriteLine(string.Join(Environment.NewLine, results)); // resultat attendu : gros gro gras
