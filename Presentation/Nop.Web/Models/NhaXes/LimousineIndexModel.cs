using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Web.Models.NhaXes
{
    public class LimousineIndexModel
    {
        public ThongKe ngayhientai { get; set; }
        public ThongKe ngayhomqua { get; set; }
        public ThongKe trongthang { get; set; }
        public decimal hopdongchuyentoday { get; set; }
        public decimal hopdongchuyenlastday { get; set; }
        public decimal hopdongchuyentrongthang { get; set; }
        public class ThongKe
        {
            public ThongKe(int _sldv,decimal _dt,int _slchuyen)
            {
                SoLuongDatVe = _sldv;
                DoanhThuDatVe = _dt;
                SoLuongChuyen = _slchuyen;
            }
            public int SoLuongDatVe { get; set; }
            public decimal DoanhThuDatVe { get; set; }
            public int SoLuongChuyen { get; set; }
        }
    }
}