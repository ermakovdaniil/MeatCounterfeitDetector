using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.User
{
    public class DeleteUserDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "TypeId is required")]
        public Guid TypeId { get; set; }
    }
}
