using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

/// <summary>
/// jobs table with job titles and salary ranges.
/// References with employees and job_history table.
/// </summary>
[Table("JOBS")]
public partial class Job
{
    /// <summary>
    /// Primary key of jobs table.
    /// </summary>
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Job_Id { get; set; } = null!;

    /// <summary>
    /// A not null column that shows job title, e.g. AD_VP, FI_ACCOUNTANT
    /// </summary>
    [StringLength(35)]
    [Unicode(false)]
    public string Job_Title { get; set; } = null!;

    /// <summary>
    /// Minimum salary for a job title.
    /// </summary>
    [Precision(6)]
    public int? Min_Salary { get; set; }

    /// <summary>
    /// Maximum salary for a job title
    /// </summary>
    [Precision(6)]
    public int? Max_Salary { get; set; }

    [InverseProperty("Job")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("Job")]
    public virtual ICollection<JobHistory> Job_Histories { get; set; } = new List<JobHistory>();
}
