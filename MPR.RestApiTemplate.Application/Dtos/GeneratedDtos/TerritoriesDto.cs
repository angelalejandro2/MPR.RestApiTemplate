// Autogenerated Code - Do not modify
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPR.RestApiTemplate.Application.DTOs
{
    public class TerritoriesDto
    {
        [Key]
        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }
        public RegionDto Region { get; set; }
        public ICollection<EmployeesDto> Employee { get; set; }
    }
}
