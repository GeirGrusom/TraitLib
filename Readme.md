# Traitor trait library

This project aims to provide a simple algorithm neatly packaged and usable for genetic algorithms though primarily aimed at game development.

The algorithm it implements is one that picks random traits from one or more parents into a new set of genes with a chance of random mutation and novel traits.

## Usage

This project is based primarily on two classes: `Genes<TKey, TValue>` and `GeneCombiner<TKey, TValue>`. The generic arguments indicate the type on the trait key and trait value. A trait key, `TKey`, is usually either a `string` or an `int`. The trait value, `TValue`, is usually either `int` or `double`.

A new set of genes is created using `Gene<TKey, TValue>`'s constructor which takes an `IEnumerable<Trait<string, int>>`.

```csharp
var genes = new Genes<string, int>(new[] { new Trait<string, int>("funk", 10) });
```

If you, for whatever reason, need an empty gene set use `Gene<TKey, TValue>` use the static property `Gene<TKey, TValue>.Empty`. This value is a singleton, but `Gene<TKey, TValue>` is an immutable type so it shouldn't matter.

The `GeneCombiner<TKey, TValue>` is used to combine genes or at the very least create a somewhat random offspring of one or more sets of genes. It is initialized through its constructor and you likely just need one instance. By default it uses a thread static `Random` instance and should be thread safe.

The most demanind constructor requires a random function, a novel trait function, a trait mutation function, trait mutation chance and finally a novel trait chance.

The random function simply a function that returns a random number. When this is omitted it will use `System.Random`.

After the random functio you can provide a trait factory. This is the factory to produce a novel trait. It takes in an `IEnumerable` of the current traits and returns a novel trait. The input argument can be used to make sure that a novel trait is always returned if possible.

Following is a mutation function, or "mutationValue" function. This takes the current trait and returns the new trait value. This parameter must always be provided. Some examples are provided in the `Mutators` class. The Hello World example uses `Mutators.Gaussian`.

Then you have trait mutation chance which is a rational. The rational type has an implicit cast from tuple so for a 1/100 chance simply write `(1, 100)`.

Following that is the novel trait chance. This follows trait mutation chance.

When an instance is available you can use the `Combine` function to produce "offspring". It takes at least one set of genes (for asexual reproduction I guess) but you can provide as many sets as you like. It will always produce a single instance.