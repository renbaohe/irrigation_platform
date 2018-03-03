using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwen.RMS.Core
{
    public static class ControHelper
    {
        public static SearchCondition OR_SearchCondition(SearchCondition condition, SearchCondition new_condition)
        {
            if (condition != null)
            {
                if (condition.Items.Count > 0)
                {
                    foreach (var item in condition.Items)
                    {
                        new_condition.Items.Add(item);
                    }
                }
                else
                {
                    new_condition.Items.Add(condition);
                }
            }
            return new_condition;
        }
            


            
    }
}
