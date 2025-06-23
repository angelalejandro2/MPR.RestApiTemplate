using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

/// <summary>
/// Regions table that contains region numbers and names. references with the Countries table.
/// </summary>
[Table("REGIONS")]
public partial class Region
{
    /// <summary>
    /// Primary key of regions table.
    /// </summary>
    [Key]
    [Column(TypeName = "NUMBER")]
    public decimal Region_Id { get; set; }

    /// <summary>
    /// Names of regions. Locations are in the countries of these regions.
    /// </summary>
    [StringLength(25)]
    [Unicode(false)]
    public string? Region_Name { get; set; }

    [InverseProperty("Region")]
    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
