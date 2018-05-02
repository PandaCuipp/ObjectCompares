using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClass c1 = GetModelTestClass();
            TestClass c2 = GetModelTestClass();
            c2.OtherCalss.Param1 = "para1 modify";

            //深度比较1：各属性字段这个比较
            //bool result = ObjectHelper.Compare<TestClass>(c1, c2, typeof(TestClass));

            //深度比较2：先序列化为二进制数组，然后md5，进行比较
            bool result = ObjectHelper.Compare(c1, c2);
            Console.WriteLine(result);

            Console.ReadKey();
        }

        private static TestClass GetModelTestClass()
        {
            TestClass model = new TestClass();
            model.ID = 1;
            model.Name = "Panda";
            model.OtherCalss.Param1 = "Test Param1";
            model.OtherCalss.Param2 = "Test Param2";
            return model;
        }
    }


    [Serializable]
    public class TestClass
    {
        public TestClass()
        {
            OtherCalss = new ExClass();
        }
        public int ID;

        public string Name;

        public ExClass OtherCalss;

        public ExClass2 OtherCalss2;
    }
    [Serializable]
    public class ExClass
    {
        public string Param1;

        public string Param2;
    }
    [Serializable]
    public class ExClass2
    {
        public string P1 { get; set; }

        public string P2;
    }
}
