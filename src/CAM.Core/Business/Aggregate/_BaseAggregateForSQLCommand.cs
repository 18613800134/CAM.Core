
namespace CAM.Core.Business.Aggregate
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Reflection;
    using CAM.Core.Model.Entity;
    using CAM.Core.Model.Filter;
    using CAM.Common.Data;
    using CAM.Common.DataProtocol;
    using System.Linq;

    public abstract partial class _BaseAggregateForSQLCommand
    {

        #region 为标准SQL SELECT语句生成分页读取的包装代码

        protected string createPageSQL(string sql, BaseFilter filter)
        {
            return createPageSQLFor2008(sql, filter);
        }

        private string createPageSQLFor2008(string sql, BaseFilter filter)
        {
            //生成orderby字符串
            StringBuilder sbOrderBy = new StringBuilder("");
            long orderCount = filter.OrderName.Length;
            for (long i = 0; i < orderCount; i++)
            {
                sbOrderBy.AppendFormat(" {0} {1},", filter.OrderName[i], filter.IsAsc[i] ? "asc" : "desc");
            }
            string orderBy = sbOrderBy.ToString();
            //去掉orderBy连接字符串中的最后一个逗号
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = orderBy.Substring(0, orderBy.Length - 1);
            }

            //如果filter中指定的页面大小为0，表示不用分页，此时返回带order的非分页SQL代码
            if (filter.PageInfo.PageSize == 0)
            {
                string sqlNotWithPage = string.Format("{0} order by {1}", sql, orderBy);
                return sqlNotWithPage;
            }

            //计算分页的起始条数和结束条数
            int beginRow = 0, endRow = filter.PageInfo.PageSize + 1;
            if (filter.PageInfo.PageIndex > 1)
            {
                beginRow = (filter.PageInfo.PageSize * (filter.PageInfo.PageIndex - 1));
                endRow = beginRow + filter.PageInfo.PageSize + 1;
            }

            string sqlInSide = string.Format("SELECT *, ROW_NUMBER() OVER(ORDER BY {1}) AS ROWNUMBER FROM ({0}) AS A", sql, orderBy);
            string sqlPage = string.Format("SELECT * FROM ({0}) AS B WHERE B.ROWNUMBER > {1} AND B.ROWNUMBER < {2} ", sqlInSide, beginRow, endRow);

            return sqlPage;
        }

        protected string createCountSQL(string sql, BaseFilter filter)
        {
            string sqlCount = string.Format("SELECT COUNT(1) FROM ({0}) as tmpTable", sql);
            return sqlCount;
        }


        protected List<TEntity> getDataBySQL<TEntity, TFilter>(string sql, TFilter filter)
            where TEntity : class
            where TFilter : BaseFilter
        {
            //string childNodeIdList = this.dbContext.Database.SqlQuery(typeof(string), sbSQL.ToString()).Cast<string>().FirstOrDefault();
            if (filter.PageInfo.PageSize > 0)
            {
                string sqlCount = createCountSQL(sql, filter);
                int totalRecord = this.dbContext.Database.SqlQuery(typeof(int), sqlCount).Cast<int>().FirstOrDefault();
                int pageCount = (int)Math.Ceiling((double)totalRecord / (double)filter.PageInfo.PageSize);
                int pageIndex = filter.PageInfo.PageIndex;
                if (pageIndex > pageCount) pageIndex = pageCount;

                filter.PageInfo.PageIndex = pageIndex;
                filter.PageInfo.PageCount = pageCount;
                filter.PageInfo.TotalCount = totalRecord;
            }

            string sqlPage = createPageSQL(sql, filter);
            List<TEntity> result = this.dbContext.Database.SqlQuery<TEntity>(sqlPage).ToList();

            return result;
        }

        #endregion

    }
}
