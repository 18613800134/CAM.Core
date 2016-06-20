
namespace CAM.Core.Model.Entity.PropertySafeValueSetter
{
    using System.Reflection;

    public class PropertySafeValueSetterFactory
    {

        public static IPropertySafeValueSetter createASafeValueSetter(PropertyInfo property, _BaseEntity sourceEntity)
        {

            string propertyTypeName = sourceEntity.GetType().GetProperty(property.Name).PropertyType.Name.ToLower();

            if (propertyTypeName == "string")
            {
                return new PropertySetter_String();
            }
            if (propertyTypeName == "datetime")
            {
                return new PropertySetter_DateTime();
            }

            return null;
        }

    }
}
