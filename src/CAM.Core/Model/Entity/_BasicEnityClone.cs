
namespace CAM.Core.Model.Entity
{
    using System;
    using System.Reflection;
    using System.Linq;
    using PropertySafeValueSetter;

    public partial class _BasicEnityClone
    {
        /// <summary>
        /// 从源对象处克隆所有属性到当前对象上
        /// </summary>
        /// <param name="sourceEntity">源对象</param>
        public void cloneFrom(_BasicEnityClone sourceEntity)
        {
            foreach (PropertyInfo property in sourceEntity.GetType().GetProperties())
            {
                setPropertyValue(property, sourceEntity);
            }
        }

        /// <summary>
        /// 从源对象处克隆所有属性到当前对象上：只克隆限定的属性
        /// </summary>
        /// <param name="sourceEntity">源对象</param>
        /// <param name="propertyNames">指定克隆的属性</param>
        public void cloneFromWithProperties(_BasicEnityClone sourceEntity, params string[] propertyNames)
        {
            foreach (PropertyInfo property in sourceEntity.GetType().GetProperties())
            {
                if (!propertyNames.Contains(property.Name))
                {
                    continue;
                }
                setPropertyValue(property, sourceEntity);
            }
        }




        /// <summary>
        /// 从源对象处克隆所有属性到当前对象上：只克隆限定之外的属性
        /// </summary>
        /// <param name="sourceEntity">源对象</param>
        /// <param name="propertyNames">指定不要克隆的属性</param>
        public void cloneFromWithOutProperties(_BasicEnityClone sourceEntity, string[] propertyNames)
        {
            foreach (PropertyInfo property in sourceEntity.GetType().GetProperties())
            {
                if (propertyNames.Contains(property.Name))
                {
                    continue;
                }
                setPropertyValue(property, sourceEntity);
            }
        }

        /// <summary>
        /// 为属性赋值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="sourceEntity"></param>
        private void setPropertyValue(PropertyInfo property, _BasicEnityClone sourceEntity)
        {
            IPropertySafeValueSetter svs = PropertySafeValueSetterFactory.createASafeValueSetter(property, sourceEntity);
            if (svs == null)
            {
                property.SetValue(this, property.GetValue(sourceEntity));
            }
            else
            {
                //尝试赋予属性安全值
                bool isSafeValueSetted = svs.trySetSafeValue(property, this, sourceEntity);
                if (!isSafeValueSetted)
                {
                    property.SetValue(this, property.GetValue(sourceEntity));
                }
            }
        }

    }
}
