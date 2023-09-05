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
        SqlConnection baglanti = new SqlConnection("Data Source=KLMNTB139;Initial Catalog=KLIMASAN;Persist Security Info=True;User ID=sa; password=Klm_1234");
        private void Form1_Load(object sender, EventArgs e)
        {

            baglanti.Open();

            SqlCommand komut = new SqlCommand("Select DOLAR from KASATABLES", baglanti);

            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                label13.Text = read["DOLAR"].ToString();
            }


            baglanti.Close();





            string bugun = "https://tcmb.gov.tr/kurlar/today.xml";
            var xmlDosya = new XmlDocument();
            xmlDosya.Load(bugun);
            txtKur.Enabled = false;
            txtKalan.Enabled = false;

            string dolaralis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            lblDolarAlış.Text = dolaralis;
            string dolarsatis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            lblDolarSatış.Text = dolarsatis;
            string euroalis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            lblEuroAlış.Text = euroalis;
            string eurosatis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            lblEuroSatış.Text = eurosatis;
        }
        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            double kur, miktar, tutar, kasa;

            if (radioButton1.Checked)
            {
                kur = Convert.ToDouble(txtKur.Text);
                miktar = Convert.ToDouble(txtMiktar.Text);
                tutar = kur * miktar;
                txtTutar.Text = tutar.ToString();


                kasa = Convert.ToInt32(label13.Text) - miktar;
                txtKalan.Text = kasa.ToString();
                label13.Text = kasa.ToString();


                baglanti.Open();
                SqlCommand cmd = new SqlCommand("UPDATE KASATABLES SET DOLAR ='" + txtKalan.Text.ToString() + "'", baglanti);
                cmd.ExecuteNonQuery();
                baglanti.Close();

            }















        }





        private void txtKur_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double kur = Convert.ToDouble(txtKur.Text);
            int miktar = Convert.ToInt32(txtMiktar.Text);
            int tutar = Convert.ToInt32(miktar / kur);
            txtTutar.Text = tutar.ToString();
            double kalan;
            kalan = miktar % kur;
            txtKalan.Text = kalan.ToString();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txtKur.Text = lblDolarAlış.Text;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            txtKur.Text = lblDolarSatış.Text;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            txtKur.Text = lblEuroAlış.Text;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            txtKur.Text = lblEuroSatış.Text;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            txtKur.Text = lblDolarAlış.Text;
        }
    }
}
