namespace Aspire.MassTransit.GraphQL;

public class Query
{
    private readonly List<Customer> _customers = new()
    {
        new Customer(Guid.NewGuid(), "John Doe", DateTime.Now),
        new Customer(Guid.NewGuid(), "Jane Smith", DateTime.Now)
    };

    public IEnumerable<Customer> GetCustomers() => _customers;
}