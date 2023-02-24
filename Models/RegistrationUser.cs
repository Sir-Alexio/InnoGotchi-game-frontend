using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_frontend.Models
{
    public class RegistrationUser
    {
        public UserDto Dto { get; set; }
        public IFormFile Image { get; set; }
    }
}
