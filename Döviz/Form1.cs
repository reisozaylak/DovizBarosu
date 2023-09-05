using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace Döviz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=KLMNTB038;Initial Catalog=DbDoviz;Persist Security Info=True;User ID=sa; password=Klm_1234");
        private void Form1_Load(object sender, EventArgs e)
        {

            baglanti.Open();// Label13'e Databasedeki Dolar değerini yazdırma 
            SqlCommand komut = new SqlCommand("Select Dolar from TblKASA", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                label13.Text = read["Dolar"].ToString();
            }
            baglanti.Close();


            baglanti.Open(); // Label16'e Databasedeki Euro değerini yazdırma 
            SqlCommand komut1 = new SqlCommand("Select Euro from TblKASA", baglanti);
            SqlDataReader read1 = komut1.ExecuteReader();
            while (read1.Read())
            {
                label16.Text = read1["Euro"].ToString();
            }
            baglanti.Close();


            baglanti.Open(); // Label18'e Databasedeki TL değerini yazdırma 
            SqlCommand komut2 = new SqlCommand("Select TL from TblKASA", baglanti);
            SqlDataReader read2 = komut2.ExecuteReader();
            while (read2.Read())
            {
                label18.Text = read2["TL"].ToString();
            }
            baglanti.Close();


            string bugun = "https://tcmb.gov.tr/kurlar/today.xml";
            var xmlDosya = new XmlDocument();
            xmlDosya.Load(bugun);
            txtKur.Enabled = false;
            txtKalan.Enabled = false;
            txtTutar.Enabled=false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox4.Enabled = false;



            // Üstteki linkten alınan XML dosyalarını Labellara aktarma
            string dolaralis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            lblDolarAlış.Text = dolaralis;
            string dolarsatis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            lblDolarSatış.Text = dolarsatis;
            string euroalis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            lblEuroAlış.Text = euroalis;
            string eurosatis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            lblEuroSatış.Text = eurosatis;
        }
        private void btnSatisYap_Click(object sender, EventArgs e) // Girilen Miktara Göre Seçilen radiobuttondaki işlemi yapan Button
        {
            double kur, miktar, tutar, kasa;

          
            if (radioButton1.Checked) // Database'e Dolar alış işlemi için seçilen radiobutton
            {
                kur = Convert.ToDouble(txtKur.Text);
                miktar = Convert.ToDouble(txtMiktar.Text);
                tutar = kur * miktar;
                txtTutar.Text = tutar.ToString();
                kasa = Convert.ToInt32(label13.Text)+miktar;
                txtKalan.Text = kasa.ToString();
                label13.Text = kasa.ToString();
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("UPDATE TblKASA SET Dolar ='" + txtKalan.Text.ToString() + "'", baglanti);
                cmd.ExecuteNonQuery();
                baglanti.Close();
            }

            if (radioButton2.Checked) // Database'den Dolar satış işlemi için seçilen radiobutton
            {
                kur = Convert.ToDouble(txtKur.Text);
                miktar = Convert.ToDouble(txtMiktar.Text);
                tutar = kur * miktar;
                txtTutar.Text = tutar.ToString();
                kasa = Convert.ToInt32(label13.Text) - miktar;
                txtKalan.Text = kasa.ToString();
                label13.Text = kasa.ToString();
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("UPDATE TblKASA SET Dolar ='" + txtKalan.Text.ToString() + "'", baglanti);
                cmd.ExecuteNonQuery();
                baglanti.Close();
            }


            if (radioButton3.Checked) // Database'e Euro alış işlemi için seçilen radiobutton
            {
                kur = Convert.ToDouble(txtKur.Text);
                miktar = Convert.ToDouble(txtMiktar.Text);
                tutar = kur * miktar;
                txtTutar.Text = tutar.ToString();
                kasa = Convert.ToInt32(label16.Text) + miktar;
                txtKalan.Text = kasa.ToString();
                label16.Text = kasa.ToString();
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("UPDATE TblKASA SET Euro ='" + txtKalan.Text.ToString() + "'", baglanti);
                cmd.ExecuteNonQuery();
                baglanti.Close();
            }

            if (radioButton4.Checked) // Database'den Euro satış işlemi için seçilen radiobutton
            {
                kur = Convert.ToDouble(txtKur.Text);
                miktar = Convert.ToDouble(txtMiktar.Text);
                tutar = kur * miktar;
                txtTutar.Text = tutar.ToString();
                kasa = Convert.ToInt32(label16.Text) - miktar;
                txtKalan.Text = kasa.ToString();
                label16.Text = kasa.ToString();
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("UPDATE TblKASA SET Euro ='" + txtKalan.Text.ToString() + "'", baglanti);
                cmd.ExecuteNonQuery();
                baglanti.Close();
            }
        }
        private void txtKur_TextChanged(object sender, EventArgs e)
        {

        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e) //seçilen radiobuttondaki kur oranına göre txt e aktaran kodlar
        {
            txtKur.Text = lblEuroAlış.Text;
            textBox4.Text = lblEuroAlış.Text;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            txtKur.Text = lblEuroSatış.Text;
            textBox4.Text = lblEuroSatış.Text;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txtKur.Text = lblDolarAlış.Text;
            textBox4.Text = lblDolarAlış.Text;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
             txtKur.Text = lblDolarSatış.Text;
            textBox4.Text = lblDolarSatış.Text;

        }

       /* private void button1_Click(object sender, EventArgs e) // Girilen Fiyata Göre Seçilen radiobuttondaki işlemi yapan Button
        {
           
        }*/

        private void button1_Click_1(object sender, EventArgs e)
        {
            double kur, fiyat, kalan, kasa, tutar;
            

            if (radioButton1.Checked) // Database'e Dolar alış işlemi için seçilen radiobutton
            {
                
                kur = Convert.ToDouble(textBox4.Text);
                fiyat = Convert.ToDouble(textBox3.Text);                             
                tutar = fiyat/kur;
                //kalan = (kur * tutar) - fiyat;
                string tutarString = tutar.ToString("0."); // İki basamak ondalık kısmı olan bir string

                textBox2.Text = tutarString;
                kasa = Convert.ToInt32(label13.Text) + Convert.ToInt32(tutarString);

                textBox2.Text = tutar.ToString();
                label13.Text = kasa.ToString();
               // textBox1.Text = kalan.ToString();

                baglanti.Open();
                SqlCommand cmd = new SqlCommand("UPDATE TblKASA SET Dolar ='" + textBox1.Text.ToString() + "'", baglanti);
                cmd.ExecuteNonQuery();
                baglanti.Close();
            }
        }
    }
}
