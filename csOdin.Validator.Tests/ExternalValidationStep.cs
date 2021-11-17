namespace csOdin.Validator.Tests
{
    using System;

    internal class ExternalValidationStep : Validator.ExternalValidationStep, IExternalValidationStep
    {
        public ExternalValidationStep(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public ValidationResult Validate() =>
            Id == null
                ? ValidationResult.Failure("Null id")
                : Id == Guid.Empty
                    ? ValidationResult.Failure("Empty id")
                    : ValidationResult.Success();
    }
}