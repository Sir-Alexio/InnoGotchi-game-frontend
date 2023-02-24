using InnoGotchi_backend.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InnoGotchi_frontend.Services
{
    public interface IValidationService
    {
        public Task<bool> Validation(UserDto userDto, ModelStateDictionary modelState);
        public Task AddError(UserDto userDto, string error, ModelStateDictionary modelstate);
    }
}
