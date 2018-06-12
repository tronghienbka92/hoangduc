using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class ChuyenDi : BaseEntity
    {
        public int NhaXeId { get; set; }
        public string Ma { get; set; }
        public DateTime NgayDi { get; set; }
        public DateTime NgayDiThuc { get; set; }        
        public int LichTrinhId { get; set; }
        public virtual LichTrinh lichtrinh { get; set; }
        public int? LaiXeId { get; set; }
        public virtual NhanVien laixe { get; set; }
        public int? XeVanChuyenId { get; set; }
        public virtual XeVanChuyen xevanchuyen { get; set; }
        public int HanhTrinhId { get; set; }
        public virtual HanhTrinh hanhtrinh { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTaoId { get; set; }
        public virtual NhanVien nguoitao { get; set; }
        public int TrangThaiId { get; set; }
        public ENTrangThaiXeXuatBen trangthai
        {
            get
            {
                return (ENTrangThaiXeXuatBen)TrangThaiId;
            }
            set
            {
                TrangThaiId = (int)value;
            }
        }
        public string GhiChu { get; set; }
        private ICollection<DatVe> _datves;
        public virtual ICollection<DatVe> DatVes
        {
            get { return _datves ?? (_datves = new List<DatVe>()); }
            protected set { _datves = value; }
        }
        public List<DatVe> DatVeHopLes()
        {            
            return DatVes.Where(c => c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI).ToList();

        }
        private ICollection<HistoryXeXuatBenLog> _nhatkys;
        public virtual ICollection<HistoryXeXuatBenLog> NhatKys
        {
            get { return _nhatkys ?? (_nhatkys = new List<HistoryXeXuatBenLog>()); }
            protected set { _nhatkys = value; }
        }
        public int LichTrinhLoaiXeId { get; set; }
        public virtual LichTrinhLoaiXe lichtrinhloaixe { get; set; }
        public int STTChuyen { get; set; }
    }
}
