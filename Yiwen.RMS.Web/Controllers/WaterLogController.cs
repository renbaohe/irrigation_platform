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
    public class WaterLogController : EasyUICURDController<Yiwen.RMS.Data.water_logs, int>
    {


        public override EasyUIIndexViewInfo IndexViewInfo
        {
            get
            {
                return new EasyUIIndexViewInfo()
                {
                    DataTitle = "行水日志",
                    IDField = "id",
                    ShowSearch = false,
                    AddWindowSize = new Size(400, 380),
                    EditWindowSize = new Size(400, 380)
                };
            }
        }

        protected override IEntityCURD<water_logs> Service
        {
            get
            {
                return new WaterLogService();
            }
        }
        public ActionResult Show()
        {
            return View();
        }

        public ActionResult ShowDatas()
        {
            var service = this.Service as WaterLogService;
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
            var service = this.Service as WaterLogService;
            if (service.hasData(date))
            {
                code = 1;
                var datas = service.GetDatas(date);  //水库水情
                return datas.Replace(":0.00", ":");
            }
            else
            {
                return service.GetItems("礼泉总站水情");
            }
        }

        [HttpPost]
        public ActionResult Save()
        {
            var service = this.Service as WaterLogService;
            service.DelDatas();
            var data = Request.Form["data"].ToString();
            JArray jArray = (JArray)JsonConvert.DeserializeObject(data);

            decimal? temp = 0;
            for (int i = 0; i < jArray.Count; i++)
            {
                water_logs item = new water_logs();
                item.site = jArray[i]["0"].ToString();
                item.observ_station = jArray[i]["1"].ToString();
                item.observ_item = jArray[i]["2"].ToString();
                item.at2 = jArray[i]["3"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["3"]);
                item.at4 = jArray[i]["4"].ToString() == "" ? item.at2 : Convert.ToDecimal(jArray[i]["4"]);
                item.at6 = jArray[i]["5"].ToString() == "" ? item.at4 : Convert.ToDecimal(jArray[i]["5"]);
                item.at8 = jArray[i]["6"].ToString() == "" ? item.at6 : Convert.ToDecimal(jArray[i]["6"]);
                item.at10 = jArray[i]["7"].ToString() == "" ? item.at8 : Convert.ToDecimal(jArray[i]["7"]);
                item.at12 = jArray[i]["8"].ToString() == "" ? item.at10 : Convert.ToDecimal(jArray[i]["8"]);
                item.at14 = jArray[i]["9"].ToString() == "" ? item.at12 : Convert.ToDecimal(jArray[i]["9"]);
                item.at16 = jArray[i]["10"].ToString() == "" ? item.at14 : Convert.ToDecimal(jArray[i]["10"]);
                item.at18 = jArray[i]["11"].ToString() == "" ? item.at16 : Convert.ToDecimal(jArray[i]["11"]);
                item.at20 = jArray[i]["12"].ToString() == "" ? item.at18 : Convert.ToDecimal(jArray[i]["12"]);
                item.at22 = jArray[i]["13"].ToString() == "" ? item.at20 : Convert.ToDecimal(jArray[i]["13"]);
                item.at24 = jArray[i]["14"].ToString() == "" ? item.at22 : Convert.ToDecimal(jArray[i]["14"]);
                item.statis_item = jArray[i]["15"].ToString();
                if (i % 2 == 0)
                {
                    item.daily_water = Math.Round((decimal)(item.at2 + item.at4 + item.at6 + item.at8 + item.at10 + item.at12 + item.at14 + item.at16 + item.at18 + item.at20 + item.at22 + item.at24) / 12, 2);
                    temp = item.daily_water;
                }
                else
                {
                    item.daily_water = Math.Round((decimal)temp * (decimal)8.64 , 2);
                }
                item.process_water = jArray[i]["17"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["17"]);

                service.Save(item);
            }

            return Json(new ResultInfo() { Success = true });
        }
    }
}