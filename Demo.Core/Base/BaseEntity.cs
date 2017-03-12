using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Demo.Core
{
    /// <summary>
    /// ���ݿ�ʵ��Ļ���
    /// </summary>
    public abstract partial class BaseEntity
    {
        /// <summary>
        /// Ĭ�ϵ�������ֶ�
        /// </summary>
        [NotMapped]
        public static string SortField { get { return "DateCreated"; } }

        /// <summary>
        /// ������ʶ(��������)
        /// </summary>
        [Required]
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ������(��������)
        /// </summary>
        [MaxLength(32)]
        public string Creator { get; set; }

        /// <summary>
        /// �޸���(��������)
        /// </summary>
        [MaxLength(32)]
        public string Modifier { get; set; }

        /// <summary>
        /// ����ʱ��(��������)
        /// </summary>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// �޸�ʱ��(��������)
        /// </summary>
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// �Ƿ�ɾ�����߼�ɾ����
        /// </summary>
        [MaxLength(8)]
        public string IsDeleted { get; set; } = "N";

        /// <summary>
        /// �汾
        /// </summary>
        [MaxLength(128)]
        public string Version { get; set; }

        /// <summary>
        /// �ж����������Ƿ�һ��
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
        /// ͨ��ID��֤���������Ƿ�һ��
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
        /// ��ȡHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Equals(ID, default(int)))
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        /// <summary>
        /// �ж�����ʵ���Ƿ����
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// �ж�����ʵ���Ƿ����
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
