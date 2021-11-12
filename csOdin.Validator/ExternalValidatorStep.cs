namespace csOdin.Validator
{
    public abstract class ExternalValidatorStep : ValidationStep
    {
        public override bool IsExternalValidationStep
        {
            get => true;
            protected set => base.IsExternalValidationStep = value;
        }
    }
}