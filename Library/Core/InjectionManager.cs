using System.Reflection;
using Library.Core.Attributes;

namespace Library.Core
{
    public class InjectionManager
    {
        private static InjectionManager? _injection;

        private Object? _configObj;
        private readonly Dictionary<Type, Injection> _injectorsMap;
        private readonly List<Injection> _simpleInjects;

        private InjectionManager()
        {
            _injectorsMap = new Dictionary<Type, Injection>();
            _simpleInjects = new List<Injection>();

            var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes());

            FindConfigObject(types);
            FillInjectorMap(types);
            FillSimpleInjects(types);
        }

        private void FindConfigObject(IEnumerable<Type> types)
        {
            var configClass = types.Where(
                    t => t.GetCustomAttribute(typeof(InjectionConfigAttribute)) is not null)
                .FirstOrDefault();
            
            if (configClass is null)
                return;
            
            var constructor = configClass.GetConstructor(Array.Empty<Type>()) 
                ?? throw new InvalidOperationException("InjectionConfig class should have a constructor with zero params.");
            
            _configObj = constructor.Invoke(Array.Empty<Type>());
        }

        private void FillInjectorMap(IEnumerable<Type> types)
        {
            var injectors = types.SelectMany(t => t.GetMethods())
                    .Where(m => m.GetCustomAttribute(typeof(InjectorAttribute)) is not null);
            
            foreach (var i in injectors)
                _injectorsMap.Add(i.ReturnType, new Injection(i.ReturnType, i));
        }

        private void FillSimpleInjects(IEnumerable<Type> types)
        {
            var injects = types.Where(t => t.GetCustomAttribute(typeof(InjectAttribute)) is not null);
            
            foreach (var i in injects)
                _simpleInjects.Add(new Injection(i));
        }

        public static InjectionManager GetInstance()
        {
            _injection ??= new();
            return _injection;
        }

        public Object? DoInject(Type type)
        {
            var simpleInjectTypes = _simpleInjects.Select(i => i.Type);

            if (simpleInjectTypes.Contains(type))
                return SolveSimpleInject(type);
            
            return SolveInject(type);
        }

        private Object? SolveSimpleInject(Type type)
        {

        }

        private Object? SolveInject(Type type)
        {
            bool isFound = _injectorsMap.TryGetValue(type, out var injection);
            if (!isFound)
                return null;

            var fieldTypes = type.GetFields()
                    .Select(f => f.GetType());
            
            var propertyTypes = type.GetProperties()
                    .Select(p => p.GetType());
            
            var children = fieldTypes.Union(propertyTypes)
                    .Where(t => _injectorsMap.ContainsKey(t) || _simpleInjects.Contains(t));

            InjectionTree tree = new(injection!);
            foreach (var c in children)
            {
                if (solveStack.Contains(c))
                    throw new InvalidOperationException("Circular dependency found!");

                solveStack.Push(c); 
            }

            while (solveStack.Any())
            {
                var 
            }
            
            return injection!.Invoke(_configObj, Array.Empty<Type>());
        }
    }
}