using System.Reflection;
using Library.Core.Attributes;

namespace Library.Core
{
    public class Injection
    {
        private static Injection? _injection;
        private Dictionary<Type, MethodInfo> _injectorsMap;

        public Dictionary<Type, MethodInfo> Injectors => _injectorsMap;

        private Injection()
        {
            _injectorsMap = new();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            foreach (var t in types)
            {
                var typeInjectors = t.GetMethods()
                        .Where(m => m.GetCustomAttribute(typeof(InjectorAttribute)) != null);
    
                foreach (var i in typeInjectors)
                    _injectorsMap.Add(i.ReturnType, i);
            }
        }

        public static Injection GetInstance()
        {
            _injection ??= new();
            return _injection;
        }
    }
}