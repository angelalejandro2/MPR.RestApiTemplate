using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

/// <summary>
/// employees table. References with departments,
/// jobs, job_history tables. Contains a self reference.
/// </summary>
[Table("EMPLOYEES")]
[Index("Department_Id", Name = "EMP_DEPARTMENT_IX")]
[Index("Email", Name = "EMP_EMAIL_UK", IsUnique = true)]
[Index("Job_Id", Name = "EMP_JOB_IX")]
[Index("Manager_Id", Name = "EMP_MANAGER_IX")]
[Index("Last_Name", "First_Name", Name = "EMP_NAME_IX")]
public partial class Employee
{
    /// <summary>
    /// Primary key of employees table.
    /// </summary>
    [Key]
    [Precision(6)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Employee_Id { get; set; }

    /// <summary>
    /// First name of the employee. A not null column.
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? First_Name { get; set; }

    /// <summary>
    /// Last name of the employee. A not null column.
    /// </summary>
    [StringLength(25)]
    [Unicode(false)]
    public string Last_Name { get; set; } = null!;

    /// <summary>
    /// Email id of the employee
    /// </summary>
    [StringLength(25)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Phone number of the employee; includes country code and area code
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? Phone_Number { get; set; }

    /// <summary>
    /// Date when the employee started on this job. A not null column.
    /// </summary>
    [Column(TypeName = "DATE")]
    public DateTime Hire_Date { get; set; }

    /// <summary>
    /// Current job of the employee; foreign key to job_id column of the
    /// jobs table. A not null column.
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Job_Id { get; set; } = null!;

    /// <summary>
    /// Monthly salary of the employee. Must be greater
    /// than zero (enforced by constraint emp_salary_min)
    /// </summary>
    [Column(TypeName = "NUMBER(8,2)")]
    public decimal? Salary { get; set; }

    /// <summary>
    /// Commission percentage of the employee; Only employees in sales
    /// department elgible for commission percentage
    /// </summary>
    [Column(TypeName = "NUMBER(2,2)")]
    public decimal? Commission_Pct { get; set; }

    /// <summary>
    /// Manager id of the employee; has same domain as manager_id in
    /// departments table. Foreign key to employee_id column of employees table.
    /// (useful for reflexive joins and CONNECT BY query)
    /// </summary>
    [Precision(6)]
    public int? Manager_Id { get; set; }

    /// <summary>
    /// Department id where employee works; foreign key to department_id
    /// column of the departments table
    /// </summary>
    [Precision(4)]
    public int? Department_Id { get; set; }

    [ForeignKey("Department_Id")]
    [InverseProperty("Employees")]
    public virtual Department? Department { get; set; }

    [InverseProperty("Manager")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    [InverseProperty("Manager")]
    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    [ForeignKey("Job_Id")]
    [InverseProperty("Employees")]
    public virtual Job Job { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<JobHistory> Job_Histories { get; set; } = new List<JobHistory>();

    [ForeignKey("Manager_Id")]
    [InverseProperty("InverseManager")]
    public virtual Employee? Manager { get; set; }
}
