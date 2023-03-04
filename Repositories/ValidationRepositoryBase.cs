using FluentValidation;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InnoGotchi_frontend.Repositories
{
    public abstract class ValidationRepositoryBase<T>:IValidationRepositoryBase<T> where T : class
    {
        private readonly IValidator<T> _validator;

        public ValidationRepositoryBase(IValidator<T> validator)
        {
            _validator = validator;
        }
        public async Task<bool> Validation(T dto, ModelStateDictionary modelState)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(modelState);

                return false;
            }

            return true;
        }

        public async Task<ValidationResult> AddError(T dto, string error, ModelStateDictionary modelstate)
        {
            modelstate.AddModelError(string.Empty, error);

            ValidationResult validationResult = await _validator.ValidateAsync(dto);

            validationResult.AddToModelState(modelstate);
            return validationResult;
        }
    }
}
