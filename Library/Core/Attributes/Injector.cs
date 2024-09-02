namespace Library.Core.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class InjectorAttribute : System.Attribute
    {
        public Scope InjectorScope { get; private set; }
        public Type Class { get; private set; }
        public Func<Object> Injector { get; private set; }

        public InjectorAttribute(Scope scope)
        {
            InjectorScope = scope;
        }
    }
}