using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class DatVeNote : BaseEntity
    {
        public DatVeNote()
        {
            NgayTao = DateTime.Now;
        }

        public int DatVeId { get; set; }
        public string Note { get; set; }      
        public virtual DatVe DatVe { get; set; }
        public DateTime NgayTao { get; set; }
        
      
       
    }
   
}
