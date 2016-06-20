
namespace CAM.Core.Business.Rule
{
    using CAM.Core.Model.Validation;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CAM.Common.Data;

    public abstract class BaseRule<TEntity> : IValidationRule
        where TEntity : class
    {

        protected IRepository<TEntity> _res;
        protected TEntity _checkObj;

        public BaseRule(IRepository<TEntity> res, TEntity checkObj)
        {
            _res = res;
            _checkObj = checkObj;
        }

        //在子类中重写这个方法，实现具体的验证过程
        public abstract ValidationResult validate();

        /// <summary>
        /// 生成一个验证失败的结果
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        protected ValidationResult createValidationResult(string Key, string ErrorMessage)
        {
            List<string> MemberNames = new List<string>();
            MemberNames.Add(Key);
            ValidationResult result = new ValidationResult(ErrorMessage, MemberNames);
            return result;
        }


    }
}
