
namespace CAM.Core.Model.Entity
{
    using System;

    #region _BaseEntity 数据实体基类

    /// <summary>
    /// 数据实体基类
    /// </summary>
    public abstract class _BaseEntity : IDisposable
    {
        //do nothing...
        public virtual void Dispose()
        {
        }
    }

    #endregion



    #region _BasicEnityClone 数据实体克隆类：继承自此的类均可实现Clone自身的操作

    /// <summary>
    /// 数据实体克隆类：继承自此的类均可实现Clone自身的操作
    /// ----------------
    /// 本类为抽象类，不能直接实例化
    /// </summary>
    public abstract partial class _BasicEnityClone : _BaseEntity
    {
        //在分部类_BasicEnityClone中实现具体定义
    }

    #endregion



    #region _BaseEntityValidation 数据实体自我验证类：继承自此的类可以进行数据合法性自我验证

    /// <summary>
    /// 数据实体自我验证类：继承自此的类可以进行数据合法性自我验证
    /// </summary>
    public abstract partial class _BaseEntityValidation : _BasicEnityClone
    {
        //在分部类_BaseEntityValidation中实现具体定义
    }

    #endregion






    #region BaseEntityNormal 标准数据实体类：构成一个数据实体的基本属性

    /// <summary>
    /// 标准数据实体类：构成一个数据实体的基本属性
    /// </summary>
    public abstract partial class BaseEntityNormal : _BaseEntityValidation
    {
        //在分部类BaseEntityNormal中实现具体定义
    }

    #endregion



    #region BaseEntityOrder 可自定义排序的数据实体类：通过增加OrderIndex属性实现自定义排序

    /// <summary>
    /// 可自定义排序的数据实体类：通过增加OrderIndex属性实现自定义排序
    /// </summary>
    public abstract partial class BaseEntityOrder : BaseEntityNormal
    {
        //在分部类BaseEntityOrder中实现具体定义
    }

    #endregion



    #region BaseEntityTree 支持树形结构的数据实体类：通过增加ParentId属性实现树形结构

    /// <summary>
    /// 支持树形结构的数据实体类：通过增加ParentId属性实现树形结构
    /// </summary>
    public abstract partial class BaseEntityTree : BaseEntityOrder
    {
        //在分部类BaseEntityTree中实现具体定义
    }

    #endregion
}
