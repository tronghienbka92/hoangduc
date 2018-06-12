using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class BangGiaVe : BaseEntity
    {
        public string ThongTin { get; set; }
        public decimal GiaVe { get; set; }
        public int NhaXeId { get; set; }
    }
}
