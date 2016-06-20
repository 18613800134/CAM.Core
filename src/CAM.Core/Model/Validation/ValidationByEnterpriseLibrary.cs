
namespace CAM.Core.Model.Validation
{
    using System.Collections.Generic;
    using Microsoft.Practices.EnterpriseLibrary.Validation;

    public class ValidationByEnterpriseLibrary : IValidation
    {

        /// <summary>
        /// 验证实例的合法性
        /// </summary>
        /// <param name="target">验证对象</param>
        /// <returns></returns>
        public IValidationResultCollection validate(object target)
        {
            Validator validator = Microsoft.Practices.EnterpriseLibrary.Validation.ValidationFactory.CreateValidator(target.GetType());
            ValidationResults results = validator.Validate(target);
            return GetResult(results);
        }

        private IValidationResultCollection GetResult(IEnumerable<ValidationResult> results)
        {
            IValidationResultCollection result = new ValidationResultCollection();
            foreach (var item in results)
            {
                List<string> MemberNames = new List<string>();
                MemberNames.Add(item.Key);
                result.addResult(new System.ComponentModel.DataAnnotations.ValidationResult(item.Message, MemberNames));
            }
            return result;
        }
    }

}
