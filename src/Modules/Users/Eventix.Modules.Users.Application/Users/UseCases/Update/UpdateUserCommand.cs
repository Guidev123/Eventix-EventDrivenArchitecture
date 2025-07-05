using Eventix.Shared.Application.Messaging;

namespace Eventix.Modules.Users.Application.Users.UseCases.Update
{
    public sealed record UpdateUserCommand : ICommand
    {
        public UpdateUserCommand(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid UserId { get; private set; }
        public string FirstName { get; }
        public string LastName { get; }
        public void SetUserId(Guid userId) => UserId = userId;
    }
}