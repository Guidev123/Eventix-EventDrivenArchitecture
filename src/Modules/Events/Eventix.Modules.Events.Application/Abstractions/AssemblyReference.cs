using System.Reflection;

namespace Eventix.Modules.Events.Application.Abstractions
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}