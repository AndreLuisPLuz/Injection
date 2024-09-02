using Library.Core;
using Library.Core.Attributes;

var injectors = Injection.GetInstance().Injectors;
foreach (var i in injectors)
    Console.WriteLine("a");

[Injector]
Mamao mamaoInjector() => new Mamao();

public class Mamao
{
    public string Name { get; set; } = "mamão";
};