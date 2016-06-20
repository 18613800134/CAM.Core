
namespace CAM.Core.Model.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BaseEntityNormal
    {
        /// <summary>
        /// 实体索引，在数据库中是一个自增标识，并且定义为主键，具备唯一性和聚集索引特性
        /// </summary>
        [Key]
        public long Id { get; set; }

        public class EntityNormalPropertyClass
        {


            /// <summary>
            /// 实体创建时间，此时间作为数据创建的备案记录，在任何条件下均不允许修改
            /// </summary>
            [Required]
            [Index(IsClustered = false, IsUnique = false)]
            public DateTime GenerateTime { get; set; }

            /// <summary>
            /// 数据删除标志位，当为true时表示这是一条已删除数据
            /// </summary>
            [Index(IsClustered = false, IsUnique = false)]
            public bool DeleteFlag { get; set; }

            /// <summary>
            /// 实体数据删除时间
            /// </summary>
            [Index(IsClustered = false, IsUnique = false)]
            public DateTime DeleteTime { get; set; }
        }

        public EntityNormalPropertyClass System { get; set; }
    }
}
