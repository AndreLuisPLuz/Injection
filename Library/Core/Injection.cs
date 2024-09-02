namespace Library.Core
{
    public class Injection
    {
        private static Injection? _injection;
        private Dictionary<Type, Func<Object>> _injectors;

        private Injection()
        {
            _injectors = new();
        }

        public Injection GetInstance()
        {
            _injection ??= new();
            return _injection;
        }
    }
}