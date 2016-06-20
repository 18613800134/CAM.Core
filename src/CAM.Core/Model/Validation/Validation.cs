
namespace CAM.Core.Model.Validation
{
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CAM.Common.Error;

    public class ValidationResultCollection :
        IValidationResultCollection,
        IEnumerable<ValidationResult>
    {
        private readonly List<ValidationResult> _results;

        public ValidationResultCollection()
        {
            _results = new List<ValidationResult>();
        }

        public bool isValid
        {
            get { return _results.Count == 0; }
        }

        public void addResult(ValidationResult result)
        {
            if (result == null)
            {
                return;
            }
            _results.Add(result);
        }


        public IEnumerator<ValidationResult> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _results.GetEnumerator();
        }
    }


    public class ValidationHandler
    {

        public void handle(IValidationResultCollection results)
        {
            if (!results.isValid)
            {
                
                Exception ex = new Exception("数据验证未通过");
                foreach (ValidationResult item in results)
                {
                    string MemberName = item.MemberNames.FirstOrDefault();
                    if (!ex.Data.Contains(MemberName))
                    {
                        ex.Data.Add(MemberName, item.ErrorMessage);
                    }
                }

                ErrorHandler.ThrowException(ex);
            }
        }
    }
}
