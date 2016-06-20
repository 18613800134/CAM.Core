
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

    public abstract partial class _BaseAggregateForEntityPlugins
    {

        #region DataLock操作方法

        /// <summary>
        /// 为一条数据实体加锁
        /// </summary>
        /// <typeparam name="TEntity">实体泛型</typeparam>
        /// <param name="Id">实体索引</param>
        /// <param name="lockReason">加锁原因</param>
        protected void lockEntity<TEntity>(long Id, string lockReason)
            where TEntity : BaseEntityNormal, IEntityDataLocker
        {
            try
            {
                IRepository<TEntity> res = createRepository<TEntity>();
                TEntity obj = res.read(m => m.Id == Id);
                obj.Locker.lockIt(lockReason);
                res.update(obj);
                commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 为一条加锁数据实体进行解锁
        /// </summary>
        /// <typeparam name="TEntity">实体泛型</typeparam>
        /// <param name="Id">实体索引</param>
        protected void unLockEntity<TEntity>(long Id)
            where TEntity : BaseEntityNormal, IEntityDataLocker
        {
            try
            {
                IRepository<TEntity> res = createRepository<TEntity>();
                TEntity obj = res.read(m => m.Id == Id);
                obj.Locker.unLockIt();
                res.update(obj);
                commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ExpirationState操作方法

        /// <summary>
        /// 更新一条实体数据的过期限制（状态）
        /// </summary>
        /// <typeparam name="TEntity">实体泛型</typeparam>
        /// <param name="Id">实体索引</param>
        /// <param name="expirationState">过期限制</param>
        protected void updateEntityExpirationState<TEntity>(long Id, ExpirationState expirationState)
            where TEntity : BaseEntityNormal, IEntityExpirationState
        {
            try
            {
                IRepository<TEntity> res = createRepository<TEntity>();
                TEntity obj = res.read(m => m.Id == Id);
                obj.Expiration = expirationState;
                res.update(obj);
                commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 延长（改变）实体数据的过期时间到某个具体时间节点
        /// </summary>
        /// <typeparam name="TEntity">实体泛型</typeparam>
        /// <param name="Id">实体索引</param>
        /// <param name="expirationDate">新的过期时间</param>
        protected void delayEntityExpirationToDate<TEntity>(long Id, DateTime expirationDate)
            where TEntity : BaseEntityNormal, IEntityExpirationState
        {
            try
            {
                IRepository<TEntity> res = createRepository<TEntity>();
                TEntity obj = res.read(m => m.Id == Id);
                obj.Expiration.ExpirationTime = expirationDate;
                res.update(obj);
                commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 为一个实体数据延期多长时间（天）
        /// </summary>
        /// <typeparam name="TEntity">实体泛型</typeparam>
        /// <param name="Id">实体索引</param>
        /// <param name="days">延期天数</param>
        protected void delayEntityExpirationByDays<TEntity>(long Id, int days)
            where TEntity : BaseEntityNormal, IEntityExpirationState
        {
            try
            {
                IRepository<TEntity> res = createRepository<TEntity>();
                TEntity obj = res.read(m => m.Id == Id);
                obj.Expiration.delayExpirationDays(days);
                res.update(obj);
                commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}
