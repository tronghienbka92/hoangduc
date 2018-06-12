using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class KhachHang : BaseEntity
    {
        public KhachHang()
        {
            NgayTao = DateTime.Now;
        }
        public int NhaXeId { get; set; }
        public string DienThoai { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayTao { get; set; }

    }
}
