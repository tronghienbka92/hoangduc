using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Web.Models.NhaXes
{
    public class LichTrinhLoaiXeModel
    {
        public int Id { get; set; }
        public int HanhTrinhId { get; set; }
        public string tenhanhtrinh { get; set; }
        public int LoaiXeId { get; set; }
        public string tenloaixe { get; set; }
        public decimal GiaVe { get; set; }
    }
    public class GiaVeChungModel
    {
        public int Id { get; set; }
        public int HanhTrinhId { get; set; }
        public string tenhanhtrinh { get; set; }
        public int LoaiXeId { get; set; }
        public string tenloaixe { get; set; }
        public decimal GiaVe { get; set; }
    }
}