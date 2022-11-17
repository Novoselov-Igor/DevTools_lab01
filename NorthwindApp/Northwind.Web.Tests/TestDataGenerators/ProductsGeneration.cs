using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Northwind.Model;
using Northwind.Web.Tests.TestDataGenerators;

namespace Northwind.Web.Controllers
{
    public class ProductsGeneration : ITestDataGenerator<Product>
    {
        private readonly NorthwindContext? northwindContext;
        private Faker<Product> faker;

        public ProductsGeneration(NorthwindContext? northwindContext = null)
        {
            this.northwindContext = northwindContext;
            faker = new Faker<Product>()
                .StrictMode(false)
                .RuleFor(c => c.ProductName, f => f.Commerce.ProductName())
                .RuleFor(c => c.QuantityPerUnit, f => f.Lorem.Word())
                .RuleFor(c => c.UnitPrice, f => f.Random.Decimal(1, 5000))
                .RuleFor(c => c.UnitsInStock, f => f.Random.Short(0, 32767))
                .RuleFor(c => c.UnitsOnOrder, f => f.Random.Short(0, 32767))
                .RuleFor(c => c.ReorderLevel, f => f.Random.Short(0, 15000))
                .RuleFor(c => c.Discontinued, f => f.Random.Bool());
        }

        public Product Generate()
        {
            var product = faker.Generate();
            northwindContext?.Products.Add(product);
            return product;
        }

        public IEnumerable<Product> Generate(int count)
        {
            var products = faker.Generate(count);
            northwindContext?.Products.AddRange(products);
            return products;
        }
    }
}
