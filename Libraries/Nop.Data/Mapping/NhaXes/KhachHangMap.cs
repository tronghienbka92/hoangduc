using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.NhaXes
{
    public class KhachHangMap : NopEntityTypeConfiguration<KhachHang>
    {
        public KhachHangMap()
        {
            this.ToTable("CV_KhachHang");
            this.HasKey(c => c.Id);
            this.Property(u => u.DienThoai).HasMaxLength(50);
            this.Property(u => u.Ten).HasMaxLength(200);
            this.Property(u => u.DiaChi).HasMaxLength(1000);
            
        }
    }
}
