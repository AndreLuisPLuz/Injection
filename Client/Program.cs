using Library.Core;
using Library.Core.Attributes;

var injectors = Injection.GetInstance().Injectors;
foreach (var i in injectors)
    Console.WriteLine(i.Value.Name);


namespace Client
{
    public class Mamao
    {
        public string Name { get; set; } = "mamão";

        [Injector(Scope.SINGLETON)]
        public Mamao MamaoInjector() => new();
    };
}
