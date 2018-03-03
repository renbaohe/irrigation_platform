using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Yiwen.RMS.Data;

namespace Yiwen.RMS.Bll
{
    public class UserService : System.Data.Entity.EFLogic<users, YWContext>
    {

        public ResultData<users> DoLogin(string loginname, string userpassword)
        {
            using (var context = this.OnCreateContext())
            {
                var passwordhash = HashUtility.ToMd5HashString(userpassword);
                var res = context.users.Where(p => p.uname == loginname && (p.password==userpassword || p.password == passwordhash)).FirstOrDefault();
                return new ResultData<users>() { Data = res, Success = res != null };
            }
        }
        protected override ResultInfo OnBeforeAddEntity(YWContext context, users entity)
        {
            if (context.users.Where(p => p.uname == entity.uname).Count() > 0)
            {
                return new ResultInfo() { Success = false, Message = "该登录名已经存在。" };
            }
            return base.OnBeforeAddEntity(context, entity);
        }
        protected override void OnFillAddEntityPropertys(YWContext context, users entity)
        {
            entity.password = HashUtility.ToMd5HashString(entity.password);
            base.OnFillAddEntityPropertys(context, entity);
        }

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

        protected override object[] OnGetEntityKey(users entity)
        {
            return new object[] { entity.id };
        }

        public users FindUser(string userName)
        {
            using (var context = this.OnCreateContext())
            {
                return context.users.FirstOrDefault(p => p.uname == userName);
            }
        }
    }
}
