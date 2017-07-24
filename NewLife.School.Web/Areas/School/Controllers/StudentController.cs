using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewLife.Cube;
using NewLife.School;
using NewLife.School.Entity;
using NewLife.Web;
using XCode;

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

        protected override EntityList<Student> FindAll(Pager p)
        {
            //return base.FindAll(p);
            var classid = p["classid"].ToInt();
            return Student.Search(classid, DateTime.MinValue, DateTime.MinValue, p["q"], p);
        }
    }
}