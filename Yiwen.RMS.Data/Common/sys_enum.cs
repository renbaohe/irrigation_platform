using System;
using System.ComponentModel;

namespace Yiwen.RMS.Data
{
    public enum RoleEnum
    {
        [Description("水调中心")]
        [DisplayText("水调中心")]
        center = 1,
        [Description("水管员")]
        [DisplayText("水管员")]
        admin = 2
    }

    public enum Approval_statusEnum
    {
        [Description("待审批")]
        [DisplayText("待审批")]
        Init = 0,
        [Description("审批通过")]
        [DisplayText("审批通过")]
        Check = 1,
        [Description("审批驳回")]
        [DisplayText("审批驳回")]
        NoCheck = 2
    }
}
