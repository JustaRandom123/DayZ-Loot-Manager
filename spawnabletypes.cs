using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace LootManager
{

    
    public partial class spawnabletypes : Form
    {
        public spawnabletypes()
        {
            InitializeComponent();
            this.FormClosing += CloseForm;     
        }

        private void CloseForm (object sender, EventArgs e)
        {
            this.Dispose();
            Form1 frm = new Form1();
            frm.Show();
        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open cfgspawnabletypes.xml File";
            theDialog.Filter = "xml files|*.xml";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                LootManager.Properties.Settings.Default.spawnabletypesPath = theDialog.FileName.ToString();
                textBox1.Text = theDialog.FileName.ToString();
                LootManager.Properties.Settings.Default.Save();
                loadItemNames();
            }
            else
            {
                theDialog.Dispose();
            }
        }

        public ArrayList data = new ArrayList();
        private void loadItemNames ()
        {
            using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.spawnabletypesPath))
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
                                comboBox1.Items.Add(reader.GetAttribute("name").ToString());
                                XElement el = (XElement)XNode.ReadFrom(reader);
                                data.Add(el.ToString());                        
                                                            
                            }
                            break;
                    }
                }
            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void spawnabletypes_Load(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.spawnabletypesPath == "")
            {
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open cfgspawnabletypes.xml File";
                theDialog.Filter = "xml files|*.xml";
                theDialog.InitialDirectory = @"C:\";
                if (theDialog.ShowDialog() == DialogResult.OK)
                {
                    LootManager.Properties.Settings.Default.spawnabletypesPath = theDialog.FileName.ToString();
                    textBox1.Text = theDialog.FileName.ToString();
                    LootManager.Properties.Settings.Default.Save();
                    loadItemNames();
                }
                else
                {
                    this.Hide();
                    Form1 frm = new Form1();
                    frm.Show();
                }
            }
            else
            {
                if (!File.Exists(LootManager.Properties.Settings.Default.spawnabletypesPath))
                {
                    OpenFileDialog theDialog = new OpenFileDialog();
                    theDialog.Title = "Open cfgspawnabletypes.xml File";
                    theDialog.Filter = "xml files|*.xml";
                    theDialog.InitialDirectory = @"C:\";
                    if (theDialog.ShowDialog() == DialogResult.OK)
                    {
                        LootManager.Properties.Settings.Default.spawnabletypesPath = theDialog.FileName.ToString();
                        textBox1.Text = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.Save();
                        loadItemNames();
                    }
                    else
                    {
                        this.Hide();
                        Form1 frm = new Form1();
                        frm.Show();
                    }
                }
                else
                {
                    textBox1.Text = LootManager.Properties.Settings.Default.spawnabletypesPath;
                    loadItemNames();
                }
            }
        }

        public int counter0 = 0;
        public int counter = 0;
        public int counter2 = 0;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //showInList();
            counter = 0;
            counter0 = 0;
            counter2 = 0;
            dataGridView1.Rows.Clear();

            using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.spawnabletypesPath))
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
                                if (reader.GetAttribute("name").ToString() == comboBox1.SelectedItem.ToString())
                                {
                                    dataGridView1.Rows.Insert(counter0, "");
                                    XElement el = (XElement)XNode.ReadFrom(reader);
                                    using (var reader2 = el.CreateReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            if (reader2.Name.ToString() == "attachments")
                                            {                                               
                                                dataGridView1.Rows.Insert(counter, reader2.GetAttribute("chance").ToString());
                                              
                                                counter++;

                                                XElement el3 = (XElement)XNode.ReadFrom(reader2);
                                                using (var reader3 = el3.CreateReader())
                                                {
                                                    while (reader3.Read())
                                                    {
                                                        if (reader3.Name.ToString() == "item")
                                                        {
                                                            
                                                            //  dataGridView1.Rows.Insert(1, reader3.GetAttribute("name").ToString());
                                                            dataGridView1["Attachments", counter2].Value = reader3.GetAttribute("name").ToString();
                                                            dataGridView1["ItemChance", counter2].Value = reader3.GetAttribute("chance").ToString();
                                                            counter2++;
                                                            
                                                        }
                                               
                                                      

                                                        // dataGridView1.Rows.Insert(0, reader3.GetAttribute("chance").ToString());                                                     
                                                    }
                                                   
                                                }
                                               break;
                                            }                                         
                                        }
                                       
                                    }
                                    break;
                                }
                                break;
                            }
                            break;
                    }
                }
            }
        }
    }   
}
