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
                Products = /* TODO: set value for ICollection<ProductsModel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/categoriess", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<CategoriesModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/categoriess/{created.CategoryId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<CategoriesModel>();
            Assert.Equal(created.CategoryId, retrieved!.CategoryId);

            // UPDATE
            var updateDto = new CategoriesModel
            {
                CategoryId = created.CategoryId,
                CategoryName = "Test2",
                Description = "Test2",
                Products = /* TODO: set value for ICollection<ProductsModel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/categoriess/"+created.CategoryId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/categoriess/"+created.CategoryId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<CategoriesModel>();
            Assert.Equal(created.CategoryId, updated!.CategoryId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/categoriess/"+created.CategoryId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/categoriess/"+created.CategoryId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
                CustomerDesc = "Test1",
                Customer = /* TODO: set value for ICollection<CustomersModel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/customerdemographicss", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<CustomerDemographicsModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/customerdemographicss/{created.CustomerTypeId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<CustomerDemographicsModel>();
            Assert.Equal(created.CustomerTypeId, retrieved!.CustomerTypeId);

            // UPDATE
            var updateDto = new CustomerDemographicsModel
            {
                CustomerTypeId = created.CustomerTypeId,
                CustomerDesc = "Test2",
                Customer = /* TODO: set value for ICollection<CustomersModel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/customerdemographicss/"+created.CustomerTypeId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/customerdemographicss/"+created.CustomerTypeId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<CustomerDemographicsModel>();
            Assert.Equal(created.CustomerTypeId, updated!.CustomerTypeId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/customerdemographicss/"+created.CustomerTypeId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/customerdemographicss/"+created.CustomerTypeId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
                Orders = /* TODO: set value for ICollection<OrdersModel> */ default,
                CustomerType = /* TODO: set value for ICollection<CustomerDemographicsModel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/customerss", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<CustomersModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/customerss/{created.CustomerId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<CustomersModel>();
            Assert.Equal(created.CustomerId, retrieved!.CustomerId);

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
                Orders = /* TODO: set value for ICollection<OrdersModel> */ default,
                CustomerType = /* TODO: set value for ICollection<CustomerDemographicsModel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/customerss/"+created.CustomerId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/customerss/"+created.CustomerId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<CustomersModel>();
            Assert.Equal(created.CustomerId, updated!.CustomerId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/customerss/"+created.CustomerId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/customerss/"+created.CustomerId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
                BirthDate = DateTime.UtcNow,
                HireDate = DateTime.UtcNow,
                Address = "Test1",
                City = "Test1",
                Region = "Test1",
                PostalCode = "Test1",
                Country = "Test1",
                HomePhone = "Test1",
                Extension = "Test1",
                Notes = "Test1",
                ReportsTo = 1,
                PhotoPath = "Test1",
                InverseReportsToNavigation = /* TODO: set value for ICollection<EmployeesModel> */ default,
                Orders = /* TODO: set value for ICollection<OrdersModel> */ default,
                ReportsToNavigation = /* TODO: set value for EmployeesModel */ default,
                Territory = /* TODO: set value for ICollection<TerritoriesModel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/employeess", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<EmployeesModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/employeess/{created.EmployeeId}");
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
                BirthDate = DateTime.UtcNow,
                HireDate = DateTime.UtcNow,
                Address = "Test2",
                City = "Test2",
                Region = "Test2",
                PostalCode = "Test2",
                Country = "Test2",
                HomePhone = "Test2",
                Extension = "Test2",
                Notes = "Test2",
                ReportsTo = 2,
                PhotoPath = "Test2",
                InverseReportsToNavigation = /* TODO: set value for ICollection<EmployeesModel> */ default,
                Orders = /* TODO: set value for ICollection<OrdersModel> */ default,
                ReportsToNavigation = /* TODO: set value for EmployeesModel */ default,
                Territory = /* TODO: set value for ICollection<TerritoriesModel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/employeess/"+created.EmployeeId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/employeess/"+created.EmployeeId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<EmployeesModel>();
            Assert.Equal(created.EmployeeId, updated!.EmployeeId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/employeess/"+created.EmployeeId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/employeess/"+created.EmployeeId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
                UnitPrice = /* TODO: set value for decimal */ default,
                Quantity = /* TODO: set value for short */ default,
                Discount = /* TODO: set value for float */ default,
                Order = /* TODO: set value for OrdersModel */ default,
                Product = /* TODO: set value for ProductsModel */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/orderdetailss", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<OrderDetailsModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/orderdetailss/{created.OrderId}/{created.ProductId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<OrderDetailsModel>();
            Assert.Equal(created.OrderId, retrieved!.OrderId);
            Assert.Equal(created.ProductId, retrieved!.ProductId);

            // UPDATE
            var updateDto = new OrderDetailsModel
            {
                OrderId = created.OrderId,
                ProductId = created.ProductId,
                UnitPrice = /* TODO: set value for decimal */ default,
                Quantity = /* TODO: set value for short */ default,
                Discount = /* TODO: set value for float */ default,
                Order = /* TODO: set value for OrdersModel */ default,
                Product = /* TODO: set value for ProductsModel */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/orderdetailss/"+created.OrderId + "/" + created.ProductId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/orderdetailss/"+created.OrderId + "/" + created.ProductId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<OrderDetailsModel>();
            Assert.Equal(created.OrderId, updated!.OrderId);
            Assert.Equal(created.ProductId, updated!.ProductId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/orderdetailss/"+created.OrderId + "/" + created.ProductId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/orderdetailss/"+created.OrderId + "/" + created.ProductId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
                EmployeeId = 1,
                OrderDate = DateTime.UtcNow,
                RequiredDate = DateTime.UtcNow,
                ShippedDate = DateTime.UtcNow,
                ShipVia = 1,
                Freight = /* TODO: set value for decimal */ default,
                ShipName = "Test1",
                ShipAddress = "Test1",
                ShipCity = "Test1",
                ShipRegion = "Test1",
                ShipPostalCode = "Test1",
                ShipCountry = "Test1",
                Customer = /* TODO: set value for CustomersModel */ default,
                Employee = /* TODO: set value for EmployeesModel */ default,
                OrderDetails = /* TODO: set value for ICollection<OrderDetailsModel> */ default,
                ShipViaNavigation = /* TODO: set value for ShippersModel */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/orderss", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<OrdersModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/orderss/{created.OrderId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<OrdersModel>();
            Assert.Equal(created.OrderId, retrieved!.OrderId);

            // UPDATE
            var updateDto = new OrdersModel
            {
                OrderId = created.OrderId,
                CustomerId = "Test2",
                EmployeeId = 2,
                OrderDate = DateTime.UtcNow,
                RequiredDate = DateTime.UtcNow,
                ShippedDate = DateTime.UtcNow,
                ShipVia = 2,
                Freight = /* TODO: set value for decimal */ default,
                ShipName = "Test2",
                ShipAddress = "Test2",
                ShipCity = "Test2",
                ShipRegion = "Test2",
                ShipPostalCode = "Test2",
                ShipCountry = "Test2",
                Customer = /* TODO: set value for CustomersModel */ default,
                Employee = /* TODO: set value for EmployeesModel */ default,
                OrderDetails = /* TODO: set value for ICollection<OrderDetailsModel> */ default,
                ShipViaNavigation = /* TODO: set value for ShippersModel */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/orderss/"+created.OrderId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/orderss/"+created.OrderId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<OrdersModel>();
            Assert.Equal(created.OrderId, updated!.OrderId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/orderss/"+created.OrderId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/orderss/"+created.OrderId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
                SupplierId = 1,
                CategoryId = 1,
                QuantityPerUnit = "Test1",
                UnitPrice = /* TODO: set value for decimal */ default,
                UnitsInStock = /* TODO: set value for short */ default,
                UnitsOnOrder = /* TODO: set value for short */ default,
                ReorderLevel = /* TODO: set value for short */ default,
                Discontinued = true,
                Category = /* TODO: set value for CategoriesModel */ default,
                OrderDetails = /* TODO: set value for ICollection<OrderDetailsModel> */ default,
                Supplier = /* TODO: set value for SuppliersModel */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/productss", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<ProductsModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/productss/{created.ProductId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<ProductsModel>();
            Assert.Equal(created.ProductId, retrieved!.ProductId);

            // UPDATE
            var updateDto = new ProductsModel
            {
                ProductId = created.ProductId,
                ProductName = "Test2",
                SupplierId = 2,
                CategoryId = 2,
                QuantityPerUnit = "Test2",
                UnitPrice = /* TODO: set value for decimal */ default,
                UnitsInStock = /* TODO: set value for short */ default,
                UnitsOnOrder = /* TODO: set value for short */ default,
                ReorderLevel = /* TODO: set value for short */ default,
                Discontinued = false,
                Category = /* TODO: set value for CategoriesModel */ default,
                OrderDetails = /* TODO: set value for ICollection<OrderDetailsModel> */ default,
                Supplier = /* TODO: set value for SuppliersModel */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/productss/"+created.ProductId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/productss/"+created.ProductId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<ProductsModel>();
            Assert.Equal(created.ProductId, updated!.ProductId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/productss/"+created.ProductId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/productss/"+created.ProductId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
            // CREATE
            var createDto = new RegionModel
            {
                RegionDescription = "Test1",
                Territories = /* TODO: set value for ICollection<TerritoriesModel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/regions", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<RegionModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/regions/{created.RegionId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<RegionModel>();
            Assert.Equal(created.RegionId, retrieved!.RegionId);

            // UPDATE
            var updateDto = new RegionModel
            {
                RegionId = created.RegionId,
                RegionDescription = "Test2",
                Territories = /* TODO: set value for ICollection<TerritoriesModel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/regions/"+created.RegionId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/regions/"+created.RegionId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<RegionModel>();
            Assert.Equal(created.RegionId, updated!.RegionId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/regions/"+created.RegionId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/regions/"+created.RegionId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
                Orders = /* TODO: set value for ICollection<OrdersModel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/shipperss", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<ShippersModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/shipperss/{created.ShipperId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<ShippersModel>();
            Assert.Equal(created.ShipperId, retrieved!.ShipperId);

            // UPDATE
            var updateDto = new ShippersModel
            {
                ShipperId = created.ShipperId,
                CompanyName = "Test2",
                Phone = "Test2",
                Orders = /* TODO: set value for ICollection<OrdersModel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/shipperss/"+created.ShipperId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/shipperss/"+created.ShipperId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<ShippersModel>();
            Assert.Equal(created.ShipperId, updated!.ShipperId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/shipperss/"+created.ShipperId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/shipperss/"+created.ShipperId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
                Products = /* TODO: set value for ICollection<ProductsModel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/supplierss", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<SuppliersModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/supplierss/{created.SupplierId}");
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
                Products = /* TODO: set value for ICollection<ProductsModel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/supplierss/"+created.SupplierId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/supplierss/"+created.SupplierId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<SuppliersModel>();
            Assert.Equal(created.SupplierId, updated!.SupplierId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/supplierss/"+created.SupplierId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/supplierss/"+created.SupplierId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
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
            // CREATE
            var createDto = new TerritoriesModel
            {
                TerritoryDescription = "Test1",
                RegionId = 1,
                Region = /* TODO: set value for RegionModel */ default,
                Employee = /* TODO: set value for ICollection<EmployeesModel> */ default,
            };

            var postResponse = await _client.PostAsJsonAsync("/api/territoriess", createDto);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<TerritoriesModel>();

            // READ
            var getResponse = await _client.GetAsync($"/api/territoriess/{created.TerritoryId}");
            getResponse.EnsureSuccessStatusCode();
            var retrieved = await getResponse.Content.ReadFromJsonAsync<TerritoriesModel>();
            Assert.Equal(created.TerritoryId, retrieved!.TerritoryId);

            // UPDATE
            var updateDto = new TerritoriesModel
            {
                TerritoryId = created.TerritoryId,
                TerritoryDescription = "Test2",
                RegionId = 2,
                Region = /* TODO: set value for RegionModel */ default,
                Employee = /* TODO: set value for ICollection<EmployeesModel> */ default,
            };

            var putResponse = await _client.PutAsJsonAsync($"/api/territoriess/"+created.TerritoryId, updateDto);
            putResponse.EnsureSuccessStatusCode();

            var getAfterPut = await _client.GetAsync($"/api/territoriess/"+created.TerritoryId);
            getAfterPut.EnsureSuccessStatusCode();
            var updated = await getAfterPut.Content.ReadFromJsonAsync<TerritoriesModel>();
            Assert.Equal(created.TerritoryId, updated!.TerritoryId);

            // DELETE
            var deleteResponse = await _client.DeleteAsync($"/api/territoriess/"+created.TerritoryId);
            deleteResponse.EnsureSuccessStatusCode();

            var getAfterDelete = await _client.GetAsync($"/api/territoriess/"+created.TerritoryId);
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }
    }
}
