using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Yiwen.RMS.Data;

namespace Yiwen.RMS.Bll
{
    public class WaterLogService : System.Data.Entity.EFLogic<water_logs, YWContext>
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
            //LoginName不能修改，若修改可能会导致登录名重复
            return new string[] { "id" };
        }

        protected override object[] OnGetEntityKey(water_logs entity)
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
                    re += item.type+":"+item.item;
                }
                return re;
            }
        }

        public void Save(water_logs entity)
        {
            using (var context = this.OnCreateContext())
            {
                water_logs item = new water_logs();
                item.site = entity.site;
                item.observ_station = entity.observ_station;
                item.observ_item = entity.observ_item;
                item.at2 = entity.at2;
                item.at4 = entity.at4;
                item.at6 = entity.at6;
                item.at8 = entity.at8;
                item.at10 = entity.at10;
                item.at12 = entity.at12;
                item.at14 = entity.at14;
                item.at16 = entity.at16;
                item.at18 = entity.at18;
                item.at20 = entity.at20;
                item.at22 = entity.at22;
                item.at24 = entity.at24;
                item.statis_item = entity.statis_item;
                item.daily_water = entity.daily_water;
                item.process_water = entity.process_water;
                context.water_logs.Add(item);
                context.SaveChanges();
            }
        }

        public bool hasData(DateTime date)
        {
            using (var context = this.OnCreateContext())
            {
                return context.water_logs.Any(p => p.create_at == date );
            }
        }

        public string GetDatas(DateTime dt)
        {
            using (var context = this.OnCreateContext())
            {
                var data =  context.water_logs.Where(p => p.create_at == dt ).ToList();
                string re = "";
                foreach (var item in data)
                {
                    re = re == "" ? re : re + ",";
                    re += item.site + ":" + item.observ_station + ":" + item.observ_item + ":" + item.at2 + ":" + item.at4 + ":" + item.at6 + ":" + item.at8 + ":" + item.at10 
                        + ":" + item.at12 + ":" + item.at14 + ":" + item.at16 + ":" + item.at18 + ":" + item.at20 + ":" + item.at22 + ":" + item.at24 
                        + ":" + item.statis_item + ":" + item.daily_water + ":" + item.process_water;
                }
                return re;
            }
        }

        public DateTime? GetMaxDate()
        {
            using (var context = this.OnCreateContext())
            {
                return context.water_logs.Max(p => p.create_at);
            }
        }

        public void DelDatas()
        {
            var dt = DateTime.Now.Date;
            using (var context = this.OnCreateContext())
            {
                var list = context.water_logs.Where(p => p.create_at == dt).ToList();
                foreach (var item in list)
                {
                    context.Delete(item);
                }
                context.SaveChanges();
            }
        }
    }
}
