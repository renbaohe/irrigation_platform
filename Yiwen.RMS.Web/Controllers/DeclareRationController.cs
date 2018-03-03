using Newtonsoft.Json;
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
    public class DeclareRationController : EasyUICURDController<Yiwen.RMS.Data.declare_rations, long>
    {


        public override EasyUIIndexViewInfo IndexViewInfo
        {
            get
            {
                return new EasyUIIndexViewInfo()
                {
                    DataTitle = "申报配给",
                    IDField = "id",
                    ShowSearch = false,
                    AddWindowSize = new Size(600, 400),
                    EditWindowSize = new Size(600, 400)
                };
            }
        }

        protected override IEntityCURD<declare_rations> Service
        {
            get
            {
                return new DeclareRationService();
            }
        }

        public override ActionResult Index()
        {
            var service = this.Service as DeclareRationService;
            ViewBag.Role = GetRole();
            ViewBag.HasDatas = service.HasDatas();
            return base.Index();
        }
        public ActionResult GetItems()
        {
            var service = this.Service as DeclareRationService;
            var data = service.GetItems("乾县总站");
            return Json(new ResultInfo() { Success = true, Message = data }, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Add(declare_rations item)
        {
            var datas = Request.Form["hidDatas"];
            item.create_user = Convert.ToInt32(this.User.Identity.Name);
            item.status = 0;
            item.datas = datas;
            return base.Add(item); 
        }


        public ActionResult GetDatas(int id)
        {
            var service = this.Service as DeclareRationService;
            var datas = service.GetDatas(id);
            return Json(new ResultInfo() { Success = true,  Message = datas }, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Edit(long id)
        {
            ViewBag.Role = GetRole();
            return base.Edit(id);
        }
        public override ActionResult Edit(declare_rations item)
        {
            if (item.status == 0 && GetRole() == 2)
            {
                item.status = 1;
                item.ration_at = DateTime.Now;
            }
            else if(item.status==1 && GetRole() ==1)
            {
                item.status = 2;
                item.report_at = DateTime.Now;
            }
            return base.Edit(item);
        }
        public override ActionResult List(int rows, int page, [JsonTextModel] SearchCondition condition, [JsonTextModel] OrderCondition order)
        {
            DeclareRationService service = this.Service as DeclareRationService;
            UserService us = new UserService();

            var pager = service.ListPage(condition, order, new PageInfo() { PageIndex = page - 1, PageSize = rows });
            if (pager.Success)
            {
                foreach (var item in pager.Data.ListData)
                {
                    item.create_username = us.FindByKey((int)item.create_user).Data.uname;
                }
                return Json(new { total = pager.Data.TotalCount, rows = pager.Data.ListData });
            }
            else
            {
                return Json(null);
            }
        }
    }
}