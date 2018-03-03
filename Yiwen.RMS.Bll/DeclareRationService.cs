using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Yiwen.RMS.Data;

namespace Yiwen.RMS.Bll
{
    public class DeclareRationService : System.Data.Entity.EFLogic<declare_rations, YWContext>
    {

      
        protected override System.Data.OrderCondition OnGetDefaultOrderItems()
        {
            return new System.Data.OrderCondition()
            {
                new OrderItem(){ FieldName="id", OrderType= OrderType.DESC}
            };
        }

        protected override string[] IgnoreUpdateFields()
        {
            return new string[] { "id","datas", "create_username" };
        }

        protected override object[] OnGetEntityKey(declare_rations entity)
        {
            return new object[] { entity.id };
        }
        public string GetItems(string pro)
        {
            using (var context = this.OnCreateContext())
            {
                var data = context.water_regime_temps.Where(p => p.project == pro).ToList();
                string re = "";
                foreach (var item in data)
                {
                    re = re == "" ? re : re + ",";
                    re += item.project + ":" + item.type + ":" + item.item;
                }
                return re;
            }
        }
        protected override void OnAfterAddEntity(YWContext context, declare_rations entity)
        {
            SaveDatas(context, entity);
            base.OnAfterAddEntity(context, entity);
        }

        public void SaveDatas(YWContext context, declare_rations entity)
        {
             JArray jArray = (JArray)JsonConvert.DeserializeObject(entity.datas);
            for (int i = 0; i < jArray.Count; i++)
            {
                declare_ration_details item = new declare_ration_details();
                item.dr_id = entity.id;
                item.site = jArray[i]["0"].ToString();
                item.subsite = jArray[i]["1"].ToString();
                item.channel = jArray[i]["2"].ToString();
                if (jArray[i]["3"] != null && jArray[i]["3"].ToString() != "")
                {
                    item.declare_water = Convert.ToDecimal(jArray[i]["3"]);
                }               
                if (jArray[i]["4"] !=null && jArray[i]["4"].ToString()!="")
                {
                    item.ration_water = Convert.ToDecimal(jArray[i]["4"]);
                }
                if (jArray[i]["5"] != null && jArray[i]["5"].ToString() != "")
                {
                    item.at8_water = Convert.ToDecimal(jArray[i]["5"]);
                }
                if (jArray[i]["6"] != null && jArray[i]["6"].ToString() != "")
                {
                    item.yesterday_water = Convert.ToDecimal(jArray[i]["6"]);
                }
               
                context.declare_ration_details.Add(item); 
            }
            context.SaveChanges();
        }


        public string GetDatas(int id)
        {
            using (var context = this.OnCreateContext())
            {
                var data = context.declare_ration_details.Where(p => p.dr_id == id).ToList();
                string re = "";
                foreach (var item in data)
                {
                    re = re == "" ? re : re + ",";
                    re += item.site + ":" + item.subsite + ":" + item.channel + ":" + item.declare_water + ":" + item.ration_water + ":" + item.at8_water + ":" + item.yesterday_water;
                }
                return re;
            }
        }
        protected override ResultInfo OnBeforeUpdateEntity(YWContext context, declare_rations entity)
        {
            DelDatas(entity.id);
            SaveDatas(context, entity);
            return base.OnBeforeUpdateEntity(context, entity);
        }

        public void DelDatas(int id)
        {
            var dt = DateTime.Now.Date;
            using (var context = this.OnCreateContext())
            {
                var list = context.declare_ration_details.Where(p => p.dr_id == id).ToList();
                foreach (var item in list)
                {
                    context.Delete(item);
                }
                context.SaveChanges();
            }
        }

        public bool HasDatas()
        {
            using (var context = this.OnCreateContext())
            {
                var dt = DateTime.Now.Date;
                var dd = dt.AddDays(1);
                return context.declare_rations.Any(p => p.create_at > dt && p.create_at < dd);
            }
        }
    }
}
