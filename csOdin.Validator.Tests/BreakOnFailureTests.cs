namespace csOdin.Validator.Tests
{
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class BreakOnFailureTests
    {
        private const string _message1 = "message 1";
        private const string _message2 = "message 2";
        private const string _message3 = "message 3";
        private const string _message4 = "message 4";

        private InternalValidationStep<string> _validationStepFailure2;
        private InternalValidationStep<string> _validationStepFailure3;
        private InternalValidationStep<string> _validationStepFailure4;
        private InternalValidationStep<string> _validationStepSuccess1;

        [Fact]
        public void ShouldBreakOnFailure()
        {
            ResetValidationSteps();

            _validationStepFailure3.BreakOnFailure();

            var validator = DummyValidator<string>
                .Create(
                breakOnAnyFailure: false,
                _validationStepSuccess1,
                _validationStepFailure2,
                _validationStepFailure3,
                _validationStepFailure4);

            var result = validator.Validate("command");
            result.Should().BeOfType(typeof(ValidationResult));
            result.IsFailure.Should().BeTrue();
            result.ErrorCount.Should().Be(2);
            result.Errors.Should().Contain(_message2);
            result.Errors.Should().Contain(_message3);
        }

        [Fact]
        public void ShouldBreakOnFirstFailure()
        {
            ResetValidationSteps();
            var validator = DummyValidator<string>
                .Create(
                breakOnAnyFailure: true,
                _validationStepSuccess1,
                _validationStepFailure2,
                _validationStepFailure3,
                _validationStepFailure4);

            var result = validator.Validate("command");
            result.Should().BeOfType(typeof(ValidationResult));
            result.IsFailure.Should().BeTrue();
            result.ErrorCount.Should().Be(1);
            result.Errors.Should().Contain(_message2);
        }

        [Fact]
        public void ShouldNotBreakOnFailure()
        {
            ResetValidationSteps();

            var validator = DummyValidator<string>
                .Create(
                breakOnAnyFailure: false,
                _validationStepSuccess1,
                _validationStepFailure2,
                _validationStepFailure3,
                _validationStepFailure4);

            var result = validator.Validate("command");
            result.Should().BeOfType(typeof(ValidationResult));
            result.IsFailure.Should().BeTrue();
            result.ErrorCount.Should().Be(3);
            result.Errors.Should().Contain(_message2);
            result.Errors.Should().Contain(_message3);
            result.Errors.Should().Contain(_message4);
        }

        private void ResetValidationSteps()
        {
            Func<string, Task<ValidationResult>> successValidationFunction1 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Success());
            Func<string, Task<ValidationResult>> successValidationFunction2 = (string a) => Task.FromResult(ValidationResult.Success());
            Func<string, Task<ValidationResult>> successValidationFunction3 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Success());
            Func<string, Task<ValidationResult>> successValidationFunction4 = (string a) => Task.FromResult(ValidationResult.Success());

            Func<string, Task<ValidationResult>> failureValidationFunction1 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Failure(_message1));
            Func<string, Task<ValidationResult>> failureValidationFunction2 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Failure(_message2));
            Func<string, Task<ValidationResult>> failureValidationFunction3 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Failure(_message3));
            Func<string, Task<ValidationResult>> failureValidationFunction4 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Failure(_message4));

            _validationStepSuccess1 = InternalValidationStep<string>.Create(successValidationFunction1);
            _validationStepFailure2 = InternalValidationStep<string>.Create(failureValidationFunction2);
            _validationStepFailure3 = InternalValidationStep<string>.Create(failureValidationFunction3);
            _validationStepFailure4 = InternalValidationStep<string>.Create(failureValidationFunction4);
        }
    }
}