using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

/// <summary>
/// Departments table that shows details of departments where employees
/// work. references with locations, employees, and job_history tables.
/// </summary>
[Table("DEPARTMENTS")]
[Index("Location_Id", Name = "DEPT_LOCATION_IX")]
public partial class Department
{
    /// <summary>
    /// Primary key column of departments table.
    /// </summary>
    [Key]
    [Precision(4)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Department_Id { get; set; }

    /// <summary>
    /// A not null column that shows name of a department. Administration,
    /// Marketing, Purchasing, Human Resources, Shipping, IT, Executive, Public
    /// Relations, Sales, Finance, and Accounting. 
    /// </summary>
    [StringLength(30)]
    [Unicode(false)]
    public string Department_Name { get; set; } = null!;

    /// <summary>
    /// Manager_id of a department. Foreign key to employee_id column of employees table. The manager_id column of the employee table references this column.
    /// </summary>
    [Precision(6)]
    public int? Manager_Id { get; set; }

    /// <summary>
    /// Location id where a department is located. Foreign key to location_id column of locations table.
    /// </summary>
    [Precision(4)]
    public int? Location_Id { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [InverseProperty("Department")]
    public virtual ICollection<JobHistory> Job_Histories { get; set; } = new List<JobHistory>();

    [ForeignKey("Location_Id")]
    [InverseProperty("Departments")]
    public virtual Location? Location { get; set; }

    [ForeignKey("Manager_Id")]
    [InverseProperty("Departments")]
    public virtual Employee? Manager { get; set; }
}
