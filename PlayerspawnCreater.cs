using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace LootManager
{
    public partial class PlayerspawnCreater : Form
    {
        mapSettings ms = new mapSettings();

        public PlayerspawnCreater()
        {
            InitializeComponent();
            this.Shown += Form1_Shown;      
            pictureBox1.MouseMove += hover;
            this.FormClosing += CloseForm;
            //this.SizeChanged += FormSizeChanged;

        }

        //private void FormSizeChanged (object sender, EventArgs e)
        //{
        //    int Width = this.Size.Width;
        //    int Height = this.Size.Height;
        //}

        private void CloseForm(object sender, EventArgs e)
        {
            this.Dispose();      
            Form1 frm = new Form1();
            frm.Show();
        }

        private void SetFont(Form f, string name, int size, FontStyle style)
        {
            Font replacementFont = new Font(name, size, style);
            f.Font = replacementFont;
        }


        Label display = new Label();


        private void hover(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            int newValue = 15360 - Convert.ToInt32(coordinates.Y);
            display.Text = " X : " + coordinates.X.ToString() + " / Y: " + newValue.ToString();
            display.BackColor = System.Drawing.Color.Transparent;
            display.ForeColor = Color.White;
            display.Font = new Font("Arial", 13, FontStyle.Bold);
            display.Location = new Point(coordinates.X, coordinates.Y + 100);           
            pictureBox1.Controls.Add(display);
            display.BringToFront();

        }

        Image ZoomPicture(Image img, Size size)
        {
            Bitmap bm = new Bitmap(img, Convert.ToInt32(img.Width * size.Width), Convert.ToInt32(img.Height * size.Height));
            Graphics gpu = Graphics.FromImage(bm);
            gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bm;
        }

        private void Form1_Shown(Object sender, EventArgs e)
        {
            this.DoubleBuffered = false;          
            if (LootManager.Properties.Settings.Default.chernarusMapDownloaded == false)
            {
                this.Hide();
                mapDownloader frm = new mapDownloader();
                frm.Show();
            }
            else
            {
                if (File.Exists(Application.StartupPath + "\\ChernarusHighQuality.jpg"))
                {
                    pictureBox1.ImageLocation = Application.StartupPath + "\\ChernarusHighQuality.jpg";
                }
                else
                {
                    this.Hide();
                    mapDownloader frm = new mapDownloader();
                    frm.Show();
                }
            }
        }

        int newValue;

        public void vehiclePositionReader()
        {
            using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.cfgeventspawnsPath))
            {

                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name.ToString() == "eventposdef")
                            {                             
                                XElement el = (XElement)XNode.ReadFrom(reader);
                                using (var reader2 = el.CreateReader())
                                {
                                    while (reader2.Read())
                                    {
                                        if (reader2.Name.ToString() == "event")
                                        {                                          
                                            string vehicle = reader2.GetAttribute("name").ToString();
                                        
                                            XElement el2 = (XElement)XNode.ReadFrom(reader2);
                                            using (var reader3 = el2.CreateReader())
                                            {
                                                while (reader3.Read())
                                                {                                                   
                                                    if (reader3.Name.ToString() == "pos")
                                                    {                                              
                                                        string cleanXCoordinate = String.Empty;
                                                        string cleanYCoordinate = String.Empty;
                                                        /////////////////////////////////////////////////////
                                                        if (reader3.GetAttribute("x").Contains("."))
                                                        {
                                                            cleanXCoordinate = reader3.GetAttribute("x").Substring(0, reader3.GetAttribute("x").IndexOf('.'));
                                                        }
                                                        if (reader3.GetAttribute("z").Contains("."))
                                                        {
                                                            cleanYCoordinate = reader3.GetAttribute("z").Substring(0, reader3.GetAttribute("z").IndexOf('.'));
                                                            newValue = 15360 - Convert.ToInt32(cleanYCoordinate);
                                                        }

                                                        ////////////////////////////////////////////////////
                                                        if (!reader3.GetAttribute("x").Contains("."))
                                                        {
                                                            cleanXCoordinate = reader3.GetAttribute("x").ToString();
                                                        }
                                                        if (!reader3.GetAttribute("z").Contains("."))
                                                        {
                                                            cleanYCoordinate = reader3.GetAttribute("z").ToString();
                                                            newValue = 15360 - Convert.ToInt32(cleanYCoordinate);
                                                        }


                                                        if (vehicle == "VehicleCivilianSedan")
                                                        {
                                                            PictureBox marker = new PictureBox();
                                                            marker.Image = LootManager.Properties.Resources.civiliansedan;   // -27    -50
                                                            marker.Location = new Point(Convert.ToInt32(cleanXCoordinate), newValue - 14);
                                                            marker.BackColor = System.Drawing.Color.Transparent;
                                                            marker.Size = new Size(50, 62);
                                                            marker.BringToFront();
                                                            marker.MouseClick += ClickedMarker;                                                           
                                                            pictureBox1.Controls.Add(marker);

                                                            Label vehiclename = new Label();
                                                            vehiclename.BackColor = System.Drawing.Color.Transparent;
                                                            vehiclename.ForeColor = Color.Red;
                                                            vehiclename.Font = new Font("Arial", 9, FontStyle.Bold);
                                                            vehiclename.Text = "Olga24";
                                                            vehiclename.Location = new Point(-3, 40);
                                                            vehiclename.Size = new Size(130, 13);
                                                            marker.Controls.Add(vehiclename);

                                                        }     
                                                        else if (vehicle == "VehicleSedan02")
                                                        {
                                                            PictureBox marker = new PictureBox();
                                                            marker.Image = LootManager.Properties.Resources.civiliansedan;   // -27    -50
                                                            marker.Location = new Point(Convert.ToInt32(cleanXCoordinate), newValue - 14);
                                                            marker.BackColor = System.Drawing.Color.Transparent;
                                                            marker.Size = new Size(50, 62);
                                                            marker.BringToFront();
                                                            marker.MouseClick += ClickedMarker;
                                                            pictureBox1.Controls.Add(marker);

                                                            Label vehiclename = new Label();
                                                            vehiclename.BackColor = System.Drawing.Color.Transparent;
                                                            vehiclename.ForeColor = Color.Red;
                                                            vehiclename.Font = new Font("Arial", 9, FontStyle.Bold);
                                                            vehiclename.Text = "Sedan";
                                                            vehiclename.Location = new Point(-3, 40);
                                                            vehiclename.Size = new Size(130, 13);
                                                            marker.Controls.Add(vehiclename);
                                                        }
                                                        else if ( vehicle == "VehicleHatchback02")
                                                        {
                                                            PictureBox marker = new PictureBox();
                                                            marker.Image = LootManager.Properties.Resources.hatchback;   // -27    -50
                                                            marker.Location = new Point(Convert.ToInt32(cleanXCoordinate), newValue - 14);
                                                            marker.BackColor = System.Drawing.Color.Transparent;
                                                            marker.Size = new Size(50, 62);
                                                            marker.BringToFront();
                                                            marker.MouseClick += ClickedMarker;
                                                            pictureBox1.Controls.Add(marker);

                                                            Label vehiclename = new Label();
                                                            vehiclename.BackColor = System.Drawing.Color.Transparent;
                                                            vehiclename.ForeColor = Color.Red;
                                                            vehiclename.Font = new Font("Arial", 9, FontStyle.Bold);
                                                            vehiclename.Text = "Gunter";
                                                            vehiclename.Location = new Point(-3, 40);
                                                            vehiclename.Size = new Size(130, 13);
                                                            marker.Controls.Add(vehiclename);
                                                        }
                                                        else if (vehicle == "VehicleOffroadHatchback")
                                                        {
                                                            PictureBox marker = new PictureBox();
                                                            marker.Image = LootManager.Properties.Resources.hatchback;   // -27    -50
                                                            marker.Location = new Point(Convert.ToInt32(cleanXCoordinate), newValue - 14);
                                                            marker.BackColor = System.Drawing.Color.Transparent;
                                                            marker.Size = new Size(50, 62);
                                                            marker.BringToFront();
                                                            marker.MouseClick += ClickedMarker;
                                                            pictureBox1.Controls.Add(marker);

                                                            Label vehiclename = new Label();
                                                            vehiclename.BackColor = System.Drawing.Color.Transparent;
                                                            vehiclename.ForeColor = Color.Red;
                                                            vehiclename.Font = new Font("Arial", 9, FontStyle.Bold);
                                                            vehiclename.Text = "Ada 4x4";
                                                            vehiclename.Location = new Point(-3, 40);
                                                            vehiclename.Size = new Size(130, 13);
                                                            marker.Controls.Add(vehiclename);
                                                        }                                                     
                                                    }
                                                }
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
        }


        int newValuePlayer;


        public void defaultPositionReader()
        {              
            using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.cfgplayerspawnpointsPath))
            {
             
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            
                            if (reader.Name.ToString() == "generator_posbubbles")
                            {
                              
                                XElement el = (XElement)XNode.ReadFrom(reader);
                                using (var reader2 = el.CreateReader())
                                {
                                    while (reader2.Read())
                                    {
                                       // MessageBox.Show(reader2.Name.ToString());
                                        if (reader2.Name.ToString() == "pos")
                                        {
                                          
                                            string cleanXCoordinate = String.Empty;
                                            string cleanYCoordinate = String.Empty;
                                            /////////////////////////////////////////////////////
                                            if (reader2.GetAttribute("x").Contains("."))
                                            {
                                                cleanXCoordinate = reader2.GetAttribute("x").Substring(0, reader2.GetAttribute("x").IndexOf('.'));
                                                
                                            }
                                            if (reader2.GetAttribute("z").Contains("."))
                                            {
                                                cleanYCoordinate = reader2.GetAttribute("z").Substring(0, reader2.GetAttribute("z").IndexOf('.'));
                                                newValuePlayer = 15360 - Convert.ToInt32(cleanYCoordinate);
                                            }

                                            ////////////////////////////////////////////////////
                                            if (!reader2.GetAttribute("x").Contains("."))
                                            {
                                                cleanXCoordinate = reader2.GetAttribute("x").ToString();
                                            }
                                            if (!reader2.GetAttribute("z").Contains("."))
                                            {
                                                cleanYCoordinate = reader2.GetAttribute("z").ToString();
                                                newValuePlayer = 15360 - Convert.ToInt32(cleanYCoordinate);
                                            }

                                         

                                            PictureBox marker = new PictureBox();
                                            marker.Image = LootManager.Properties.Resources.marker; 
                                            marker.Location = new Point(Convert.ToInt32(cleanXCoordinate) - 27, newValuePlayer - 50);
                                            marker.BackColor = System.Drawing.Color.Transparent;
                                            marker.Size = new Size(50, 62);
                                            marker.BringToFront();
                                            marker.MouseClick += ClickedMarker;
                                            pictureBox1.Controls.Add(marker);

                                            Label playerspawn = new Label();
                                            playerspawn.BackColor = System.Drawing.Color.Transparent;
                                            playerspawn.ForeColor = Color.Red;
                                            playerspawn.Font = new Font("Arial", 9, FontStyle.Bold);
                                            playerspawn.Text = "Spawn";
                                            playerspawn.Location = new Point(-3, 40);
                                            playerspawn.Size = new Size(130, 13);
                                            marker.Controls.Add(playerspawn);                                         
                                        }                                      
                                    }
                                    break;
                                }
                            }
                            break;
                    }
                }         
            }
        }


        private void ClickedMarker(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = (PictureBox)sender;
            pictureBox1.Controls.Remove(clickedPictureBox);
        }


        public int nummer = 0;
        private void pictureBox1_Click(object sender, EventArgs e)
        {           
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                Point coordinates = me.Location;
                int newValue = 15360 - Convert.ToInt32(coordinates.Y);  //immer hiermit rechnen sonst falsch!!!
                PictureBox marker = new PictureBox();
                nummer++;

                if (LootManager.Properties.Settings.Default.currentMarker == "hatchback")
                {
                    marker.Image = LootManager.Properties.Resources.hatchback;
                    string vline = LootManager.Properties.Settings.Default.vehicleString;
                    vline = vline.Replace("#X", coordinates.X.ToString());
                    vline = vline.Replace("#Y", newValue.ToString());


                    LootManager.Properties.Settings.Default.richtxtboxString = LootManager.Properties.Settings.Default.richtxtboxString + "\n" + vline;
                    LootManager.Properties.Settings.Default.Save();

                    //ms.richTextBox1.Text = ms.richTextBox1.Text + "\n" + vline;
                    //Clipboard.SetText(vline);
                }
                else if (LootManager.Properties.Settings.Default.currentMarker == "marker")
                {
                    marker.Image = LootManager.Properties.Resources.marker;               
                    string vline = LootManager.Properties.Settings.Default.spawnString;
                    vline = vline.Replace("#X", coordinates.X.ToString());
                    vline = vline.Replace("#Y", newValue.ToString());
                    LootManager.Properties.Settings.Default.richtxtboxString = LootManager.Properties.Settings.Default.richtxtboxString + "\n" + vline;
                    LootManager.Properties.Settings.Default.Save();
                    // ms.richTextBox1.Text = ms.richTextBox1.Text + "\n" + vline;
                    // Clipboard.SetText(vline);
                }
                else if (LootManager.Properties.Settings.Default.currentMarker == "civiliansedan")
                {
                    marker.Image = LootManager.Properties.Resources.civiliansedan;               
                    string vline = LootManager.Properties.Settings.Default.vehicleString;
                    vline = vline.Replace("#X", coordinates.X.ToString());
                    vline = vline.Replace("#Y", newValue.ToString());
                    LootManager.Properties.Settings.Default.richtxtboxString = LootManager.Properties.Settings.Default.richtxtboxString + "\n" + vline;
                    LootManager.Properties.Settings.Default.Save();
                    //  ms.richTextBox1.Text = ms.richTextBox1.Text + "\n" + vline;
                    //   Clipboard.SetText(vline);
                }
                else if (LootManager.Properties.Settings.Default.currentMarker == "moddedCar")
                {
                    marker.Image = LootManager.Properties.Resources.moddedCar;
                    string vline = LootManager.Properties.Settings.Default.vehicleString;
                    vline = vline.Replace("#X", coordinates.X.ToString());
                    vline = vline.Replace("#Y", newValue.ToString());
                    LootManager.Properties.Settings.Default.richtxtboxString = LootManager.Properties.Settings.Default.richtxtboxString + "\n" + vline;
                    LootManager.Properties.Settings.Default.Save();
                    // ms.richTextBox1.Text = ms.richTextBox1.Text + "\n" + vline;
                    // Clipboard.SetText(vline);
                }
                else if (LootManager.Properties.Settings.Default.currentMarker == "policecar")
                {
                    marker.Image = LootManager.Properties.Resources.policecar;
                    string vline = LootManager.Properties.Settings.Default.vehicleString;
                    vline = vline.Replace("#X", coordinates.X.ToString());
                    vline = vline.Replace("#Y", newValue.ToString());
                    LootManager.Properties.Settings.Default.richtxtboxString = LootManager.Properties.Settings.Default.richtxtboxString + "\n" + vline;
                    LootManager.Properties.Settings.Default.Save();
                }
                else if (LootManager.Properties.Settings.Default.currentMarker == "planks")
                {
                    marker.Image = LootManager.Properties.Resources.planks;
                    string vline = LootManager.Properties.Settings.Default.vehicleString;
                    vline = vline.Replace("#X", coordinates.X.ToString());
                    vline = vline.Replace("#Y", newValue.ToString());
                    LootManager.Properties.Settings.Default.richtxtboxString = LootManager.Properties.Settings.Default.richtxtboxString + "\n" + vline;
                    LootManager.Properties.Settings.Default.Save();
                }
                else
                {
                    marker.Image = LootManager.Properties.Resources.marker;
                }

                marker.Location = new Point(Convert.ToInt32(coordinates.X) - 27, Convert.ToInt32(coordinates.Y) - 50);
                marker.BackColor = System.Drawing.Color.Transparent;
                marker.Size = new Size(52, 65); //y bissle größer
                marker.BringToFront();
                marker.MouseClick += ClickedMarker;

                //Label markerName = new Label();
                //markerName.Text = "Marker " + nummer;
                //markerName.BackColor = System.Drawing.Color.Transparent;
                //markerName.ForeColor = Color.Red;
                //markerName.Location = new Point(0, 50);
                //markerName.Size = new Size(61, 13);
                //marker.Controls.Add(markerName);
                 pictureBox1.Controls.Add(marker);               
              //  MessageBox.Show("Saved to clipboard : " + Clipboard.GetText());
            }
            else if (me.Button == MouseButtons.Right)
            {
                Point coordinates = me.Location;
                int Width = Size.Width;  //x
                int Height = Size.Height;  //y

                int newValueX = coordinates.X - Width / 2;
                int newValueY = coordinates.Y - Height / 2;

                if (newValueY < 0)
                {
                    panel1.VerticalScroll.Value = 0;
                }
                else if (newValueX < 0)
                {
                    panel1.HorizontalScroll.Value = 0;
                }
                else
                {
                    panel1.HorizontalScroll.Value = newValueX;
                    panel1.VerticalScroll.Value = newValueY;
                }             
            }
            else if (me.Button == MouseButtons.Middle)
            {
                mapSettings frm = new mapSettings();
                frm.Show();
            }
        }    

        private void button1_Click(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.cfgeventspawnsPath == "")
            {
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open cfgeventspawns.xml File";
                theDialog.Filter = "xml files|*.xml";
                theDialog.InitialDirectory = @"C:\";
                if (theDialog.ShowDialog() == DialogResult.OK)
                {

                    LootManager.Properties.Settings.Default.cfgeventspawnsPath = theDialog.FileName.ToString();
                    LootManager.Properties.Settings.Default.Save();
                }
                else
                {
                    theDialog.Dispose();
                    return;
                }
            }
            else
            {
                if (File.Exists(LootManager.Properties.Settings.Default.cfgeventspawnsPath))
                {
                    vehiclePositionReader();
                    MessageBox.Show("Loaded!");
                }
                else
                {
                    OpenFileDialog theDialog = new OpenFileDialog();
                    theDialog.Title = "Open cfgeventspawns.xml File";
                    theDialog.Filter = "xml files|*.xml";
                    theDialog.InitialDirectory = @"C:\";
                    if (theDialog.ShowDialog() == DialogResult.OK)
                    {
                        LootManager.Properties.Settings.Default.cfgeventspawnsPath = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.Save();
                    }
                    else
                    {
                        theDialog.Dispose();
                        return;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.cfgplayerspawnpointsPath == "")
            {
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open cfgplayerspawnpoints.xml File";
                theDialog.Filter = "xml files|*.xml";
                theDialog.InitialDirectory = @"C:\";
                if (theDialog.ShowDialog() == DialogResult.OK)
                {

                    LootManager.Properties.Settings.Default.cfgplayerspawnpointsPath = theDialog.FileName.ToString();
                    LootManager.Properties.Settings.Default.Save();
                }
                else
                {
                    theDialog.Dispose();
                    return;
                }
            }
            else
            {
                if (File.Exists(LootManager.Properties.Settings.Default.cfgplayerspawnpointsPath))
                {
                    defaultPositionReader();
                    MessageBox.Show("Loaded!");
                }
                else
                {
                    OpenFileDialog theDialog = new OpenFileDialog();
                    theDialog.Title = "Open cfgeventspawns.xml File";
                    theDialog.Filter = "xml files|*.xml";
                    theDialog.InitialDirectory = @"C:\";
                    if (theDialog.ShowDialog() == DialogResult.OK)
                    {
                        LootManager.Properties.Settings.Default.cfgplayerspawnpointsPath = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.Save();
                    }
                    else
                    {
                        theDialog.Dispose();
                        return;
                    }
                }
            }
        }

        private void PlayerspawnCreater_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.cfgeventspawnsPath == "")
            {
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open cfgplayerspawnpoints.xml File";
                theDialog.Filter = "xml files|*.xml";
                theDialog.InitialDirectory = @"C:\";
                if (theDialog.ShowDialog() == DialogResult.OK)
                {

                    LootManager.Properties.Settings.Default.cfgeventspawnsPath = theDialog.FileName.ToString();
                    LootManager.Properties.Settings.Default.Save();
                }
                else
                {
                    theDialog.Dispose();
                    return;
                }
            }
            else
            {
                if (File.Exists(LootManager.Properties.Settings.Default.cfgeventspawnsPath))
                {
                    readPlankSpawns();
                    MessageBox.Show("Loaded!");
                }
                else
                {
                    OpenFileDialog theDialog = new OpenFileDialog();
                    theDialog.Title = "Open cfgeventspawns.xml File";
                    theDialog.Filter = "xml files|*.xml";
                    theDialog.InitialDirectory = @"C:\";
                    if (theDialog.ShowDialog() == DialogResult.OK)
                    {
                        LootManager.Properties.Settings.Default.cfgeventspawnsPath = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.Save();
                    }
                    else
                    {
                        theDialog.Dispose();
                        return;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (LootManager.Properties.Settings.Default.cfgeventspawnsPath == "")
            {
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open cfgplayerspawnpoints.xml File";
                theDialog.Filter = "xml files|*.xml";
                theDialog.InitialDirectory = @"C:\";
                if (theDialog.ShowDialog() == DialogResult.OK)
                {

                    LootManager.Properties.Settings.Default.cfgeventspawnsPath = theDialog.FileName.ToString();
                    LootManager.Properties.Settings.Default.Save();
                }
                else
                {
                    theDialog.Dispose();
                    return;
                }
            }
            else
            {
                if (File.Exists(LootManager.Properties.Settings.Default.cfgeventspawnsPath))
                {
                    readPoliceCarSpawns();
                    MessageBox.Show("Loaded!");
                }
                else
                {
                    OpenFileDialog theDialog = new OpenFileDialog();
                    theDialog.Title = "Open cfgeventspawns.xml File";
                    theDialog.Filter = "xml files|*.xml";
                    theDialog.InitialDirectory = @"C:\";
                    if (theDialog.ShowDialog() == DialogResult.OK)
                    {
                        LootManager.Properties.Settings.Default.cfgeventspawnsPath = theDialog.FileName.ToString();
                        LootManager.Properties.Settings.Default.Save();
                    }
                    else
                    {
                        theDialog.Dispose();
                        return;
                    }
                }
            }
        }

        private void readPlankSpawns()
        {
            using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.cfgeventspawnsPath))
            {

                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name.ToString() == "eventposdef")
                            {
                                XElement el = (XElement)XNode.ReadFrom(reader);
                                using (var reader2 = el.CreateReader())
                                {
                                    while (reader2.Read())
                                    {
                                        if (reader2.Name.ToString() == "event")
                                        {
                                            string name = reader2.GetAttribute("name").ToString();

                                            XElement el2 = (XElement)XNode.ReadFrom(reader2);
                                            using (var reader3 = el2.CreateReader())
                                            {
                                                while (reader3.Read())
                                                {
                                                    if (reader3.Name.ToString() == "pos")
                                                    {
                                                        string cleanXCoordinate = String.Empty;
                                                        string cleanYCoordinate = String.Empty;
                                                        /////////////////////////////////////////////////////
                                                        if (reader3.GetAttribute("x").Contains("."))
                                                        {
                                                            cleanXCoordinate = reader3.GetAttribute("x").Substring(0, reader3.GetAttribute("x").IndexOf('.'));
                                                        }
                                                        if (reader3.GetAttribute("z").Contains("."))
                                                        {
                                                            cleanYCoordinate = reader3.GetAttribute("z").Substring(0, reader3.GetAttribute("z").IndexOf('.'));
                                                            newValue = 15360 - Convert.ToInt32(cleanYCoordinate);
                                                        }

                                                        ////////////////////////////////////////////////////
                                                        if (!reader3.GetAttribute("x").Contains("."))
                                                        {
                                                            cleanXCoordinate = reader3.GetAttribute("x").ToString();
                                                        }
                                                        if (!reader3.GetAttribute("z").Contains("."))
                                                        {
                                                            cleanYCoordinate = reader3.GetAttribute("z").ToString();
                                                            newValue = 15360 - Convert.ToInt32(cleanYCoordinate);
                                                        }


                                                        if (name == "ItemPlanks")
                                                        {
                                                            PictureBox marker = new PictureBox();
                                                            marker.Image = LootManager.Properties.Resources.planks;
                                                            marker.Location = new Point(Convert.ToInt32(cleanXCoordinate), newValue - 14);
                                                            marker.BackColor = System.Drawing.Color.Transparent;
                                                            marker.Size = new Size(50, 62);
                                                            marker.BringToFront();
                                                            marker.MouseClick += ClickedMarker;
                                                            pictureBox1.Controls.Add(marker);

                                                            Label itemName = new Label();
                                                            itemName.BackColor = System.Drawing.Color.Transparent;
                                                            itemName.ForeColor = Color.Red;
                                                            itemName.Font = new Font("Arial", 9, FontStyle.Bold);
                                                            itemName.Text = "Planks";
                                                            itemName.Location = new Point(-3, 40);
                                                            itemName.Size = new Size(130, 13);
                                                            marker.Controls.Add(itemName);
                                                        }                                                     
                                                    }
                                                }
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
        }

        private void readPoliceCarSpawns()
        {
            using (XmlReader reader = XmlReader.Create(LootManager.Properties.Settings.Default.cfgeventspawnsPath))
            {

                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name.ToString() == "eventposdef")
                            {
                                XElement el = (XElement)XNode.ReadFrom(reader);
                                using (var reader2 = el.CreateReader())
                                {
                                    while (reader2.Read())
                                    {
                                        if (reader2.Name.ToString() == "event")
                                        {
                                            string name = reader2.GetAttribute("name").ToString();

                                            XElement el2 = (XElement)XNode.ReadFrom(reader2);
                                            using (var reader3 = el2.CreateReader())
                                            {
                                                while (reader3.Read())
                                                {
                                                    if (reader3.Name.ToString() == "pos")
                                                    {
                                                        string cleanXCoordinate = String.Empty;
                                                        string cleanYCoordinate = String.Empty;
                                                        /////////////////////////////////////////////////////
                                                        if (reader3.GetAttribute("x").Contains("."))
                                                        {
                                                            cleanXCoordinate = reader3.GetAttribute("x").Substring(0, reader3.GetAttribute("x").IndexOf('.'));
                                                        }
                                                        if (reader3.GetAttribute("z").Contains("."))
                                                        {
                                                            cleanYCoordinate = reader3.GetAttribute("z").Substring(0, reader3.GetAttribute("z").IndexOf('.'));
                                                            newValue = 15360 - Convert.ToInt32(cleanYCoordinate);
                                                        }

                                                        ////////////////////////////////////////////////////
                                                        if (!reader3.GetAttribute("x").Contains("."))
                                                        {
                                                            cleanXCoordinate = reader3.GetAttribute("x").ToString();
                                                        }
                                                        if (!reader3.GetAttribute("z").Contains("."))
                                                        {
                                                            cleanYCoordinate = reader3.GetAttribute("z").ToString();
                                                            newValue = 15360 - Convert.ToInt32(cleanYCoordinate);
                                                        }

                                                        if (name == "StaticPoliceCar")
                                                        {
                                                            PictureBox marker = new PictureBox();
                                                            marker.Image = LootManager.Properties.Resources.policecar;
                                                            marker.Location = new Point(Convert.ToInt32(cleanXCoordinate), newValue - 14);
                                                            marker.BackColor = System.Drawing.Color.Transparent;
                                                            marker.Size = new Size(50, 62);
                                                            marker.BringToFront();
                                                            marker.MouseClick += ClickedMarker;
                                                            pictureBox1.Controls.Add(marker);

                                                            Label itemName = new Label();
                                                            itemName.BackColor = System.Drawing.Color.Transparent;
                                                            itemName.ForeColor = Color.Red;
                                                            itemName.Font = new Font("Arial", 9, FontStyle.Bold);
                                                            itemName.Text = "Police Car";
                                                            itemName.Location = new Point(-3, 40);
                                                            itemName.Size = new Size(130, 13);
                                                            marker.Controls.Add(itemName);
                                                        }

                                                    }
                                                }
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
        }
    }
}
