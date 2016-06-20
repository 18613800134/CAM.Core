
namespace CAM.Core.Model.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [ComplexType]
    public class ExpirationState
    {
        /// <summary>
        /// 有效期类型：-1：永久有效 0：立即到期 >0：有效期天数
        /// </summary>
        public int ExpirationDays { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }
        /// <summary>
        /// 激活时间：初次激活
        /// </summary>
        public DateTime ActiveTime { get; set; }
        /// <summary>
        /// 最近一次激活时间：如果只激活过一次，和ActiveTime相同；如果激活过多次，记录最近一次的激活时间
        /// </summary>
        public DateTime LastActiveTime { get; set; }
        /// <summary>
        /// 是否处于激活状态：只读
        /// </summary>
        [NotMapped]
        public bool IsActive
        {
            get
            {
                if (ExpirationDays == -1)
                {
                    return true;
                }
                else if (ExpirationDays == 0)
                {
                    return false;
                }
                else
                {
                    return ExpirationTime >= DateTime.Now;
                }
            }
        }

        public ExpirationState()
        {
            ExpirationDays = -1;
            ExpirationTime = DateTime.Now;
            ActiveTime = DateTime.Now;
            LastActiveTime = DateTime.Now;
        }


        /// <summary>
        /// 将到期状态修改为永不过期
        /// </summary>
        public void changeStateToForever()
        {
            this.ExpirationDays = -1;
        }
        /// <summary>
        /// 将到期状态修改为立即到期
        /// </summary>
        public void changeStateToImmediately()
        {
            this.ExpirationDays = 0;
        }
        /// <summary>
        /// 将到期状态修改为正常指定日期模式
        /// </summary>
        public void changeStateToDays()
        {
            this.ExpirationDays = (this.ExpirationTime - this.ActiveTime).Days;
        }
        /// <summary>
        /// 延期操作
        /// </summary>
        /// <param name="days">延长天数</param>
        public void delayExpirationDays(int days)
        {
            this.ExpirationDays += days;
            if (this.ExpirationTime < DateTime.Now)
            {
                this.ExpirationTime = DateTime.Now.AddDays((double)days);
            }
            else
            {
                this.ExpirationTime = this.ExpirationTime.AddDays((double)days);
            }
            this.LastActiveTime = DateTime.Now;
        }
        /// <summary>
        /// 延期操作，延期到指定日期
        /// </summary>
        /// <param name="date">指定到期日期</param>
        public void delayExpirationToDateTime(DateTime date)
        {
            this.ExpirationDays = (date - this.ActiveTime).Days;
            this.ExpirationTime = date;
            this.LastActiveTime = DateTime.Now;
        }
    }


    public class ExpirationStateFactory
    {
        /// <summary>
        /// 创建一个永久有效的有效期实例
        /// </summary>
        /// <returns></returns>
        public static ExpirationState createExpirationStateForever()
        {
            return new ExpirationState()
            {
                ExpirationDays = -1,
                ExpirationTime = DateTime.Now,
                ActiveTime = DateTime.Now,
                LastActiveTime = DateTime.Now,
            };
        }

        /// <summary>
        /// 创建一个立即过期的有效期实例
        /// </summary>
        /// <returns></returns>
        public static ExpirationState createExpirationStateImmediately()
        {
            return new ExpirationState()
            {
                ExpirationDays = 0,
                ExpirationTime = DateTime.Now,
                ActiveTime = DateTime.Now,
                LastActiveTime = DateTime.Now,
            };
        }

        /// <summary>
        /// 创建一个指定天数的有效期实例
        /// </summary>
        /// <param name="days">有效期天数</param>
        /// <returns></returns>
        public static ExpirationState createExpirationStateByDay(int days)
        {
            return new ExpirationState()
            {
                ExpirationDays = days,
                ExpirationTime = DateTime.Now.AddDays((double)days),
                ActiveTime = DateTime.Now,
                LastActiveTime = DateTime.Now,
            };
        }
    }



    public interface IEntityExpirationState
    {
        ExpirationState Expiration { get; set; }
    }

    public enum EMExpirationState
    {
        Ignore = 0,
        Expired = 1,
        UnExpired = 2,
    }
}
