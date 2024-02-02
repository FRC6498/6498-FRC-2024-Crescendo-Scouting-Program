using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace _6498_FRC_2024_Crescendo_Scouting_Program
{
    public partial class Form1 : Form
    {
        public static string path = $"{Application.UserAppDataPath}\\2024Scouting.csv";
        public Form1()
        {
            InitializeComponent();

            plusAutoAmp.Click += HandleUpDown;
            plusAutoSpeaker.Click += HandleUpDown;
            plusTeleAmpNotesSpeaker.Click += HandleUpDown;
            plusTeleUnampSpeakerNotes.Click += HandleUpDown;

            minusAutoAmp.Click += HandleUpDown;
            minusAutoSpeaker.Click += HandleUpDown;
            minusTeleAmpNotesSpeaker.Click += HandleUpDown;
            minusTeleUnampSpeakerNotes.Click += HandleUpDown;

            //set inputs to default when the porgram is stanting
            Reset();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Takes the tag of the sending element and increments the value of the control with the same name up or down depending on the text of the sending element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleUpDown(object sender, EventArgs e)
        {
            TabPage selectedTab = tabControl1.SelectedTab;
            var controls = selectedTab.Controls; //have to use controls in the context of the tab we are on
            Button button = (Button)sender;
            if (button.Text == "-")
            {
                if (Convert.ToInt32(controls[button.Tag.ToString()].Text) > 0) controls[button.Tag.ToString()].Text = (Convert.ToInt32(controls[button.Tag.ToString()].Text) - 1).ToString();
            }
            else
            {
                controls[button.Tag.ToString()].Text = (Convert.ToInt32(controls[button.Tag.ToString()].Text) + 1).ToString();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hashtable hashtable = new Hashtable();
            foreach (TabControl tab in tabControl1.TabPages)
            {
                foreach (Control control in (ControlCollection)tab.Controls)
                {
                    switch (control)
                    {
                        case TextBox txtBox:
                            hashtable.Add(txtBox.Name, txtBox.Text);
                            break;
                        case ComboBox comboBox:
                            hashtable.Add(comboBox.Name, comboBox.SelectedText);
                            break;
                        case CheckBox checkBox:
                            hashtable.Add(checkBox.Name, Convert.ToString(checkBox.Checked));
                            break;
                    }
                }
            }
            SaveData(hashtable);
        }
        /// <summary>
        /// Resets all inputs back to default
        /// </summary>
        private void Reset()
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                foreach (Control control in tab.Controls)
                {
                    switch (control)
                    {
                        case TextBox txtBox:
                            txtBox.Clear();
                            txtBox.Text = "0";
                            break;
                        case ComboBox comboBox:
                            comboBox.SelectedIndex = 0;
                            break;
                        case CheckBox checkBox:
                            checkBox.Checked = false;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Saves Program data to the user's appdata directory
        /// </summary>
        /// <param name="data"></param>
        private void SaveData(Hashtable data)
        {
            StringBuilder builder = new StringBuilder();
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(path)))
            {
                //file could contain up to 6 bytes as a file marker
                if (new FileInfo(path).Length < 6)
                {
                    //write header
                    foreach (object key in data.Keys)
                    {
                        builder.Append($"{key},");
                    }
                    writer.WriteLine(builder.ToString());
                    writer.Flush();
                }
                foreach (object key in data.Keys)
                {
                    builder.AppendLine($"{key},{data[key]},");
                }

            }
            data = new Hashtable();
        }
        /// <summary>
        /// exports data saved to the appdata directory to a path of the user's choice
        /// </summary>
        /// <param name="savePath"></param>
        private void ExportData(string savePath)
        {
            if (new FileInfo(path).Length > 6)
            {
                using (StreamWriter writer = new StreamWriter(savePath))
                {
                    string currentData;
                    using (StreamReader reader = new StreamReader(path))
                    {
                        currentData = reader.ReadToEnd();
                    }
                    writer.Write(currentData);
                };
            }
            else
            {
                MessageBox.Show("No data saved");
            }
        }
    }
}
