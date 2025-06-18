using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

/// <summary>
/// country table. References with locations table.
/// </summary>
[Table("COUNTRIES")]
public partial class Country
{
    /// <summary>
    /// Primary key of countries table.
    /// </summary>
    [Key]
    [StringLength(2)]
    [Unicode(false)]
    public string Country_Id { get; set; } = null!;

    /// <summary>
    /// Country name
    /// </summary>
    [StringLength(60)]
    [Unicode(false)]
    public string? Country_Name { get; set; }

    /// <summary>
    /// Region ID for the country. Foreign key to region_id column in the departments table.
    /// </summary>
    [Column(TypeName = "NUMBER")]
    public decimal? Region_Id { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    [ForeignKey("Region_Id")]
    [InverseProperty("Countries")]
    public virtual Region? Region { get; set; }
}
