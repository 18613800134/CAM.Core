
namespace CAM.Core.Model.Entity
{
    using System;

    public class EntityBuilder
    {

        private const string EntityNormalPropertyName = "System";
        private const string EntityOrderPropertyName = "Order";
        private const string EntityTreePropertyName = "Tree";


        public static T build<T>()
            where T : new()
        {
            T obj;
            try
            {
                obj = new T();
                initEntityProperties(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }


        private static void initEntityProperties(object obj)
        {
            if (obj.GetType().GetProperty(EntityNormalPropertyName) != null)
            {
                initEntityNormalProperty(obj);
            }
            if (obj.GetType().GetProperty(EntityOrderPropertyName) != null)
            {
                initEntityOrderProperty(obj);
            }
            if (obj.GetType().GetProperty(EntityTreePropertyName) != null)
            {
                initEntityTreeProperty(obj);
            }
        }



        private static void initEntityNormalProperty(object obj)
        {
            obj.GetType().GetProperty(EntityNormalPropertyName).SetValue(obj,
                        new BaseEntityNormal.EntityNormalPropertyClass()
                        {
                            GenerateTime = DateTime.Now,
                            //OwnershipId = 0,
                            DeleteFlag = false,
                            DeleteTime = DateTime.Now,
                        });
        }

        private static void initEntityOrderProperty(object obj)
        {
            obj.GetType().GetProperty(EntityOrderPropertyName).SetValue(obj,
                        new BaseEntityOrder.EntityOrderPropertyClass()
                        {
                            Index = 0,
                        });
        }

        private static void initEntityTreeProperty(object obj)
        {
            obj.GetType().GetProperty(EntityTreePropertyName).SetValue(obj,
                        new BaseEntityTree.EntityTreePropertyClass()
                        {
                            ParentId = 0,
                            Level = 0,
                        });
        }

    }
}
