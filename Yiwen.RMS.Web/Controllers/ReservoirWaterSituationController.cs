using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
    public class ReservoirWaterSituationController : EasyUICURDController<Yiwen.RMS.Data.reservoir_water_situations, int>
    {


        public override EasyUIIndexViewInfo IndexViewInfo
        {
            get
            {
                return new EasyUIIndexViewInfo()
                {
                    DataTitle = "水库水情",
                    IDField = "id",
                    ShowSearch = false,
                    AddWindowSize = new Size(400, 380),
                    EditWindowSize = new Size(400, 380)
                };
            }
        }

        protected override IEntityCURD<reservoir_water_situations> Service
        {
            get
            {
                return new ReservoirWaterSituationService();
            }
        }
        public ActionResult Show()
        {
            return View();
        }
        
        public ActionResult ShowDatas()
        {
            var service = this.Service as ReservoirWaterSituationService;
            var date = service.GetMaxDate();
            date = date == null ? DateTime.Now.Date : date;
            var code = 0;
            var data = GetDatas((DateTime)date, ref code);
            return Json(new ResultInfo() { Success = true, MessageCode = code, Message = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            var code = 0;
            var data = GetDatas(Convert.ToDateTime(Request.QueryString["date"]), ref code);
            return Json(new ResultInfo() { Success = true, MessageCode = code, Message = data }, JsonRequestBehavior.AllowGet);
        }

        public string GetDatas(DateTime date, ref int code)
        {
            var service = this.Service as ReservoirWaterSituationService;
            if (service.hasData(date))
            {
                code = 1;
                var datas = service.GetDatas(date);  //水库水情
                return datas.Replace(":0.00", ":");
            }
            else
            {
                var tempService = new WaterRegimeService();
                var items1 = tempService.GetItems("大北沟", "测量项");
                var items2 = tempService.GetItems("大北沟", "统计项");
                return items1 + "|" + items2 + "|" + date.ToString();
            }
        }

        [HttpPost]
        public ActionResult Save()
        {
            var service = this.Service as ReservoirWaterSituationService;
            service.DelDatas();
            var data = Request.Form["data"].ToString();
            JArray jArray = (JArray)JsonConvert.DeserializeObject(data);

            for (int i = 0; i < jArray.Count; i++)
            {
                reservoir_water_situations entity = new reservoir_water_situations();
                entity.reservoir = jArray[i]["0"].ToString();
                entity.subject= jArray[i]["1"].ToString();
                entity.at8= jArray[i]["2"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["2"]);
                entity.at20 = jArray[i]["3"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["3"]);
                entity.statis= jArray[i]["4"].ToString();
                if (i < 2)
                {
                    entity.data = jArray[i]["5"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["5"]);
                }
                else
                {
                    entity.data = Math.Round((decimal)(entity.at8 + entity.at20) * (decimal)8.64 / 2, 2);
                }
                service.Save(entity);
            }

            return Json(new ResultInfo() { Success = true });
        }
    }
}