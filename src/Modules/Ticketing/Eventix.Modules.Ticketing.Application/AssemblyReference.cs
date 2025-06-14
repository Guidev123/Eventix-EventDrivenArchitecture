using System.Reflection;

namespace Eventix.Modules.Ticketing.Application
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}