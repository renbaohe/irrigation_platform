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
    public class ReservoirWaterSituationService : System.Data.Entity.EFLogic<reservoir_water_situations, YWContext>
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

        protected override object[] OnGetEntityKey(reservoir_water_situations entity)
        {
            return new object[] { entity.id };
        }     

        public void Save(reservoir_water_situations entity)
        {
            using (var context = this.OnCreateContext())
            {
                reservoir_water_situations item = new reservoir_water_situations();
                item.at20 = entity.at20;
                item.at8 = entity.at8;
                item.data = entity.data;
                item.reservoir = entity.reservoir;
                item.statis = entity.statis;
                item.subject = entity.subject;
                context.reservoir_water_situations.Add(item);
                context.SaveChanges();
            }
        }

        public bool hasData(DateTime date)
        {
            using (var context = this.OnCreateContext())
            {
                return context.reservoir_water_situations.Any(p => p.create_at == date );
            }
        }

        public string GetDatas(DateTime dt)
        {
            using (var context = this.OnCreateContext())
            {
                var data =  context.reservoir_water_situations.Where(p => p.create_at == dt ).ToList();
                string re = "";
                foreach (var item in data)
                {
                    re = re == "" ? re : re + ",";
                    re += item.reservoir + ":" + item.subject + ":" + item.at8 + ":" + item.at20 + ":" + item.statis + ":" + item.data;
                }
                return re;
            }
        }

        public DateTime? GetMaxDate()
        {
            using (var context = this.OnCreateContext())
            {
                return context.reservoir_water_situations.Max(p => p.create_at);
            }
        }

        public void DelDatas()
        {
            var dt = DateTime.Now.Date;
            using (var context = this.OnCreateContext())
            {
                var list = context.reservoir_water_situations.Where(p => p.create_at == dt).ToList();
                foreach (var item in list)
                {
                    context.Delete(item);
                }
                context.SaveChanges();
            }
        }
    }
}
