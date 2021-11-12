namespace csOdin.Validator
{
    using System;
    using System.Threading.Tasks;

    public class InternalValidationStep<T> : ValidationStep
    {
        internal Func<T, Task<ValidationResult>> AsyncValidateFunction { get; private set; }

        internal Func<T, ValidationResult> ValidateFunction { get; private set; }

        public static InternalValidationStep<T> Create(Func<T, Task<ValidationResult>> validationFunction) => new InternalValidationStep<T>()
        {
            IsAsync = true,
            AsyncValidateFunction = validationFunction,
        };

        public static InternalValidationStep<T> Create(Func<T, ValidationResult> validationFunction) => new InternalValidationStep<T>()
        {
            IsAsync = false,
            ValidateFunction = validationFunction,
        };
    }
}