
namespace CAM.Core.Model.Entity.PropertySafeValueSetter
{
    using System.Reflection;

    public interface IPropertySafeValueSetter
    {
        /// <summary>
        /// 尝试为具备非安全性值的属性赋予安全的初始值
        /// </summary>
        /// <param name="property"></param>
        /// <returns>如果赋值成功，返回true</returns>
        bool trySetSafeValue(PropertyInfo property, _BaseEntity desEntity, _BaseEntity sourceEntity);
    }
}
