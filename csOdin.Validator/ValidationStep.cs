namespace csOdin.Validator
{
    using System;
    using System.Threading.Tasks;

    public class ValidationStep<T>
    {
        public bool IsAsync { get; private set; }
        internal Func<T, Task<ValidationResult>> AsyncValidateFunction { get; private set; }
        internal bool ShouldBreakOnFailure { get; private set; } = false;
        internal Func<T, ValidationResult> ValidateFunction { get; private set; }

        public static ValidationStep<T> Create(Func<T, Task<ValidationResult>> validationFunction) => new ValidationStep<T>()
        {
            IsAsync = true,
            AsyncValidateFunction = validationFunction,
        };

        public static ValidationStep<T> Create(Func<T, ValidationResult> validationFunction) => new ValidationStep<T>()
        {
            IsAsync = false,
            ValidateFunction = validationFunction,
        };

        public ValidationStep<T> BreakOnFailure()
        {
            ShouldBreakOnFailure = true;
            return this;
        }
    }
}