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
    public class WaterRegimeService : System.Data.Entity.EFLogic<at8_water_regimes, YWContext>
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

        protected override object[] OnGetEntityKey(at8_water_regimes entity)
        {
            return new object[] { entity.id };
        }

        public string GetItems(string pro, string type)
        {
            using (var context = this.OnCreateContext())
            {
                var data = context.water_regime_temps.Where(p => p.project == pro && p.type == type).Select(p => p.item).ToList<string>();
                string re = "";
                foreach (var item in data)
                {
                    re = re == "" ? re : re + ",";
                    re += item;
                }
                return re;
            }
        }
        public string GetItems(string pro)
        {
            using (var context = this.OnCreateContext())
            {
                var data = context.water_regime_temps.Where(p => p.project == pro ).Select(p => p.item).ToList<string>();
                string re = "";
                foreach (var item in data)
                {
                    re = re == "" ? re : re + ",";
                    re += item;
                }
                return re;
            }
        }

        public void Save(string project,string type,string item,decimal? value,string remark)
        {
            using (var context = this.OnCreateContext())
            {
                at8_water_regimes entity = new at8_water_regimes();
                entity.create_at = DateTime.Now.Date;
                entity.project = project;
                entity.type = type;
                entity.item = item;
                entity.value = value;
                entity.remark = remark;
                context.at8_water_regimes.Add(entity);
                context.SaveChanges();
            }
        }

        public void SaveReservoir(string item, decimal? water_level, decimal? water_storage, string dispatch, string flood, decimal? normal_level)
        {
            using (var context = this.OnCreateContext())
            {
                reservoir_water_regimes entity = new reservoir_water_regimes();
                entity.create_at = DateTime.Now.Date;
                entity.item = item;
                entity.water_level = water_level;
                entity.water_storage = water_storage;
                entity.dispatch = dispatch;
                entity.flood = flood;
                entity.normal_level = normal_level;
                context.reservoir_water_regimes.Add(entity);
                context.SaveChanges();
            }
        }
        public bool hasData(DateTime date)
        {
            using (var context = this.OnCreateContext())
            {
                return context.at8_water_regimes.Any(p => p.create_at == date );
            }
        }

        public string GetDatas(DateTime dt,string pro, string type)
        {
            using (var context = this.OnCreateContext())
            {
                var data =  context.at8_water_regimes.Where(p => p.create_at == dt && p.project == pro && p.type == type).ToList();
                string re = "";
                foreach (var item in data)
                {
                    re = re == "" ? re : re + ",";
                    re += item.item + ":" + item.value;
                }
                return re;
            }
        }
        public string GetDatas(DateTime dt, string pro)
        {
            using (var context = this.OnCreateContext())
            {
                var data = context.at8_water_regimes.Where(p => p.create_at == dt && p.project == pro ).ToList();
                string re = "";
                if (pro == "备注")
                {
                    re = data[0].remark;
                }
                else
                {
                    foreach (var item in data)
                    {
                        re = re == "" ? re : re + ",";
                        re += item.item + ":" + item.value;
                    }
                }
                return re;
            }
        }
        public string GetDatas(DateTime dt)
        {
            using (var context = this.OnCreateContext())
            {
                var data = context.reservoir_water_regimes.Where(p => p.create_at == dt ).ToList();
                string re = "";
                foreach (var item in data)
                {
                    re = re == "" ? re : re + ",";
                    re += item.item + ":" + item.water_level.ToString() + ":" + item.water_storage.ToString() 
                        + ":" + item.dispatch.ToString() + ":" + item.flood.ToString() + ":" + item.normal_level.ToString();
                }
                return re;
            }
        }

        public DateTime? GetMaxDate()
        {
            using (var context = this.OnCreateContext())
            {
                return context.at8_water_regimes.Max(p => p.create_at);
            }
        }

        public void DelDatas()
        {
            var dt = DateTime.Now.Date;
            using (var context = this.OnCreateContext())
            {
                var list = context.at8_water_regimes.Where(p => p.create_at == dt).ToList();
                foreach (var item in list)
                {
                    context.Delete(item);
                }
                var list1 = context.reservoir_water_regimes.Where(p => p.create_at == dt).ToList();
                foreach (var item in list1)
                {
                    context.Delete(item);
                }
                context.SaveChanges();
            }
        }
    }
}
