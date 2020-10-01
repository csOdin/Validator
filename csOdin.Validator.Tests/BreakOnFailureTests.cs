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

        private ValidationStep<string> _validationStepFailure2;
        private ValidationStep<string> _validationStepFailure3;
        private ValidationStep<string> _validationStepFailure4;
        private ValidationStep<string> _validationStepSuccess1;

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

            var result = validator.Validate("command").Result;
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

            var result = validator.Validate("command").Result;
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

            var result = validator.Validate("command").Result;
            result.Should().BeOfType(typeof(ValidationResult));
            result.IsFailure.Should().BeTrue();
            result.ErrorCount.Should().Be(3);
            result.Errors.Should().Contain(_message2);
            result.Errors.Should().Contain(_message3);
            result.Errors.Should().Contain(_message4);
        }

        private void ResetValidationSteps()
        {
            Func<string, Task<ValidationResult>> s1 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Success());
            Func<string, Task<ValidationResult>> s2 = (string a) => Task.FromResult(ValidationResult.Success());
            Func<string, Task<ValidationResult>> s3 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Success());
            Func<string, Task<ValidationResult>> s4 = (string a) => Task.FromResult(ValidationResult.Success());

            Func<string, Task<ValidationResult>> f1 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Failure(_message1));
            Func<string, Task<ValidationResult>> f2 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Failure(_message2));
            Func<string, Task<ValidationResult>> f3 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Failure(_message3));
            Func<string, Task<ValidationResult>> f4 = (string a) => Task.FromResult<ValidationResult>(ValidationResult.Failure(_message4));

            _validationStepSuccess1 = new ValidationStep<string>() { ValidateFunction = s1 };
            _validationStepFailure2 = new ValidationStep<string>() { ValidateFunction = f2 };
            _validationStepFailure3 = new ValidationStep<string>() { ValidateFunction = f3 };
            _validationStepFailure4 = new ValidationStep<string>() { ValidateFunction = f4 };
        }
    }
}