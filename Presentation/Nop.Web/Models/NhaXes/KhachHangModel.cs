using FluentValidation.Attributes;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Nop.Web.Models.NhaXes
{
    public class KhachHangModel : BaseNopEntityModel
    {
        public string DienThoai { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string ThongTin { get; set; }
        public DateTime NgayTao { get; set; }
        public int SoLuongDat { get; set; }
        public int SoLuongHuy { get; set; }
        //lay thong tin diem don lan dat cuoi
        public string TenDiemDon { get; set; }
        public string TenDiemTra { get; set; }
        public string ChuyenDiTrongNgay { get; set; }
    }
}