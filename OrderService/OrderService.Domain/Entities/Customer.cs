namespace OrderService.Domain.Entities;
public class Customer
{
    public string Id {get; private set;} = default!;
    public string Name {get; private set;} = default!;
    public string Email {get; private set;} = default!;
    public string Phone {get; private set;} = default!;
    public DateTime CreatedAt {get; private set;}

    private Customer() { }

    private Customer(string id, string name, string email, string phone)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
    }


    public static Customer Create(string id, string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Customer ID can not be empty");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer Name can not be empty");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Customer email can not be empty");
            
        return new Customer(id, name, email, phone);
    }
}