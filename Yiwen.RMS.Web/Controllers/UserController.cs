using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Yiwen.RMS.Bll;
using Yiwen.RMS.Core;
using Yiwen.RMS.Data;

namespace Yiwen.RMS.Web.Controllers
{
    [Authorize]
    public class UserController : EasyUICURDController<Yiwen.RMS.Data.users, long>
    {


        public override EasyUIIndexViewInfo IndexViewInfo
        {
            get
            {
                return new EasyUIIndexViewInfo()
                {
                    DataTitle = "用户管理",
                    IDField = "id",
                    ShowSearch = false,
                    AddWindowSize = new Size(400, 380),
                    EditWindowSize = new Size(400, 380)
                };
            }
        }

        protected override IEntityCURD<users> Service
        {
            get
            {
                return new UserService();
            }
        }


        public override ActionResult List(int rows, int page, [JsonTextModel] SearchCondition condition, [JsonTextModel] OrderCondition order)
        {
            UserService service = this.Service as UserService;

            var pager = service.ListPage(condition, order, new PageInfo() { PageIndex = page - 1, PageSize = rows });
            if (pager.Success)
            {
                return Json(new { total = pager.Data.TotalCount, rows = pager.Data.ListData });
            }
            else
            {
                return Json(null);
            }
        }

        


        public ActionResult SelectIndex(int moreSelect = 0, string Sys_RoleName = "")
        {
            ViewBag.moreSelect = moreSelect;
            string sys_rolevalue = null;
            if (!string.IsNullOrEmpty(Sys_RoleName))
            {
                sys_rolevalue = Sys_RoleName;
            }
            ViewBag.Sys_RoleValue = sys_rolevalue;
            return View();
        }

        public ActionResult GetRoleEnum()
        {
            return Json(ServiceHelper.EnumListDic<RoleEnum>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePwd(long id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost()]
        public ActionResult ChangePwd()
        {
            var token = ((FormsIdentity)(System.Web.HttpContext.Current.User.Identity)).Ticket.UserData;

            var id = Request.Form["user_id"].ToString();
            var originPwd = Request.Form["OriginPwd"].ToString();
            var newPwd = Request.Form["NewPwd"].ToString();

            var url = ConfigurationManager.AppSettings["InterfaceUrl"];
            Uri address = new Uri(url + "/users/change_password");
            // Create the web request
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST
            request.Method = "POST";
            request.ContentType = "application/json";
            var header = token;
            request.Headers.Add("authorization", header);

            string data = "{\"user\":{\"password\":\"" + originPwd + "\",\"new_password\":\"" + newPwd + "\"}}";

            // Create a byte array of the data we want to send
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            // Set the content length in the request headers
            request.ContentLength = byteData.Length;
            // Write data
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            string[] result = null;
            JObject obj = null;
            // Get response
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // 读取并解析返回JSON
                obj = JObject.Parse(reader.ReadToEnd());
                result = obj.Properties().Select(p => p.Value.ToString()).ToArray();
            }

            //WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe);
            if (result[0] == "200")
            {
                return Json(new ResultInfo() { Success = true, Message = "操作成功!" });
            }
            else
            {
                return Json(new ResultInfo() { Success = false, Message = "操作失败，请重试！" });
            }
        }
    }
}