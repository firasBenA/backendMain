using System.ComponentModel.DataAnnotations;

namespace TestApi.Models.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } 
      
    }
}
