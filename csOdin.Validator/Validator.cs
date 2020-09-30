namespace csOdin.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Validator<T> : IValidator<T>
    {
        private bool _breakOnAnyFailure { get; set; } = false;

        private List<ValidationStep<T>> _validationSteps { get; set; } = new List<ValidationStep<T>>();

        public async Task<ValidationResults> Validate(T command)
        {
            Setup(command);

            var results = new ValidationResults();

            foreach (var step in _validationSteps)
            {
                var result = await step.ValidateFunction(command);
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

        protected ValidationStep<T> AddValidationStep(Func<T, Task<ValidationResults>> validateFunction)
        {
            var newStep = new ValidationStep<T>
            {
                ValidateFunction = validateFunction,
            };

            _validationSteps.Add(newStep);
            return newStep;
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

        protected abstract void Setup(T command);
    }
}