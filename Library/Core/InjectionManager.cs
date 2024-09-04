using System.Reflection;
using Library.Core.Attributes;

namespace Library.Core
{
    public class InjectionManager
    {
        private static InjectionManager? _injection;

        public static InjectionManager GetInstance()
        {
            _injection ??= new();
            return _injection;
        }

        private Object? _configObj;
        private readonly Dictionary<Type, Injection> _injectorsMap;
        private readonly List<Injection> _simpleInjects;
        private readonly Stack<InjectionNode> _solveStack;
        private readonly InjectionTree _tree;

        private InjectionManager()
        {
            _injectorsMap = new Dictionary<Type, Injection>();
            _simpleInjects = new List<Injection>();
            _solveStack = new Stack<InjectionNode>();
            _tree = new InjectionTree();

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

        public void DoInject(Type type)
        {
            // ..if its a simple inject
            var simpleInjectTypes = _simpleInjects.Select(i => i.Type);

            if (simpleInjectTypes.Contains(type))
                SolveSimpleInject(type);
            
            // ..if its a normal inject
            SolveInject(type);
        }

        private void SolveInject(Type type)
        {
            if(!_solveStack.Any())
                _solveStack.Push(new InjectionNode(new Injection(type)));

            while (_solveStack.Any())
            {
                var crrInjectionNode = _solveStack.Pop();
                var obj = Invoke(crrInjectionNode.Injection.Type);

                foreach(var n in crrInjectionNode.GetAncestors())
                {
                    Invoke(n.GetType());
                }
            }
        }

        private Object? Invoke(Type type)
        {
            bool isFound = _injectorsMap.TryGetValue(type, out var injection);
            if (!isFound)
                return null;

            if(_tree.Root == null)
                _tree.AddRoot(new InjectionNode(injection!));

            AddChildren(type, _tree.Root!);

            return injection!.InjectorInfo!.Invoke(_configObj, Array.Empty<Type>());
        }

        private Object? SolveSimpleInject(Type type)
        {
            throw new NotImplementedException();
        }

        private void AddChildren(Type type, InjectionNode parent)
        {
            var children = getChildren(type);
            foreach (var c in children)
            {
                AddOnTree(c, parent);
            }
        }

        private void AddOnTree(Type type, InjectionNode parent)
        {
            var injection = new Injection(type);
            _tree.NewLeaf(injection, parent);

            if(!_solveStack.Contains(parent))
                _solveStack.Push(parent);

            AddChildren(type, parent);
        }

        private IEnumerable<Type> getChildren(Type type)
        {
            var fieldTypes = type.GetFields()
                    .Select(f => f.GetType());
            
            var propertyTypes = type.GetProperties()
                    .Select(p => p.GetType());
            
            // ..needs to verify if simple injection contains t
            var children = fieldTypes.Union(propertyTypes)
                    // .Where(t => _injectorsMap.ContainsKey(t) || _simpleInjects.Contains(t));
                    .Where(t => _injectorsMap.ContainsKey(t));
            
            return children;
        }
    }
}