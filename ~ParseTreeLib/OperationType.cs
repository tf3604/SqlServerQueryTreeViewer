using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public enum OperationType
    {
        LogOp_Project,
        LogOp_GbAgg,
        LogOp_Select,
        LogOp_Join,
        LogOp_Get,
        ScaOp_Const,
        ScaOp_Logical,
        ScaOp_Comp,
        ScaOp_Identifier,
        ScaOp_Intrinsic,
        ScaOp_AggFunc,
        AncOp_PrjList,
        AncOp_PrjEl
    }
}
