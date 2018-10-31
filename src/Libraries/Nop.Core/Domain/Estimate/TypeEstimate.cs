using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Core.Domain.Estimate
{
    public enum TypeEstimate
    {
        CatalogueKhongRuot = 1,
        CatalogueCoRuot = 2,
    }

    public enum TypeProductPrint
    {
        TinhTheoDonVi = 1,
        TinhTheoDienTichMat = 2
    }

    public enum TypeEstimateStep
    {
        TatCa = 0,
        Bia = 1,
        BiaTruocIn = 2,
        Ruot = 3,
        RuotTruocIn = 4,
        ThanhPham = 5
    }
}
