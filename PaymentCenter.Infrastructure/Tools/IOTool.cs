using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace PaymentCenter.Infrastructure.Tools
{
    public class IOTool
    {

        /// <summary>
        /// 拷贝目录文件
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(srcDir);
            DirectoryInfo directoryInfo1 = new DirectoryInfo(tgtDir);
            if (directoryInfo1.FullName.StartsWith(directoryInfo.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }
            if (directoryInfo.Exists)
            {
                if (!directoryInfo1.Exists)
                {
                    directoryInfo1.Create();
                }
                FileInfo[] files = directoryInfo.GetFiles();
                for (int i = 0; i < (int)files.Length; i++)
                {
                    File.Copy(files[i].FullName, string.Concat(directoryInfo1.FullName, "\\", files[i].Name), true);
                }
                DirectoryInfo[] directories = directoryInfo.GetDirectories();
                for (int j = 0; j < (int)directories.Length; j++)
                {
                    IOTool.CopyDirectory(directories[j].FullName, string.Concat(directoryInfo1.FullName, "\\", directories[j].Name));
                }
            }
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="filepath"></param>
        public static void CreateDirectory(string filepath)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(filepath);
                if (directoryName != null && !Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(string.Concat("路径", filepath), exception);
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadTextFile(string path)
        {
            return File.ReadAllText(path);
        }

        private static string ToCsvSafeString(string s)
        {
            s = s ?? "";
            bool flag = s.Contains(",");
            bool flag1 = s.Contains("\"");
            if (flag1)
            {
                s = s.Replace("\"", "\"\"");
            }
            if ((flag || flag1))
            {
                s = string.Concat("\"", s, "\"");
            }
            return s;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="append">是否追加到文件</param>
        /// <param name="line">是否新起一行</param>
        public static void WriteTextFile(string path, string content, bool append = false, bool line = true)
        {
            if (append)
            {
                if (line)
                    File.AppendAllLines(path, new List<string>() { content });
                else
                    File.AppendAllText(path, content);
            }
            else
                File.WriteAllText(path, content);
        }

    }
}
