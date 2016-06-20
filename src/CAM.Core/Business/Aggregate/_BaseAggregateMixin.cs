
namespace CAM.Core.Business.Aggregate
{
    using System;
    //using System.Data.Entity;
    //using CAM.Common.Data;
    using Interface;
    using System.Linq;
    using Common.QueryMaker;
    using System.Reflection;
    using System.Collections.Generic;
    using Model.Filter;

    public abstract partial class _BaseAggregateMixin
    {

        public TMixin readMixin<TMixin>(QueryMakerObjectQueue qm)
        {
            try
            {
                Type ViewModeType = typeof(object);
                string sql = qm.ToString();
                return readMixinBySql<TMixin>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TMixin readMixinBySql<TMixin>(string sql)
        {
            try
            {
                TMixin result = this.dbContext.Database.SqlQuery(typeof(TMixin), sql).Cast<TMixin>().FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<TMixin> readMixinList<TMixin, TFilter>(QueryMakerObjectQueue qm, TFilter filter) where TFilter : BaseFilter
        {
            try
            {
                Type ViewModeType = typeof(object);
                string sql = qm.ToString();
                return readMixinListBySql<TMixin, TFilter>(sql, filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TMixin> readMixinListBySql<TMixin, TFilter>(string sql, TFilter filter) where TFilter : BaseFilter
        {
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

            List<TMixin> result = this.dbContext.Database.SqlQuery<TMixin>(sqlPage).ToList();

            return result;

        }

        public List<TMixin> readMixinTree<TMixin>(QueryMakerObjectQueue qm)
        {
            try
            {
                string sql = qm.ToString();
                return readMixinTreeBySql<TMixin>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TMixin> readMixinTreeBySql<TMixin>(string sql)
        {

            List<TMixin> result = this.dbContext.Database.SqlQuery<TMixin>(sql).ToList();

            return result;

        }
    }
}
