# TermSuggester

Mini-kata en C# implémentant un moteur de suggestion de termes basé sur un score de similarité simple (nombre de lettres à remplacer, sans insertion).

---

## Règles de suggestion

À partir d’un terme d’entrée et d’une liste de termes candidats (match) :

- La similarité est calculée comme le **nombre minimal de lettres à remplacer** pour faire correspondre le terme d’entrée à une sous-chaîne du terme candidat.
- Moins il y a de différences, meilleure est la suggestion.
- Les termes trop courts ou non pertinents sont ignorés.

Les suggestions sont triées selon :
1. le score de différence (croissant)
2. la différence de longueur avec le terme recherché
3. l’ordre alphabétique

---

## Architecture

```
src/
  Core/
    Services/
      ITermSuggester.cs
      TermSuggesterWithDictionary.cs
      TermSuggesterWithMatcher.cs
    Tools/
      SimilarityCalculator.cs
  Console/
    Program.cs

tests/
  UnitTests/
    Services/
      TermSuggesterContractTests.cs
      TermSuggesterDictionaryTests.cs
      TermSuggesterMatcherTests.cs
    Tools/
      SimilarityCalculatorTests.cs
```




---

## Implémentations

Deux implémentations de `ITermSuggester` sont fournies :
- une approche basée sur un dictionnaire
- une approche utilisant un matcher explicite

Elles respectent le même contrat fonctionnel.

---

## Tests

Les tests sont orientés **contrat** :
- les règles métier sont testées une seule fois dans `TermSuggesterContractTests`
- chaque implémentation est validée contre ce contrat
- des scénarios simples, des contraintes d’entrée et des cas combinés sont couverts

---

## Objectif

J'ai privilégié la séparation des responsabilités et des tests lisibles, plutôt que l’optimisation des performances.


```csharp
var suggester = new TermSuggesterWithMatcher();
var results = suggester.Suggest("gros", new[] { "gros", "gras", "graisse" }, 4);
Console.WriteLine(string.Join(Environment.NewLine, results)); // resultat attendu : gros gro gras
