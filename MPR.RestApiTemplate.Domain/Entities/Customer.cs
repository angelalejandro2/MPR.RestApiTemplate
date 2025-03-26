using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPR.RestApiTemplate.Domain.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }

        [ForeignKey("CustomerTypeId")]
        public virtual CustomerType? CustomerType { get; set; }
    }
}
