using System;
using System.Collections.Generic;
using MPR.RestApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MPR.RestApiTemplate.Infrastructure.Extensions;

namespace MPR.RestApiTemplate.Infrastructure.Context;

public partial class HrContext : DbContext
{
    public HrContext(DbContextOptions<HrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmpDetailsView> EmpDetailsViews { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobHistory> Job_Histories { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("HR")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Country_Id).HasName("COUNTRY_C_ID_PK");

            entity.ToTable("COUNTRIES", tb => tb.HasComment("country table. References with locations table."));

            entity.Property(e => e.Country_Id)
                .IsFixedLength()
                .HasComment("Primary key of countries table.");
            entity.Property(e => e.Country_Name).HasComment("Country name");
            entity.Property(e => e.Region_Id).HasComment("Region ID for the country. Foreign key to region_id column in the departments table.");

            entity.HasOne(d => d.Region).WithMany(p => p.Countries).HasConstraintName("COUNTR_REG_FK");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Department_Id).HasName("DEPT_ID_PK");

            entity.ToTable("DEPARTMENTS", tb => tb.HasComment("Departments table that shows details of departments where employees\nwork. references with locations, employees, and job_history tables."));

            entity.Property(e => e.Department_Id).HasComment("Primary key column of departments table.").HasDefaultValueSql("DEPARTMENTS_SEQ.NEXTVAL"); ;
            entity.Property(e => e.Department_Name).HasComment("A not null column that shows name of a department. Administration,\nMarketing, Purchasing, Human Resources, Shipping, IT, Executive, Public\nRelations, Sales, Finance, and Accounting. ");
            entity.Property(e => e.Location_Id).HasComment("Location id where a department is located. Foreign key to location_id column of locations table.");
            entity.Property(e => e.Manager_Id).HasComment("Manager_id of a department. Foreign key to employee_id column of employees table. The manager_id column of the employee table references this column.");

            entity.HasOne(d => d.Location).WithMany(p => p.Departments).HasConstraintName("DEPT_LOC_FK");

            entity.HasOne(d => d.Manager).WithMany(p => p.Departments).HasConstraintName("DEPT_MGR_FK");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Employee_Id).HasName("EMP_EMP_ID_PK");

            entity.ToTable("EMPLOYEES", tb => tb.HasComment("employees table. References with departments,\njobs, job_history tables. Contains a self reference."));

            entity.Property(e => e.Employee_Id).HasComment("Primary key of employees table.").HasDefaultValueSql("EMPLOYEES_SEQ.NEXTVAL"); ;
            entity.Property(e => e.Commission_Pct).HasComment("Commission percentage of the employee; Only employees in sales\ndepartment elgible for commission percentage");
            entity.Property(e => e.Department_Id)
                .ValueGeneratedOnAdd()
                .HasComment("Department id where employee works; foreign key to department_id\ncolumn of the departments table");
            entity.Property(e => e.Email).HasComment("Email id of the employee");
            entity.Property(e => e.First_Name).HasComment("First name of the employee. A not null column.");
            entity.Property(e => e.Hire_Date)
                .ValueGeneratedOnAdd()
                .HasComment("Date when the employee started on this job. A not null column.");
            entity.Property(e => e.Job_Id)
                .ValueGeneratedOnAdd()
                .HasComment("Current job of the employee; foreign key to job_id column of the\njobs table. A not null column.");
            entity.Property(e => e.Last_Name).HasComment("Last name of the employee. A not null column.");
            entity.Property(e => e.Manager_Id).HasComment("Manager id of the employee; has same domain as manager_id in\ndepartments table. Foreign key to employee_id column of employees table.\n(useful for reflexive joins and CONNECT BY query)");
            entity.Property(e => e.Phone_Number).HasComment("Phone number of the employee; includes country code and area code");
            entity.Property(e => e.Salary).HasComment("Monthly salary of the employee. Must be greater\nthan zero (enforced by constraint emp_salary_min)");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees).HasConstraintName("EMP_DEPT_FK");

            entity.HasOne(d => d.Job).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMP_JOB_FK");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager).HasConstraintName("EMP_MANAGER_FK");
        });

        modelBuilder.Entity<EmpDetailsView>(entity =>
        {
            entity.ToView("EMP_DETAILS_VIEW");

            entity.Property(e => e.Country_Id).IsFixedLength();
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Job_Id).HasName("JOB_ID_PK");

            entity.ToTable("JOBS", tb => tb.HasComment("jobs table with job titles and salary ranges.\nReferences with employees and job_history table."));

            entity.Property(e => e.Job_Id).HasComment("Primary key of jobs table.");
            entity.Property(e => e.Job_Title).HasComment("A not null column that shows job title, e.g. AD_VP, FI_ACCOUNTANT");
            entity.Property(e => e.Max_Salary).HasComment("Maximum salary for a job title");
            entity.Property(e => e.Min_Salary).HasComment("Minimum salary for a job title.");
        });

        modelBuilder.Entity<JobHistory>(entity =>
        {
            entity.HasKey(e => new { e.Employee_Id, e.Start_Date }).HasName("JHIST_EMP_ID_ST_DATE_PK");

            entity.ToTable("JOB_HISTORY", tb => tb.HasComment("Table that stores job history of the employees. If an employee\nchanges departments within the job or changes jobs within the department,\nnew rows get inserted into this table with old job information of the\nemployee. Contains a complex primary key: employee_id+start_date.\nReferences with jobs, employees, and departments tables."));

            entity.Property(e => e.Employee_Id).HasComment("A not null column in the complex primary key employee_id+start_date.\nForeign key to employee_id column of the employee table");
            entity.Property(e => e.Start_Date).HasComment("A not null column in the complex primary key employee_id+start_date.\nMust be less than the end_date of the job_history table. (enforced by\nconstraint jhist_date_interval)");
            entity.Property(e => e.Department_Id).HasComment("Department id in which the employee worked in the past; foreign key to deparment_id column in the departments table");
            entity.Property(e => e.End_Date).HasComment("Last day of the employee in this job role. A not null column. Must be\ngreater than the start_date of the job_history table.\n(enforced by constraint jhist_date_interval)");
            entity.Property(e => e.Job_Id).HasComment("Job role in which the employee worked in the past; foreign key to\njob_id column in the jobs table. A not null column.");

            entity.HasOne(d => d.Department).WithMany(p => p.Job_Histories).HasConstraintName("JHIST_DEPT_FK");

            entity.HasOne(d => d.Employee).WithMany(p => p.Job_Histories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("JHIST_EMP_FK");

            entity.HasOne(d => d.Job).WithMany(p => p.Job_Histories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("JHIST_JOB_FK");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Location_Id).HasName("LOC_ID_PK");

            entity.ToTable("LOCATIONS", tb => tb.HasComment("Locations table that contains specific address of a specific office,\nwarehouse, and/or production site of a company. Does not store addresses /\nlocations of customers. references with the departments and countries tables. "));

            entity.Property(e => e.Location_Id).HasComment("Primary key of locations table").HasDefaultValueSql("LOCATIONS_SEQ.NEXTVAL"); ;
            entity.Property(e => e.City).HasComment("A not null column that shows city where an office, warehouse, or\nproduction site of a company is located. ");
            entity.Property(e => e.Country_Id)
                .IsFixedLength()
                .HasComment("Country where an office, warehouse, or production site of a company is\nlocated. Foreign key to country_id column of the countries table.");
            entity.Property(e => e.Postal_Code).HasComment("Postal code of the location of an office, warehouse, or production site\nof a company. ");
            entity.Property(e => e.State_Province).HasComment("State or Province where an office, warehouse, or production site of a\ncompany is located.");
            entity.Property(e => e.Street_Address).HasComment("Street address of an office, warehouse, or production site of a company.\nContains building number and street name");

            entity.HasOne(d => d.Country).WithMany(p => p.Locations).HasConstraintName("LOC_C_ID_FK");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Region_Id).HasName("REG_ID_PK");

            entity.ToTable("REGIONS", tb => tb.HasComment("Regions table that contains region numbers and names. references with the Countries table."));

            entity.Property(e => e.Region_Id).HasComment("Primary key of regions table.");
            entity.Property(e => e.Region_Name).HasComment("Names of regions. Locations are in the countries of these regions.");
        });
        modelBuilder.HasSequence("DEPARTMENTS_SEQ").IncrementsBy(10);
        modelBuilder.HasSequence("EMPLOYEES_SEQ");
        modelBuilder.HasSequence("LOCATIONS_SEQ").IncrementsBy(100);

        modelBuilder.UseOracleUpperCaseNamingConvention();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
