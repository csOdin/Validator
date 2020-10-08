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

        private List<ValidationStep<T>> _validationSteps { get; set; } = new List<ValidationStep<T>>();

        public ValidationStep<T> AddValidationStep(Func<T, Task<ValidationResult>> validateFunction)
        {
            if (validateFunction == null)
            {
                return null;
            }

            var newStep = ValidationStep<T>.Create(validateFunction);
            _validationSteps.Add(newStep);
            return newStep;
        }

        public ValidationStep<T> AddValidationStep(Func<T, ValidationResult> validateFunction)
        {
            if (validateFunction == null)
            {
                return null;
            }

            var newStep = ValidationStep<T>.Create(validateFunction);

            _validationSteps.Add(newStep);
            return newStep;
        }

        public ValidationStep<T> AddValidationStep(ValidationStep<T> validationStep)
        {
            if (validationStep == null)
            {
                return null;
            }
            _validationSteps.Add(validationStep);
            return validationStep;
        }

        public ValidationResult Validate(T command)
        {
            Setup(command);

            var results = new ValidationResult();

            foreach (var step in _validationSteps)
            {
                ValidationResult result;
                if (step.IsAsync)
                {
                    result = Task.Run(() => step.AsyncValidateFunction(command), new CancellationToken()).GetAwaiter().GetResult();
                }
                else
                {
                    result = step.ValidateFunction(command);
                }
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
    }
}