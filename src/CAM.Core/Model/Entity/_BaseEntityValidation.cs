
namespace CAM.Core.Model.Entity
{
    using System.Collections.Generic;
    using Validation;

    public partial class _BaseEntityValidation
    {
        private readonly List<IValidationRule> _validationRules;
        private ValidationHandler _validationHandler;

        protected _BaseEntityValidation()
        {
            _validationRules = new List<IValidationRule>();
            _validationHandler = new ValidationHandler();
        }

        public override void Dispose()
        {
            base.Dispose();
            _validationRules.Clear();
        }

        /// <summary>
        /// 为实体对象添加一条验证规则
        /// </summary>
        /// <param name="rule">验证规则</param>
        public void addValidationRule(IValidationRule rule)
        {
            if (rule != null)
            {
                _validationRules.Add(rule);
            }
        }

        /// <summary>
        /// 对实体对象进行合法性验证，如果验证失败，将通过throw Exception进行抛出
        /// </summary>
        public void validate()
        {
            IValidationResultCollection results = getValidationResult();
            _validationHandler.handle(results);
        }

        private IValidationResultCollection getValidationResult()
        {
            IValidationResultCollection result = ValidationFactory.createValidationor().validate(this);
            foreach (var rule in _validationRules)
                result.addResult(rule.validate());
            return result;
        }



    }
}
