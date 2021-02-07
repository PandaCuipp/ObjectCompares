using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;


/*
* 项目名称 ：ObjectCompare
* 类 名 称 ：ObjectHelper
* 命名空间 ：ObjectCompare
* 机器名称 ：CUIPP 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：cuipp
* 创建时间 ：2018-04-28 12:06:56
*/
namespace ObjectCompare
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// 比较两个对象的属性值和字段值是否全部相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Compare<T>(T obj1, T obj2, Type type)
        {
            return CompareProperties(obj1, obj2, type) && CompareFields(obj1, obj2, type);
        }

        public static bool Compare(object obj1, object obj2)
        {
            //将对象序列化成内存中的二进制流  
            BinaryFormatter inputFormatter = new BinaryFormatter();
            MemoryStream inputStream;
            MemoryStream inputStream2;
            using (inputStream = new MemoryStream())
            {
                inputFormatter.Serialize(inputStream, obj1);
            }
            using (inputStream2 = new MemoryStream())
            {
                inputFormatter.Serialize(inputStream2, obj2);
            }
            string md5_1 = HashHelper.MD5Encrypt(inputStream.ToArray());
            string md5_2 = HashHelper.MD5Encrypt(inputStream2.ToArray());

            return (md5_1 == md5_2);
        }

        /// <summary>
        /// 判断两个相同引用类型的对象的属性值是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1">对象1</param>
        /// <param name="obj2">对象2</param>
        /// <param name="type">按type类型中的属性进行比较</param>
        /// <returns></returns>
        public static bool CompareProperties<T>(T obj1, T obj2, Type type)
        {
            //为空判断
            if (obj1 == null && obj2 == null)
                return true;
            else if (obj1 == null || obj2 == null)
                return false;

            Type t = type;

            PropertyInfo[] props = t.GetProperties();
            foreach (var po in props)
            {
                if (IsCanCompare(po.PropertyType))
                {
                    if (!po.GetValue(obj1).Equals(po.GetValue(obj2)))
                    {
                        return false;
                    }
                }
                else
                {
                    var b = CompareProperties(po.GetValue(obj1), po.GetValue(obj2), po.PropertyType);
                    if(!b) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断两个相同引用类型的对象的字段值是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1">对象1</param>
        /// <param name="obj2">对象2</param>
        /// <param name="type">按type类型中的字段进行比较</param>
        /// <returns></returns>
        public static bool CompareFields<T>(T obj1, T obj2, Type type)
        {
            //为空判断
            if (obj1 == null && obj2 == null)
                return true;
            else if (obj1 == null || obj2 == null)
                return false;

            Type t = type;

            FieldInfo[] fields = t.GetFields();
            foreach (var fd in fields)
            {
                if (IsCanCompare(fd.FieldType))
                {
                    if (!fd.GetValue(obj1).Equals(fd.GetValue(obj2)))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!CompareFields(fd.GetValue(obj1), fd.GetValue(obj2), fd.FieldType))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 该类型是否可直接进行值的比较
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsCanCompare(Type t)
        {
            if (t.IsValueType)
            {
                return true;
            }
            else
            {
                //String是特殊的引用类型，它可以直接进行值的比较
                if (t.FullName == typeof(String).FullName)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
