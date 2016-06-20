
namespace CAM.Core.Model.Entity.PropertySafeValueSetter
{
    using System;
    using System.Reflection;

    public class PropertySetter_DateTime : IPropertySafeValueSetter
    {

        public bool trySetSafeValue(PropertyInfo property, _BaseEntity desEntity, _BaseEntity sourceEntity)
        {
            bool isResetValue = false;
            if ((DateTime)property.GetValue(sourceEntity) == DateTime.MinValue)
            {
                property.SetValue(desEntity, new DateTime(1900, 1, 1, 0, 0, 0));
                isResetValue = true;
            }
            return isResetValue;
        }
    }
}
