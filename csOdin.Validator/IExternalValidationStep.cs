namespace csOdin.Validator
{
    public interface IExternalValidationStep : IValidationStep
    {
        ValidationResult Validate();
    }
}