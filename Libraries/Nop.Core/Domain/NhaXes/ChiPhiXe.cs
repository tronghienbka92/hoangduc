using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class ChiPhiXe : BaseEntity
    {
        public ChiPhiXe()
        {
            NgayTao = DateTime.Now;            
        }
        public int NhaXeId { get; set; }
        public virtual HangMucChiPhi hangmuc { get; set; }
        public int HangMucChiPhiId { get; set; }
        public DateTime NgayTao { get; set; }
        public virtual NhanVien nguoitao { get; set; }
        public int NguoiTaoId { get; set; }
        public virtual XeVanChuyen xevanchuyen { get; set; }
        public int XeVanChuyenId { get; set; }
        public string TenCongViec { get; set; }
        public Decimal ChiPhi { get; set; }
        public string ThoiGian { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public string GhiChu { get; set; }
        public string Ma { get; set; }
        public bool IsDeleted { get; set; }
    }
}
