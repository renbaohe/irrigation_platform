using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Yiwen.RMS.Bll
{
    public static class ServiceHelper
    {
        public static TChild AutoCopy<TParent, TChild>(TParent parent) where TChild : TParent, new()
        {
            TChild child = new TChild();
            var ParentType = typeof(TParent);
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                //循环遍历属性
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    //进行属性拷贝
                    Propertie.SetValue(child, Propertie.GetValue(parent, null), null);
                }
            }
            return child;
        }

        /// <summary>  
        /// 枚举转字典集合  
        /// </summary>  
        /// <typeparam name="T">枚举类名称</typeparam>  
        /// <param name="idDefault">默认id值</param>  
        /// <param name="nameDefault">默认name值</param>  
        /// <returns>返回生成的字典集合</returns>  
        public static List<EnumData> EnumListDic<T>(string nameDefault="", int idDefault = -1)
        {
            List<EnumData> dicEnum = new List<EnumData>();
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return dicEnum;
            }
            if (!string.IsNullOrEmpty(nameDefault)) //判断是否添加默认选项  
            {
                dicEnum.Add(new EnumData() { id = idDefault, name = nameDefault });
            }
            string[] fieldstrs = Enum.GetNames(enumType); //获取枚举字段数组  
            foreach (var item in fieldstrs)
            {
                string description = string.Empty;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true); //获取属性字段数组  
                if (arr != null && arr.Length > 0)
                {
                    description = ((DescriptionAttribute)arr[0]).Description;   //属性描述  
                }
                else
                {
                    description = item;  //描述不存在取字段名称  
                }
                dicEnum.Add(new EnumData() {id=(int)Enum.Parse(enumType, item),name=description });  //不用枚举的value值作为字典key值的原因从枚举例子能看出来，其实这边应该判断他的值不存在，默认取字段名称  
            }
            return dicEnum;
        }

        //public static void SendMessage(int? receiver, string code, string content)
        //{
        //    MessageTemplateService mtService = new MessageTemplateService();
        //    var token = ((FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;
        //    string sendMessage = "";
        //    if (code == "clan_system")
        //    {
        //        sendMessage = content;
        //    }
        //    else
        //    {
        //        sendMessage = mtService.GetMessage(code).Replace("\"", "\\\"");
        //        if (content != null)
        //        {
        //            sendMessage = string.Format(sendMessage, content);
        //        }
        //    }

        //    var url = ConfigurationManager.AppSettings["InterfaceUrl"];
        //    Uri address = new Uri(url + "/pub/sys/"+ receiver);
        //    HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    var header = token;
        //    request.Headers.Add("authorization", header);

        //    string data = "{\"who\":\"" + receiver + "\",\"type\":\"" + code + "\",\"what\":\"" + sendMessage + "\"}";

        //    byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

        //    request.ContentLength = byteData.Length;

        //    using (Stream postStream = request.GetRequestStream())
        //    {
        //        postStream.Write(byteData, 0, byteData.Length);
        //    }
        //}
    }

    public class EnumData
    {
        public int id { get; set; }

        public string name { get; set; }
    }
}
