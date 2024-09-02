namespace Library.Core
{
    public class Injection
    {
        private static Injection? _injection;
        private Dictionary<Type, Func<Object>> _injectorsMap;

        private Injection()
        {
            _injectorsMap = new Dictionary<Type, Func<Object>>();
        }

        public Injection GetInstance()
        {
            _injection ??= new();
            return _injection;
        }
    }
}