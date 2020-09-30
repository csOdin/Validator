namespace csOdin.Validator.Tests
{
    using System.Threading.Tasks;

    internal class DummyValidator : Validator<string>
    {
        private ValidationResult _result1;
        private ValidationResult _result2;
        private ValidationResult _result3;
        private ValidationResult _result4;
        private ValidationResult _result5;

        public static Validator<string> Create(
            ValidationResult result1,
            ValidationResult result2,
            ValidationResult result3,
            ValidationResult result4,
            ValidationResult result5
            )
        {
            var validator = new DummyValidator
            {
                _result1 = result1,
                _result2 = result2,
                _result3 = result3,
                _result4 = result4,
                _result5 = result5
            };

            return validator;
        }

        protected override void Setup(string command)
        {
            AddValidationStep(ValidationStep1);
            AddValidationStep(ValidationStep2);
            AddValidationStep(ValidationStep3);
            AddValidationStep(ValidationStep4);
            AddValidationStep(ValidationStep5);
        }

        private Task<ValidationResult> ValidationStep1(string arg) => Task.FromResult(_result1);

        private Task<ValidationResult> ValidationStep2(string arg) => Task.FromResult(_result2);

        private Task<ValidationResult> ValidationStep3(string arg) => Task.FromResult(_result3);

        private Task<ValidationResult> ValidationStep4(string arg) => Task.FromResult(_result4);

        private Task<ValidationResult> ValidationStep5(string arg) => Task.FromResult(_result5);
    }
}