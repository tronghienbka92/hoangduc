using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nop.Services.NhaXes
{
    public partial interface IChiPhiXeVanChuyenService
    {
        ChiPhiXe GetById(int itemid);
        void Insert(ChiPhiXe _item);
        void Update(ChiPhiXe _item);
        void Delete(ChiPhiXe _item);
        List<ChiPhiXe> GetAllChiPhiXes(int NhaXeId, int XeVanChuyenId = 0, int HangMucId = 0, DateTime? TuNgay = null, DateTime? DenNgay = null);
        List<HangMucChiPhi> GetAllHangMucChiPhi(int NhaXeId);
        HangMucChiPhi GetHangMucById(int itemid);
        void InsertHangMuc(HangMucChiPhi _item);
        void UpdateHangMuc(HangMucChiPhi _item);
        void DeleteHangMuc(HangMucChiPhi _item);
    }
}
