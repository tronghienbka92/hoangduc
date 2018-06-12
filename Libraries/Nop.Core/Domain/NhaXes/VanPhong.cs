using System;
using System.Collections.Generic;


namespace Nop.Core.Domain.NhaXes
{
    public class VanPhong : BaseEntity
    {
        public VanPhong()
        {
            IsYeuCauDuyetHuy = false;
        }
        public int NhaXeId { get; set; }
        public string Ma { get; set; }
        public string TenVanPhong { get; set; }
        public int KieuVanPhongID { get; set; }        
        public string DienThoaiDatVe { get; set; }
        public string DienThoaiGuiHang { get; set; }        
        public int DiaChiID { get; set; }
        public virtual DiaChi diachiinfo { get; set; }
        public bool isDelete { get; set; }
        public bool IsYeuCauDuyetHuy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdate { get; set; }

        public ENKieuVanPhong KieuVanPhong
        {

            get
            {
                return (ENKieuVanPhong)KieuVanPhongID;
            }
            set
            {
                KieuVanPhongID = (int)value;
            }
        }
        //private ICollection<HanhTrinh> _hanhtrinhs;
        //public virtual ICollection<HanhTrinh> hanhtrinhs
        //{
        //    get { return _hanhtrinhs ?? (_hanhtrinhs = new List<HanhTrinh>()); }
        //    protected set { _hanhtrinhs = value; }
        //}
    }
}
