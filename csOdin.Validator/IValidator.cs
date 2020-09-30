namespace csOdin.Validator
{
    using System.Threading.Tasks;

    public interface IValidator<T>
    {
        Task<ValidationResult> Validate(T command);
    }
}