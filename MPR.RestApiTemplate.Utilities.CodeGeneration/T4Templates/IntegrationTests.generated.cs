
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MPR.RestApiTemplate.Application.Models;

namespace MPR.RestApiTemplate.IntegrationTests
{
    public class CategoriesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CategoriesIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Categories()
        {

            // CREATE
            var createDto = new CategoriesModel
            {
                CategoryName = "Test1",
                Description = "Test1",
                Products = /* TODO: set value for icollection<productsmodel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/categories", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<CategoriesModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/categories/{created.CategoryId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<CategoriesModel>();
            Assert.Equal(created.CategoryId, retrieved!.CategoryId);

            // UPDATE
            var updateDto = new CategoriesModel
            {
                CategoryId = created.CategoryId,
                CategoryName = "Test2",
                Description = "Test2",
                Products = /* TODO: set value for icollection<productsmodel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/categories/"+created.CategoryId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/categories/"+created.CategoryId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<CategoriesModel>();
            Assert.Equal(created.CategoryId, updated!.CategoryId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/categories/"+created.CategoryId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/categories/"+created.CategoryId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class CustomerDemographicsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CustomerDemographicsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_CustomerDemographics()
        {

            // CREATE
            var createDto = new CustomerDemographicsModel
            {
                CustomerTypeId = "Test1",
                CustomerDesc = "Test1",
                Customer = /* TODO: set value for icollection<customersmodel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/customerdemographics", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<CustomerDemographicsModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/customerdemographics/{created.CustomerTypeId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<CustomerDemographicsModel>();
            Assert.Equal(created.CustomerTypeId.Trim(), retrieved!.CustomerTypeId.Trim());

            // UPDATE
            var updateDto = new CustomerDemographicsModel
            {
                CustomerTypeId = created.CustomerTypeId,
                CustomerDesc = "Test2",
                Customer = /* TODO: set value for icollection<customersmodel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/customerdemographics/"+created.CustomerTypeId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/customerdemographics/"+created.CustomerTypeId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<CustomerDemographicsModel>();
            Assert.Equal(created.CustomerTypeId.Trim(), updated!.CustomerTypeId.Trim());

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/customerdemographics/"+created.CustomerTypeId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/customerdemographics/"+created.CustomerTypeId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class CustomersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CustomersIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Customers()
        {

            // CREATE
            var createDto = new CustomersModel
            {
                CustomerId = "Test1",
                CompanyName = "Test1",
                ContactName = "Test1",
                ContactTitle = "Test1",
                Address = "Test1",
                City = "Test1",
                Region = "Test1",
                PostalCode = "Test1",
                Country = "Test1",
                Phone = "Test1",
                Fax = "Test1",
                Orders = /* TODO: set value for icollection<ordersmodel> */ default,
                CustomerType = /* TODO: set value for icollection<customerdemographicsmodel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/customers", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<CustomersModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/customers/{created.CustomerId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<CustomersModel>();
            Assert.Equal(created.CustomerId.Trim(), retrieved!.CustomerId.Trim());

            // UPDATE
            var updateDto = new CustomersModel
            {
                CustomerId = created.CustomerId,
                CompanyName = "Test2",
                ContactName = "Test2",
                ContactTitle = "Test2",
                Address = "Test2",
                City = "Test2",
                Region = "Test2",
                PostalCode = "Test2",
                Country = "Test2",
                Phone = "Test2",
                Fax = "Test2",
                Orders = /* TODO: set value for icollection<ordersmodel> */ default,
                CustomerType = /* TODO: set value for icollection<customerdemographicsmodel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/customers/"+created.CustomerId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/customers/"+created.CustomerId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<CustomersModel>();
            Assert.Equal(created.CustomerId.Trim(), updated!.CustomerId.Trim());

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/customers/"+created.CustomerId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/customers/"+created.CustomerId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class EmployeesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EmployeesIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Employees()
        {

            // CREATE
            var createDto = new EmployeesModel
            {
                LastName = "Test1",
                FirstName = "Test1",
                Title = "Test1",
                TitleOfCourtesy = "Test1",
                BirthDate = /* TODO: set value for datetime */ default,
                HireDate = /* TODO: set value for datetime */ default,
                Address = "Test1",
                City = "Test1",
                Region = "Test1",
                PostalCode = "Test1",
                Country = "Test1",
                HomePhone = "Test1",
                Extension = "Test1",
                Notes = "Test1",
                ReportsTo = -99,
                PhotoPath = "Test1",
                InverseReportsToNavigation = /* TODO: set value for icollection<employeesmodel> */ default,
                Orders = /* TODO: set value for icollection<ordersmodel> */ default,
                ReportsToNavigation = /* TODO: set value for employeesmodel */ default,
                Territory = /* TODO: set value for icollection<territoriesmodel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/employees", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<EmployeesModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/employees/{created.EmployeeId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<EmployeesModel>();
            Assert.Equal(created.EmployeeId, retrieved!.EmployeeId);

            // UPDATE
            var updateDto = new EmployeesModel
            {
                EmployeeId = created.EmployeeId,
                LastName = "Test2",
                FirstName = "Test2",
                Title = "Test2",
                TitleOfCourtesy = "Test2",
                BirthDate = /* TODO: set value for datetime */ default,
                HireDate = /* TODO: set value for datetime */ default,
                Address = "Test2",
                City = "Test2",
                Region = "Test2",
                PostalCode = "Test2",
                Country = "Test2",
                HomePhone = "Test2",
                Extension = "Test2",
                Notes = "Test2",
                ReportsTo = -99,
                PhotoPath = "Test2",
                InverseReportsToNavigation = /* TODO: set value for icollection<employeesmodel> */ default,
                Orders = /* TODO: set value for icollection<ordersmodel> */ default,
                ReportsToNavigation = /* TODO: set value for employeesmodel */ default,
                Territory = /* TODO: set value for icollection<territoriesmodel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/employees/"+created.EmployeeId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/employees/"+created.EmployeeId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<EmployeesModel>();
            Assert.Equal(created.EmployeeId, updated!.EmployeeId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/employees/"+created.EmployeeId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/employees/"+created.EmployeeId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class OrderDetailsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrderDetailsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_OrderDetails()
        {

            // CREATE
            var createDto = new OrderDetailsModel
            {
                OrderId = -99,
                ProductId = -99,
                UnitPrice = -99.99m,
                Quantity = (short)-99,
                Withhold = -99,
                Discount = -99.99f,
                Order = /* TODO: set value for ordersmodel */ default,
                Product = /* TODO: set value for productsmodel */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/orderdetails", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<OrderDetailsModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/orderdetails/{created.OrderId}/{created.ProductId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<OrderDetailsModel>();
            Assert.Equal(created.OrderId, retrieved!.OrderId);
            Assert.Equal(created.ProductId, retrieved!.ProductId);

            // UPDATE
            var updateDto = new OrderDetailsModel
            {
                OrderId = created.OrderId,
                ProductId = created.ProductId,
                UnitPrice = -99.99m,
                Quantity = (short)-99,
                Withhold = -99,
                Discount = -99.99f,
                Order = /* TODO: set value for ordersmodel */ default,
                Product = /* TODO: set value for productsmodel */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/orderdetails/"+created.OrderId + "/" + created.ProductId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/orderdetails/"+created.OrderId + "/" + created.ProductId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<OrderDetailsModel>();
            Assert.Equal(created.OrderId, updated!.OrderId);
            Assert.Equal(created.ProductId, updated!.ProductId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/orderdetails/"+created.OrderId + "/" + created.ProductId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/orderdetails/"+created.OrderId + "/" + created.ProductId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class OrdersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrdersIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Orders()
        {

            // CREATE
            var createDto = new OrdersModel
            {
                CustomerId = "Test1",
                EmployeeId = -99,
                OrderDate = /* TODO: set value for datetime */ default,
                RequiredDate = /* TODO: set value for datetime */ default,
                ShippedDate = /* TODO: set value for datetime */ default,
                ShipVia = -99,
                Freight = -99.99m,
                ShipName = "Test1",
                ShipAddress = "Test1",
                ShipCity = "Test1",
                ShipRegion = "Test1",
                ShipPostalCode = "Test1",
                ShipCountry = "Test1",
                Customer = /* TODO: set value for customersmodel */ default,
                Employee = /* TODO: set value for employeesmodel */ default,
                OrderDetails = /* TODO: set value for icollection<orderdetailsmodel> */ default,
                ShipViaNavigation = /* TODO: set value for shippersmodel */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/orders", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<OrdersModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/orders/{created.OrderId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<OrdersModel>();
            Assert.Equal(created.OrderId, retrieved!.OrderId);

            // UPDATE
            var updateDto = new OrdersModel
            {
                OrderId = created.OrderId,
                CustomerId = "Test2",
                EmployeeId = -99,
                OrderDate = /* TODO: set value for datetime */ default,
                RequiredDate = /* TODO: set value for datetime */ default,
                ShippedDate = /* TODO: set value for datetime */ default,
                ShipVia = -99,
                Freight = -99.99m,
                ShipName = "Test2",
                ShipAddress = "Test2",
                ShipCity = "Test2",
                ShipRegion = "Test2",
                ShipPostalCode = "Test2",
                ShipCountry = "Test2",
                Customer = /* TODO: set value for customersmodel */ default,
                Employee = /* TODO: set value for employeesmodel */ default,
                OrderDetails = /* TODO: set value for icollection<orderdetailsmodel> */ default,
                ShipViaNavigation = /* TODO: set value for shippersmodel */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/orders/"+created.OrderId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/orders/"+created.OrderId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<OrdersModel>();
            Assert.Equal(created.OrderId, updated!.OrderId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/orders/"+created.OrderId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/orders/"+created.OrderId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class ProductsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Products()
        {

            // CREATE
            var createDto = new ProductsModel
            {
                ProductName = "Test1",
                SupplierId = -99,
                CategoryId = -99,
                QuantityPerUnit = "Test1",
                UnitPrice = -99.99m,
                UnitsInStock = (short)-99,
                UnitsOnOrder = (short)-99,
                ReorderLevel = (short)-99,
                Discontinued = true,
                Category = /* TODO: set value for categoriesmodel */ default,
                OrderDetails = /* TODO: set value for icollection<orderdetailsmodel> */ default,
                Supplier = /* TODO: set value for suppliersmodel */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/products", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<ProductsModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/products/{created.ProductId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<ProductsModel>();
            Assert.Equal(created.ProductId, retrieved!.ProductId);

            // UPDATE
            var updateDto = new ProductsModel
            {
                ProductId = created.ProductId,
                ProductName = "Test2",
                SupplierId = -99,
                CategoryId = -99,
                QuantityPerUnit = "Test2",
                UnitPrice = -99.99m,
                UnitsInStock = (short)-99,
                UnitsOnOrder = (short)-99,
                ReorderLevel = (short)-99,
                Discontinued = false,
                Category = /* TODO: set value for categoriesmodel */ default,
                OrderDetails = /* TODO: set value for icollection<orderdetailsmodel> */ default,
                Supplier = /* TODO: set value for suppliersmodel */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/products/"+created.ProductId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/products/"+created.ProductId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<ProductsModel>();
            Assert.Equal(created.ProductId, updated!.ProductId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/products/"+created.ProductId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/products/"+created.ProductId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class RegionIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RegionIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Region()
        {

            await _client.PostAsJsonAsync("/api/v1/region", new RegionModel
            {
                RegionId = -99
            });
            // CREATE
            var createDto = new RegionModel
            {
                RegionId = -99,
                RegionDescription = "Test1",
                Territories = /* TODO: set value for icollection<territoriesmodel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/region", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<RegionModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/region/{created.RegionId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<RegionModel>();
            Assert.Equal(created.RegionId, retrieved!.RegionId);

            // UPDATE
            var updateDto = new RegionModel
            {
                RegionId = created.RegionId,
                RegionDescription = "Test2",
                Territories = /* TODO: set value for icollection<territoriesmodel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/region/"+created.RegionId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/region/"+created.RegionId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<RegionModel>();
            Assert.Equal(created.RegionId, updated!.RegionId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/region/"+created.RegionId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/region/"+created.RegionId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
            await _client.DeleteAsync("/api/v1/region/-99");
        }
    }
    public class ShippersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ShippersIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Shippers()
        {

            // CREATE
            var createDto = new ShippersModel
            {
                CompanyName = "Test1",
                Phone = "Test1",
                Orders = /* TODO: set value for icollection<ordersmodel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/shippers", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<ShippersModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/shippers/{created.ShipperId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<ShippersModel>();
            Assert.Equal(created.ShipperId, retrieved!.ShipperId);

            // UPDATE
            var updateDto = new ShippersModel
            {
                ShipperId = created.ShipperId,
                CompanyName = "Test2",
                Phone = "Test2",
                Orders = /* TODO: set value for icollection<ordersmodel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/shippers/"+created.ShipperId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/shippers/"+created.ShipperId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<ShippersModel>();
            Assert.Equal(created.ShipperId, updated!.ShipperId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/shippers/"+created.ShipperId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/shippers/"+created.ShipperId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class SuppliersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SuppliersIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Suppliers()
        {

            // CREATE
            var createDto = new SuppliersModel
            {
                CompanyName = "Test1",
                ContactName = "Test1",
                ContactTitle = "Test1",
                Address = "Test1",
                City = "Test1",
                Region = "Test1",
                PostalCode = "Test1",
                Country = "Test1",
                Phone = "Test1",
                Fax = "Test1",
                HomePage = "Test1",
                Products = /* TODO: set value for icollection<productsmodel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/suppliers", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<SuppliersModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/suppliers/{created.SupplierId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<SuppliersModel>();
            Assert.Equal(created.SupplierId, retrieved!.SupplierId);

            // UPDATE
            var updateDto = new SuppliersModel
            {
                SupplierId = created.SupplierId,
                CompanyName = "Test2",
                ContactName = "Test2",
                ContactTitle = "Test2",
                Address = "Test2",
                City = "Test2",
                Region = "Test2",
                PostalCode = "Test2",
                Country = "Test2",
                Phone = "Test2",
                Fax = "Test2",
                HomePage = "Test2",
                Products = /* TODO: set value for icollection<productsmodel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/suppliers/"+created.SupplierId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/suppliers/"+created.SupplierId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<SuppliersModel>();
            Assert.Equal(created.SupplierId, updated!.SupplierId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/suppliers/"+created.SupplierId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/suppliers/"+created.SupplierId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
        }
    }
    public class TerritoriesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TerritoriesIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Full_Crud_For_Territories()
        {

            await _client.PostAsJsonAsync("/api/v1/region", new RegionModel
            {
                RegionId = -99
            });
            // CREATE
            var createDto = new TerritoriesModel
            {
                TerritoryId = "Test1",
                TerritoryDescription = "Test1",
                RegionId = -99,
                Region = /* TODO: set value for regionmodel */ default,
                Employee = /* TODO: set value for icollection<employeesmodel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/v1/territories", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<TerritoriesModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/v1/territories/{created.TerritoryId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<TerritoriesModel>();
            Assert.Equal(created.TerritoryId.Trim(), retrieved!.TerritoryId.Trim());

            // UPDATE
            var updateDto = new TerritoriesModel
            {
                TerritoryId = created.TerritoryId,
                TerritoryDescription = "Test2",
                RegionId = -99,
                Region = /* TODO: set value for regionmodel */ default,
                Employee = /* TODO: set value for icollection<employeesmodel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/v1/territories/"+created.TerritoryId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/v1/territories/"+created.TerritoryId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<TerritoriesModel>();
            Assert.Equal(created.TerritoryId.Trim(), updated!.TerritoryId.Trim());

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/v1/territories/"+created.TerritoryId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/v1/territories/"+created.TerritoryId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);

            // Cleanup realted entities
            await _client.DeleteAsync("/api/v1/region/-99");
        }
    }
}
