using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InnoGotchi_frontend.Services
{
    public interface IPasswordValidationService
    {
        public Task<bool> Validation(ChangePasswordModel model, ModelStateDictionary modelState);
    }
}
