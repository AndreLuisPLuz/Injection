namespace Library.Core.Attributes
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class InjectorAttribute : System.Attribute
    {
        public Scope InjectorScope { get; private set; }

        public InjectorAttribute(Scope scope)
        {
            InjectorScope = scope;
        }
    }
}