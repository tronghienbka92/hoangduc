using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class LichTrinhLoaiXe : BaseEntity
    {
        public int HanhTrinhId { get; set; }
        public virtual HanhTrinh hanhtrinh { get; set; }
        public int LichTrinhId { get; set; }
        public virtual LichTrinh lichtrinh { get; set; }
        public int LoaiXeId { get; set; }
        public virtual LoaiXe loaixe { get; set; }
        public decimal GiaVe { get; set; }
    }
    public class LichTrinhLoaiXeGiaVe
    {
        public int HanhTrinhId { get; set; }
        public virtual HanhTrinh hanhtrinh { get; set; }
        public int LichTrinhId { get; set; }
        public virtual LichTrinh lichtrinh { get; set; }
        public int LoaiXeId { get; set; }
        public virtual LoaiXe loaixe { get; set; }
        public decimal GiaVe { get; set; }
    }
}
