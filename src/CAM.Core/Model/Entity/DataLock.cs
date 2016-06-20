
namespace CAM.Core.Model.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.SqlServer;

    [Serializable]
    [ComplexType]
    public class DataLock
    {
        /// <summary>
        /// 是否被锁定
        /// </summary>
        [Index(IsClustered = false, IsUnique = false)]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 锁定原因
        /// </summary>
        [MaxLength(50)]
        public string LockReason { get; set; }

        /// <summary>
        /// 锁定时间
        /// </summary>
        public DateTime LockTime { get; set; }

        /// <summary>
        /// 为数据锁加锁
        /// </summary>
        /// <param name="lockReason">锁定原因</param>
        public void lockIt(string lockReason)
        {
            this.IsLocked = true;
            this.LockReason = lockReason;
            this.LockTime = DateTime.Now;
        }

        /// <summary>
        /// 为数据锁解锁
        /// </summary>
        public void unLockIt()
        {
            this.IsLocked = false;
            this.LockReason = "";
        }
    }

    public class DataLockFactory
    {
        /// <summary>
        /// 创建一个未锁定的数据锁
        /// </summary>
        /// <returns></returns>
        public static DataLock createUnLockedLock()
        {
            DataLock accountLocker = new DataLock()
            {
                IsLocked = false,
                LockReason = "",
                LockTime = DateTime.Now,
            };
            return accountLocker;
        }

        /// <summary>
        /// 创建一个被锁定的数据锁
        /// </summary>
        /// <returns></returns>
        public static DataLock createLockedLock()
        {
            DataLock accountLocker = new DataLock()
            {
                IsLocked = true,
                LockReason = "系统初始化自动锁定",
                LockTime = DateTime.Now,
            };
            return accountLocker;
        }

        /// <summary>
        /// 创建一个被锁定的数据锁
        /// </summary>
        /// <param name="lockReason">锁定原因</param>
        /// <returns></returns>
        public static DataLock createLockedLock(string lockReason)
        {
            DataLock accountLocker = new DataLock()
            {
                IsLocked = true,
                LockReason = lockReason,
                LockTime = DateTime.Now,
            };
            return accountLocker;
        }
    }



    public interface IEntityDataLocker
    {
        DataLock Locker { get; set; }
    }

    public enum EMDataLockState
    {
        Ignore = 0,
        Locked = 1,
        UnLocked = 2,
    }
}
