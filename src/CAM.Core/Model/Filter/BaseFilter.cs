
namespace CAM.Core.Model.Filter
{
    using CAM.Common.DataProtocol;

    public abstract class BaseFilter
    {
        public PageInfo PageInfo { get; set; }
        public string[] OrderName { get; set; }
        public bool[] IsAsc { get; set; }

        /// <summary>
        /// 支持IdList模式
        /// </summary>
        public string Id { get; set; }
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 具体时间
        /// </summary>
        public string GenerateTime { get; set; }
        /// <summary>
        /// 时间段-开始
        /// </summary>
        public string GenerateTime_start { get; set; }
        /// <summary>
        /// 时间段-结束
        /// </summary>
        public string GenerateTime_end { get; set; }
        public string DeleteTime { get; set; }
        public string DeleteTime_start { get; set; }
        public string DeleteTime_end { get; set; }

        public string MultiSearchKeywords { get; set; }

        public BaseFilter()
        {
            PageInfo = new PageInfo() { PageIndex = 1, PageSize = 30, TotalCount = 0, PageCount = 0, };
            OrderName = new string[] { "Id" };
            IsAsc = new bool[] { false };

            Id = "";
            DeleteFlag = false;
            GenerateTime = "";
            GenerateTime_start = "";
            GenerateTime_end = "";
            DeleteTime = "";
            DeleteTime_start = "";
            DeleteTime_end = "";

            MultiSearchKeywords = "";
        }
    }
}
