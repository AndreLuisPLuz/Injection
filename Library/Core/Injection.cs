using System.Reflection;

namespace Library.Core
{
    public class Injection
    {
        public Type Type { get; set; }
        public MethodInfo? InjectorInfo { get; set; }

        public Injection(Type type, MethodInfo? injectorInfo = null)
        {
            Type = type;
            InjectorInfo = injectorInfo;
        }
    }
}