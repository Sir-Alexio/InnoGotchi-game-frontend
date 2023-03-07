using FluentValidation;
using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using InnoGotchi_frontend.Services.Abstract;

namespace InnoGotchi_frontend.Services
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
