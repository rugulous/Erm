# ERM
ERM is a .NET Standard Object-Relational Mapping (ORM) tool developed mostly for educational/research purposes.

## Basic Usage
First, create an object representing your database (here I'm using an example from Northwind)

```
class Customers
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
```

We now need to add a custom attribute to this class. The **Db** attribute can be used to specify the connection string to be used for loading these entities.
```
[Db("Northwind")]
class Customers
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
```

Great, that's our class set up.
To load our Customers, we first need to create a query by passing the class to the `Db` class' generic `GetAll<T>` method

```
Query query = Db.GetAll<Customer>();
```

We can set a "where" clause on this query if we want by using the `.Where()` method on the query. For example, to get all customers with the ID "AROUT":

```
Query query = Db.GetAll<Customer>();
query.Where(cust => cust.CustomerID == "AROUT");
```

`Where` returns a `Query` object so we can actually chain this call:
`Query query = Db.GetAll<Customer>.Where(cust => cust.CustomerID == "AROUT");`

And, finally, we need to execute the query to get our customers
```
Query query = Db.GetAll<Customer>();
query.Where(cust => cust.CustomerID == "AROUT");
List<Customer> customers = query.Execute();
```
or, in a chain:
```List<Customer> customers = Db.GetAll<Customer>().Where(cust => cust.CustomerID == "AROUT").Execute();```
