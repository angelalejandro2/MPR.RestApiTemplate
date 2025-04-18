﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

[Index("CategoryId", Name = "CategoriesProducts")]
[Index("CategoryId", Name = "CategoryID")]
[Index("ProductName", Name = "ProductName")]
[Index("SupplierId", Name = "SupplierID")]
[Index("SupplierId", Name = "SuppliersProducts")]
public partial class Products
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ProductID")]
    public int ProductId { get; set; }

    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    [Column("SupplierID")]
    public int? SupplierId { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [StringLength(20)]
    public string? QuantityPerUnit { get; set; }

    [Column(TypeName = "money")]
    public decimal? UnitPrice { get; set; }

    public short? UnitsInStock { get; set; }

    public short? UnitsOnOrder { get; set; }

    public short? ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Categories? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Suppliers? Supplier { get; set; }
}
