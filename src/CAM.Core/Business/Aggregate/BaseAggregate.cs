
namespace CAM.Core.Business.Aggregate
{
    using System;
    using System.Data.Entity;
    using CAM.Common.Data;
    using Interface;
    using System.Linq;

    public abstract class _BaseAggregate : IDisposable
    {
        protected DbContext dbContext { get; set; }

        public virtual void Dispose()
        {
            //在子类按需实现
        }
    }
    public abstract partial class _BaseAggregateFilter : _BaseAggregate
    {
        //具体代码到分部代码_BaseAggregateFilter页中实现
    }

    public abstract partial class _BaseAggregateForEntityModel : _BaseAggregateFilter
    {

    }

    public abstract partial class _BaseAggregateForEntityPlugins : _BaseAggregateForEntityModel
    {

    }

    public abstract partial class _BaseAggregateForSQLCommand : _BaseAggregateForEntityPlugins
    {

    }

    public abstract partial class _BaseAggregateMixin : _BaseAggregateForSQLCommand, IMixinInterface
    {


    }

    public abstract class BaseAggregate : _BaseAggregateMixin
    {

        protected IUnitOfWork unitOfWork { get; set; }


        public BaseAggregate()
            : base()
        {
            this.unitOfWork = new UnitOfWork();

        }

        public override void Dispose()
        {
            base.Dispose();
            this.unitOfWork.Dispose();
            this.dbContext.Dispose();
        }

        protected override IRepository<TEntity> createRepository<TEntity>()
        {
            return new Repository<TEntity>(this.dbContext, this.unitOfWork);
        }

        protected override void commit()
        {
            this.unitOfWork.commit();
        }



    }
}
