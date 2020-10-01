namespace csOdin.Validator.Tests
{
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class SuccessTests
    {
        private ValidationStep<string> _validationStepSuccess1;
        private ValidationStep<string> _validationStepSuccess2;
        private ValidationStep<string> _validationStepSuccess3;
        private ValidationStep<string> _validationStepSuccess4;

        [Fact]
        public void ShouldReturnSuccess()
        {
            ResetValidationSteps();

            var validator = DummyValidator<string>
                .Create(
                breakOnAnyFailure: false,
                _validationStepSuccess1,
                _validationStepSuccess2,
                _validationStepSuccess3,
                _validationStepSuccess4);

            var result = validator.Validate("command").Result;
            result.Should().BeOfType(typeof(ValidationResult));
            result.IsSuccess.Should().BeTrue();
        }

        private void ResetValidationSteps()
        {
            Func<string, Task<ValidationResult>> s1 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Success());
            Func<string, Task<ValidationResult>> s2 = (string a) => Task.FromResult(ValidationResult.Success());
            Func<string, Task<ValidationResult>> s3 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Success());
            Func<string, Task<ValidationResult>> s4 = (string a) => Task.FromResult(ValidationResult.Success());

            _validationStepSuccess1 = new ValidationStep<string>() { ValidateFunction = s1 };
            _validationStepSuccess2 = new ValidationStep<string>() { ValidateFunction = s2 };
            _validationStepSuccess3 = new ValidationStep<string>() { ValidateFunction = s3 };
            _validationStepSuccess4 = new ValidationStep<string>() { ValidateFunction = s4 };
        }
    }
}