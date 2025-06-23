using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

/// <summary>
/// Locations table that contains specific address of a specific office,
/// warehouse, and/or production site of a company. Does not store addresses /
/// locations of customers. references with the departments and countries tables. 
/// </summary>
[Table("LOCATIONS")]
[Index("City", Name = "LOC_CITY_IX")]
[Index("Country_Id", Name = "LOC_COUNTRY_IX")]
[Index("State_Province", Name = "LOC_STATE_PROVINCE_IX")]
public partial class Location
{
    /// <summary>
    /// Primary key of locations table
    /// </summary>
    [Key]
    [Precision(4)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Location_Id { get; set; }

    /// <summary>
    /// Street address of an office, warehouse, or production site of a company.
    /// Contains building number and street name
    /// </summary>
    [StringLength(40)]
    [Unicode(false)]
    public string? Street_Address { get; set; }

    /// <summary>
    /// Postal code of the location of an office, warehouse, or production site
    /// of a company. 
    /// </summary>
    [StringLength(12)]
    [Unicode(false)]
    public string? Postal_Code { get; set; }

    /// <summary>
    /// A not null column that shows city where an office, warehouse, or
    /// production site of a company is located. 
    /// </summary>
    [StringLength(30)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    /// <summary>
    /// State or Province where an office, warehouse, or production site of a
    /// company is located.
    /// </summary>
    [StringLength(25)]
    [Unicode(false)]
    public string? State_Province { get; set; }

    /// <summary>
    /// Country where an office, warehouse, or production site of a company is
    /// located. Foreign key to country_id column of the countries table.
    /// </summary>
    [StringLength(2)]
    [Unicode(false)]
    public string? Country_Id { get; set; }

    [ForeignKey("Country_Id")]
    [InverseProperty("Locations")]
    public virtual Country? Country { get; set; }

    [InverseProperty("Location")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
