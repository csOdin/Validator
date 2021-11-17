namespace csOdin.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class Validator<T> : IValidator<T>
    {
        private bool _breakOnAnyFailure { get; set; } = false;

        private List<ValidationStep> _validationSteps { get; set; } = new List<ValidationStep>();

        public ValidationResult Validate(T command)
        {
            Setup(command);

            var results = new ValidationResult();

            foreach (var step in _validationSteps)
            {
                var result = step.IsExternalValidationStep
                    ? ValidateExternalValidationStep(command, step)
                    : ValidateInternalValidationStep(command, step);

                results.Add(result);
                if (result.IsFailure)
                {
                    if (_breakOnAnyFailure || step.ShouldBreakOnFailure)
                    {
                        break;
                    }
                }
            }

            return results;
        }

        protected ValidationStep AddValidationStep(Func<T, Task<ValidationResult>> validateFunction)
        {
            if (validateFunction == null)
            {
                return null;
            }

            var newStep = InternalValidationStep<T>.Create(validateFunction);
            _validationSteps.Add(newStep);
            return newStep;
        }

        protected ValidationStep AddValidationStep(Func<T, ValidationResult> validateFunction)
        {
            if (validateFunction == null)
            {
                return null;
            }

            var newStep = InternalValidationStep<T>.Create(validateFunction);

            _validationSteps.Add(newStep);
            return newStep;
        }

        protected ValidationStep AddValidationStep(InternalValidationStep<T> validationStep)
        {
            if (validationStep == null)
            {
                return null;
            }
            _validationSteps.Add(validationStep);
            return validationStep;
        }

        protected ValidationStep AddValidatonStep<TValidationStep>(params object[] ctorParams)
        {
            var validationStep = (ValidationStep)Activator.CreateInstance(typeof(TValidationStep), ctorParams);
            _validationSteps.Add(validationStep);
            return validationStep;
        }

        protected void BreakOnAnyFailure() => _breakOnAnyFailure = true;

        protected void BreakOnLastFailure()
        {
            if (!_validationSteps.Any())
            {
                return;
            }

            _validationSteps.Last().BreakOnFailure();
        }

        protected void ClearValidationSteps() => _validationSteps.Clear();

        protected virtual void Setup(T command)
        {
            return;
        }

        private ValidationResult ValidateExternalValidationStep(T command, ValidationStep step)
        {
            var validationStep = step as IExternalValidationStep;
            var result = step.IsAsync
                ? Task.Run(() => validationStep.Validate(), new CancellationToken()).GetAwaiter().GetResult()
                : validationStep.Validate();
            return result;
        }

        private ValidationResult ValidateInternalValidationStep(T command, ValidationStep step)
        {
            var validationStep = step as InternalValidationStep<T>;
            var result = step.IsAsync
                ? Task.Run(() => validationStep.AsyncValidateFunction(command), new CancellationToken()).GetAwaiter().GetResult()
                : validationStep.ValidateFunction(command);
            return result;
        }
    }
}