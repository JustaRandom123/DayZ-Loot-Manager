using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace LootManager
{
    public partial class TraderChecker : Form
    {
        public TraderChecker()
        {
            InitializeComponent();
            this.FormClosing += closedForm;

        }

        private void closedForm (object sender, EventArgs e)
        {
            this.Dispose();
            Form1 frm = new Form1();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Trader Config",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                LootManager.Properties.Settings.Default.traderPath = textBox1.Text;
                LootManager.Properties.Settings.Default.Save();
            }
            else 
            {
                openFileDialog1.Dispose();
               // this.Hide();
                //Form1 frm = new Form1();
                //frm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.traderPath != "")
            {
                if (!File.Exists(LootManager.Properties.Settings.Default.traderPath) == true)
                {
                    OpenFileDialog theDialog = new OpenFileDialog();
                    theDialog.Title = "Select your Trader File";
                    theDialog.Filter = "xml files|*.xml";
                    theDialog.InitialDirectory = @"C:\";
                    if (theDialog.ShowDialog() == DialogResult.OK)
                    {
                        //  LootManager.Properties.Settings.Default.path = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.xmlPath = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.Save();
                    }
                }
            }
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Types xml",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "xml",
                Filter = "xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
         
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
            else
            {
                openFileDialog1.Dispose();
               // this.Hide();
                //Form1 frm = new Form1();
                //frm.Show();
            }
        }

        ArrayList DatenListe = new ArrayList();
        ArrayList tools = new ArrayList();
        ArrayList clothes = new ArrayList();
        ArrayList food = new ArrayList();
        ArrayList weapons = new ArrayList();

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                richTextBox1.Text = "";
                string wholeTraderFile = File.ReadAllText(textBox1.Text);
                using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.xmlPath))
                {
                    XmlWriterSettings ws = new XmlWriterSettings();
                    ws.Indent = true;
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name.ToString() == "type")
                                {
                                    XElement el = (XElement)XNode.ReadFrom(reader);
                                    using (var reader2 = el.CreateReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            if (reader2.Name.ToString() == "category")
                                            {
                                                if (reader2.GetAttribute("name").ToString() == "tools")
                                                {
                                                    tools.Add(el.ToString());
                                                    //   MessageBox.Show(el.ToString());
                                                }
                                                else if (reader2.GetAttribute("name").ToString() == "clothes")
                                                {
                                                    clothes.Add(el.ToString());
                                                    //   MessageBox.Show(el.ToString());
                                                }
                                                else if (reader2.GetAttribute("name").ToString() == "food")
                                                {
                                                    food.Add(el.ToString());
                                                    //   MessageBox.Show(el.ToString());
                                                }
                                                else if (reader2.GetAttribute("name").ToString() == "weapons")
                                                {
                                                    weapons.Add(el.ToString());
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }

                richTextBox1.Text = richTextBox1.Text + "------------------Tools-----------------";
                foreach (string mod in tools)
                {
                    string modname = String.Empty;
                    using (StringReader sr = new StringReader(mod.Trim()))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string currentwort = line;
                            currentwort = currentwort.Trim();

                            if (currentwort.StartsWith("<type name="))  //mod name 
                            {

                                modname = currentwort;
                                modname = modname.Substring(11);
                                modname = modname.Substring(1, modname.Length - 3);
                                if (!wholeTraderFile.ToLower().Contains(modname.ToLower()))
                                {
                                    richTextBox1.Text = richTextBox1.Text + "\n" + modname + ",          *,      800,   400";
                                }

                            }
                        }
                    }
                }
                richTextBox1.Text = richTextBox1.Text + "\n\n\n\n------------------Weapons-----------------";

                foreach (string mod in weapons)
                {
                    string modname = String.Empty;
                    using (StringReader sr = new StringReader(mod.Trim()))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string currentwort = line;
                            currentwort = currentwort.Trim();

                            if (currentwort.StartsWith("<type name="))  //mod name 
                            {

                                modname = currentwort;
                                modname = modname.Substring(11);
                                modname = modname.Substring(1, modname.Length - 3);
                                if (!wholeTraderFile.ToLower().Contains(modname.ToLower()))
                                {
                                    richTextBox1.Text = richTextBox1.Text + "\n" + modname + ",          W,      2400,   1000";
                                }
                            }
                        }
                    }
                }
                richTextBox1.Text = richTextBox1.Text + "\n\n\n\n------------------Food-----------------";
                foreach (string mod in food)
                {
                    string modname = String.Empty;
                    using (StringReader sr = new StringReader(mod.Trim()))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string currentwort = line;
                            currentwort = currentwort.Trim();

                            if (currentwort.StartsWith("<type name="))  //mod name 
                            {

                                modname = currentwort;
                                modname = modname.Substring(11);
                                modname = modname.Substring(1, modname.Length - 3);
                                if (!wholeTraderFile.ToLower().Contains(modname.ToLower()))
                                {
                                    richTextBox1.Text = richTextBox1.Text.Trim() + "\n" + modname + ",				*,		40,		-1";
                                }
                            }
                        }
                    }
                }
                richTextBox1.Text = richTextBox1.Text + "\n\n\n\n------------------Clothes-----------------";
                foreach (string mod in clothes)
                {
                    string modname = String.Empty;
                    using (StringReader sr = new StringReader(mod.Trim()))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string currentwort = line;
                            currentwort = currentwort.Trim();

                            if (currentwort.StartsWith("<type name="))  //mod name 
                            {

                                modname = currentwort;
                                modname = modname.Substring(11);
                                modname = modname.Substring(1, modname.Length - 3);
                                if (!wholeTraderFile.ToLower().Contains(modname.ToLower()))
                                {
                                    richTextBox1.Text = richTextBox1.Text + "\n" + modname + ",          *,      500,   250";
                                }
                            }
                        }
                    }
                }
                richTextBox1.Text = richTextBox1.Text + "\n------------------END-----------------";
            }
            else
            {
                MessageBox.Show("Please select your paths!");
                return;
            }
        }

        private void TraderChecker_Load(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.xmlPath != "")
            {
                textBox2.Text = LootManager.Properties.Settings.Default.xmlPath;
            }
            if (LootManager.Properties.Settings.Default.traderPath != "")
            {

                if (!File.Exists(LootManager.Properties.Settings.Default.traderPath) == true)
                {
                    textBox1.Text = "";
                    LootManager.Properties.Settings.Default.traderPath = "";
                    LootManager.Properties.Settings.Default.Save();
                 
                }
                else
                {
                    textBox1.Text = LootManager.Properties.Settings.Default.traderPath;
                }             
            }
        }
    }
}
