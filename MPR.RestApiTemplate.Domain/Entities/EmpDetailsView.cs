using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

[Keyless]
public partial class EmpDetailsView
{
    [Precision(6)]
    public int Employee_Id { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Job_Id { get; set; } = null!;

    [Precision(6)]
    public int? Manager_Id { get; set; }

    [Precision(4)]
    public int? Department_id { get; set; }

    [Precision(4)]
    public int? Location_Id { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? Country_Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? First_Name { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string Last_Name { get; set; } = null!;

    [Column(TypeName = "NUMBER(8,2)")]
    public decimal? Salary { get; set; }

    [Column(TypeName = "NUMBER(2,2)")]
    public decimal? Commission_Pct { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Department_Name { get; set; } = null!;

    [StringLength(35)]
    [Unicode(false)]
    public string Job_Title { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [StringLength(25)]
    [Unicode(false)]
    public string? State_Province { get; set; }

    [StringLength(60)]
    [Unicode(false)]
    public string? Country_Name { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? Region_Name { get; set; }
}
