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
            string bugun = "https://tcmb.gov.tr/kurlar/today.xml";
            var xmlDosya= new XmlDocument();
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
            double kur, miktar, tutar;

            if (radioButton1.Checked)
            {
                kur = Convert.ToDouble(txtKur.Text);
                miktar = Convert.ToDouble(txtMiktar.Text);
                tutar = kur * miktar;
                txtTutar.Text = tutar.ToString();
                
                baglanti.Open();
                
                SqlCommand komut=new SqlCommand("Select Dolar from TblKASA",baglanti);
               ;
                baglanti.Close();

            }
            if (radioButton2.Checked)
            {

            }
            if (radioButton3.Checked)
            {

            }
            if (radioButton4.Checked)
            {

            }
         
        }

        private void txtKur_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double kur = Convert.ToDouble(txtKur.Text);
            int miktar = Convert.ToInt32(txtMiktar.Text);
            int tutar = Convert.ToInt32( miktar / kur );
            txtTutar.Text=tutar.ToString();
            double kalan;
            kalan = miktar%kur;
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
    }
}
