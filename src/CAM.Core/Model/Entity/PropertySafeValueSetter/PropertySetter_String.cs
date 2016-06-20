
namespace CAM.Core.Model.Entity.PropertySafeValueSetter
{
    using System.Reflection;

    public class PropertySetter_String : IPropertySafeValueSetter
    {

        public bool trySetSafeValue(PropertyInfo property, _BaseEntity desEntity, _BaseEntity sourceEntity)
        {
            bool isResetValue = false;
            if (string.IsNullOrEmpty((string)property.GetValue(sourceEntity)))
            {
                property.SetValue(desEntity, "");
                isResetValue = true;
            }
            return isResetValue;
        }
    }
}
