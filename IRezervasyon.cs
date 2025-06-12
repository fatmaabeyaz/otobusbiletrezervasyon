using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace otobusbilet
{
   
        public interface IRezervasyon
        {
            decimal Fiyat { get; set; }
            void RezerveEt(Yolcu yolcu, Koltuk koltuk);
        }
    }


