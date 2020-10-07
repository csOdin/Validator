namespace csOdin.Validator
{
    using System.Collections.Generic;

    public class ValidationResult
    {
        public static bool DefaultConfigureAwait;
        public static string ErrorMessagesSeparator;

        public ValidationResult()
        {
            Errors = new List<string>();
            IsSuccess = true;
        }

        public int ErrorCount => Errors.Count;
        public List<string> Errors { get; private set; }
        public bool IsFailure => !IsSuccess;
        public bool IsSuccess { get; protected set; }

        public static ValidationResult Failure(string message)
        {
            var result = new ValidationResult
            {
                IsSuccess = false
            };
            result.Errors.Add(message);
            return result;
        }

        public static ValidationResult Failure(IEnumerable<string> messages)
        {
            var result = new ValidationResult
            {
                IsSuccess = false
            };
            result.Errors.AddRange(messages);
            return result;
        }

        public static ValidationResult Success()
        {
            var result = new ValidationResult
            {
                IsSuccess = true
            };
            return result;
        }

        public void Add(ValidationResult result)
        {
            if (result.IsSuccess)
            {
                return;
            }

            IsSuccess = false;
            Errors.AddRange(result.Errors);
        }

        public ValidationResult<T> ToResultWithValue<T>(T value) => IsFailure ?
                ValidationResult<T>.Failure(Errors) :
                ValidationResult<T>.Success(value);
    }

    public class ValidationResult<T> : ValidationResult
    {
        public T Value { get; private set; }

        public static ValidationResult<T> Failure(string message)
        {
            var result = new ValidationResult<T>
            {
                IsSuccess = false
            };
            result.Errors.Add(message);
            return result;
        }

        public static ValidationResult<T> Failure(IEnumerable<string> messages)
        {
            var result = new ValidationResult<T>
            {
                IsSuccess = false
            };
            result.Errors.AddRange(messages);
            return result;
        }

        public static ValidationResult<T> Success(T value)
        {
            var result = new ValidationResult<T>
            {
                IsSuccess = true,
                Value = value,
            };
            return result;
        }
    }
}