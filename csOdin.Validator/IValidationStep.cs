namespace csOdin.Validator
{
    public interface IValidationStep
    {
        public bool IsAsync { get; }
        public bool IsExternalValidationStep { get; }
    }
}