using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Nop.Data.Mapping.NhaXes
{
    public class ChuyenDiMap : NopEntityTypeConfiguration<ChuyenDi>
    {
        public ChuyenDiMap()
        {
            this.ToTable("CV_ChuyenDi");
            this.HasKey(c => c.Id);
            this.Property(u => u.Ma).HasMaxLength(50);
            this.Property(u => u.GhiChu).HasMaxLength(2000);


            this.HasOptional(p => p.laixe)
                .WithMany()
                .HasForeignKey(p => p.LaiXeId);

            this.HasRequired(p => p.lichtrinh)
             .WithMany()
             .HasForeignKey(p => p.LichTrinhId);

            this.HasRequired(p => p.hanhtrinh)
             .WithMany()
             .HasForeignKey(p => p.HanhTrinhId);

            this.HasRequired(p => p.lichtrinhloaixe)
            .WithMany()
            .HasForeignKey(p => p.LichTrinhLoaiXeId);

            this.HasOptional(p => p.xevanchuyen)
             .WithMany()
             .HasForeignKey(p => p.XeVanChuyenId);

            this.HasRequired(p => p.nguoitao)
             .WithMany()
             .HasForeignKey(p => p.NguoiTaoId);

   

            this.Ignore(u => u.trangthai);
            
        }
    }
}
