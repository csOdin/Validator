namespace csOdin.Validator.Tests.InClassValidation
{
    using FluentAssertions;
    using Xunit;

    public class InClassTests
    {
        [Theory]
        [InlineData("", 10)]
        [InlineData("a", 10)]
        [InlineData("aa", 10)]
        [InlineData("aaaa", 2020)]
        public void ShouldReturnFailure(string name, int yearOfBirth)
        {
            var cmd = new DummyCommand
            {
                Name = name,
                YearOfBirth = yearOfBirth,
            };

            var createResult = DummyClassWithValidator.Create(cmd);

            createResult.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnSuccess()
        {
            var cmd = new DummyCommand
            {
                Name = "Odin",
                YearOfBirth = 1,
            };

            var createResult = DummyClassWithValidator.Create(cmd);

            createResult.IsSuccess.Should().BeTrue();
            createResult.Value.Should().BeOfType(typeof(DummyClassWithValidator));
            createResult.Value.Name.Should().Be(cmd.Name);
            createResult.Value.YearOfBirth.Should().Be(cmd.YearOfBirth);
        }
    }
}