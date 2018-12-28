using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace HGMark.Common.CommunicationAnalysis
{
    static public   class CommonUtil
    {
        internal static int FOffset(Type type, string field)
        {
            FieldInfo f = type.GetField(field);
            if (f == null)
            {
                throw new Exception("Invalid field");
            }
            else
            {
                return (int)Marshal.OffsetOf(type, field);
            }
        }

        internal static int FSize(Type type, string field)
        {
            FieldInfo f = type.GetField(field);
            if (f == null)
            {
                throw new Exception("Invalid field");
            }
            else
            {
                return Marshal.SizeOf(f.FieldType);
            }
        }


        //obj转换为Byte数组类型
        internal static byte[] ObjectToBytes(object structObj, int size)
        {
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);

            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, true);

            //从内存空间拷贝到byte 数组
            Marshal.Copy(structPtr, bytes, 0, size);

            //释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return bytes;
        }


        //将结构体转换为Byte数组类型
        internal static byte[] StructToBytes(object structObj, int size)
        {
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);

            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, true);

            //从内存空间拷贝到byte 数组
            Marshal.Copy(structPtr, bytes, 0, size);

            //释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return bytes;
        }

        //将Byte转换为结构体类型
        internal static object ByteToStruct(byte[] bytes, int size, Type type)
        {

            if (bytes.Length != size)
            {
                return null;
            }

            //分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);

            //将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);

            //将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);

            //释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return obj;
        }

        internal static unsafe object BytePtrToStruct(byte* pdata, Type structType)
        {
            int size = Marshal.SizeOf(structType);
            byte[] buf = new byte[size];
            for (int i = 0; i < size; i++)
            {
                buf[i] = *(pdata + i);
            }

            return ByteToStruct(buf, size, structType);
        }

        public static void ToLog(string LogFile, Exception ex)
        {
            if (LogFile != "")
            {
                string strErr = string.Format("Catch Time:  {0}\r\n", DateTime.Now.ToString());
                strErr += string.Format("Exception Message:\r\n  {0}\r\n", ex.Message);

                if (ex.InnerException != null)
                {
                    strErr += string.Format("  {0}\r\n", ex.InnerException.Message);
                }

                string strTrace = ex.StackTrace;
                string sNamespace = MethodBase.GetCurrentMethod().DeclaringType.Namespace;
                int nIndex = strTrace.IndexOf(sNamespace);
                if (nIndex != -1)
                {
                    strTrace = strTrace.Substring(nIndex);
                }
                strErr += string.Format("Exception Trace:\r\n  {0}\r\n\r\n", strTrace);

                ToFile(LogFile, strErr);
            }
        }

        private static void ToFile(string fileName, string content)
       {
            FileStream fs = new FileStream(fileName,
                    FileMode.Append,
                    FileAccess.Write,
                    FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);

            //开始写入  
            sw.Write(content);

            //清空缓冲区  
            sw.Flush();
            //关闭流  
            sw.Close();

            fs.Close();
        }
       //大端小端转换
        public static short SwapInt16(this short n)
        {
            return (short)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
        }

        public static ushort SwapUInt16(this ushort n)
        {
            return (ushort)(((n & 0xff) << 8) | ((n >> 8) & 0xff));
        }

        public static int SwapInt32(this int n)
        {
            return (int)(((SwapInt16((short)n) & 0xffff) << 0x10) |
                          (SwapInt16((short)(n >> 0x10)) & 0xffff));
        }

        public static uint SwapUInt32(this uint n)
        {
            return (uint)(((SwapUInt16((ushort)n) & 0xffff) << 0x10) |
                           (SwapUInt16((ushort)(n >> 0x10)) & 0xffff));
        }

        public static long SwapInt64(this long n)
        {
            return (long)(((SwapInt32((int)n) & 0xffffffffL) << 0x20) |
                           (SwapInt32((int)(n >> 0x20)) & 0xffffffffL));
        }

        public static ulong SwapUInt64(this ulong n)
        {
            return (ulong)(((SwapUInt32((uint)n) & 0xffffffffL) << 0x20) |
                            (SwapUInt32((uint)(n >> 0x20)) & 0xffffffffL));
        }


        internal static void SwapField(object obj)
        {
            FieldInfo[] fs = obj.GetType().GetFields();

            foreach (var field in fs)
            {
                object value = field.GetValue(obj);

                if (value == null)
                {
                    continue;
                }

                if (field.FieldType.IsPrimitive)	//判断是否为原始类型
                {
                    if (field.FieldType.Name == "Int16")
                    {
                        field.SetValue(obj, SwapInt16((Int16)value));
                    }
                    else if (field.FieldType.Name == "UInt16")
                    {
                        field.SetValue(obj, SwapUInt16((UInt16)value));
                    }
                    else if (field.FieldType.Name == "Int32")
                    {
                        field.SetValue(obj, SwapInt32((Int32)value));
                    }
                }
                else    //为自定义类型
                {
                    SwapField(value);
                }
            }
        }

        #region UnUsed
        // 
        // 		public static UInt16 GetStructFieldValue(byte[] bytes, Type structType, string field)
        // 		{
        // 			int offset = FOffset(structType, field);
        // 
        // 			if (offset == -1)
        // 			{
        // 				throw new Exception("Invalid field");
        // 			}
        // 			else
        // 			{
        // 				return BitConverter.ToUInt16(bytes, offset);
        // 			}
        // 		}

        #endregion




    }
}
