// Autogenerated Code - Do not modify
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPR.RestApiTemplate.Application.DTOs
{
    public class CustomerDemographicsDto
    {
        [Key]
        public string CustomerTypeId { get; set; }
        public string? CustomerDesc { get; set; }
        public ICollection<CustomersDto> Customer { get; set; }
    }
}
