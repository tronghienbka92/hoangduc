using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.NhaXes
{
    public class DatVeNoteMap : NopEntityTypeConfiguration<DatVeNote>
    {
        public DatVeNoteMap()
        {
            this.ToTable("CV_DatVeNote");
            this.HasKey(c => c.Id);
           
            this.HasRequired(p => p.DatVe)
              .WithMany()
              .HasForeignKey(p => p.DatVeId);

            
        }
    }
}
