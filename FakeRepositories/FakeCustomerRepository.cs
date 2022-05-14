using Bogus;
using minimal_api.IRepositories;
using minimal_api.Models;
using System.Collections.Generic;
using System.Linq;

namespace minimal_api.FakeRepositories
{
    public class FakeCustomerRepository : ICustomerRepository
    {
        private IDictionary<int, Customer> customers;

        public FakeCustomerRepository(Faker<Customer> faker)
        {
            customers = faker.Generate(100).ToDictionary(p=>p.Id);
        }

        public void Add(Customer customer) => customers.Add(customer.Id, customer);

        public IEnumerable<Customer> Get() => customers.Values;

        public Customer Get(int id)
        {
            if (customers.TryGetValue(id, out var entity))
            {
                return entity;
            }
            else
            {
                return null;
            }
        }

        public void Remove(int id) => customers.Remove(id);

        public void Update(Customer customer)
        {
            Remove(customer.Id);
            Add(customer);
        }

        public bool IsExists(int id) => customers.ContainsKey(id);
    }
}
