using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.NhaXes
{
    public class DatVeMap : NopEntityTypeConfiguration<DatVe>
    {
        public DatVeMap()
        {
            this.ToTable("CV_DatVe");
            this.HasKey(c => c.Id);
            this.Property(u => u.Ma).HasMaxLength(50);
            this.Property(u => u.DiaChiNha).HasMaxLength(500);
            this.Property(u => u.GhiChu).HasMaxLength(500);
            this.Property(u => u.TenDiemDon).HasMaxLength(500);
            this.Property(u => u.TenDiemTra).HasMaxLength(500);
            this.Property(u => u.SessionID).HasMaxLength(200);
            this.Property(u => u.TenKhachHangDiKem).HasMaxLength(200);
            this.Property(u => u.GiaTien).HasPrecision(18,0);

            this.HasOptional(p => p.khachhang)
                .WithMany()
                .HasForeignKey(p => p.KhachHangId);

            this.HasRequired(p => p.lichtrinh)
             .WithMany()
             .HasForeignKey(p => p.LichTrinhId);

            this.HasRequired(p => p.hanhtrinh)
             .WithMany()
             .HasForeignKey(p => p.HanhTrinhId);

            this.HasOptional(p => p.diemdon)
             .WithMany()
             .HasForeignKey(p => p.DiemDonId);

            this.HasRequired(p => p.nguoitao)
             .WithMany()
             .HasForeignKey(p => p.NguoiTaoId);

            this.HasRequired(p => p.nguoichuyen)
             .WithMany()
             .HasForeignKey(p => p.NguoiChuyenId);

            this.HasRequired(p => p.nguoihuy)
             .WithMany()
             .HasForeignKey(p => p.NguoiHuyId);

            this.HasOptional(p => p.chuyendi)
              .WithMany(o=>o.DatVes)
              .HasForeignKey(p => p.ChuyenDiId);

            this.HasOptional(p => p.sodoghe)
              .WithMany()
              .HasForeignKey(p => p.SoDoGheId);



            this.Ignore(u => u.trangthai);
            this.Ignore(u => u.ThuTuDon);
        }
    }
}
