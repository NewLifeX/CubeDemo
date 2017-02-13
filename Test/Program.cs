using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Log;
using NewLife.School.Entity;
using NewLife.Security;
using XCode;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            XTrace.UseConsole();

            try
            {
                TestInsert();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("OK!");
            Console.ReadKey(true);
        }

        static void TestInsert()
        {
            // 关闭SQL日志
            XCode.Setting.Current.ShowSQL = false;

            Console.WriteLine(Trade.Meta.Count);

            // 准备数据
            var list = new List<Trade>();
            Console.Write("正在准备数据：");
            for (int i = 0; i < 100000; i++)
            {
                if (i % 1000 == 0) Console.Write(".");

                var td = new Trade();
                foreach (var item in Trade.Meta.Fields)
                {
                    if (item.IsIdentity) continue;

                    if (item.Type == typeof(Int32))
                        td.SetItem(item.Name, Rand.Next());
                    else if (item.Type == typeof(String))
                        td.SetItem(item.Name, Rand.NextString(8));
                }
                list.Add(td);
            }
            Console.WriteLine();
            Console.WriteLine("数据准备完毕！");

            var sw = new Stopwatch();
            sw.Start();

            Console.Write("正在准备写入：");
            EntityTransaction tr = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 1000 == 0)
                {
                    Console.Write(".");
                    if (tr != null) tr.Commit();

                    tr = Trade.Meta.CreateTrans();
                }

                list[i].SaveWithoutValid();
            }
            if (tr != null) tr.Commit();

            sw.Stop();
            Console.WriteLine("数据写入完毕！");
            var ms = sw.ElapsedMilliseconds;
            Console.WriteLine("耗时：{0:n0}ms 平均速度：{1:n0}tps", ms, list.Count * 1000L / ms);
        }
    }
}