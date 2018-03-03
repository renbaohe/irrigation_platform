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
    public class WaterRegimeController : EasyUICURDController<Yiwen.RMS.Data.at8_water_regimes, int>
    {


        public override EasyUIIndexViewInfo IndexViewInfo
        {
            get
            {
                return new EasyUIIndexViewInfo()
                {
                    DataTitle = "8时水情",
                    IDField = "id",
                    ShowSearch = false,
                    AddWindowSize = new Size(400, 380),
                    EditWindowSize = new Size(400, 380)
                };
            }
        }

        protected override IEntityCURD<at8_water_regimes> Service
        {
            get
            {
                return new WaterRegimeService();
            }
        }
        [AllowAnonymous]
        public override ActionResult Index()
        {
            return base.Index();
        }
        [AllowAnonymous]
        public ActionResult Show()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ShowDatas()
        {
            var service = this.Service as WaterRegimeService;
            var date = service.GetMaxDate();
            date = date == null ? DateTime.Now.Date : date;
            //date = Convert.ToDateTime("2018-02-23");
            var code = 0;
            var data = GetDatas((DateTime)date, ref code);
            return Json(new ResultInfo() { Success = true, MessageCode = code, Message = data }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Search()
        {
            var code = 0;
            var data =GetDatas(Convert.ToDateTime(Request.QueryString["date"]),ref code);
            return Json(new ResultInfo() { Success = true, MessageCode=code, Message=data }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public string GetDatas(DateTime date,ref int code)
        {
            var service = this.Service as WaterRegimeService;
            if (service.hasData(date))
            {
                code = 1;
                var datas1 = service.GetDatas(date,"干渠输水情况", "塬上");
                var datas2 = service.GetDatas(date, "干渠输水情况", "塬下");
                var datas3 = service.GetDatas(date);  //水库水情
                var datas4 = service.GetDatas(date, "灌区用水情况");
                var datas5 = service.GetDatas(date, "干渠退水");
                var datas6 = service.GetDatas(date, "含沙量");
                var datas7 = service.GetDatas(date, "备注");
                var datas8 = service.GetDatas(date, "干渠输水情况", "全局引抽总量");
                var re= datas1 + "|" + datas2 + "|" + datas3 + "|" + datas4 + "|" + datas5 + "|" + datas6 + "|" + datas7 + "|" + datas8 + "|" + date.ToString();
                return re.Replace(":0.00",":");
            }
            else
            {
                var items1 = service.GetItems("干渠输水情况", "塬上");
                var items2 = service.GetItems("干渠输水情况", "塬下");
                var items3 = service.GetItems("水库水情");
                var items4 = service.GetItems("灌区用水情况");
                return items1 + "|" + items2 + "|" + items3 + "|" + items4 + "|" + date.ToString();
            }
        }

        [HttpPost]
        public ActionResult Save()
        {
            var service = this.Service as WaterRegimeService;
            service.DelDatas();
            var data = Request.Form["data"].ToString();
            JArray jArray = (JArray)JsonConvert.DeserializeObject(data);
            ArrayList arr1 = new ArrayList();
            ArrayList arr2 = new ArrayList();
            ArrayList arr3 = new ArrayList();
            ArrayList arr4 = new ArrayList();
            ArrayList arr5 = new ArrayList();
            for (int i = 0; i < jArray.Count; i++)
            {
                if (i < 9)
                {
                    if (!string.IsNullOrEmpty(jArray[i]["3"].ToString()))
                    {
                        arr1.Add(jArray[i]["3"].ToString() + "," + jArray[i]["4"].ToString());
                    }
                    if (!string.IsNullOrEmpty(jArray[i]["5"].ToString()))
                    {
                        arr2.Add(jArray[i]["5"].ToString() + "," + jArray[i]["6"].ToString());
                    }
                    if (!string.IsNullOrEmpty(jArray[i]["7"].ToString()))
                    {
                        arr3.Add(jArray[i]["7"].ToString() + "," + jArray[i]["8"].ToString());
                    }
                    if (!string.IsNullOrEmpty(jArray[i]["9"].ToString()))
                    {
                        arr4.Add(jArray[i]["9"].ToString() + "," + jArray[i]["10"].ToString());
                    }
                    if (!string.IsNullOrEmpty(jArray[i]["11"].ToString()))
                    {
                        arr5.Add(jArray[i]["11"].ToString() + "," + jArray[i]["12"].ToString());
                    }
                }
                if (i==10)           //备注
                {
                    service.Save("备注", "", "", 0, jArray[i]["11"].ToString());
                }
                if (i > 10)  
                {
                    if (!string.IsNullOrEmpty(jArray[i]["3"].ToString()))   //干渠退水
                    {
                        service.Save("干渠退水", "", jArray[i]["3"].ToString(), jArray[i]["4"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["4"]), "");
                    }
                    if (i< jArray.Count-1)   //水库水情
                    {
                        var water_level = jArray[i]["6"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["6"]);
                        var water_storage = jArray[i]["7"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["7"]);
                        var dispatch = jArray[i]["8"].ToString();
                        var flood = jArray[i]["9"].ToString();
                        var normal_level = jArray[i]["10"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["10"]);
                        service.SaveReservoir(jArray[i]["5"].ToString(), water_level, water_storage, dispatch, flood, normal_level);
                    }
                    if (i==jArray.Count-1)  //含沙量
                    {
                        service.Save("含沙量", "", jArray[i]["5"].ToString(), jArray[i]["7"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["7"]), "");
                        service.Save("含沙量", "", jArray[i]["8"].ToString(), jArray[i]["10"].ToString() == "" ? 0 : Convert.ToDecimal(jArray[i]["10"]), "");
                    }
                }
                string str0 = jArray[i]["0"].ToString();
                string str1 = jArray[i]["1"].ToString() == "" ? str0 : jArray[i]["1"].ToString();
                decimal str2 = Convert.ToDecimal(jArray[i]["2"].ToString() == "" ? 0 : jArray[i]["2"]);

                service.Save("干渠输水情况", str0, str1, str2, "");            //干渠输水情况
            }
            #region 灌区用水情况          
            arr1.AddRange(arr2);
            arr1.AddRange(arr3);
            arr1.AddRange(arr4);
            arr1.AddRange(arr5);
            ArrayList list = new ArrayList();
            var project = "灌区用水情况";
            decimal project_sum = 0;
           
            foreach (var item in arr1)
            {
                list.Add(item);
                if (item.ToString().Contains("总站"))
                {
                    decimal type_sum = 0;
                    var type = item.ToString().Split(',')[0];
                    foreach (var item1 in list)
                    {
                        var temp_arr = item1.ToString().Split(',');
                        var it = temp_arr[0];
                        var val = Convert.ToDecimal(temp_arr[1] == "" ? "0" : temp_arr[1]);
                        if (item1.ToString().Contains("总站"))
                        {
                            val = type_sum;
                        }
                        else
                        {
                            type_sum += val;
                            project_sum += val;
                        }
                        service.Save(project, type, it, val, "");
                    }
                    list.Clear();
                }
                else if (item.ToString().Contains("全局合计"))
                {
                    service.Save(project, "全局合计", "全局合计", project_sum, "");
                }
            }
            #endregion
            return Json(new ResultInfo() { Success = true });
        }
    }
}