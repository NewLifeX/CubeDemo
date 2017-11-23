using System;
using System.Collections.Generic;
using NewLife.Cube;
using NewLife.School.Entity;
using NewLife.Web;

namespace NewLife.School.Web.Areas.School.Controllers
{
    public class StudentController : EntityController<Student>
    {
        static StudentController()
        {
            ListFields.RemoveField("CreateUserID");
            ListFields.RemoveField("UpdateUserID");
            //FormFields
        }

        protected override Student Find(Object key)
        {
            return base.Find(key);
        }

        protected override IEnumerable<Student> Search(Pager p)
        {
            var classid = p["classid"].ToInt();
            return Student.Search(classid, p["dtStart"].ToDateTime(), p["dtEnd"].ToDateTime(), p["q"], p);
        }
    }
}