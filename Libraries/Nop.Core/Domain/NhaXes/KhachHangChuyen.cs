using System;
using System.Collections.Generic;


namespace Nop.Core.Domain.NhaXes
{
    public class KhachHangChuyen : BaseEntity
    {
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public int NamSinh { get; set; }
        public int HopDongChuyenId { get; set; }
        
        public virtual HopDongChuyen HopDongInfo { get; set; }

        public bool isDaiDien { get; set; }
        //cac tien ich
        public string GhiChu { get; set; }

        public int NhaXeId { get; set; }
        
        
    }
}
