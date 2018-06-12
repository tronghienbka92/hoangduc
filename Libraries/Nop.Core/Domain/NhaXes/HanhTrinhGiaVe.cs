using System;
using System.Collections.Generic;

namespace Nop.Core.Domain.NhaXes
{
    public class HanhTrinhGiaVe : BaseEntity
    {
        public int HanhTrinhId { get; set; }
        public virtual HanhTrinh HanhTrinh { get; set; }
        public int NhaXeId { get; set; }
        public int DiemDonId { get; set; }
        public virtual DiemDon DiemDon { get; set; }
        public int DiemDenId { get; set; }
        public virtual DiemDon DiemDen { get; set; }
        public Decimal GiaVe { get; set; }
        public bool IsDelete { get; set; }
        
    }
}
