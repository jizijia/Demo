using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Core.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        /// <summary>
        /// 通过主键ID获取对象实体
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetByID(int id);

        /// <summary>
        /// 通过指定的条件表达式获取一个对象实体
        /// </summary>
        /// <param name="where">条件表达式</param>
        /// <returns></returns>
        T GetByCondition(Expression<Func<T, bool>> where);

        /// <summary>
        /// 通过指定实体插入数据;
        /// </summary>
        /// <param name="entity">实体</param>
        T Insert(T entity);

        /// <summary>
        /// 通过指定实体列表批量插入数据;
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns></returns>
        IEnumerable<T> Insert(IEnumerable<T> entities);

        /// <summary>
        /// 通过指定实体修改数据
        /// </summary>
        /// <param name="entity">实体</param>
        T Update(T entity);

        /// <summary>
        /// 批量修改指定条件下的对象
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <param name="expre">表达式</param>
        void UpdateSetByCondition(Expression<Func<T, bool>> where, Func<T, T> expre);

        /// <summary>
        /// 批量修改指定条件下的对象
        /// </summary>
        /// <param name="list">过滤条件</param>
        /// <param name="expre">表达式</param>
        void UpdateSetList(List<T> list, Func<T, T> expre);

        /// <summary>
        /// 通过指定实体列表批量修改数据;
        /// </summary>
        /// <param name="entities">实体列表</param>
        IEnumerable<T> Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// 通过指定实体列表批量删除数据;
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns></returns>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="idList">Entities' id</param>
        void Delete(int[] idList);

        /// <summary>
        /// 通过指定主键编号删除数据;
        /// </summary>
        /// <param name="sysNo">主键编号</param>
        void Delete(object sysNo);

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> where);

        /// <summary>
        /// 根据给定条件获取符合条件的记录数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序字段名</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> where, string orderBy);

        ///// <summary>
        ///// 根据给定条件获取符合条件的记录数量
        ///// </summary>
        ///// <param name="strWhere">查询条件</param>
        ///// <returns></returns>
        //int Count(string strWhere);

        ///// <summary>
        ///// 根据给定条件获取符合条件的记录数量
        ///// </summary>
        ///// <param name="strWhere">查询条件</param>
        ///// <param name="orderBy"></param>
        ///// <returns></returns>
        //int Count(string strWhere, string orderBy);

        ///// <summary>
        ///// 判断是否存在任何记录
        ///// </summary>
        ///// <returns></returns>
        //bool Exists();

        /// <summary>
        /// 根据给定条件判断是否存在符合条件的记录
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        bool Exists(Expression<Func<T, bool>> where);

        ///// <summary>
        ///// 获取数据实体列表(OK)
        ///// </summary>
        ///// <returns></returns>
        //List<T> GetList();

        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> where);

        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        List<T> GetList(string strWhere);

        /// <summary>
        /// 根据给定条件获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> where, string orderBy);

        /// <summary>
        /// 获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <returns></returns>
        List<T> GetList(string strWhere, string orderBy);

        /// <summary>
        /// 分页获取数据实体列表
        /// </summary>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        List<T> GetPagedList(int startRowIndex, int maximumRows);

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        List<T> GetPagedList(Expression<Func<T, bool>> where, int startRowIndex, int maximumRows);

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        List<T> GetPagedList(string strWhere, int startRowIndex, int maximumRows);

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        List<T> GetPagedList(Expression<Func<T, bool>> where, string orderBy, int startRowIndex, int maximumRows);

        /// <summary>
        /// 根据给定条件分页获取数据实体列表
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="startRowIndex">起始记录号</param>
        /// <param name="maximumRows">每页记录数</param>
        /// <returns></returns>
        List<T> GetPagedList(string strWhere, string orderBy, int startRowIndex, int maximumRows);

        ///// <summary>
        ///// 获取指定个数排名靠前的实体列表
        ///// </summary>
        ///// <param name="top">个数</param>
        ///// <returns></returns>
        //List<T> Top(int top);

        ///// <summary>
        ///// 获取指定个数排名靠前的实体列表
        ///// </summary>
        ///// <param name="top">个数</param>
        ///// <returns></returns>
        //List<T> Top(Expression<Func<T, bool>> where, int top);

        ///// <summary>
        ///// 根据给定条件获取符合条件指定个数排名靠前的实体列表
        ///// </summary>
        ///// <param name="where">查询条件</param>
        ///// <param name="top">个数</param>
        ///// <returns></returns>
        //List<T> Top(Expression<Func<T, bool>> where, string orderBy, int top);

        ///// <summary>
        ///// 根据给定条件获取符合条件的第一个实体(OK)
        ///// </summary>
        ///// <param name="where">查询条件</param>
        ///// <returns></returns>
        //T TopOne(Expression<Func<T, bool>> where);

        ///// <summary>
        ///// 根据给定条件获取符合条件的首个实体(OK)
        ///// </summary>
        ///// <param name="where">查询条件</param>
        ///// <param name="orderBy">排序表达式</param>
        ///// <returns></returns>
        //T TopOne(Expression<Func<T, bool>> where, string orderBy);
    }
}
