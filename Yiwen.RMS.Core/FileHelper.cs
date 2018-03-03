using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;



namespace Yiwen.RMS.Core
{


    /// <summary>
    /// 上传文件
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 图片文件类型
        /// </summary>
        public const string ImgTypes = ".jpg,.jpeg,.gif,.png,.bmp";
        /// <summary>
        /// 文件最大值
        /// </summary>
        public const int MaxSize = 4000 * 1024;




        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">需要上传的文件</param>
        /// <param name="path">文件上传保存到服务的路径，如果使用默认值空的话，则会保存在 项目根目录+file 文件夹下</param>
        /// <param name="FileType">上传文件的类型，默认是图片类型</param>
        /// <param name="Maxsize">上传问题的大小，默认4m</param>
        /// <returns>文件保存在服务器上的路径</returns>
        public static string UpFile(HttpPostedFileBase file, string path = "", string FileType = ImgTypes, int Maxsize = MaxSize)
        {
            string FileName;
            string filename;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                throw new Exception("请选择文件");
            }
            else
            {
                filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名

                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                                                                                         //int Maxsize = 4000 * 1024;//定义上传文件的最大空间大小为4M
                                                                                         // string FileType = fileType;//".jpg,.jpeg,.gif,.png,.bmp";//定义上传文件的类型字符串

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    throw new Exception("文件类型不对，只能导入" + FileType + "格式的文件");

                }
                if (filesize >= Maxsize)
                {
                    throw new Exception("上传文件超过4M，不能上传");

                }
                if (string.IsNullOrEmpty(path))
                    path = AppDomain.CurrentDomain.BaseDirectory + "file/";//文件上传到服务器地址

                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);

                return savePath;
            }
        }


        public bool UpImg(string url, string bucketname, string username, string password)
        {
            UpYun ypyun = new UpYun(bucketname, username, password);

            Hashtable headers = new Hashtable();
           
            FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            byte[] postArray = r.ReadBytes((int)fs.Length);


            bool b = ypyun.writeFile("/a/test.jpg", postArray, true);
            return b;

        }
        /// <summary>
        /// 删除项目和文件一对一的文件
        /// </summary>
        /// <param name="item"></param>
        public static void DelFiles(string item)
        {
            if (item!=null &&item!="")
            {
                File.Delete(System.Web.HttpContext.Current.Server.MapPath(item));
            }
           
        }
        /// <summary>
        /// 删除项目和文件一对一的文件
        /// </summary>
        /// <param name="item"></param>
        public static void DelFiles(List<string> list)
        {
            foreach (var item in list)
            {
                DelFiles(item);
            }
        }
    }


    
}
