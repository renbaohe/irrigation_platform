using System;
using System.Configuration;
using System.Web.Mvc;
using Yiwen.RMS.Bll;
using Yiwen.RMS.Data;

namespace Yiwen.RMS.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        
        public string uploadUrl = ConfigurationManager.AppSettings["UploadUrl"];
        
        protected JsonResult EasyUIPageData<T>(System.Data.PageData<T> pagedata)
        {
            return Json(new { total = pagedata.TotalCount, rows = pagedata.ListData }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetApprovalStatus()
        {
            return Json(ServiceHelper.EnumListDic<Approval_statusEnum>("全部"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRoleStatus()
        {
            return Json(ServiceHelper.EnumListDic<RoleEnum>("全部"), JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetDictionarys(string table, string field)
        //{
        //    DictionaryService service = new DictionaryService();
        //    return Json(service.GetDictionarys(table, field), JsonRequestBehavior.AllowGet);
        //}

        public string ModifyUeditContent(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                return str.Replace("src=\"/ueditor/net/", "src=\"" + uploadUrl + "/ueditor/net/");
            }
        }
        public int? GetRole()
        {
            UserService service = new UserService();
            return service.FindByKey(Convert.ToInt32(this.User.Identity.Name)).Data.role;
        }
    } 
}
