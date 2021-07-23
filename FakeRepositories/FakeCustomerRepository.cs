using Bogus;
using minimal_api.IRepositories;
using minimal_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.FakeRepositories
{
    public class FakeCustomerRepository : ICustomerRepository
    {
        private ICollection<Customer> customers;

        private readonly Faker<Customer> faker;

        public FakeCustomerRepository(Faker<Customer> faker)
        {
            this.faker = faker;

            customers = faker.Generate(100);
        }

        public void Add(Customer customer)
        {            
            customers.Add(customer);
        }

        public IEnumerable<Customer> Get()
        {
            return customers;
        }

        public Customer Get(int id)
        {
            return customers.SingleOrDefault(c => c.Id == id);
        }

        public void Remove(int id)
        {
            customers.Remove(Get(id));
        }

        public void Update(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
