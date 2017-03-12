using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Demo.Core
{
    /// <summary>
    /// 数据库实体的基类
    /// </summary>
    public abstract partial class BaseEntity
    {
        /// <summary>
        /// 默认的排序的字段
        /// </summary>
        [NotMapped]
        public static string SortField { get { return "DateCreated"; } }

        /// <summary>
        /// 自增标识(基类属性)
        /// </summary>
        [Required]
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 创建人(基类属性)
        /// </summary>
        [MaxLength(32)]
        public string Creator { get; set; }

        /// <summary>
        /// 修改人(基类属性)
        /// </summary>
        [MaxLength(32)]
        public string Modifier { get; set; }

        /// <summary>
        /// 创建时间(基类属性)
        /// </summary>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// 修改时间(基类属性)
        /// </summary>
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// 是否删除（逻辑删除）
        /// </summary>
        [MaxLength(8)]
        public string IsDeleted { get; set; } = "N";

        /// <summary>
        /// 版本
        /// </summary>
        [MaxLength(128)]
        public string Version { get; set; }

        /// <summary>
        /// 判断两个对象是否一致
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.ID, default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        /// <summary>
        /// 通过ID验证两个对象是否一致
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(ID, other.ID))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        /// <summary>
        /// 获取HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Equals(ID, default(int)))
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        /// <summary>
        /// 判断两个实例是否相等
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// 判断两个实例是否相等
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}
