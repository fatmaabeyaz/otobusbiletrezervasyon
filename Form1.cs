using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace otobusbilet
{
    public partial class Form1 : Form
    {
        Sefer aktifSefer;
        Button tiklananKoltuk;

        public Form1()
        {
            InitializeComponent();

            cmbOtobus.Items.Clear();
            cmbOtobus.Items.AddRange(new string[] { "Travego", "Setra", "Neoplan" });

            cmbOtobus.SelectedIndexChanged += (s, e) => SeferOlustur();
            cmbNereden.SelectedIndexChanged += (s, e) => SeferOlustur();
            cmbNereye.SelectedIndexChanged += (s, e) => SeferOlustur();
            dtpTarih.ValueChanged += (s, e) => SeferOlustur();

            // Radyo butonlar için event eklemelisin:
            rbSehirici.CheckedChanged += (s, e) => SeferOlustur();
            rbSehirlerarasi.CheckedChanged += (s, e) => SeferOlustur();
        }

        private void SeferOlustur()
        {
            if (cmbOtobus.SelectedIndex == -1 || cmbNereden.SelectedIndex == -1 || cmbNereye.SelectedIndex == -1)
                return;

            string otobusAdi = cmbOtobus.Text;
            int kapasite = 0;

            switch (otobusAdi)
            {
                case "Travego": kapasite = 40; break;
                case "Setra": kapasite = 50; break;
                case "Neoplan": kapasite = 45; break;
                default:
                    aktifSefer = null;
                    return;
            }

            Otobus otobus = new Otobus(otobusAdi, kapasite);

            decimal fiyat = nudFiyat.Value; // fiyat bilgisini al

            if (rbSehirici.Checked)
                aktifSefer = new SehiriciSefer(cmbNereden.Text, cmbNereye.Text, dtpTarih.Value, otobus, fiyat);
            else if (rbSehirlerarasi.Checked)
                aktifSefer = new SehirlerarasiSefer(cmbNereden.Text, cmbNereye.Text, dtpTarih.Value, otobus, fiyat);
            else
            {
                aktifSefer = null;
                return;
            }

            KoltuklariOlustur(aktifSefer.Otobus.Koltuklar);
        }

        private void KoltuklariOlustur(List<Koltuk> koltuklar)
        {
            // Eski koltuk butonlarını sil
            List<Control> silinecekler = new List<Control>();
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn && btn.Text != "Kaydet")
                    silinecekler.Add(ctrl);
            }
            foreach (var ctrl in silinecekler)
                this.Controls.Remove(ctrl);

            if (koltuklar == null) return;

            // Koltuk boyutları ve boşluklar
            int koltukGenislik = 40;
            int koltukYukseklik = 40;
            int satirBosluk = 10;

            int koridorBosluk = 100;  // Koridorun yatay genişliğini büyüttük (örnek 100 px)

            // X konumları
            int sol1X = 30;
            int sol2X = sol1X + koltukGenislik + 10;
            int sagTekliX = sol2X + koridorBosluk + 10;  // Burada koridor genişliği etkili

            for (int i = 0; i < koltuklar.Count; i++)
            {
                Koltuk koltuk = koltuklar[i];
                Button btn = new Button();
                btn.Width = btn.Height = koltukGenislik;
                btn.Text = koltuk.Numara.ToString();

                int sira = i / 3;
                int pozisyon = i % 3;

                if (pozisyon == 0)       // Sol koltuk 1
                    btn.Left = sol1X;
                else if (pozisyon == 1)  // Sol koltuk 2
                    btn.Left = sol2X;
                else if (pozisyon == 2)  // Sağdaki tekli koltuk
                    btn.Left = sagTekliX;

                btn.Top = 30 + sira * (koltukYukseklik + satirBosluk);

                btn.Tag = koltuk;
                btn.ContextMenuStrip = contextMenuStrip1;
                btn.MouseDown += Koltuk_MouseDown;

                btn.BackColor = koltuk.Rezerve && koltuk.Yolcu != null
                    ? (koltuk.Yolcu.Cinsiyet == "ERKEK" ? Color.Blue : Color.Red)
                    : SystemColors.Control;

                this.Controls.Add(btn);
            }
        }





        private void Koltuk_MouseDown(object sender, MouseEventArgs e)
        {
            tiklananKoltuk = sender as Button;
        }

        private void rezerveEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aktifSefer == null)
            {
                MessageBox.Show("Lütfen önce sefer oluşturun.");
                return;
            }

            if (tiklananKoltuk == null)
            {
                MessageBox.Show("Lütfen bir koltuk seçiniz.");
                return;
            }

            Koltuk koltuk = tiklananKoltuk.Tag as Koltuk;

            if (koltuk == null)
            {
                MessageBox.Show("Geçersiz koltuk seçimi.");
                return;
            }

            if (koltuk.Rezerve)
            {
                MessageBox.Show("Bu koltuk zaten rezerve edilmiş.");
                return;
            }

            KayıtFormu kf = new KayıtFormu();
            if (kf.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(kf.txtİsim.Text) ||
                    string.IsNullOrWhiteSpace(kf.txtSoyisim.Text) ||
                    string.IsNullOrWhiteSpace(kf.mskdTelefon.Text) ||
                    (!kf.rdbBay.Checked && !kf.rdbBayan.Checked))
                {
                    MessageBox.Show("Lütfen tüm alanları eksiksiz doldurun.");
                    return;
                }

                Yolcu yolcu = new Yolcu()
                {
                    Isim = kf.txtİsim.Text,
                    Soyisim = kf.txtSoyisim.Text,
                    Telefon = kf.mskdTelefon.Text,
                    Cinsiyet = kf.rdbBay.Checked ? "ERKEK" : "KADIN"
                };

                // Burada parametre sıralaması düzeltildi: (yolcu, koltuk)
                aktifSefer.RezerveEt(yolcu, koltuk);
                tiklananKoltuk.BackColor = yolcu.Cinsiyet == "ERKEK" ? Color.Blue : Color.Red;

                ListViewItem item = new ListViewItem($"{yolcu.Isim} {yolcu.Soyisim}");
                item.SubItems.Add(yolcu.Telefon);
                item.SubItems.Add(yolcu.Cinsiyet);
                item.SubItems.Add(aktifSefer.Nereden);
                item.SubItems.Add(aktifSefer.Nereye);
                item.SubItems.Add(koltuk.Numara.ToString());
                item.SubItems.Add(aktifSefer.Tarih.ToShortDateString());
                item.SubItems.Add(aktifSefer.Fiyat.ToString("C"));
                listView1.Items.Add(item);
            }
        }
    }
}
