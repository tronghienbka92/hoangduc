using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Chonves
{
    public class DangKyPhanMem : BaseEntity
    {
        public DangKyPhanMem()
        {
            NgayTao = DateTime.Now;
        }
        public string Ten { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayTao { get; set; }
        
    }
}
