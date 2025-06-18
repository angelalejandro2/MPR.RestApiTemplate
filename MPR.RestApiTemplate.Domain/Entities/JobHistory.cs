using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MPR.RestApiTemplate.Domain.Entities;

/// <summary>
/// Table that stores job history of the employees. If an employee
/// changes departments within the job or changes jobs within the department,
/// new rows get inserted into this table with old job information of the
/// employee. Contains a complex primary key: employee_id+start_date.
/// References with jobs, employees, and departments tables.
/// </summary>
[PrimaryKey("Employee_Id", "Start_Date")]
[Table("JOB_HISTORY")]
[Index("Department_Id", Name = "JHIST_DEPARTMENT_IX")]
[Index("Employee_Id", Name = "JHIST_EMPLOYEE_IX")]
[Index("Job_Id", Name = "JHIST_JOB_IX")]
public partial class JobHistory
{
    /// <summary>
    /// A not null column in the complex primary key employee_id+start_date.
    /// Foreign key to employee_id column of the employee table
    /// </summary>
    [Key]
    [Precision(6)]
    public int Employee_Id { get; set; }

    /// <summary>
    /// A not null column in the complex primary key employee_id+start_date.
    /// Must be less than the end_date of the job_history table. (enforced by
    /// constraint jhist_date_interval)
    /// </summary>
    [Key]
    [Column(TypeName = "DATE")]
    public DateTime Start_Date { get; set; }

    /// <summary>
    /// Last day of the employee in this job role. A not null column. Must be
    /// greater than the start_date of the job_history table.
    /// (enforced by constraint jhist_date_interval)
    /// </summary>
    [Column(TypeName = "DATE")]
    public DateTime End_Date { get; set; }

    /// <summary>
    /// Job role in which the employee worked in the past; foreign key to
    /// job_id column in the jobs table. A not null column.
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string Job_Id { get; set; } = null!;

    /// <summary>
    /// Department id in which the employee worked in the past; foreign key to deparment_id column in the departments table
    /// </summary>
    [Precision(4)]
    public int? Department_Id { get; set; }

    [ForeignKey("Department_Id")]
    [InverseProperty("Job_Histories")]
    public virtual Department? Department { get; set; }

    [ForeignKey("Employee_Id")]
    [InverseProperty("Job_Histories")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("Job_Id")]
    [InverseProperty("Job_Histories")]
    public virtual Job Job { get; set; } = null!;
}
