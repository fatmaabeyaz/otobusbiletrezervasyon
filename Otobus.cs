using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace otobusbilet
{
    public class Otobus
    {
        public string Marka { get; set; }
        public List<Koltuk> Koltuklar { get; set; }

        public Otobus(string marka, int koltukSayisi)
        {
            Marka = marka;
            Koltuklar = new List<Koltuk>();
            for (int i = 1; i <= koltukSayisi; i++)
            {
                Koltuklar.Add(new Koltuk() { Numara = i });
            }
        }
    }
}
