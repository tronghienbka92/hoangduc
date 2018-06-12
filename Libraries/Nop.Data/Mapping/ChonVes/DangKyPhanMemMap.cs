using Nop.Core.Domain.Chonves;

namespace Nop.Data.Mapping.ChonVes
{
    public class DangKyPhanMemMap : NopEntityTypeConfiguration<DangKyPhanMem>
    {
        public DangKyPhanMemMap()
        {
            this.ToTable("CV_DangKyPhanMem");
            this.HasKey(c => c.Id);
            this.Property(u => u.Ten).HasMaxLength(500);
            this.Property(u => u.Email).HasMaxLength(500);
            this.Property(u => u.DiaChi).HasMaxLength(500);
            this.Property(u => u.SoDienThoai).HasMaxLength(500);
            this.Property(u => u.GhiChu).HasColumnType("ntext");
        }
    }
}
