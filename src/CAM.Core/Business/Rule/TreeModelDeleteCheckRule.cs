
namespace CAM.Core.Business.Rule
{
    using System.ComponentModel.DataAnnotations;
    using CAM.Common.Data;
    using CAM.Core.Model.Validation;
    using CAM.Core.Model.Entity;
    using System.Text;
    using System.Linq;

    public class TreeModelDeleteCheckRule<TEntity> : BaseRule<TEntity>
        where TEntity : BaseEntityTree
    {
        public TreeModelDeleteCheckRule(IRepository<TEntity> res, TEntity checkObj)
            : base(res, checkObj)
        {

        }

        public override ValidationResult validate()
        {
            ValidationResult result = ValidationResult.Success;
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("with cte as(select Id, Tree_ParentId from {0} where Tree_ParentId = {1} and System_DeleteFlag=0 union all select a.Id, a.Tree_ParentId from {0} a inner join cte on a.Tree_ParentId = cte.Id and a.System_DeleteFlag=0)select COUNT(1) from cte;", tableName, _checkObj.Id);

            int childCount = _res.querySQLCommand<int>(sbSQL.ToString()).FirstOrDefault();
            if (childCount > 0)
            {
                result = createValidationResult("", "当前节点下存在子节点数据，无法被删除");
            }
            return result;
        }
    }
}
