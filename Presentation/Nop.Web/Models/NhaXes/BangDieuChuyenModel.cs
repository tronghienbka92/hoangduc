using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Web.Models.NhaXes
{
    public class BangDieuChuyenModel
    {
        public bool IsQuyenTaoChuyen { get; set; }
        public BangDieuChuyenItem[,] arrBangDieuChuyen { get; set; }
        public class BangDieuChuyenItem
        {
            public BangDieuChuyenItem(int _id, int _hantrinhid,string _tenhanhtrinh,int _lichtrinhid,string _tenlichtrinh)
            {
                Id = _id;
                HanhTrinhId = _hantrinhid;
                TenHanhTrinh = _tenhanhtrinh;
                LichTrinhId = _lichtrinhid;
                TenLichTrinh = _tenlichtrinh;
            }
            public int Id { get; set; }
            public int HanhTrinhId { get; set; }
            public string TenHanhTrinh { get; set;}
            public int LichTrinhId { get; set; }
            public string TenLichTrinh { get; set; }
            public List<ChuyenDiModel> chuyendis { get; set; }
        }
        public string ThongTinXeDaDieu { get; set; }
        public string ThongTinXeChuaDieu { get; set; }
    }
}