namespace csOdin.Validator.Tests.InClassValidation
{
    using System;

    public class DummyClassWithValidator
    {
        private DummyClassWithValidator()
        {
        }

        public string Name { get; private set; }

        public int YearOfBirth { get; private set; }

        public static ValidationResult<DummyClassWithValidator> Create(DummyCommand command)
        {
            var entity = new DummyClassWithValidator();

            var validator = new Validator<DummyCommand>();
            validator.AddValidationStep(entity.SetName);
            validator.AddValidationStep(entity.SetYearOfBirth);

            var result = validator.Validate(command);

            return result.Result.ToResultWithValue(entity);
        }

        private ValidationResult SetName(DummyCommand arg)
        {
            if (string.IsNullOrEmpty(arg.Name))
            {
                return ValidationResult.Failure("Empty name");
            }
            if (arg.Name.Length < 3)
            {
                return ValidationResult.Failure("Name is too short");
            }
            Name = arg.Name;
            return ValidationResult.Success();
        }

        private ValidationResult SetYearOfBirth(DummyCommand arg)
        {
            if (arg.YearOfBirth > DateTime.Now.Year - 10)
            {
                return ValidationResult.Failure("You are too young");
            }
            YearOfBirth = arg.YearOfBirth;
            return ValidationResult.Success();
        }
    }
}