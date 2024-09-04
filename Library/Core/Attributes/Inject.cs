namespace Library.Core.Attributes
{
    // ..simple inject attribute (dont need a config class)
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class InjectAttribute : System.Attribute
    {
        
    }
}