using System;
using System.Linq;
using System.Collections.Generic;
using Nop.Core.Domain.Chonves;


namespace Nop.Core.Domain.NhaXes
{
    public class HanhTrinh:BaseEntity
    {
        public HanhTrinh ()
        {
            STT = 100;
        }
        public int NhaXeId { get; set; }
        public string MaHanhTrinh { get; set; }
        /// <summary>
        /// thong tin mo ta hanh trinh duoi dang text
        /// </summary>
        public string MoTa { get; set; }

        /// <summary>
        /// Chieu dai hanh trinh tinh theo km
        /// </summary>
        public int TongKhoangCach { get; set; }
       

        private ICollection<HanhTrinhDiemDon> _diemdons;
        public virtual ICollection<HanhTrinhDiemDon> DiemDons
        {
            get { return _diemdons ?? (_diemdons = new List<HanhTrinhDiemDon>()); }
            protected set { _diemdons = value; }
        }
     
        private ICollection<VanPhong> _vanphongs;
        public virtual ICollection<VanPhong> VanPhongs
        {
            get { return _vanphongs ?? (_vanphongs = new List<VanPhong>()); }
            protected set { _vanphongs = value; }
        }
        public bool isTuyenDi { get; set; }
        public int STT { get; set; }
    }
}
