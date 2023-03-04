using FluentValidation;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Repositories;
using InnoGotchi_backend.Services;
using InnoGotchi_frontend.Services;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_frontend.Repositories
{
    public class ValidationManager : IValidationManager
    {
        private IFarmValidation _farmValidator;
        private readonly IValidator<FarmDto> _validator;
        public ValidationManager(IValidator<FarmDto> validator)
        {
            _validator = validator;
        }

        public IFarmValidation FarmValidator
        {
            get
            {
                if (_farmValidator == null)
                {
                    _farmValidator = new FarmValidationRepository(_validator);
                }
                return _farmValidator;
            }

        }
    }
}
