using System;

namespace csOdin.Validator.Tests
{
    internal class DummyValidator<T> : Validator<T>
    {
        public static Validator<T> Create(bool breakOnAnyFailure, params InternalValidationStep<T>[] validationSteps)
        {
            var validator = new DummyValidator<T>();

            if (breakOnAnyFailure)
            {
                validator.BreakOnAnyFailure();
            }

            foreach (var validationStep in validationSteps)
            {
                validator.AddValidationStep(validationStep);
            }

            validator.AddValidatonStep<ExternalValidationStep>(Guid.NewGuid());

            return validator;
        }

        protected override void Setup(T command)
        {
        }
    }
}