using InnoGotchi_backend.Models;

namespace InnoGotchi_frontend.Services
{
    public interface IValidationController
    {
        public Task<bool> Validation(UserDto userDto);
    }
}
