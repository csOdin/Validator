namespace csOdin.Validator
{
    public abstract class ExternalValidationStep : ValidationStep
    {
        public override bool IsExternalValidationStep
        {
            get => true;
            protected set => base.IsExternalValidationStep = value;
        }
    }
}