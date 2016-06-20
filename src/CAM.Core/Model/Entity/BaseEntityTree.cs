
namespace CAM.Core.Model.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BaseEntityTree
    {
        public class EntityTreePropertyClass
        {
            /// <summary>
            /// 实体数据的父节点索引，在Tree模型或父子关系模型中，用于表现层级结构的属性
            /// </summary>
            [Index(IsClustered = false, IsUnique = false)]
            public long ParentId { get; set; }

            /// <summary>
            /// 当前节点所在的层次
            /// </summary>
            public int Level { get; set; }
        }

        public EntityTreePropertyClass Tree { get; set; }
    }
}
