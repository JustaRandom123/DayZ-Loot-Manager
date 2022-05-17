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


    public partial class playerSpawnItems : Form
    {
        private Log logClass = new Log();
        public ArrayList tools = new ArrayList();
        public ArrayList clothes = new ArrayList();
        public ArrayList food = new ArrayList();
        public ArrayList weapons = new ArrayList();


        public playerSpawnItems()
        {
            InitializeComponent();
            this.FormClosing += ClosingForm;
        }

        private void ClosingForm(object sender, EventArgs e)
        {
            this.Dispose();
            Form1 frm = new Form1();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open init.c File";
            theDialog.Filter = "c files|*.c";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                LootManager.Properties.Settings.Default.initPath = theDialog.FileName.ToString();
                textBox1.Text = theDialog.FileName.ToString();
                LootManager.Properties.Settings.Default.Save();
            }
            else
            {
                theDialog.Dispose();
            }
        }

        private void playerSpawnItems_Load(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.initPath == "")
            {
                logClass.LogWriter("[init.c] No file path existing select one...");
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open init.c File";
                theDialog.Filter = "c files|*.c";
                theDialog.InitialDirectory = @"C:\";
                if (theDialog.ShowDialog() == DialogResult.OK)
                {
                    LootManager.Properties.Settings.Default.initPath = theDialog.FileName.ToString();
                    textBox1.Text = theDialog.FileName.ToString();
                    LootManager.Properties.Settings.Default.Save();
                }
                else
                {
                    theDialog.Dispose();
                }
            }
            else
            {
                if (!File.Exists(LootManager.Properties.Settings.Default.initPath) == true)
                {
                    logClass.LogWriter("[init.c]  file path existing...");
                    OpenFileDialog theDialog = new OpenFileDialog();
                    theDialog.Title = "Open init.c File";
                    theDialog.Filter = "c files|*.c";
                    theDialog.InitialDirectory = @"C:\";
                    if (theDialog.ShowDialog() == DialogResult.OK)
                    {
                        //  LootManager.Properties.Settings.Default.path = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.initPath = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.Save();
                        textBox1.Text = theDialog.FileName.ToString();
                    }
                    else
                    {
                        theDialog.Dispose();
                    }
                }
                else
                {
                    textBox1.Text = LootManager.Properties.Settings.Default.initPath;

                    using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.xmlPath))
                    {
                        logClass.LogWriter("[init.c]  Reading xml for item names in combobox...");
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

                    logClass.LogWriter("[init.c]  Reading xml done!...");


                }
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            //  StringBuilder sb = new StringBuilder("", 50);
            logClass.LogWriter("[init.c]  Added Item to configurator...");
            if (textBox2.Text != "" )//&& comboBox1.SelectedItem.ToString() != null && comboBox2.Se != null)
            {
                string quantity = textBox2.Text;
                string item = comboBox1.SelectedItem.ToString();
                string number1 = "itemEnt = player.GetInventory().CreateInInventory('#item');";
                string number2 = "itemBS = ItemBase.Cast(itemEnt);";
                string number3 = "itemBS.SetQuantity(#count);";


                number1 = number1.Replace("#item", item);
                number3 = number3.Replace("#count", quantity);

                // sb.Append(new string[] { number1,number2,number3 });

                listBox1.Items.Add(number1 + "\n" + number2 + "\n" + number3);
            }
            else
            {
                MessageBox.Show("Please select an item and the quantity!");
                return;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string modname = String.Empty;
            string item = comboBox2.SelectedItem.ToString();

            comboBox1.Items.Clear();

            if (item == "Clothes")
            {

                foreach (string clothesCategory in clothes)
                {

                    using (StringReader sr = new StringReader(clothesCategory.Trim()))
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
                                comboBox1.Items.Add(modname);
                            }
                        }
                        sr.Dispose();
                        sr.Close();
                    }
                }

            }
            if (item == "Tools")
            {
                foreach (string toolCategory in tools)
                {

                    using (StringReader sr = new StringReader(toolCategory.Trim()))
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
                                comboBox1.Items.Add(modname);
                            }
                        }
                        sr.Dispose();
                        sr.Close();
                    }
                }

            }
            if (item == "Weapons")
            {
                foreach (string weaponsCategory in weapons)
                {

                    using (StringReader sr = new StringReader(weaponsCategory.Trim()))
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
                                comboBox1.Items.Add(modname);

                            }
                        }
                        sr.Dispose();
                        sr.Close();
                    }
                }

            }
            if (item == "Food")
            {
                foreach (string foodCategory in food)
                {

                    using (StringReader sr = new StringReader(foodCategory.Trim()))
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
                                comboBox1.Items.Add(modname);

                            }
                        }
                        sr.Dispose();
                        sr.Close();
                    }
                }
            }
            logClass.LogWriter("[init.c]  Adding items to combobox finished...");
        }
       
        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private int startCount;
        private int endCount;

        private void button3_Click(object sender, EventArgs e)
        {
            
          
            if (listBox1.Items.Count <= 4)
            {
                MessageBox.Show("Please select some items you want to add to your spawn gear!");
                return;
            }

            listBox1.Items.Add("}");

            int counter1 = 0;
            int counter2 = 0;


            string[] lines = File.ReadAllLines(LootManager.Properties.Settings.Default.initPath);

            foreach (string line in lines)
            {
                if (line.Trim().StartsWith("override void StartingEquipSetup"))
                {
                    //   MessageBox.Show("Lines until the function: " + counter1);
                    startCount = counter1 - 1;
                }
                else
                {
                    counter1++;
                }

                if (line.Trim().StartsWith("};"))
                {
                    // MessageBox.Show("Lines until the function end: " + counter2);
                    endCount = counter2 - 1;
                }
                else
                {
                    counter2++;
                }
            }

            foreach (string line in lines)
            {
                if (startCount != endCount)
                {
                    // removeLine(LootManager.Properties.Settings.Default.initPath, endCount);
                    lines[endCount] = "";
                    File.WriteAllLines(LootManager.Properties.Settings.Default.initPath, lines);
                    endCount--;
                }
                else
                {
                    string modstring = String.Empty;
                    foreach (string data in listBox1.Items)
                    {
                        modstring = modstring + "\n" + data;
                    }

                    lines[endCount] = modstring;
                    File.WriteAllLines(LootManager.Properties.Settings.Default.initPath, lines);
                    logClass.LogWriter("[init.c]  Writing lines...");
                }
            }

            logClass.LogWriter("[init.c]  Saved...");
            MessageBox.Show("Init got successfully changed!");
            listBox1.Items.Clear();

            listBox1.Items.Add("override void StartingEquipSetup(PlayerBase player, bool clothesChosen)");
            listBox1.Items.Add("{");
            listBox1.Items.Add("EntityAI itemEnt;");
            listBox1.Items.Add("ItemBase itemBS;");

        }



      
        //private void removeLine(string path, int line)
        //{
        //    List<string> lines = File.ReadAllLines(path).ToList();
        //    if (lines.Count >= line)
        //    {
        //        lines.RemoveAt(line - 1);
        //        File.WriteAllLines(path, lines);
        //    }
        //    else
        //    {
        //        lines[endCount] = "#insertData";
        //        File.WriteAllLines(LootManager.Properties.Settings.Default.initPath, lines);
        //    }
        //}
    }
}
