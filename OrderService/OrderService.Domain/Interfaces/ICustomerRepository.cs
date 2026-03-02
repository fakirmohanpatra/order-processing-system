namespace OrderService.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetIdByAsync(string id, CancellationToken cancellationToken);
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
}