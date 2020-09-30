namespace csOdin.Validator
{
    using System.Threading.Tasks;

    public interface IValidator<T>
    {
        Task<ValidationResults> Validate(T command);
    }
}