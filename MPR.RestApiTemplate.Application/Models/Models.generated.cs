using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPR.RestApiTemplate.Application.Models
{

    public class AlphabeticalListOfProductsModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoriesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }
        public ICollection<ProductsModel> Products { get; set; }
    }

    public class CategorySalesFor1997Model
    {
        public string CategoryName { get; set; }
        public decimal? CategorySales { get; set; }
    }

    public class CurrentProductListModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class CustomerAndSuppliersByCityModel
    {
        public string? City { get; set; }
        public string CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string Relationship { get; set; }
    }

    public class CustomerDemographicsModel
    {
        [Key]
        public string CustomerTypeId { get; set; }
        public string? CustomerDesc { get; set; }
        public ICollection<CustomersModel> Customer { get; set; }
    }

    public class CustomersModel
    {
        [Key]
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public ICollection<OrdersModel> Orders { get; set; }
        public ICollection<CustomerDemographicsModel> CustomerType { get; set; }
    }

    public class EmployeesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? Title { get; set; }
        public string? TitleOfCourtesy { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? HomePhone { get; set; }
        public string? Extension { get; set; }
        public byte[]? Photo { get; set; }
        public string? Notes { get; set; }
        public int? ReportsTo { get; set; }
        public string? PhotoPath { get; set; }
        public ICollection<EmployeesModel> InverseReportsToNavigation { get; set; }
        public ICollection<OrdersModel> Orders { get; set; }
        public EmployeesModel? ReportsToNavigation { get; set; }
        public ICollection<TerritoriesModel> Territory { get; set; }
    }

    public class InvoicesModel
    {
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public string? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string Salesperson { get; set; }
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string ShipperName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public decimal? ExtendedPrice { get; set; }
        public decimal? Freight { get; set; }
    }

    public class OrderDetailsModel
    {
        [Key]
        public int OrderId { get; set; }
        [Key]
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public OrdersModel Order { get; set; }
        public ProductsModel Product { get; set; }
    }

    public class OrderDetailsExtendedModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public decimal? ExtendedPrice { get; set; }
    }

    public class OrdersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public CustomersModel? Customer { get; set; }
        public EmployeesModel? Employee { get; set; }
        public ICollection<OrderDetailsModel> OrderDetails { get; set; }
        public ShippersModel? ShipViaNavigation { get; set; }
    }

    public class OrdersQryModel
    {
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public string CompanyName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }

    public class OrderSubtotalsModel
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }

    public class ProductsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public CategoriesModel? Category { get; set; }
        public ICollection<OrderDetailsModel> OrderDetails { get; set; }
        public SuppliersModel? Supplier { get; set; }
    }

    public class ProductsAboveAveragePriceModel
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }

    public class ProductSalesFor1997Model
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductSales { get; set; }
    }

    public class ProductsByCategoryModel
    {
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string? QuantityPerUnit { get; set; }
        public short? UnitsInStock { get; set; }
        public bool Discontinued { get; set; }
    }

    public class QuarterlyOrdersModel
    {
        public string? CustomerId { get; set; }
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }

    public class RegionModel
    {
        [Key]
        public int RegionId { get; set; }
        public string RegionDescription { get; set; }
        public ICollection<TerritoriesModel> Territories { get; set; }
    }

    public class SalesByCategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductSales { get; set; }
    }

    public class SalesTotalsByAmountModel
    {
        public decimal? SaleAmount { get; set; }
        public int OrderId { get; set; }
        public string CompanyName { get; set; }
        public DateTime? ShippedDate { get; set; }
    }

    public class ShippersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string? Phone { get; set; }
        public ICollection<OrdersModel> Orders { get; set; }
    }

    public class SummaryOfSalesByQuarterModel
    {
        public DateTime? ShippedDate { get; set; }
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }

    public class SummaryOfSalesByYearModel
    {
        public DateTime? ShippedDate { get; set; }
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }

    public class SuppliersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? HomePage { get; set; }
        public ICollection<ProductsModel> Products { get; set; }
    }

    public class TerritoriesModel
    {
        [Key]
        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }
        public RegionModel Region { get; set; }
        public ICollection<EmployeesModel> Employee { get; set; }
    }
}