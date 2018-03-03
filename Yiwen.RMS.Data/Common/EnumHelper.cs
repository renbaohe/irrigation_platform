using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Yiwen.RMS.Data
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="en">枚举类型的值</param>
        /// <returns>返回该枚举类型值的描述信息</returns>
        public static string GetEnumDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }
        

        //public static DicDetail GetDicDetails(DictionaryEnum en)
        //{
        //    DicDetail detail = new DicDetail();
        //    switch (en)
        //    {
        //        case DictionaryEnum.period:
        //            detail.table = "clan_history_celebrity";
        //            detail.field = "period";
        //            break;
        //        case DictionaryEnum.industry:
        //            detail.table = "clan_company";
        //            detail.field = "industry";
        //            break;
        //        default:
        //            break;
        //    }
        //    return detail;
        //}

    }

    public class DicDetail {
        public string table { get; set; }
        public string field { get; set; }
    }
}
