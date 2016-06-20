
namespace CAM.Core.Business.Aggregate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Data.Entity.SqlServer;
    using CAM.Common.Data;
    using CAM.Core.Model.Entity;
    using CAM.Core.Model.Filter;

    public partial class _BaseAggregateFilter
    {
        protected static class FilterToLambdaBuilder
        {

            public static Expression<Func<TEntity, bool>> build<TEntity, TFilter>(TFilter filter)
                where TFilter : BaseFilter
                where TEntity : BaseEntityNormal
            {

                if (filter == null)
                {
                    throw new Exception("未设置数据过滤条件！");
                }

                Expression<Func<TEntity, bool>> lambda = PredicateBuilder.True<TEntity>();

                lambda = linkLambdaForId<TEntity, TFilter>(lambda, filter.Id);
                //lambda = linkLambdaForOwnershipId<TEntity, TFilter>(lambda, filter.OwnershipId);
                lambda = linkLambdaForDeleteFlag<TEntity, TFilter>(lambda, filter.DeleteFlag);

                if (!string.IsNullOrWhiteSpace(filter.GenerateTime))
                {
                    lambda = linkLambdaForGenerateTime<TEntity, TFilter>(lambda, filter.GenerateTime);
                }
                else
                {
                    lambda = linkLambdaForGenerateTimeRegion<TEntity, TFilter>(lambda, filter.GenerateTime_start, filter.GenerateTime_end);
                }

                if (!string.IsNullOrWhiteSpace(filter.DeleteTime))
                {
                    lambda = linkLambdaForDeleteTime<TEntity, TFilter>(lambda, filter.DeleteTime);
                }
                else
                {
                    lambda = linkLambdaForDeleteTimeRegion<TEntity, TFilter>(lambda, filter.DeleteTime_start, filter.DeleteTime_end);
                }

                return lambda;
            }

            private static Expression<Func<TEntity, bool>> linkLambdaForId<TEntity, TFilter>(Expression<Func<TEntity, bool>> lambda, string Id)
                where TFilter : BaseFilter
                where TEntity : BaseEntityNormal
            {

                if (string.IsNullOrWhiteSpace(Id))
                {
                    return lambda;
                }

                if (Id.IndexOf(',') == -1)
                {
                    long desId = 0;
                    long.TryParse(Id, out desId);
                    lambda = lambda.And<TEntity>(m => m.Id == desId);
                    return lambda;
                }
                else
                {
                    string[] arrIdList = Id.Split(',');
                    lambda = lambda.And<TEntity>(m => arrIdList.Contains(m.Id.ToString()));
                    return lambda;
                }
            }

            //private static Expression<Func<TEntity, bool>> linkLambdaForOwnershipId<TEntity, TFilter>(Expression<Func<TEntity, bool>> lambda, string OwnershipId)
            //    where TFilter : BaseFilter
            //    where TEntity : BaseEntityNormal
            //{
            //    if (string.IsNullOrWhiteSpace(OwnershipId))
            //    {
            //        return lambda;
            //    }

            //    if (OwnershipId.IndexOf(',') == -1)
            //    {
            //        long desId = 0;
            //        long.TryParse(OwnershipId, out desId);
            //        lambda = lambda.And<TEntity>(m => m.System.OwnershipId == desId);
            //        return lambda;
            //    }
            //    else
            //    {

            //        string[] arrIdList = OwnershipId.Split(',');
            //        lambda = lambda.And<TEntity>(m => arrIdList.Contains(m.System.OwnershipId.ToString()));
            //        return lambda;
            //    }
            //}

            private static Expression<Func<TEntity, bool>> linkLambdaForDeleteFlag<TEntity, TFilter>(Expression<Func<TEntity, bool>> lambda, bool DeleteFlag)
                where TFilter : BaseFilter
                where TEntity : BaseEntityNormal
            {
                lambda = lambda.And<TEntity>(m => m.System.DeleteFlag == DeleteFlag);
                return lambda;
            }

            private static Expression<Func<TEntity, bool>> linkLambdaForGenerateTime<TEntity, TFilter>(Expression<Func<TEntity, bool>> lambda, string GenerateTime)
                where TFilter : BaseFilter
                where TEntity : BaseEntityNormal
            {
                if (string.IsNullOrWhiteSpace(GenerateTime))
                {
                    return lambda;
                }

                DateTime compareDate;
                bool isDateTime = DateTime.TryParse(GenerateTime, out compareDate);
                if (isDateTime)
                {
                    lambda = lambda.And<TEntity>(m => SqlFunctions.DateDiff("d", m.System.GenerateTime, compareDate) == 0);
                }
                return lambda;
            }

            private static Expression<Func<TEntity, bool>> linkLambdaForGenerateTimeRegion<TEntity, TFilter>(Expression<Func<TEntity, bool>> lambda, string GenerateTimeStart, string GenerateTimeEnd)
                where TFilter : BaseFilter
                where TEntity : BaseEntityNormal
            {
                if (!string.IsNullOrWhiteSpace(GenerateTimeStart))
                {
                    DateTime compareDate;
                    bool isDateTime = DateTime.TryParse(GenerateTimeStart, out compareDate);
                    if (isDateTime)
                    {
                        lambda = lambda.And<TEntity>(m => SqlFunctions.DateDiff("d", m.System.GenerateTime, compareDate) <= 0);
                    }
                }
                if (!string.IsNullOrWhiteSpace(GenerateTimeEnd))
                {
                    DateTime compareDate;
                    bool isDateTime = DateTime.TryParse(GenerateTimeEnd, out compareDate);
                    if (isDateTime)
                    {
                        lambda = lambda.And<TEntity>(m => SqlFunctions.DateDiff("d", m.System.GenerateTime, compareDate) >= 0);
                    }
                }
                return lambda;
            }

            private static Expression<Func<TEntity, bool>> linkLambdaForDeleteTime<TEntity, TFilter>(Expression<Func<TEntity, bool>> lambda, string DeleteTime)
                where TFilter : BaseFilter
                where TEntity : BaseEntityNormal
            {
                if (string.IsNullOrWhiteSpace(DeleteTime))
                {
                    return lambda;
                }

                DateTime compareDate;
                bool isDateTime = DateTime.TryParse(DeleteTime, out compareDate);
                if (isDateTime)
                {
                    lambda = lambda.And<TEntity>(m => SqlFunctions.DateDiff("d", m.System.DeleteTime, compareDate) == 0);
                }
                return lambda;
            }

            private static Expression<Func<TEntity, bool>> linkLambdaForDeleteTimeRegion<TEntity, TFilter>(Expression<Func<TEntity, bool>> lambda, string DeleteTimeStart, string DeleteTimeEnd)
                where TFilter : BaseFilter
                where TEntity : BaseEntityNormal
            {
                if (!string.IsNullOrWhiteSpace(DeleteTimeStart))
                {
                    DateTime compareDate;
                    bool isDateTime = DateTime.TryParse(DeleteTimeStart, out compareDate);
                    if (isDateTime)
                    {
                        lambda = lambda.And<TEntity>(m => SqlFunctions.DateDiff("d", m.System.DeleteTime, compareDate) <= 0);
                    }
                }
                if (!string.IsNullOrWhiteSpace(DeleteTimeEnd))
                {
                    DateTime compareDate;
                    bool isDateTime = DateTime.TryParse(DeleteTimeEnd, out compareDate);
                    if (isDateTime)
                    {
                        lambda = lambda.And<TEntity>(m => SqlFunctions.DateDiff("d", m.System.DeleteTime, compareDate) >= 0);
                    }
                }
                return lambda;
            }







            public static Expression<Func<TEntity, bool>> linkLambdaForDataLock<TEntity>(Expression<Func<TEntity, bool>> lambda, EMDataLockState state)
                where TEntity : _BaseEntity, IEntityDataLocker
            {

                if (state == EMDataLockState.Locked)
                {
                    lambda = lambda.And<TEntity>(m => m.Locker.IsLocked == true);
                }
                else if (state == EMDataLockState.UnLocked)
                {
                    lambda = lambda.And<TEntity>(m => m.Locker.IsLocked == false);
                }

                return lambda;
            }

            public static Expression<Func<TEntity, bool>> linkLambdaForExpirationState<TEntity>(Expression<Func<TEntity, bool>> lambda, EMExpirationState state)
                where TEntity : _BaseEntity, IEntityExpirationState
            {

                if (state == EMExpirationState.Expired)
                {
                    lambda = lambda.And<TEntity>(m => m.Expiration.ExpirationDays >= 0 && SqlFunctions.DateDiff("d", m.Expiration.ExpirationTime, DateTime.Now) >= 0);
                }
                else if (state == EMExpirationState.UnExpired)
                {
                    lambda = lambda.And<TEntity>(m => m.Expiration.ExpirationDays == -1 || (m.Expiration.ExpirationDays >= 0 && SqlFunctions.DateDiff("d", m.Expiration.ExpirationTime, DateTime.Now) < 0));
                }

                return lambda;
            }

        }


        protected abstract IRepository<TEntity> createRepository<TEntity>() where TEntity : class;
        protected abstract void commit();

        protected IQueryable<TEntity> getDataByFilter<TEntity, TFilter>(Expression<Func<TEntity, bool>> lambda, ref TFilter filter)
            where TEntity : class
            where TFilter : BaseFilter
        {
            IRepository<TEntity> res = createRepository<TEntity>();
            IQueryable<TEntity> result = null;

            //如果是Order模型或Tree模型，当默认排序为Id时，强制修改按照OrderIndex顺序排列
            Type EntityType = typeof(TEntity);
            if (EntityType.GetProperty("Order") != null || EntityType.GetProperty("Tree") != null)
            {
                if (filter.OrderName.Length == 1 && filter.OrderName[0] == "Id")
                {
                    filter.OrderName[0] = "Order_Index";
                    filter.IsAsc[0] = true;
                }
            }
            //如果是Tree模型，不支持分页
            if (EntityType.GetProperty("Tree") != null)
            {
                filter.PageInfo.PageSize = 0;
                filter.OrderName = new string[] { "Tree_ParentId", "Order_Index" };
                filter.IsAsc = new bool[] { true, true };
            }


            if (filter.PageInfo.PageSize == 0)
            {
                result = res.readList(lambda, filter.OrderName, filter.IsAsc);
            }
            else
            {
                int totalCount, pageCount;
                result = res.readPageList(filter.PageInfo.PageIndex, filter.PageInfo.PageSize, out totalCount, out pageCount, lambda, filter.OrderName, filter.IsAsc);

                filter.PageInfo.TotalCount = totalCount;
                filter.PageInfo.PageCount = pageCount;
            }

            return result;
        }

    }
}
