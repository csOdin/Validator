namespace csOdin.Validator
{
    using System;
    using System.Threading.Tasks;

    public class ValidationStep<T>
    {
        public Func<T, Task<ValidationResult>> ValidateFunction { get; set; }

        internal bool ShouldBreakOnFailure { get; private set; } = false;

        public ValidationStep<T> BreakOnFailure()
        {
            ShouldBreakOnFailure = true;
            return this;
        }

        public ValidationStep<T> ContinueOnFailure()
        {
            ShouldBreakOnFailure = false;
            return this;
        }
    }
}