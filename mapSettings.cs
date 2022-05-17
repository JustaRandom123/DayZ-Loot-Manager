using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LootManager
{
    public partial class mapSettings : Form
    {
        public mapSettings()
        {
            InitializeComponent();
        }

        private static PlayerspawnCreater psc = new PlayerspawnCreater();

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            LootManager.Properties.Settings.Default.currentMarker = "hatchback";
            LootManager.Properties.Settings.Default.Save();          
         //   MessageBox.Show("New marker selected: hatchback");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LootManager.Properties.Settings.Default.currentMarker = "civiliansedan";
            LootManager.Properties.Settings.Default.Save();
          //  MessageBox.Show("New marker selected: civiliansedan");
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            LootManager.Properties.Settings.Default.currentMarker = "moddedCar";
            LootManager.Properties.Settings.Default.Save();
          //  MessageBox.Show("New marker selected: moddedCar");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            LootManager.Properties.Settings.Default.currentMarker = "marker";
            LootManager.Properties.Settings.Default.Save();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            LootManager.Properties.Settings.Default.currentMarker = "policecar";
            LootManager.Properties.Settings.Default.Save();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            LootManager.Properties.Settings.Default.currentMarker = "planks";
            LootManager.Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void mapSettings_Load(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.currentMarker == "hatchback")
            {
                radioButton2.Checked = true;
            }
            else if (LootManager.Properties.Settings.Default.currentMarker == "civiliansedan")
            {
                radioButton1.Checked = true;
            }
            else if (LootManager.Properties.Settings.Default.currentMarker == "moddedCar")
            {
                radioButton4.Checked = true;
            }
            else if (LootManager.Properties.Settings.Default.currentMarker == "marker")
            {
                radioButton3.Checked = true;
            }
            else if (LootManager.Properties.Settings.Default.currentMarker == "policecar")
            {
                radioButton5.Checked = true;
            }
            else if (LootManager.Properties.Settings.Default.currentMarker == "planks")
            {
                radioButton6.Checked = true;
            }
            richTextBox1.Text = LootManager.Properties.Settings.Default.richtxtboxString;

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //psc.nummer = 0;
            richTextBox1.Text = "";
            LootManager.Properties.Settings.Default.richtxtboxString = null;
            LootManager.Properties.Settings.Default.Save();
        }

       
    }
}
