
namespace CAM.Core.Model.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class BaseEntityOrder
    {
        public class EntityOrderPropertyClass
        {
            /// <summary>
            /// 自定义排序时，数据的排序序号，可根据此来进行ASC或DESC的排序
            /// </summary>
            [Index(IsClustered = false, IsUnique = false)]
            public int Index { get; set; }
        }
        public EntityOrderPropertyClass Order { get; set; }
    }
}
