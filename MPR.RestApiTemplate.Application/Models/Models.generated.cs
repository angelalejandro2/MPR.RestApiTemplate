using System.ComponentModel.DataAnnotations;

namespace MPR.RestApiTemplate.Application.Models
{
    public class CustomerModel
    {
        
        [Key]
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string? LastName { get; set; }
        
        public CustomerTypeModel? CustomerType { get; set; }
    }
    public class CustomerTypeModel
    {
        
        [Key]
        
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}