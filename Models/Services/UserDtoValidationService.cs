using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using InnoGotchi_frontend.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_frontend.Models.Services
{
    public class UserDtoValidationService : IValidationService
    {
        private readonly IValidator<UserDto> _userValidator;


        public UserDtoValidationService(IValidator<UserDto> validator)//, IValidator<ChangePasswordModel> changePasswordValidator)
        {
            _userValidator = validator;
            //_changePasswordValidator = changePasswordValidator;
        }
        public async Task<bool> Validation(UserDto userDto, ModelStateDictionary modelState)
        {
            ValidationResult validationResult = await _userValidator.ValidateAsync(userDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(modelState);

                return false;
            }

            return true;
        }


        public async Task AddError(UserDto userDto, string error, ModelStateDictionary modelstate)
        {
            modelstate.AddModelError(string.Empty, error);

            ValidationResult validationResult = await _userValidator.ValidateAsync(userDto);

            validationResult.AddToModelState(modelstate);
        }
    }
}
