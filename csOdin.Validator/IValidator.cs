namespace csOdin.Validator
{
    using System.Threading.Tasks;

    public interface IValidator<T>
    {
        ValidationResult Validate(T command);
    }
}