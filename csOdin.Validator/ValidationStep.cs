namespace csOdin.Validator
{
    public class ValidationStep : IValidationStep
    {
        public bool IsAsync { get; protected set; } = false;

        public virtual bool IsExternalValidationStep { get; protected set; } = false;
        internal bool ShouldBreakOnFailure { get; private set; } = false;

        public void BreakOnFailure() => ShouldBreakOnFailure = true;
    }
}