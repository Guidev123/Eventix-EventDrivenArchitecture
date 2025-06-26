namespace Eventix.Modules.Users.Domain.Users.Models
{
    public sealed class Role
    {
        public static readonly Role Administrator = new("Administrator");
        public static readonly Role Member = new("Member");

        public Role(string name) => Name = name;

        public string Name { get; private set; }
    }
}