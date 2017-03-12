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
        /// ͨ��ָ��ʵ���������;
        /// </summary>
        /// <param name="entity">ʵ��</param>
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
        /// ͨ��ָ��ʵ���������;
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
        /// ͨ��ָ��ʵ���޸�����
        /// </summary>
        /// <param name="entity">ʵ��</param>
        public virtual T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            Injection<T>(OpreateActionType.UPDATE, entity);
            this._context.SaveChanges();
            return entity;
        }

        /// <summary>
        /// ͨ��ָ��ʵ���б������޸�����
        /// </summary>
        /// <param name="entities">ʵ���б�</param>
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
        /// ��������
        /// ���IDΪ�յĻ�Ĭ��Ϊ���룬����Ϊ����
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
        /// �����޸�ָ�������µĶ���
        /// </summary>
        /// <param name="where">��������</param>
        /// <param name="expre">���ʽ</param>
        public virtual void UpdateSetByCondition(Expression<Func<T, bool>> where, Func<T, T> expre)
        {
            List<T> list = GetList(where, null);
            foreach (var item in list)
            {
                Update(expre(item));
            }
        }

        /// <summary>
        /// �����޸�ָ�������µĶ���
        /// </summary>
        /// <param name="where">��������</param>
        /// <param name="expre">���ʽ</param>
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
        /// ͨ��Ψһ��ʶ(����)��ȡһ������ʵ��
        /// </summary>
        /// <param name="id">����ֵ</param>
        /// <returns>����ʵ��</returns>
        public virtual T GetByID(int id)
        {
            return this.Entities.Where(c => c.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// ͨ��ָ�����������ʽ��ȡһ������ʵ��
        /// </summary>
        /// <param name="where">�������ʽ</param>
        /// <returns></returns>
        public virtual T GetByCondition(Expression<Func<T, bool>> where)
        {
            var query = this.Entities as IQueryable<T>;
            return query.Where(where).FirstOrDefault();
        }

        /// <summary>
        /// �ж��Ƿ�����κμ�¼
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return this.Exists(null);
        }

        /// <summary>
        /// ���ݸ��������ж��Ƿ���ڷ��������ļ�¼
        /// </summary>
        /// <param name="where">��ѯ����</param>
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
        /// ��ȡ����ʵ���б�(OK)
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return this.Entities.ToList();
        }

        /// <summary>
        /// ���ݸ���������ȡ����ʵ���б�
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns></returns>
        public virtual List<T> GetList(Expression<Func<T, bool>> where)
        {
            return GetList(where, null);
        }

        /// <summary>
        /// ���ݸ���������ȡ����ʵ���б�
        /// </summary>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns></returns>
        public virtual List<T> GetList(string strWhere)
        {
            return GetList(strWhere, null);
        }

        /// <summary>
        /// ���ݸ���������ȡ����ʵ���б�
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="orderBy">������ʽ</param>
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
        /// ��ȡ����ʵ���б�
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="orderBy">������ʽ</param>
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
        /// ��ҳ��ȡ����ʵ���б�
        /// </summary>
        /// <param name="startRowIndex">��ʼ��¼��</param>
        /// <param name="maximumRows">ÿҳ��¼��</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList(int startRowIndex, int maximumRows)
        {
            startRowIndex = startRowIndex < 0 ? 0 : startRowIndex;
            return this.Entities.Skip(startRowIndex).Take(maximumRows).ToList();
        }

        /// <summary>
        /// ���ݸ���������ҳ��ȡ����ʵ���б�
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="startRowIndex">��ʼ��¼��</param>
        /// <param name="maximumRows">ÿҳ��¼��</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList(Expression<Func<T, bool>> where, int startRowIndex, int maximumRows)
        {
            return GetPagedList(where, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// ���ݸ���������ҳ��ȡ����ʵ���б�
        /// </summary>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="startRowIndex">��ʼ��¼��</param>
        /// <param name="maximumRows">ÿҳ��¼��</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList(string strWhere, int startRowIndex, int maximumRows)
        {
            return GetPagedList(strWhere, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// ���ݸ���������ҳ��ȡ����ʵ���б�
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="orderBy">������ʽ</param>
        /// <param name="startRowIndex">��ʼ��¼��</param>
        /// <param name="maximumRows">ÿҳ��¼��</param>
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
        /// ���ݸ���������ҳ��ȡ����ʵ���б�
        /// </summary>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="orderBy">������ʽ</param>
        /// <param name="startRowIndex">��ʼ��¼��</param>
        /// <param name="maximumRows">ÿҳ��¼��</param>
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
        /// ��ȡָ������������ǰ��ʵ���б�
        /// </summary>
        /// <param name="top">����</param>
        /// <returns></returns>
        public virtual List<T> Top(int top)
        {
            return this.Top(null, null, top);
        }

        /// <summary>
        /// ��ȡָ������������ǰ��ʵ���б�
        /// </summary>
        /// <param name="top">����</param>
        /// <returns></returns>
        public virtual List<T> Top(Expression<Func<T, bool>> where, int top)
        {
            return this.Top(where, null, top);
        }

        /// <summary>
        /// ���ݸ���������ȡ��������ָ������������ǰ��ʵ���б�
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="top">����</param>
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
        /// ���ݸ���������ȡ���������ĵ�һ��ʵ��(OK)
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns></returns>
        public virtual T TopOne(Expression<Func<T, bool>> where)
        {
            return this.TopOne(where, null);
        }

        /// <summary>
        /// ���ݸ���������ȡ�����������׸�ʵ��(OK)
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="orderBy">������ʽ</param>
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
        /// ͨ��ָ��ʵ��ɾ������;
        /// </summary>
        /// <param name="entity">ʵ��</param>
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
        /// ��ȡ�ܼ�¼��
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Entities.Count();
        }

        /// <summary>
        /// ���ݸ���������ȡ���������ļ�¼����
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> where)
        {
            return this.Count(where, null);
        }

        /// <summary>
        /// ���ݸ���������ȡ���������ļ�¼����
        /// </summary>
        /// <param name="where">��ѯ����</param>
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
        /// ���ݸ���������ȡ���������ļ�¼����
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns></returns>
        public int Count(string strWhere)
        {
            return this.Count(strWhere, null);
        }

        /// <summary>
        /// ���ݸ���������ȡ���������ļ�¼����
        /// </summary>
        /// <param name="strWhere">��ѯ����</param>
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
        /// ��������ֶ���Ϣ
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
                #region ���ӻ���
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