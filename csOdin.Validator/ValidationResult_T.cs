namespace csOdin.Validator
{
    using System.Collections.Generic;

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