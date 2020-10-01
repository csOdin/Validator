namespace csOdin.Validator.Tests.ValidationResults
{
    using csOdin.Validator;
    using FluentAssertions;
    using Xunit;

    public class CreateTests
    {
        [Theory]
        [InlineData("message 1")]
        [InlineData("message 2")]
        [InlineData("message 3")]
        [InlineData("message 4")]
        [InlineData("message 5")]
        public void FailureShouldReturnFailureResultWithMessage(string message)
        {
            var result = ValidationResult.Failure(message);
            result.Should().BeOfType(typeof(ValidationResult));
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.ErrorCount.Should().Be(1);
            result.Errors.Should().Contain(message);
        }

        [Fact]
        public void SuccessShouldReturnSuccessResult()
        {
            var result = ValidationResult.Success();
            result.Should().BeOfType(typeof(ValidationResult));
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.ErrorCount.Should().Be(0);
        }

        [Fact]
        public void ValidatorResultFromCombinedShouldReturnFalure()
        {
            var message1 = "message 1";
            var message4 = "message 4";

            var result1 = ValidationResult.Failure(message1);
            var result2 = ValidationResult.Success();
            var result3 = ValidationResult.Success();
            var result4 = ValidationResult.Failure(message4);

            var result = new ValidationResult();
            result.Add(result1);
            result.Add(result2);
            result.Add(result3);
            result.Add(result4);

            result.Should().BeOfType(typeof(ValidationResult));
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.ErrorCount.Should().Be(2);
            result.Errors.Should().Contain(message1);
            result.Errors.Should().Contain(message4);
        }

        [Fact]
        public void ValidatorResultFromMultipleFailureShouldReturnFalure()
        {
            var message1 = "message 1";
            var message2 = "message 2";
            var message3 = "message 3";
            var message4 = "message 4";

            var result1 = ValidationResult.Failure(message1);
            var result2 = ValidationResult.Failure(message2);
            var result3 = ValidationResult.Failure(message3);
            var result4 = ValidationResult.Failure(message4);

            var result = new ValidationResult();
            result.Add(result1);
            result.Add(result2);
            result.Add(result3);
            result.Add(result4);

            result.Should().BeOfType(typeof(ValidationResult));
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.ErrorCount.Should().Be(4);
            result.Errors.Should().Contain(message1);
            result.Errors.Should().Contain(message2);
            result.Errors.Should().Contain(message3);
            result.Errors.Should().Contain(message4);
        }

        [Fact]
        public void ValidatorResultFromMultipleSuccessShouldReturnSuccess()
        {
            var result1 = ValidationResult.Success();
            var result2 = ValidationResult.Success();
            var result3 = ValidationResult.Success();
            var result4 = ValidationResult.Success();

            var result = new ValidationResult();
            result.Add(result1);
            result.Add(result2);
            result.Add(result3);
            result.Add(result4);

            result.Should().BeOfType(typeof(ValidationResult));
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.ErrorCount.Should().Be(0);
        }
    }
}