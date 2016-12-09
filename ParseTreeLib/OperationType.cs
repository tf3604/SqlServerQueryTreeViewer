//  Copyright(c) 2016 Brian Hansen.

//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//  of the Software.
    
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//  DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bkh.ParseTreeLib
{
    public enum OperationType
    {
        Unknown,
        LogOp_Apply,
        LogOp_ConstTableGet,
        LogOp_GbAgg,
        LogOp_Get,
        LogOp_Join,
        LogOp_LeftAntiSemiJoin,
        LogOp_LeftOuterJoin,
        LogOp_LeftSemiJoin,
        LogOp_NAryJoin,
        LogOp_OrderBy,
        LogOp_Pivot,
        LogOp_Project,
        LogOp_Select,
        LogOp_SequenceProject,
        LogOp_Top,
        LogOp_Union,
        LogOp_UnionAll,
        LogOp_ViewAnchor,
        ScaOp_AggFunc,
        ScaOp_AllComp,
        ScaOp_Arithmetic,
        ScaOp_Comp,
        ScaOp_Const,
        ScaOp_Convert,
        ScaOp_IIF,
        ScaOp_Identifier,
        ScaOp_Intrinsic,
        ScaOp_Logical,
        ScaOp_NotExists,
        ScaOp_SeqFunc,
        ScaOp_SomeComp,
        ScaOp_Subquery,
        AncOp_PrjEl,
        AncOp_PrjList,
        PhyOp_Apply,
        PhyOp_Filter,
        PhyOp_HashGbAgg,
        PhyOp_HashJoinx_jtInner,
        PhyOp_Range,
        PhyOp_RestrRemap,
        Exchange
    }
}
