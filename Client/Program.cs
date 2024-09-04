using Client;
using Library.Core;
using Library.Core.Attributes;

InjectionManager.GetInstance().DoInject(typeof(Fruta));

namespace Client
{
    [InjectionConfig]
    public class Fruta
    {
        public string Name { get; set; } = "fruta";

        [Injector(Scope.SINGLETON)]
        public Fruta FrutaInjector() => new();
    };

    // [InjectionConfig]
    // public class Morango
    // {
    //     public string Name { get; set; } = "morango";

    //     [Injector(Scope.SINGLETON)]
    //     public Semente SementeInjector() => new();
    // }

    // public class Semente 
    // {
    //     public string Name { get; set; } = "semente";
    // }
}
