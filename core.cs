using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        public int type;
        private void button1_Click(object sender, EventArgs e)
        {
            Stream stream = null;
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Metin Belgesi|*.txt";
            if (file.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(file.FileName);
                filename = file.FileName;
                if (fi.Exists)
                {
                    try
                    {
                        if ((stream = file.OpenFile()) != null)
                        {

                           
                           filelines = File.ReadAllLines(filename);
                           fileline = filelines.Length;
                           string safeName = file.SafeFileName;
                           
                           if (fileline > 0)
                           {

                                   button2.Enabled = true;
                                   label1.Text = "Kullanıcı bekleniyor";
                                   toolStripStatusLabel1.Text = "Dosya Seçildi: " + safeName;
                                   toolStripStatusLabel3.Text = fileline + " Satır";

                           }

                           }
                           else
                           {
                               MessageBox.Show("Bu dosya boş.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                           }
                        } 
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: Dosyanız diskten okunamıyor, silinmiş veya izin gerektiriyor olabilir, DETAY: " + ex.Message);
                    }
                } // Dosya bulunuyorsa sonu.
                else 
                {
                    MessageBox.Show("Bir sorun oluştu.");
                }
            }
        }

        public string filename;
        public int fileline;
        public string[] filelines;
        public string satir;
        private void oku()
        {
            StreamReader sr = new StreamReader(filename);

            string currentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currentPath, "results")))
                Directory.CreateDirectory(Path.Combine(currentPath, "results"));

            string filePath_id = currentPath + "/results/uid.txt";
            string filePath_psw = currentPath + "/results/pass.txt";

            File.WriteAllText(filePath_psw, string.Empty);
            File.WriteAllText(filePath_id, string.Empty);

            // UID text file control.
            if (!File.Exists(filePath_id))
            {
                File.Create(filePath_id);
            }

            // Password text file control.
            if (!File.Exists(filePath_psw))
            {
                File.Create(filePath_psw);
            }

            while (!sr.EndOfStream)
            {

                satir = (sr.ReadLine());
                label1.Text = (sr.ReadLine());
                string[] stringSeparators = new string[] { ":" };
                string[] result;  

                try
                {
                    result = satir.Split(stringSeparators, StringSplitOptions.None);
                    if (!Directory.Exists(Path.Combine(currentPath, "results")))
                        Directory.CreateDirectory(Path.Combine(currentPath, "results"));

                    // Writing text file (UID)
                    using (System.IO.StreamWriter uid =
                new StreamWriter(Application.StartupPath + "/results/uid.txt", true))
                    {
                        uid.WriteLine(result[0]);
                    }

                    // Writing text file (PASS)
                    using (System.IO.StreamWriter pass =
    new System.IO.StreamWriter(Application.StartupPath + "/results/pass.txt", true))
                    {
                        pass.WriteLine(result[1]);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu, DETAY: "+ex.Message, "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

            } 
            
                // If file processes completed. 
                if (sr.EndOfStream)
                {
                    MessageBox.Show("Ayırma İşlemi Tamamlandı", "Tamamlandı!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(@currentPath+"/results");
                    button2.Enabled = true;
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => oku());
            t.Start();
            button2.Enabled = false;
        }

    }
}

