using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class HanhTrinhLoaiXeGiaVe : BaseEntity
    {
        public int HanhTrinhId { get; set; }
        public int LoaiXeId { get; set; }
        public int SoDoGheId { get; set; }
        public decimal GiaVe { get; set; }        
    }
}
