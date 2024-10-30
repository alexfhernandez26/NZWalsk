using System.ComponentModel.DataAnnotations;

namespace NZWalskApi.Models.DTO
{
    public class AddRegionDomainDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="El valor debe ser minimo de tres caracteres")]
        public string Code { get; set; }

        [MaxLength(50,ErrorMessage ="El valor debe ser maximo de 5 caracteres")]
        public string Name { get; set; }
        public string RegionImageUrl { get; set; }
    }
}
