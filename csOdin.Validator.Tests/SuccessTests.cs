namespace csOdin.Validator.Tests
{
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class SuccessTests
    {
        private InternalValidationStep<string> _validationStepSuccess1;
        private InternalValidationStep<string> _validationStepSuccess2;
        private InternalValidationStep<string> _validationStepSuccess3;
        private InternalValidationStep<string> _validationStepSuccess4;

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

            var result = validator.Validate("command");
            result.Should().BeOfType(typeof(ValidationResult));
            result.IsSuccess.Should().BeTrue();
        }

        private void ResetValidationSteps()
        {
            Func<string, Task<ValidationResult>> successValidationFunction1 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Success());
            Func<string, Task<ValidationResult>> successValidationFunction2 = (string a) => Task.FromResult(ValidationResult.Success());
            Func<string, Task<ValidationResult>> successValidationFunction3 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Success());
            Func<string, Task<ValidationResult>> successValidationFunction4 = (string a) => Task.FromResult(ValidationResult.Success());

            _validationStepSuccess1 = InternalValidationStep<string>.Create(successValidationFunction1);
            _validationStepSuccess2 = InternalValidationStep<string>.Create(successValidationFunction2);
            _validationStepSuccess3 = InternalValidationStep<string>.Create(successValidationFunction3);
            _validationStepSuccess4 = InternalValidationStep<string>.Create(successValidationFunction4);
        }
    }
}