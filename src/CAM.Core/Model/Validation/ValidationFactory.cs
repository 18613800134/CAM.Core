
namespace CAM.Core.Model.Validation
{
    public class ValidationFactory
    {
        public static IValidation createValidationor()
        {
            return new ValidationByEnterpriseLibrary();
        }
    }
}
