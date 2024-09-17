namespace Aspire.MassTransit.GraphQL;

public class Mutation
{
    private readonly List<Customer?> _customers = new();

    public Customer? CreateCustomer(string name)
    {
        var customer = new Customer(Guid.NewGuid(), name, DateTime.Now);
        _customers.Add(customer);
        return customer;
    }

    public Customer? UpdateCustomer(Guid id, string name)
    {
        var customer = _customers.Find(c => c.Id == id);
        if (customer == null)
        {
            return null;
        }

        var updatedCustomer = customer with { Name = name };
        _customers[_customers.IndexOf(customer)] = updatedCustomer;
        return updatedCustomer;
    }

    public bool DeleteCustomer(Guid id)
    {
        var customer = _customers.Find(c => c.Id == id);
        if (customer == null)
        {
            return false;
        }

        _customers.Remove(customer);
        return true;
    }
}