using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Demo.Core;
using Demo.Core.Caching;
using Demo.Core.Common;
using Demo.Core.Data;
using Demo.Core.Infrastructure;
using System.Linq.Dynamic;
using Demo.Core.Domain;
using Demo.Core.Base;

namespace Demo.Data
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields
        private readonly IDbContext _context;
        private IDbSet<T> _entities;
        private static readonly Type _nullableType = typeof(Nullable<>);
        private static readonly Type _baseEntityType = typeof(BaseEntity);
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            this._context = context;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        #endregion

        #region Insert/Update
        /// <summary>
        /// 通过指定实体插入数据;
        /// </summary>
        /// <param name="entity">实体</param>
        public T Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                Injection<T>(OpreateActionType.INSERT, entity);
                this.Entities.Add(entity);
                this._context.SaveChanges();
                return entity;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// 通过指定实体插入数据;
        /// </summary>
        public IEnumerable<T> Insert(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            foreach (var entity in entities)
            {
                Injection<T>(OpreateActionType.INSERT, entity);
                this.Entities.Add(entity);
            }
            this._context.SaveChanges();
            return entities;
        }

        /// <summary>
        /// 通过指定实体修改数据
        /// </summary>
        /// <param name="entity">实体</param>
        public virtual T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            Injection<T>(OpreateActionType.UPDATE, entity);
            this._context.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 通过指定实体列表批量修改数据
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns></returns>
        public virtual IEnumerable<T> Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            entities.Each(c =>
            {
                Injection<T>(OpreateActionType.UPDATE, c);
            });
            this._context.SaveChanges();
            return entities;
        }

        /// <summary>
        /// 保存数据
        /// 如果ID为空的话默认为插入，否则为更新
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Save(T entity)
        {
            if (entity.ID == 0)
            {
                Insert(entity);
            }
            else
            {
                Update(entity);
            }
        }

        /// <summary>
        /// 批量修改指定条件下的对象
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <param name="expre">表达式</param>
        public virtual void UpdateSetByCondition(Expression<Func<T, bool>> where, Func<T, T> expre)
        {
            List<T> list = GetList(where, null);
            foreach (var item in list)
            {
                Update(expre(item));
            }
        }

        /// <summary>
        /// 批量修改指定条件下的对象
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <param name="expre">表达式</param>
        public virtual void UpdateSetList(List<T> list, Func<T, T> expre)
        {
            foreach (var item in list)
            {
                Update(expre(item));
            }
        }
        #endregion

        #region Select
        /// <summary>
        /// 通过唯一标识(主键)获取一个对象实体
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>对象实体</returns>
        public virtual T GetByID(int id)
        {
            return this.Entities.Where(c => c.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// 通过指定的条件表达式获取一个对象实体
        /// </summary>
        /// <param name="where">条件表达式</param>
        /// <returns></returns>
        public virtual T GetByCondition(Expression<Func<T, bool>> where)
        {
            var query = this.Entities as IQueryable<T>;
            return query.Where(where).FirstOrDefault();
        }

        /// <summary>
        /// 判断是否存在任何记录
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return this.Exists(null);
        }

        /// <summary>
        /// 根据给定条件判断是否存在符合条件的记录
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> where)
        {
            var query = this.Entities as IQueryable<T>;
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.Any();
        }

        /// <summary>
        /// 获取数据实体列表(OK)
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return this.Entities.ToList();
        }

        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public virtual List<T> GetList(Expression<Func<T, bool>> where)
        {
            return GetList(where, null);
        }

        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public virtual List<T> GetList(string strWhere)
        {
            return GetList(strWhere, null);
        }

        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual List<T> GetList(Expression<Func<T, bool>> where, string orderBy)
        {
            var query = this.Entities as IQueryable<T>;

            if (where != null)
            {
                query = query.Where(where);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }
            return query.ToList();
        }

        /// <summary>
        /// 获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual List<T> GetList(string strWhere, string orderBy)
        {
            var query = this.Entities as IQueryable<T>;

            if (!string.IsNullOrEmpty(strWhere))
            {
                query = query.Where(strWhere);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }
            return query.ToList();
        }

        /// <summary>
        /// 分页获取数据实体列表
        /// </summary>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList(int startRowIndex, int maximumRows)
        {
            startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;
            return this.Entities.Skip(startRowIndex).Take(maximumRows).ToList();
        }

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList(Expression<Func<T, bool>> where, int startRowIndex, int maximumRows)
        {
            return GetPagedList(where, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList(string strWhere, int startRowIndex, int maximumRows)
        {
            return GetPagedList(strWhere, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList(Expression<Func<T, bool>> where, string orderBy, int startRowIndex, int maximumRows)
        {
            var query = this.Entities as IQueryable<T>;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = string.Format("{0} DESC", BaseEntity.SortField);
            }
            query = query.OrderBy(orderBy);
            startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;
            return query.Skip(startRowIndex).Take(maximumRows).ToList();
        }

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList(string strWhere, string orderBy, int startRowIndex, int maximumRows)
        {
            var query = this.Entities as IQueryable<T>;
            if (!string.IsNullOrEmpty(strWhere))
            {
                query = query.Where(strWhere);
            }
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = string.Format("{0} DESC", BaseEntity.SortField);
            }
            query = query.OrderBy(orderBy);
            startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;
            return query.Skip(startRowIndex).Take(maximumRows).ToList();
        }
        #endregion

        #region Top

        /// <summary>
        /// 获取指定个数排名靠前的实体列表
        /// </summary>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<T> Top(int top)
        {
            return this.Top(null, null, top);
        }

        /// <summary>
        /// 获取指定个数排名靠前的实体列表
        /// </summary>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<T> Top(Expression<Func<T, bool>> where, int top)
        {
            return this.Top(where, null, top);
        }

        /// <summary>
        /// 根据给定条件获取符合条件指定个数排名靠前的实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="top">个数</param>
        /// <returns></returns>
        public virtual List<T> Top(Expression<Func<T, bool>> where, string orderBy, int top)
        {
            var query = this.Entities as IQueryable<T>;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }
            return query.Take(top).ToList();
        }

        /// <summary>
        /// 根据给定条件获取符合条件的第一个实体(OK)
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public virtual T TopOne(Expression<Func<T, bool>> where)
        {
            return this.TopOne(where, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的首个实体(OK)
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        public virtual T TopOne(Expression<Func<T, bool>> where, string orderBy)
        {
            var query = this.Entities as IQueryable<T>;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy + " desc");
            }
            return query.Take(1).FirstOrDefault();
        }
        #endregion

        #region Delete

        public void Delete(object id)
        {
            T entity = this.Entities.Find(id);
            Delete(entity);
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            this.Entities.Remove(entity);
            this._context.SaveChanges();
        }

        /// <summary>
        /// 通过指定实体删除数据;
        /// </summary>
        /// <param name="entity">实体</param>
        public void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            foreach (var entity in entities)
                this.Entities.Remove(entity);
            this._context.SaveChanges();
        }

        public virtual void Delete(int[] idList)
        {
            if (idList == null)
                throw new ArgumentNullException("idList");
            List<T> list = new List<T>();
            T entity = null;
            foreach (var id in idList)
            {
                entity = GetByID(id);
                list.Add(entity);
            }
            Delete(list);
        }

        #endregion

        #region Count
        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Entities.Count();
        }

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> where)
        {
            return this.Count(where, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> where, string orderBy)
        {
            var query = this.Entities as IQueryable<T>;
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.Count();
        }

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int Count(string strWhere)
        {
            return this.Count(strWhere, null);
        }

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public int Count(string strWhere, string orderBy)
        {
            var query = this.Entities as IQueryable<T>;
            if (!string.IsNullOrEmpty(strWhere))
            {
                query = query.Where(strWhere);
            }
            return query.Count();
        }
        #endregion

        public void Injection<TEntity>(TEntity entity)
             where TEntity : BaseEntity
        {
            if (entity.ID == 0)
            {
                Injection(OpreateActionType.INSERT, entity);
            }
            else
            {
                Injection(OpreateActionType.UPDATE, entity);
            }
        }
        /// <summary>
        /// 插入必填字段信息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="action"></param>
        /// <param name="entity"></param>
        internal void Injection<TEntity>(OpreateActionType action, TEntity entity)
            where TEntity : BaseEntity
        {
            MemoryCacheManager manager = new MemoryCacheManager();
            string key = typeof(TEntity).FullName + action.ToString();
            List<PropertyInfo> list = null;

            if (!manager.IsSet(key))
            {
                #region 增加缓存
                if (list == null || list.Count < 0)
                {
                    PropertyInfo[] propertyList = typeof(TEntity).GetProperties();
                    list = new List<PropertyInfo>();
                    propertyList.Each(c =>
                    {
                        if (action == OpreateActionType.INSERT)
                        {
                            //c.Name == "ID" ||

                            if (c.Name == "DateCreated" ||
                                c.Name == "DateModified" ||
                                c.Name == "Version")
                            {
                                list.Add(c);
                            }
                        }
                        if (action == OpreateActionType.UPDATE)
                        {
                            if (c.Name == "DateModified" ||
                                c.Name == "Version")
                            {
                                list.Add(c);
                            }
                        }
                        if (c.PropertyType.IsGenericType)
                        {
                            list.Add(c);
                        }
                    });
                    manager.Set(key, list, 60);
                }
                #endregion
            }
            list = manager.Get<List<PropertyInfo>>(key);
            foreach (PropertyInfo field in list)
            {
                //if (field.Name == "ID")
                //{
                //    field.SetValue(entity, Guid.NewGuid().ToString("N"), null);
                //}
                if (field.Name == "DateCreated")
                {
                    field.SetValue(entity, DateTime.Now, null);
                }
                if (field.Name == "DateModified")
                {
                    field.SetValue(entity, DateTime.Now, null);
                }
                if (field.Name == "Version")
                {
                    field.SetValue(entity, DateTime.UtcNow.Ticks.ToString(), null);
                }
            }
        }

        [DefaultValue(false)]
        public static bool IsDebug { get; set; }
    }
    internal enum OpreateActionType
    {
        UPDATE,
        INSERT
    }
}