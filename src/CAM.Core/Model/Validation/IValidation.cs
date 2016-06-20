
namespace CAM.Core.Model.Validation
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public interface IValidationResultCollection : IEnumerable<ValidationResult>
    {
        /// <summary>
        /// 验证结果：是否有效
        /// </summary>
        bool isValid { get; }
        /// <summary>
        /// 添加验证结果
        /// </summary>
        /// <param name="result">验证结果</param>
        void addResult(ValidationResult result);
    }

    public interface IValidation
    {
        /// <summary>
        /// 验证实例的合法性
        /// </summary>
        /// <param name="target">验证对象</param>
        /// <returns></returns>
        IValidationResultCollection validate(object target);
    }

    public interface IValidationRule
    {
        /// <summary>
        /// 验证
        /// </summary>
        ValidationResult validate();
    }
}
