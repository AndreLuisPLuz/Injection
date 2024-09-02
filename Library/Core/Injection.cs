using Library.Core.Attributes;

namespace Library.Core
{
    public class Injection
    {
        private static Injection? _injection;
        private Dictionary<Type, InjectorAttribute> _injectorsMap;

        private Injection()
        {
            _injectorsMap = new();
        }

        public Injection GetInstance()
        {
            _injection ??= new();
            return _injection;
        }
    }
}