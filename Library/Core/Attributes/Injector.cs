namespace Library.Core.Attributes
{
    // ..attribute to methods inside a config class
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