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

 
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += CloseForm;         
        }

        private void CloseForm (object sender, EventArgs e)
        {
            this.Dispose();
            Process[] processes = Process.GetProcessesByName("LootManager");
            foreach (Process p in processes)
            {
                p.Kill();
            }           
        }

        private Log logClass = new Log();
       

        private Label NameHeader = new Label();
        private Label MaximumHeader = new Label();
        private Label MinimumHeader = new Label();
        private Label PriorityHeader = new Label();
        private Label RestockHeader = new Label();
        private Label LifetimeHeader = new Label();
        private Label CategoryHeader = new Label();

        public ArrayList modnamesArray = new ArrayList();
        public ArrayList maximalArray = new ArrayList();
        public ArrayList minimalArray = new ArrayList();
        public ArrayList priorityArray = new ArrayList();
        public ArrayList restockArray = new ArrayList();
        public ArrayList lifetimeArray = new ArrayList();
        public ArrayList categoryArray = new ArrayList();

        public static int counter2 = 0;
        public static int counter3 = 0;

        // categorys
        public ArrayList tools = new ArrayList();
        public ArrayList clothes = new ArrayList();
        public ArrayList food = new ArrayList();
        public ArrayList weapons = new ArrayList();



        int count = 0;

  
        WebClient wclient = new WebClient();
        private void Form1_Load(object sender, EventArgs e)
        {
            string serverversion = wclient.DownloadString("http://185.223.31.43/dayzlootmanager/version.ini");

            if (LootManager.Properties.Settings.Default.version != serverversion)
            {
                MessageBox.Show("New update available!");
                logClass.LogWriter("Getting new update!");
                Process.Start(Application.StartupPath + "\\LootManagerUpdater.exe", "confirmed");
                logClass.LogWriter("Starting Updater...");
                Application.Exit();
                
            }
            else
            {
                
              //  createHeader();
                if (LootManager.Properties.Settings.Default.xmlPath == "")
                {
                    OpenFileDialog theDialog = new OpenFileDialog();
                    theDialog.Title = "Open types.xml File";
                    theDialog.Filter = "xml files|*.xml";
                    theDialog.InitialDirectory = @"C:\";
                    if (theDialog.ShowDialog() == DialogResult.OK)
                    {
                      //  LootManager.Properties.Settings.Default.path = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.xmlPath = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Application.Exit();
                        return;
                    }
                }
                else
                {
                    if (!File.Exists(LootManager.Properties.Settings.Default.xmlPath) == true)
                    {
                        OpenFileDialog theDialog = new OpenFileDialog();
                        theDialog.Title = "Open types.xml File";
                        theDialog.Filter = "xml files|*.xml";
                        theDialog.InitialDirectory = @"C:\";
                        if (theDialog.ShowDialog() == DialogResult.OK)
                        {
                          //  LootManager.Properties.Settings.Default.path = theDialog.FileName.ToString();
                            LootManager.Properties.Settings.Default.xmlPath = theDialog.FileName.ToString();
                            LootManager.Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Application.Exit();
                            return;
                        }
                    }
                }


             
                using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.xmlPath))
                {
                    logClass.LogWriter("[TYPES.XML] Reading xml...");
                    XmlWriterSettings ws = new XmlWriterSettings();
                    ws.Indent = true;
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name.ToString() == "type")
                                {
                                   // logClass.LogWriter("[TYPES.XML] Getting xml type...");
                                    XElement el = (XElement)XNode.ReadFrom(reader);
                                    using (var reader2 = el.CreateReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            if (reader2.Name.ToString() == "category")
                                            {
                                            //    logClass.LogWriter("[TYPES.XML] Getting Category...");
                                                if (reader2.GetAttribute("name").ToString() == "tools")
                                                {
                                                  //  logClass.LogWriter("[TYPES.XML] Gettings tools...");
                                                    tools.Add(el.ToString());
                                                    //   MessageBox.Show(el.ToString());
                                                }
                                                else if (reader2.GetAttribute("name").ToString() == "clothes")
                                                {
                                                   // logClass.LogWriter("[TYPES.XML] Gettings clothes...");
                                                    clothes.Add(el.ToString());
                                                    //   MessageBox.Show(el.ToString());
                                                }
                                                else if (reader2.GetAttribute("name").ToString() == "food")
                                                {
                                                 //   logClass.LogWriter("[TYPES.XML] Gettings food...");
                                                    food.Add(el.ToString());
                                                    //   MessageBox.Show(el.ToString());
                                                }
                                                else if (reader2.GetAttribute("name").ToString() == "weapons")
                                                {
                                                  //  logClass.LogWriter("[TYPES.XML] Gettings weapons...");
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
                    logClass.LogWriter("[TYPES.XML] Finished xml reading...");
                }
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            count = 0;

            string modname = String.Empty;
            string maximal = String.Empty;
            string minimal = String.Empty;
            string priority = String.Empty;
            string restock = String.Empty;
            string lifetime = String.Empty;
            string category = String.Empty;
            string usage = String.Empty;
            string quantmin = String.Empty;
            string quantmax = String.Empty;


            string item = comboBox1.SelectedItem.ToString();

            dataGridView1.Rows.Clear();     

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            int count5 = 0;
            int count6 = 0;
            int count7 = 0;
            int count8 = 0;
            int count9 = 0;          

            if (item == "Clothes")
            {
                logClass.LogWriter("[TYPES.XML] Adding Clothes to datagridview...");
                foreach (string clothesCategory in clothes)
                {
                    count++;
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

                                dataGridView1.Rows.Insert(count1, modname);
                                count1++;
                                //   modnamesArray.Add(modname);


                            }
                            else if (currentwort.StartsWith("<nominal>")) //maximale spawnrate
                            {

                                maximal = currentwort;
                                maximal = maximal.Substring(8);
                                maximal = maximal.Substring(1, maximal.Length - 11);

                                dataGridView1["Maximum", count2].Value = maximal;
                                count2++;
                                //   maximalArray.Add(maximal);

                            }
                            else if (currentwort.StartsWith("<min>")) //minimale spawnrate
                            {

                                minimal = currentwort;
                                minimal = minimal.Substring(4);
                                minimal = minimal.Substring(1, minimal.Length - 7);

                                dataGridView1["Minimum", count3].Value = minimal;
                                count3++;

                                //  minimalArray.Add(minimal);

                            }
                            else if (currentwort.StartsWith("<cost>")) //priority
                            {

                                priority = currentwort;
                                priority = priority.Substring(5);
                                priority = priority.Substring(1, priority.Length - 8);

                                dataGridView1["Priority", count4].Value = priority;
                                count4++;

                                // priorityArray.Add(priority);
                            }
                            else if (currentwort.StartsWith("<restock>")) //restock time
                            {

                                restock = currentwort;
                                restock = restock.Substring(8);
                                restock = restock.Substring(1, restock.Length - 11);

                                dataGridView1["RestockTime", count5].Value = restock;
                                count5++;

                                // restockArray.Add(restock);

                            }
                            else if (currentwort.StartsWith("<lifetime>")) //life time
                            {

                                lifetime = currentwort;
                                lifetime = lifetime.Substring(9);
                                lifetime = lifetime.Substring(1, lifetime.Length - 12);

                                dataGridView1["Lifetime", count6].Value = lifetime;
                                count6++;

                                // lifetimeArray.Add(lifetime);

                            }

                            if (currentwort.StartsWith("<category name=")) //category
                            {

                                category = currentwort;
                                category = category.Substring(15);
                                category = category.Substring(1, category.Length - 5);

                                dataGridView1["Category", count7].Value = category;
                                count7++;

                                //  categoryArray.Add(category);
                            }
                            if (currentwort.StartsWith("<quantmin>")) //category
                            {

                                quantmin = currentwort;
                                quantmin = quantmin.Substring(9);
                                quantmin = quantmin.Substring(1, quantmin.Length - 12);
                                dataGridView1["QuantMin", count8].Value = quantmin;
                                count8++;

                             
                            }
                            if (currentwort.StartsWith("<quantmax>")) //category
                            {

                                quantmax = currentwort;
                                quantmax = quantmax.Substring(9);
                                quantmax = quantmax.Substring(1, quantmax.Length - 12);
                                dataGridView1["QuantMax", count9].Value = quantmax;
                                count9++;


                            }
                        }
                        sr.Dispose();
                        sr.Close();                    
                    }
                }            
                label14.Text = "Count: " + count;
              //  count = 0;
            }
            if (item == "Tools")
            {
                logClass.LogWriter("[TYPES.XML] Adding Tools to datagridview...");
                foreach (string toolCategory in tools)
                {
                    count++;
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

                                dataGridView1.Rows.Insert(count1, modname);
                                count1++;
                                //   modnamesArray.Add(modname);


                            }
                            else if (currentwort.StartsWith("<nominal>")) //maximale spawnrate
                            {

                                maximal = currentwort;
                                maximal = maximal.Substring(8);
                                maximal = maximal.Substring(1, maximal.Length - 11);

                                dataGridView1["Maximum", count2].Value = maximal;
                                count2++;


                                //   maximalArray.Add(maximal);

                            }
                            else if (currentwort.StartsWith("<min>")) //minimale spawnrate
                            {

                                minimal = currentwort;
                                minimal = minimal.Substring(4);
                                minimal = minimal.Substring(1, minimal.Length - 7);

                                dataGridView1["Minimum", count3].Value = minimal;
                                count3++;

                                //  minimalArray.Add(minimal);

                            }
                            else if (currentwort.StartsWith("<cost>")) //priority
                            {

                                priority = currentwort;
                                priority = priority.Substring(5);
                                priority = priority.Substring(1, priority.Length - 8);

                                dataGridView1["Priority", count4].Value = priority;
                                count4++;

                                // priorityArray.Add(priority);
                            }
                            else if (currentwort.StartsWith("<restock>")) //restock time
                            {

                                restock = currentwort;
                                restock = restock.Substring(8);
                                restock = restock.Substring(1, restock.Length - 11);

                                dataGridView1["RestockTime", count5].Value = restock;
                                count5++;

                                // restockArray.Add(restock);

                            }
                            else if (currentwort.StartsWith("<lifetime>")) //life time
                            {

                                lifetime = currentwort;
                                lifetime = lifetime.Substring(9);
                                lifetime = lifetime.Substring(1, lifetime.Length - 12);

                                dataGridView1["Lifetime", count6].Value = lifetime;
                                count6++;

                                // lifetimeArray.Add(lifetime);

                            }

                            if (currentwort.StartsWith("<category name=")) //category
                            {

                                category = currentwort;
                                category = category.Substring(15);
                                category = category.Substring(1, category.Length - 5);

                                dataGridView1["Category", count7].Value = category;
                                count7++;

                               
                            }
                            if (currentwort.StartsWith("<quantmin>")) //category
                            {

                                quantmin = currentwort;
                                quantmin = quantmin.Substring(9);
                                quantmin = quantmin.Substring(1, quantmin.Length - 12);
                                dataGridView1["QuantMin", count8].Value = quantmin;
                                count8++;
                            }
                            if (currentwort.StartsWith("<quantmax>")) //category
                            {

                                quantmax = currentwort;
                                quantmax = quantmax.Substring(9);
                                quantmax = quantmax.Substring(1, quantmax.Length - 12);
                                dataGridView1["QuantMax", count9].Value = quantmax;
                                count9++;
                            }
                        }
                        sr.Dispose();
                        sr.Close();                     
                    }
                }             
                label14.Text = "Count: " + count;
               // count = 0;
            }
            if (item == "Weapons")
            {
                logClass.LogWriter("[TYPES.XML] Adding Weapons to datagridview...");
                foreach (string weaponsCategory in weapons)
                {
                    count++;
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

                                dataGridView1.Rows.Insert(count1, modname);
                                count1++;
                                //   modnamesArray.Add(modname);


                            }
                            else if (currentwort.StartsWith("<nominal>")) //maximale spawnrate
                            {

                                maximal = currentwort;
                                maximal = maximal.Substring(8);
                                maximal = maximal.Substring(1, maximal.Length - 11);

                                dataGridView1["Maximum", count2].Value = maximal;
                                count2++;
                                //   maximalArray.Add(maximal);

                            }
                            else if (currentwort.StartsWith("<min>")) //minimale spawnrate
                            {

                                minimal = currentwort;
                                minimal = minimal.Substring(4);
                                minimal = minimal.Substring(1, minimal.Length - 7);

                                dataGridView1["Minimum", count3].Value = minimal;
                                count3++;

                                //  minimalArray.Add(minimal);

                            }
                            else if (currentwort.StartsWith("<cost>")) //priority
                            {

                                priority = currentwort;
                                priority = priority.Substring(5);
                                priority = priority.Substring(1, priority.Length - 8);


                                dataGridView1["Priority", count4].Value = priority;
                                count4++;

                                // priorityArray.Add(priority);
                            }
                            else if (currentwort.StartsWith("<restock>")) //restock time
                            {

                                restock = currentwort;
                                restock = restock.Substring(8);
                                restock = restock.Substring(1, restock.Length - 11);

                                dataGridView1["RestockTime", count5].Value = restock;
                                count5++;

                                // restockArray.Add(restock);

                            }
                            else if (currentwort.StartsWith("<lifetime>")) //life time
                            {

                                lifetime = currentwort;
                                lifetime = lifetime.Substring(9);
                                lifetime = lifetime.Substring(1, lifetime.Length - 12);

                                dataGridView1["Lifetime", count6].Value = lifetime;
                                count6++;

                                // lifetimeArray.Add(lifetime);

                            }

                            if (currentwort.StartsWith("<category name=")) //category
                            {

                                category = currentwort;
                                category = category.Substring(15);
                                category = category.Substring(1, category.Length - 5);

                                dataGridView1["Category", count7].Value = category;
                                count7++;

                                //  categoryArray.Add(category);
                            }
                            if (currentwort.StartsWith("<quantmin>")) //category
                            {

                                quantmin = currentwort;
                                quantmin = quantmin.Substring(9);
                                quantmin = quantmin.Substring(1, quantmin.Length - 12);
                                dataGridView1["QuantMin", count8].Value = quantmin;
                                count8++;
                            }
                            if (currentwort.StartsWith("<quantmax>")) //category
                            {

                                quantmax = currentwort;
                                quantmax = quantmax.Substring(9);
                                quantmax = quantmax.Substring(1, quantmax.Length - 12);
                                dataGridView1["QuantMax", count9].Value = quantmax;
                                count9++;
                            }
                        }
                        sr.Dispose();
                        sr.Close();                 
                    }
                }
                //panel1.ResumeLayout();
                label14.Text = "Count: " + count;
               // count = 0;
            }
            if (item == "Food")
            {
                logClass.LogWriter("[TYPES.XML] Adding Food to datagridview...");
                foreach (string foodCategory in food)
                {
                    count++;
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

                                dataGridView1.Rows.Insert(count1, modname);
                                count1++;
                                //   modnamesArray.Add(modname);


                            }
                            else if (currentwort.StartsWith("<nominal>")) //maximale spawnrate
                            {

                                maximal = currentwort;
                                maximal = maximal.Substring(8);
                                maximal = maximal.Substring(1, maximal.Length - 11);

                                dataGridView1["Maximum", count2].Value = maximal;
                                count2++;
                                //   maximalArray.Add(maximal);

                            }
                            else if (currentwort.StartsWith("<min>")) //minimale spawnrate
                            {

                                minimal = currentwort;
                                minimal = minimal.Substring(4);
                                minimal = minimal.Substring(1, minimal.Length - 7);

                                dataGridView1["Minimum", count3].Value = minimal;
                                count3++;

                                //  minimalArray.Add(minimal);

                            }
                            else if (currentwort.StartsWith("<cost>")) //priority
                            {

                                priority = currentwort;
                                priority = priority.Substring(5);
                                priority = priority.Substring(1, priority.Length - 8);

                                dataGridView1["Priority", count4].Value = priority;
                                count4++;

                                // priorityArray.Add(priority);
                            }
                            else if (currentwort.StartsWith("<restock>")) //restock time
                            {

                                restock = currentwort;
                                restock = restock.Substring(8);
                                restock = restock.Substring(1, restock.Length - 11);

                                dataGridView1["RestockTime", count5].Value = restock;
                                count5++;

                                // restockArray.Add(restock);

                            }
                            else if (currentwort.StartsWith("<lifetime>")) //life time
                            {

                                lifetime = currentwort;
                                lifetime = lifetime.Substring(9);
                                lifetime = lifetime.Substring(1, lifetime.Length - 12);


                                dataGridView1["Lifetime", count6].Value = lifetime;
                                count6++;

                                // lifetimeArray.Add(lifetime);

                            }

                            if (currentwort.StartsWith("<category name=")) //category
                            {

                                category = currentwort;
                                category = category.Substring(15);
                                category = category.Substring(1, category.Length - 5);
                                dataGridView1["Category", count7].Value = category;
                                count7++;                             
                            }
                            if (currentwort.StartsWith("<quantmin>")) //category
                            {

                                quantmin = currentwort;
                                quantmin = quantmin.Substring(9);
                                quantmin = quantmin.Substring(1, quantmin.Length - 12);
                                dataGridView1["QuantMin", count8].Value = quantmin;
                                count8++;
                            }
                            if (currentwort.StartsWith("<quantmax>")) //category
                            {

                                quantmax = currentwort;
                                quantmax = quantmax.Substring(9);
                                quantmax = quantmax.Substring(1, quantmax.Length - 12);
                                dataGridView1["QuantMax", count9].Value = quantmax;
                                count9++;
                            }
                        }
                        sr.Dispose();
                        sr.Close();                   
                    }
                }
            }   
            logClass.LogWriter("[TYPES.XML] Finished adding items to datagridview...");
            label14.Text = "Count: " + count;
          //  count = 0;
        }

       

        public ArrayList newData = new ArrayList();


        public ArrayList newDataName = new ArrayList();
        public ArrayList newDataMaximum = new ArrayList();
        public ArrayList newDataMinimum = new ArrayList();
        public ArrayList newDataPriority = new ArrayList();
        public ArrayList newDataRestockTime = new ArrayList();
        public ArrayList newDataLifetime = new ArrayList();
        public ArrayList newDataQuantMin = new ArrayList();
        public ArrayList newDataQuantMax = new ArrayList();

        private void button1_Click(object sender, EventArgs e)
        {      

            string current = String.Empty;
            StringBuilder sb = new StringBuilder("", 7);

            string modname = String.Empty;
            string maximal = String.Empty;
            string minimal = String.Empty;
            string priority = String.Empty;
            string restock = String.Empty;
            string lifetime = String.Empty;
            string category = String.Empty;

            //string item = comboBox1.SelectedItem.ToString();
            int counter = 0;

            //if (item != "")
            //{

            logClass.LogWriter("[TYPES.XML] Saving new values...");

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var currQty = row.Cells["Name"].Value;
                newDataName.Add(currQty.ToString());              
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var currQty = row.Cells["Maximum"].Value;
                newDataMaximum.Add(currQty.ToString());
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var currQty = row.Cells["Minimum"].Value;
                newDataMinimum.Add(currQty.ToString());
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var currQty = row.Cells["Priority"].Value;
                newDataPriority.Add(currQty.ToString());
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var currQty = row.Cells["RestockTime"].Value;
                newDataRestockTime.Add(currQty.ToString());
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var currQty = row.Cells["Lifetime"].Value;
                newDataLifetime.Add(currQty.ToString());
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var currQty = row.Cells["QuantMin"].Value;
                newDataQuantMin.Add(currQty.ToString());
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var currQty = row.Cells["QuantMax"].Value;
                newDataQuantMax.Add(currQty.ToString());
            }

            this.Enabled = false;


            int indexCounter = 0;
            foreach( string modnameInfo in newDataName)
            {
                XDocument doc = XDocument.Load(LootManager.Properties.Settings.Default.xmlPath);
                var xNodeToChange = doc.Descendants("type").Where(x => (string)x.Attribute("name") == modnameInfo).Select(x => x.Descendants("nominal")).SingleOrDefault().First();
                xNodeToChange.Value = newDataMaximum[indexCounter].ToString();

                var xNodeToChange2 = doc.Descendants("type").Where(x => (string)x.Attribute("name") == modnameInfo).Select(x => x.Descendants("min")).SingleOrDefault().First();
                xNodeToChange2.Value = newDataMinimum[indexCounter].ToString();

                var xNodeToChange3 = doc.Descendants("type").Where(x => (string)x.Attribute("name") == modnameInfo).Select(x => x.Descendants("cost")).SingleOrDefault().First();
                xNodeToChange3.Value = newDataPriority[indexCounter].ToString();

                var xNodeToChange4 = doc.Descendants("type").Where(x => (string)x.Attribute("name") == modnameInfo).Select(x => x.Descendants("restock")).SingleOrDefault().First();
                xNodeToChange4.Value = newDataRestockTime[indexCounter].ToString();

                var xNodeToChange5 = doc.Descendants("type").Where(x => (string)x.Attribute("name") == modnameInfo).Select(x => x.Descendants("lifetime")).SingleOrDefault().First();
                xNodeToChange5.Value = newDataLifetime[indexCounter].ToString();

                var xNodeToChange6 = doc.Descendants("type").Where(x => (string)x.Attribute("name") == modnameInfo).Select(x => x.Descendants("quantmin")).SingleOrDefault().First();
                xNodeToChange6.Value = newDataQuantMin[indexCounter].ToString();

                var xNodeToChange7 = doc.Descendants("type").Where(x => (string)x.Attribute("name") == modnameInfo).Select(x => x.Descendants("quantmax")).SingleOrDefault().First();
                xNodeToChange7.Value = newDataQuantMax[indexCounter].ToString();

                indexCounter++;
                doc.Save(LootManager.Properties.Settings.Default.xmlPath);
            }        
            logClass.LogWriter("[TYPES.XML] Saving done!");
            MessageBox.Show("Saved " + indexCounter.ToString() + " lines!");
            this.Enabled = true;
            indexCounter = 0;
            newDataName.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            TraderChecker frm = new TraderChecker();
            frm.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://steamcommunity.com/id/xlicexrandom/");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCMVi8rg6W64AhjJkaNqQgaA");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://discord.gg/62bbKrN");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            logClass.LogWriter("[TYPES.XML] searching item...");
            if (textBox1.Text != "")
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    var currQty = row.Cells["Name"].Value;
                    if (currQty.ToString() == textBox1.Text)
                    {
                        row.Selected = true;
                    }
                }
            }
            else
            {
                logClass.LogWriter("[TYPES.XML] No modname...");
                MessageBox.Show("Please enter a modname!");
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            playerSpawnItems frm = new playerSpawnItems();
            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            spawnabletypes frm = new spawnabletypes();
            frm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("'Middle Mouse' to open settings menu\n\nHINT:\nThe loading takes a while cuz its a 15360x15360 map img");
            this.Hide();
            PlayerspawnCreater frm = new PlayerspawnCreater();
            frm.Show();
        }
    }


    public class Log
    {
        public void LogWriter(string text)
        {
            if (File.Exists(Application.StartupPath + "\\log.txt"))
            {
                using (StreamWriter sw = File.AppendText(Application.StartupPath + "\\log.txt"))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " : " + text);
                  
                }
              //  File.AppendText(Application.StartupPath + "\\log.txt",);
            }
            else
            {
                File.WriteAllText(Application.StartupPath + "\\log.txt", DateTime.Now.ToString() + " : " + text);
            }
        }
    }
}
