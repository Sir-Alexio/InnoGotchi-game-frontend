using FluentValidation;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InnoGotchi_frontend.Models.Services
{
    public class PasswordValidationService : IPasswordValidationService
    {
        private readonly IValidator<ChangePasswordModel> _changePasswordValidator;
        public PasswordValidationService(IValidator<ChangePasswordModel> changePasswordValidator)
        {
            _changePasswordValidator = changePasswordValidator;
        }

        public async Task<bool> Validation(ChangePasswordModel model, ModelStateDictionary modelState)
        {
            ValidationResult validationResult = await _changePasswordValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(modelState);

                return false;
            }

            return true;
        }
    }
}
