using FluentValidation.Results;
using InnoGotchi_backend.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InnoGotchi_frontend.Services.Abstract
{
    public interface IValidationRepositoryBase<T>
    {
        public Task<bool> Validation(T userDto, ModelStateDictionary modelState);
        public Task<ValidationResult> AddError(T userDto, string error, ModelStateDictionary modelstate);
    }
}
