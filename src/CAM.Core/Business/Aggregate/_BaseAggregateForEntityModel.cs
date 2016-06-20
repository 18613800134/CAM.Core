
namespace CAM.Core.Business.Aggregate
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Reflection;
    using CAM.Core.Model.Entity;
    using CAM.Core.Model.Filter;
    using CAM.Common.Data;
    using CAM.Common.DataProtocol;
    using System.Linq;

    public abstract partial class _BaseAggregateForEntityModel
    {

        #region Order and Tree（排序模型与树模型）的操作方法


        #region Order模型的排序方法：没有限定条件，在所有数据中进行排序
        protected void moveLast<TEntity>(long Id)
            where TEntity : BaseEntityOrder
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @desId bigint; set @desId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @desOrderIndex int; set @desOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index from {0} where Id=@thisId;", tableName);
            sbSQL.AppendFormat("select top 1 @desId = Id from {0} where System_DeleteFlag=0 and Order_Index<@thisOrderIndex order by Order_Index desc;", tableName);
            sbSQL.AppendFormat("if @desId > 0 begin;");
            sbSQL.AppendFormat("select @desOrderIndex = Order_Index from {0} where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@thisOrderIndex where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@desOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());

        }

        protected void moveNext<TEntity>(long Id)
            where TEntity : BaseEntityOrder
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @desId bigint; set @desId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @desOrderIndex int; set @desOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index from {0} where Id=@thisId;", tableName);
            sbSQL.AppendFormat("select top 1 @desId = Id from {0} where System_DeleteFlag=0 and Order_Index>@thisOrderIndex order by Order_Index asc;", tableName);
            sbSQL.AppendFormat("if @desId > 0 begin;");
            sbSQL.AppendFormat("select @desOrderIndex = Order_Index from {0} where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@thisOrderIndex where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@desOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }

        protected void moveToTop<TEntity>(long Id)
            where TEntity : BaseEntityOrder
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @firstId bigint; set @firstId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @firstOrderIndex int; set @firstOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index from {0} where Id=@thisId;", tableName);
            sbSQL.AppendFormat("select top 1 @firstId = Id from {0} where Order_Index<@thisOrderIndex order by Order_Index asc;", tableName);
            sbSQL.AppendFormat("if @firstId > 0 begin;");
            sbSQL.AppendFormat("select @firstOrderIndex = Order_Index from {0} where Id=@firstId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=Order_Index+1 where Order_Index<@thisOrderIndex;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@firstOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }

        protected void moveToBottom<TEntity>(long Id)
            where TEntity : BaseEntityOrder
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @lastId bigint; set @lastId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @lastOrderIndex int; set @lastOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index from {0} where Id=@thisId;", tableName);
            sbSQL.AppendFormat("select top 1 @lastId = Id from {0} where Order_Index>@thisOrderIndex order by Order_Index desc;", tableName);
            sbSQL.AppendFormat("if @lastId > 0 begin;");
            sbSQL.AppendFormat("select @lastOrderIndex = Order_Index from {0} where Id=@lastId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=Order_Index-1 where Order_Index>@thisOrderIndex;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@lastOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }



        #endregion




        #region Order模型的排序方法：限定在一个范围内，dependPropertyName是一个属性（字段）的名称，针对这个字段相同的数据进行小范围排序
        protected void moveLast<TEntity>(long Id, string dependPropertyName)
            where TEntity : BaseEntityOrder
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @dependValue bigint; set @dependValue=0;");
            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @desId bigint; set @desId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @desOrderIndex int; set @desOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index, @dependValue={1} from {0} where Id=@thisId;", tableName, dependPropertyName);
            sbSQL.AppendFormat("select top 1 @desId = Id from {0} where System_DeleteFlag=0 and {1}=@dependValue and Order_Index<@thisOrderIndex order by Order_Index desc;", tableName, dependPropertyName);
            sbSQL.AppendFormat("if @desId > 0 begin;");
            sbSQL.AppendFormat("select @desOrderIndex = Order_Index from {0} where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@thisOrderIndex where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@desOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());

        }

        protected void moveNext<TEntity>(long Id, string dependPropertyName)
            where TEntity : BaseEntityOrder
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @dependValue bigint; set @dependValue=0;");
            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @desId bigint; set @desId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @desOrderIndex int; set @desOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index, @dependValue={1} from {0} where Id=@thisId;", tableName, dependPropertyName);
            sbSQL.AppendFormat("select top 1 @desId = Id from {0} where System_DeleteFlag=0 and {1}=@dependValue and Order_Index>@thisOrderIndex order by Order_Index asc;", tableName, dependPropertyName);
            sbSQL.AppendFormat("if @desId > 0 begin;");
            sbSQL.AppendFormat("select @desOrderIndex = Order_Index from {0} where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@thisOrderIndex where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@desOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }

        protected void moveToTop<TEntity>(long Id, string dependPropertyName)
            where TEntity : BaseEntityOrder
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @dependValue bigint; set @dependValue=0;");
            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @firstId bigint; set @firstId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @firstOrderIndex int; set @firstOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index, @dependValue={1} from {0} where Id=@thisId;", tableName, dependPropertyName);
            sbSQL.AppendFormat("select top 1 @firstId = Id from {0} where {1}=@dependValue and Order_Index<@thisOrderIndex order by Order_Index asc;", tableName, dependPropertyName);
            sbSQL.AppendFormat("if @firstId > 0 begin;");
            sbSQL.AppendFormat("select @firstOrderIndex = Order_Index from {0} where Id=@firstId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=Order_Index+1 where Order_Index<@thisOrderIndex and {1}=@dependValue;", tableName, dependPropertyName);
            sbSQL.AppendFormat("update {0} set Order_Index=@firstOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }

        protected void moveToBottom<TEntity>(long Id, string dependPropertyName)
            where TEntity : BaseEntityOrder
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @dependValue bigint; set @dependValue=0;");
            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @lastId bigint; set @lastId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @lastOrderIndex int; set @lastOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index, @dependValue={1} from {0} where Id=@thisId;", tableName, dependPropertyName);
            sbSQL.AppendFormat("select top 1 @lastId = Id from {0} where {1}=@dependValue and Order_Index>@thisOrderIndex order by Order_Index desc;", tableName, dependPropertyName);
            sbSQL.AppendFormat("if @lastId > 0 begin;");
            sbSQL.AppendFormat("select @lastOrderIndex = Order_Index from {0} where Id=@lastId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=Order_Index-1 where Order_Index>@thisOrderIndex and {1}=@dependValue;", tableName, dependPropertyName);
            sbSQL.AppendFormat("update {0} set Order_Index=@lastOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }


        #endregion




        #region Tree模型的排序方法：限定在一个范围内，同一个ParentId节点下的数据排序：dependPropertyName是限定字段，如BranchId，restrictName是父节点限定字段，如Tree_ParentId

        protected void moveLast<TEntity>(long Id, string dependPropertyName, string restrictName)
            where TEntity : BaseEntityTree
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @dependValue bigint; set @dependValue=0;");
            sbSQL.AppendFormat("declare @restrictValue bigint; set @restrictValue=0;");
            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @desId bigint; set @desId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @desOrderIndex int; set @desOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index, @dependValue={1}, @restrictValue={2} from {0} where Id=@thisId;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("select top 1 @desId = Id from {0} where System_DeleteFlag=0 and {1}=@dependValue and {2}=@restrictValue and Order_Index<@thisOrderIndex order by Order_Index desc;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("if @desId > 0 begin;");
            sbSQL.AppendFormat("select @desOrderIndex = Order_Index from {0} where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@thisOrderIndex where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@desOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());

        }

        protected void moveNext<TEntity>(long Id, string dependPropertyName, string restrictName)
            where TEntity : BaseEntityTree
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @dependValue bigint; set @dependValue=0;");
            sbSQL.AppendFormat("declare @restrictValue bigint; set @restrictValue=0;");
            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @desId bigint; set @desId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @desOrderIndex int; set @desOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index, @dependValue={1}, @restrictValue={2} from {0} where Id=@thisId;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("select top 1 @desId = Id from {0} where System_DeleteFlag=0 and {1}=@dependValue and {2}=@restrictValue and Order_Index>@thisOrderIndex order by Order_Index asc;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("if @desId > 0 begin;");
            sbSQL.AppendFormat("select @desOrderIndex = Order_Index from {0} where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@thisOrderIndex where Id=@desId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=@desOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }

        protected void moveToTop<TEntity>(long Id, string dependPropertyName, string restrictName)
            where TEntity : BaseEntityTree
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @dependValue bigint; set @dependValue=0;");
            sbSQL.AppendFormat("declare @restrictValue bigint; set @restrictValue=0;");
            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @firstId bigint; set @firstId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @firstOrderIndex int; set @firstOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index, @dependValue={1}, @restrictValue={2} from {0} where Id=@thisId;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("select top 1 @firstId = Id from {0} where {1}=@dependValue and {2}=@restrictValue and Order_Index<@thisOrderIndex order by Order_Index asc;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("if @firstId > 0 begin;");
            sbSQL.AppendFormat("select @firstOrderIndex = Order_Index from {0} where Id=@firstId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=Order_Index+1 where Order_Index<@thisOrderIndex and {1}=@dependValue and {2}=@restrictValue;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("update {0} set Order_Index=@firstOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }

        protected void moveToBottom<TEntity>(long Id, string dependPropertyName, string restrictName)
            where TEntity : BaseEntityTree
        {
            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            sbSQL.AppendFormat("declare @dependValue bigint; set @dependValue=0;");
            sbSQL.AppendFormat("declare @restrictValue bigint; set @restrictValue=0;");
            sbSQL.AppendFormat("declare @thisId bigint; set @thisId={0};", Id);
            sbSQL.AppendFormat("declare @lastId bigint; set @lastId=0;");
            sbSQL.AppendFormat("declare @thisOrderIndex int; set @thisOrderIndex=0;");
            sbSQL.AppendFormat("declare @lastOrderIndex int; set @lastOrderIndex=0;");
            sbSQL.AppendFormat("select @thisOrderIndex = Order_Index, @dependValue={1}, @restrictValue={2} from {0} where Id=@thisId;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("select top 1 @lastId = Id from {0} where {1}=@dependValue and {2}=@restrictValue and Order_Index>@thisOrderIndex order by Order_Index desc;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("if @lastId > 0 begin;");
            sbSQL.AppendFormat("select @lastOrderIndex = Order_Index from {0} where Id=@lastId;", tableName);
            sbSQL.AppendFormat("update {0} set Order_Index=Order_Index-1 where Order_Index>@thisOrderIndex and {1}=@dependValue and {2}=@restrictValue;", tableName, dependPropertyName, restrictName);
            sbSQL.AppendFormat("update {0} set Order_Index=@lastOrderIndex where Id=@thisId;", tableName);
            sbSQL.AppendFormat("end;");

            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }


        #endregion



        #region Tree模型中数据节点改变父节点的方法

        /// <summary>
        /// 改变普通树模型节点位置
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="Id"></param>
        /// <param name="moveToParentId"></param>
        protected void moveToParentNode<TEntity>(long Id, long moveToParentId)
            where TEntity : BaseEntityTree
        {

            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            //判断目标对象是否处于当前移动对象的子节点上。注意：操作对象一定不能被移动到自身的子节点里，否则将无法被展现出来！！！
            sbSQL.AppendFormat("declare @idlist varchar(max);set @idlist = ',';");
            sbSQL.AppendFormat("with cte as(select Id from {1} where Id = {0} union all select b.Id from cte a inner join {1} b on a.Id = b.Tree_ParentId)select @idlist=@idlist+CAST(Id as varchar(10))+',' from cte;", Id, tableName);
            sbSQL.AppendFormat("select @idlist;");

            string childNodeIdList = this.dbContext.Database.SqlQuery(typeof(string), sbSQL.ToString()).Cast<string>().FirstOrDefault();
            if (childNodeIdList.IndexOf(string.Format(",{0},", moveToParentId)) >= 0)
            {
                throw new Exception("目标对象属于当前对象的子节点，移动操作无法完成");
            }


            //判断当前节点是否被移动到其他节点下，如果父节点没有发生改变，则不进行任何操作
            sbSQL.Clear();
            sbSQL.AppendFormat("declare @parentId bigint; set @parentId=0;");
            sbSQL.AppendFormat("select @parentId=Tree_ParentId from {0} where Id={1};", tableName, Id);
            sbSQL.AppendFormat("select @parentId;");

            long moveParentId = this.dbContext.Database.SqlQuery(typeof(long), sbSQL.ToString()).Cast<long>().FirstOrDefault();
            if (moveParentId == moveToParentId)
            {
                return;
            }

            //移动操作对象后，将其Order_Index设置为最大值，排到序列最后
            sbSQL.Clear();
            //sbSQL.AppendFormat("declare @restrictValue bigint; set @restrictValue=0;");
            sbSQL.AppendFormat("declare @moveOrderIndex int; set @moveOrderIndex=0;");
            sbSQL.AppendFormat("declare @moveParentId bigint; set @moveParentId=0;");
            sbSQL.AppendFormat("select @moveOrderIndex=Order_Index, @moveParentId=Tree_ParentId from {0} where Id={1};", tableName, Id);
            sbSQL.AppendFormat("update {0} set Order_Index=Order_Index-1 where Tree_ParentId=@moveParentId and Order_Index>@moveOrderIndex;", tableName);
            sbSQL.AppendFormat("");
            sbSQL.AppendFormat("declare @desMaxOrderIndex int; set @desMaxOrderIndex=0;");
            sbSQL.AppendFormat("select @desMaxOrderIndex=isnull(Max(Order_Index), 0)+1 from {0} where Tree_ParentId={1};", tableName, moveToParentId);
            sbSQL.AppendFormat("update {0} set Tree_ParentId={1}, Order_Index=@desMaxOrderIndex where Id={2};", tableName, moveToParentId, Id);
            //更新数据中的Tree_Level层级数值
            sbSQL.AppendFormat("declare @tb_level table(Id bigint);");
            sbSQL.AppendFormat("with cte as(select Id, Tree_ParentId from {0} where Id = {1} union all select b.Id, b.Tree_ParentId from cte a inner join {0} b on a.Id=b.Tree_ParentId) insert into @tb_level select Id from cte;", tableName, Id);
            sbSQL.AppendFormat("declare cur_level cursor for(select Id from @tb_level);");
            sbSQL.AppendFormat("open cur_level;");
            sbSQL.AppendFormat("declare @level_id bigint;");
            sbSQL.AppendFormat("fetch next from cur_level into @level_id;");
            sbSQL.AppendFormat("while @@FETCH_STATUS = 0 begin;");
            sbSQL.AppendFormat("update a set a.Tree_Level = case a.Tree_ParentId when 0 then 0 else (select b.Tree_Level+1 from {0} b where b.Id = a.Tree_ParentId) end from {0} a where a.Id = @level_id;", tableName);
            sbSQL.AppendFormat("fetch next from cur_level into @level_id;");
            sbSQL.AppendFormat("end;");
            sbSQL.AppendFormat("close cur_level;");
            sbSQL.AppendFormat("deallocate cur_level;");

            //更新Tree_Level的SQL语句原型
            /*
            update Department set Tree_ParentId = 0 where Id = 2

            declare @tb_level table(Id bigint);
            with cte as(select Id, Tree_ParentId from Department where Id = 2 union all select b.Id, b.Tree_ParentId from cte a inner join Department b on a.Id=b.Tree_ParentId) insert into @tb_level select Id from cte;

            declare cur_level cursor for(select Id from @tb_level);
            open cur_level;
            declare @level_id bigint;
            fetch next from cur_level into @level_id;
            while @@FETCH_STATUS = 0 begin;
	            update a set a.Tree_Level = case a.Tree_ParentId when 0 then 0 else (select b.Tree_Level+1 from Department b where b.Id = a.Tree_ParentId) end from Department a where a.Id = @level_id;
	            fetch next from cur_level into @level_id;
            end;
            close cur_level;
            deallocate cur_level; 
            */


            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }

        /// <summary>
        /// 改变限定范围内节点位置，例如同一个BranchId内的节点数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="Id"></param>
        /// <param name="moveToParentId"></param>
        /// <param name="restrictName">限定节点字段，如BranchId</param>
        protected void moveToParentNode<TEntity>(long Id, long moveToParentId, string restrictName)
            where TEntity : BaseEntityTree
        {

            string tableName = typeof(TEntity).Name;
            StringBuilder sbSQL = new StringBuilder("");

            //判断目标对象是否处于当前移动对象的子节点上。注意：操作对象一定不能被移动到自身的子节点里，否则将无法被展现出来！！！
            sbSQL.AppendFormat("declare @idlist varchar(max);set @idlist = ',';");
            sbSQL.AppendFormat("with cte as(select Id from {1} where Id = {0} union all select b.Id from cte a inner join {1} b on a.Id = b.Tree_ParentId)select @idlist=@idlist+CAST(Id as varchar(10))+',' from cte;", Id, tableName);
            sbSQL.AppendFormat("select @idlist;");

            string childNodeIdList = this.dbContext.Database.SqlQuery(typeof(string), sbSQL.ToString()).Cast<string>().FirstOrDefault();
            if (childNodeIdList.IndexOf(string.Format(",{0},", moveToParentId)) >= 0)
            {
                throw new Exception("目标对象属于当前对象的子节点，移动操作无法完成");
            }

            //如果移动到新的父节点不是根节点的话，则需要判断当前节点和新的父节点是否是在一个限制域内
            if (moveToParentId != 0)
            {
                //判断目标对象与操作对象是否处于同一个限定范围内
                sbSQL.Clear();
                sbSQL.AppendFormat("declare @moveRestrictValue bigint; set @moveRestrictValue=0;");
                sbSQL.AppendFormat("declare @desRestrictValue bigint; set @desRestrictValue=0;");
                sbSQL.AppendFormat("select @moveRestrictValue={1} from {0} where Id={2};", tableName, restrictName, Id);
                sbSQL.AppendFormat("select @desRestrictValue={1} from {0} where Id={2};", tableName, restrictName, moveToParentId);
                sbSQL.AppendFormat("select case when @moveRestrictValue=@desRestrictValue then 1 else 0 end;");

                bool canMove = 1 == this.dbContext.Database.SqlQuery(typeof(int), sbSQL.ToString()).Cast<int>().FirstOrDefault();
                if (!canMove)
                {
                    throw new Exception("当前对象与目标对象不在同一个限制域内，移动操作无法完成");
                }
            }


            //判断当前节点是否被移动到其他节点下，如果父节点没有发生改变，则不进行任何操作
            sbSQL.Clear();
            sbSQL.AppendFormat("declare @parentId bigint; set @parentId=0;");
            sbSQL.AppendFormat("select @parentId=Tree_ParentId from {0} where Id={1};", tableName, Id);
            sbSQL.AppendFormat("select @parentId;");

            long moveParentId = this.dbContext.Database.SqlQuery(typeof(long), sbSQL.ToString()).Cast<long>().FirstOrDefault();
            if (moveParentId == moveToParentId)
            {
                return;
            }

            //移动操作对象后，将其Order_Index设置为最大值，排到序列最后
            sbSQL.Clear();
            sbSQL.AppendFormat("declare @restrictValue bigint; set @restrictValue=0;");
            sbSQL.AppendFormat("declare @moveOrderIndex int; set @moveOrderIndex=0;");
            sbSQL.AppendFormat("declare @moveParentId bigint; set @moveParentId=0;");
            sbSQL.AppendFormat("select @moveOrderIndex=Order_Index, @moveParentId=Tree_ParentId, @restrictValue={2} from {0} where Id={1};", tableName, Id, restrictName);
            sbSQL.AppendFormat("update {0} set Order_Index=Order_Index-1 where {1}=@restrictValue and Tree_ParentId=@moveParentId and Order_Index>@moveOrderIndex;", tableName, restrictName);
            sbSQL.AppendFormat("");
            sbSQL.AppendFormat("declare @desMaxOrderIndex int; set @desMaxOrderIndex=0;");
            sbSQL.AppendFormat("select @desMaxOrderIndex=isnull(Max(Order_Index), 0)+1 from {0} where Tree_ParentId={1};", tableName, moveToParentId);
            sbSQL.AppendFormat("update {0} set Tree_ParentId={1}, Order_Index=@desMaxOrderIndex where Id={2};", tableName, moveToParentId, Id);
            //更新数据中的Tree_Level层级数值
            sbSQL.AppendFormat("declare @tb_level table(Id bigint);");
            sbSQL.AppendFormat("with cte as(select Id, Tree_ParentId from {0} where Id = {1} union all select b.Id, b.Tree_ParentId from cte a inner join {0} b on a.Id=b.Tree_ParentId) insert into @tb_level select Id from cte;", tableName, Id);
            sbSQL.AppendFormat("declare cur_level cursor for(select Id from @tb_level);");
            sbSQL.AppendFormat("open cur_level;");
            sbSQL.AppendFormat("declare @level_id bigint;");
            sbSQL.AppendFormat("fetch next from cur_level into @level_id;");
            sbSQL.AppendFormat("while @@FETCH_STATUS = 0 begin;");
            sbSQL.AppendFormat("update a set a.Tree_Level = case a.Tree_ParentId when 0 then 0 else (select b.Tree_Level+1 from {0} b where b.Id = a.Tree_ParentId) end from {0} a where a.Id = @level_id;", tableName);
            sbSQL.AppendFormat("fetch next from cur_level into @level_id;");
            sbSQL.AppendFormat("end;");
            sbSQL.AppendFormat("close cur_level;");
            sbSQL.AppendFormat("deallocate cur_level;");

            //更新Tree_Level的SQL语句原型
            /*
            update Department set Tree_ParentId = 0 where Id = 2

            declare @tb_level table(Id bigint);
            with cte as(select Id, Tree_ParentId from Department where Id = 2 union all select b.Id, b.Tree_ParentId from cte a inner join Department b on a.Id=b.Tree_ParentId) insert into @tb_level select Id from cte;

            declare cur_level cursor for(select Id from @tb_level);
            open cur_level;
            declare @level_id bigint;
            fetch next from cur_level into @level_id;
            while @@FETCH_STATUS = 0 begin;
	            update a set a.Tree_Level = case a.Tree_ParentId when 0 then 0 else (select b.Tree_Level+1 from Department b where b.Id = a.Tree_ParentId) end from Department a where a.Id = @level_id;
	            fetch next from cur_level into @level_id;
            end;
            close cur_level;
            deallocate cur_level; 
            */


            this.dbContext.Database.ExecuteSqlCommand(sbSQL.ToString());
        }


        #endregion




        #region 计算Order模型中的OrderIndex和Tree模型中的TreeLevel


        /// <summary>
        /// 计算新增树节点在当前树种的节点层次
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ParentId">父节点索引，根节点为0</param>
        /// <returns></returns>
        protected int createNewTreeLevel<TEntity>(long ParentId)
            where TEntity : BaseEntityTree
        {
            int newTreeLevel = 0;
            if (ParentId == 0)
            {
                return newTreeLevel;
            }
            try
            {
                string tableName = typeof(TEntity).Name;
                string sql = string.Format("select Tree_Level from {0} where Id={1}", tableName, ParentId);
                newTreeLevel = this.dbContext.Database.SqlQuery(typeof(int), sql).Cast<int>().FirstOrDefault();
                newTreeLevel++;
                return newTreeLevel;
            }
            catch (Exception)
            {
                return newTreeLevel;
            }
        }


        /// <summary>
        /// 计算新增数据的排序索引：基本模型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        protected int createNewOrderIndex<TEntity>()
            where TEntity : BaseEntityOrder
        {
            int newOrderIndex = 0;
            try
            {
                string tableName = typeof(TEntity).Name;
                string sql = string.Format("select max(Order_Index) from {0}", tableName);
                newOrderIndex = this.dbContext.Database.SqlQuery(typeof(int), sql).Cast<int>().FirstOrDefault();
                newOrderIndex++;
                return newOrderIndex;
            }
            catch (Exception)
            {
                newOrderIndex++;
                return newOrderIndex;
            }
        }

        /// <summary>
        /// 计算新增数据的排序索引：树节点模型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="parentId">限定在某一个父节点之下</param>
        /// <returns></returns>
        protected int createNewOrderIndex<TEntity>(long parentId)
            where TEntity : BaseEntityTree
        {
            int newOrderIndex = 0;
            try
            {
                string tableName = typeof(TEntity).Name;
                string sql = string.Format("select max(Order_Index) from {0} where Tree_ParentId={1}", tableName, parentId);
                newOrderIndex = this.dbContext.Database.SqlQuery(typeof(int), sql).Cast<int>().FirstOrDefault();
                newOrderIndex++;
                return newOrderIndex;
            }
            catch (Exception)
            {
                newOrderIndex++;
                return newOrderIndex;
            }
        }

        /// <summary>
        /// 计算新增数据的排序索引：限定范围的模型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dependPropertyName">依赖于某一个范围字段，一般为公司索引或部门索引之类的</param>
        /// <param name="dependPropertyValue">依赖字段的值，如公司索引值</param>
        /// <returns></returns>
        protected int createNewOrderIndex<TEntity>(string dependPropertyName, long dependPropertyValue)
            where TEntity : BaseEntityOrder
        {
            int newOrderIndex = 0;
            try
            {
                string tableName = typeof(TEntity).Name;
                string sql = string.Format("select max(Order_Index) from {0} where {1}={2}", tableName, dependPropertyName, dependPropertyValue);
                newOrderIndex = this.dbContext.Database.SqlQuery(typeof(int), sql).Cast<int>().FirstOrDefault();
                newOrderIndex++;
                return newOrderIndex;
            }
            catch (Exception)
            {
                newOrderIndex++;
                return newOrderIndex;
            }
        }

        /// <summary>
        /// 计算新增数据的排序索引：限定范围 + 树节点模型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dependPropertyName">依赖于某一个范围字段，一般为公司索引或部门索引之类的</param>
        /// <param name="dependPropertyValue">依赖字段的值，如公司索引值</param>
        /// <param name="parentId">限定在某一个父节点之下</param>
        /// <returns></returns>
        protected int createNewOrderIndex<TEntity>(string dependPropertyName, long dependPropertyValue, long parentId)
            where TEntity : BaseEntityTree
        {
            int newOrderIndex = 0;
            try
            {
                string tableName = typeof(TEntity).Name;
                string sql = string.Format("select max(Order_Index) from {0} where {1}={2} and Tree_ParentId={3}", tableName, dependPropertyName, dependPropertyValue, parentId);
                newOrderIndex = this.dbContext.Database.SqlQuery(typeof(int), sql).Cast<int>().FirstOrDefault();
                newOrderIndex++;
                return newOrderIndex;
            }
            catch (Exception)
            {
                newOrderIndex++;
                return newOrderIndex;
            }
        }


        #endregion



        #endregion

    }
}
