namespace csOdin.Validator
{
    using System.Collections.Generic;

    public class ValidationResults
    {
        public static bool DefaultConfigureAwait;
        public static string ErrorMessagesSeparator;

        public ValidationResults()
        {
            Errors = new List<string>();
            IsSuccess = true;
        }

        public int ErrorCount => Errors.Count;
        public List<string> Errors { get; private set; }
        public bool IsFailure => !IsSuccess;
        public bool IsSuccess { get; private set; }

        public static ValidationResults Failure(string message)
        {
            var result = new ValidationResults
            {
                IsSuccess = false
            };
            result.Errors.Add(message);
            return result;
        }

        public static ValidationResults Success()
        {
            var result = new ValidationResults
            {
                IsSuccess = true
            };
            return result;
        }

        public void Add(ValidationResults result)
        {
            if (result.IsSuccess)
            {
                return;
            }

            IsSuccess = false;
            Errors.AddRange(result.Errors);
        }
    }
}