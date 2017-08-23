/*
 * XCoder v6.9.6229.20370
 * 作者：Stone/X2
 * 时间：2017-01-23 16:02:44
 * 版权：版权所有 (C) 新生命开发团队 2002~2017
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using NewLife.Data;
using NewLife.Web;
using XCode;
using XCode.Membership;

namespace NewLife.School.Entity
{
    /// <summary>学生</summary>
    public partial class Student : Entity<Student>
    {
        #region 对象操作
        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 如果没有脏数据，则不需要进行任何处理
            if (!HasDirty) return;

            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);

            if (isNew && !Dirtys[__.CreateTime]) CreateTime = DateTime.Now;
            if (!Dirtys[__.CreateIP]) CreateIP = WebHelper.UserHost;
            if (!Dirtys[__.UpdateTime]) UpdateTime = DateTime.Now;
            if (!Dirtys[__.UpdateIP]) UpdateIP = WebHelper.UserHost;
        }
        #endregion

        #region 扩展属性
        /// <summary>该学生所对应的班级</summary>
        [XmlIgnore]
        public Class Class { get { return Extends.Get(nameof(Class), k => Class.FindByID(ClassID)); } }

        /// <summary>该学生所对应的班级名称</summary>
        [XmlIgnore]
        [DisplayName("班级")]
        [Map(__.ClassID, typeof(Class), "ID")]
        public String ClassName { get { return Class?.Name; } }

        /// <summary>性别</summary>
        [DisplayName("性别")]
        [Map(__.Sex)]
        public SexKinds SexKind { get { return (SexKinds)Sex; } set { Sex = (Int32)value; } }
        #endregion

        #region 扩展查询
        public static Student FindByID(Int32 id)
        {
            if (id <= 0) return null;

            // 实体缓存
            if (Meta.Count < 1000) return Meta.Cache.Entities.FirstOrDefault(e => e.ID == id);

            // 单对象缓存
            return Meta.SingleCache[id];
        }

        public static Student FindByName(String name)
        {
            return Find(__.Name, name);
        }

        /// <summary>根据班级查找</summary>
        /// <param name="classid">班级</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IList<Student> FindAllByClassID(Int32 classid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.ClassID, classid);
            else // 实体缓存
                return Meta.Cache.Entities.Where(e => e.ClassID == classid).ToList();
        }

        #endregion

        #region 高级查询
        /// <summary>查询满足条件的记录集，分页、排序</summary>
        /// <param name="classid">班级编号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="key">关键字</param>
        /// <param name="param">分页排序参数，同时返回满足条件的总记录数</param>
        /// <returns>实体集</returns>
        public static IList<Student> Search(Int32 classid, DateTime start, DateTime end, String key, PageParameter param)
        {
            var exp = new WhereExpression();
            if (classid > 0) exp &= _.ClassID == classid;
            if (!key.IsNullOrEmpty()) exp &= _.Name.Contains(key);

            exp &= _.UpdateTime.Between(start, end);

            return FindAll(exp, param);
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        #endregion
    }
}