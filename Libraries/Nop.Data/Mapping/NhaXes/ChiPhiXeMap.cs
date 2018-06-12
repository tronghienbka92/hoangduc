using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.NhaXes
{
    public class ChiPhiXeMap : NopEntityTypeConfiguration<ChiPhiXe>
    {
        public ChiPhiXeMap()
        {
            this.ToTable("CV_ChiPhiXe");
            this.HasKey(c => c.Id);
            this.Property(u => u.GhiChu).HasMaxLength(2000);
            this.Property(u => u.TenCongViec).HasMaxLength(2000);
            this.Property(u => u.ThoiGian).HasMaxLength(500);
            this.Property(p => p.ChiPhi).HasPrecision(18, 0);

            this.HasRequired(c => c.nguoitao)
              .WithMany()
              .HasForeignKey(c => c.NguoiTaoId);
            this.HasRequired(c => c.xevanchuyen)
              .WithMany()
              .HasForeignKey(c => c.XeVanChuyenId);
            this.HasRequired(c => c.hangmuc)
              .WithMany()
              .HasForeignKey(c => c.HangMucChiPhiId);
        }
    }
}
