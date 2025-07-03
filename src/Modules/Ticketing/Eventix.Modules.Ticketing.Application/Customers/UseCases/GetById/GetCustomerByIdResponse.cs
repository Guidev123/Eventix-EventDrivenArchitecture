namespace Eventix.Modules.Ticketing.Application.Customers.UseCases.GetById
{
    public sealed record GetCustomerByIdResponse(
        Guid Id,
        string Email,
        string FirstName,
        string LastName
        );
}