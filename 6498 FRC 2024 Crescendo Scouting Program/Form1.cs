using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6498_FRC_2024_Crescendo_Scouting_Program
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        // this will be checked to make sure that the file exists and give a warning if not. Future mistake-proofing the program in the case of an error.
        string filePath = "";

        // This dictionary serves a dual purpose.
        // 1. It is the basis of the header of each file. (See save file dialog FILE OK subprogram or the btnNew click event)
        // 2. It is the basis of the backup database if for some reason the file is corrupted or accidentally closed without saving. Failsafe at worst.

        Dictionary<string, List<string>> data = new Dictionary<string, List<string>>{

            // identity and match information
            { "Scouter Name", new List<string>() },
            { "Team Number", new List<string>() },
            { "Position", new List<string>()},
            { "Match Number", new List<string>() },
            // -- end of section --

            // autonomous section of data collection
            { "Robot Mobility Achieved", new List<string>() },

            { "Auto Amp Notes Scored", new List<string>() },
            { "Auto Amp Notes Missed", new List<string>() },
            { "Auto Amp Accuracy Percentage", new List<string>() },

            { "Auto Speaker Notes Scored", new List<string>() },
            { "Auto Speaker Notes Missed", new List<string>() },
            { "Auto Speaker Accuracy Percentage", new List<string>() },
            // -- end of section --

            // teleoperated section of data collection
            { "Teleop Amp Notes Scored", new List<string>() },
            { "Teleop Amp Notes Missed", new List<string>() },
            { "Teleop Amp Accuracy Percentage", new List<string>() },

            { "Teleop Speaker Unamplified Notes Scored (~)", new List<string>() },
            { "Teleop Speaker Amplified Notes Scored (+)", new List<string>() },
            { "Teleop Speaker Notes Missed", new List<string>() },
            { "Percentage of Unamplified Shots Scored in Teleop Speaker", new List<string>() },
            { "Percentage of Amplified Shots Scored in Teleop Speaker ",new List<string>() },
            { "Percentage of Shots Missed for Teleop Speaker", new List<string>() },
            // -- end of section --

            // stage section of data collection
            { "Stage Park or Climb", new List<string>() },
            { "Stage Scored in Trap", new List < string >() },
            // -- end of section --

            // Notes formatted for literal string interpretation. see submit button for examples
            // Important Data the user will input if it is present but not able to be submitted on the form.
            // it has to be quality assured though. That is vital due to the 
            { "Other Notes", new List<string>() },
        };

        #region Variables
        //autonomous phase variables
        int autoAmpScored, autoAmpMissed = 0;
        int autoSpeakerScored, autoSpeakerMissed = 0;

        //teleoperated phase varaibles
        int teleAmpSpeakerScored, teleUnampSpeakerScored, teleSpeakerMissed = 0;
        int teleAmpScored, teleAmpMissed = 0;
        #endregion

        #region Minor Form Functions
        private void btnCheckboxes_click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.BackColor == Color.Green)
            {
                btn.BackColor = Color.Red;
                btn.Text = "X";
                btn.Tag = "False";
            }
            else
            {
                btn.BackColor = Color.Green;
                btn.Text = "✔";
                btn.Tag = "True";
            }

            this.ActiveControl = null;
        }

        private void btnNotes_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Tag.ToString() == "hidden")
            {
                btn.Text = "Hide Notes";
                this.Height = 718;
                btn.Tag = "shown";
            }
            else
            {
                btn.Text = "Show Notes";
                this.Height = 565;
                btn.Tag = "hidden";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Top -= 75;
            cmbInfoPosition.SelectedIndex = 0;
            cmbStageParkClimb.SelectedIndex = 0;
            this.ActiveControl = txtInfoScouterName;
        }

        private void txtNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == "\r" || e.KeyChar.ToString() =="\n" || e.KeyChar.ToString() == "\r\n")
            {
                e.Handled = true;
            }
        }

        private void txtInfoMatchNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region File Management Buttons
        private void btnNew_Click(object sender, EventArgs e)
        {
            sfdSave.Filter = "Comma Seperated Values (*.csv)|*.csv";
            if (sfdSave.ShowDialog() == DialogResult.OK)
            {
                filePath = sfdSave.FileName;
                string header = "";
                foreach (KeyValuePair<string, List<string>> x in data)
                {
                    header += (x.Key + ",");
                }
                StreamWriter writer = new StreamWriter(filePath);

                writer.WriteLine(header);
                writer.Close();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ofdOpen.Filter = "Comma Seperated Values (*.csv)|*.csv";
            if (ofdOpen.ShowDialog() == DialogResult.OK)
            {
                filePath = ofdOpen.FileName;
            }
        }

        private void txtInfoScouterName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnShowFolder_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Value Modification
        private void addedButtonClicked(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            string tag = clicked.Tag.ToString();


            AddOrDelete(clicked);
            this.ActiveControl = null;
        }

        private void AddOrDelete(Button sender)
        {
            if (sender.Name.Contains("Add"))
            {
                ModifyValues(1, sender.Name);
            }
            else if (sender.Name.Contains("Delete"))
            {
                ModifyValues(-1, sender.Name);
            }
        }

        private void ModifyValues(int x, string itemToChange)
        {
            if (itemToChange.ToLower().Contains("autoampscore"))
            {
                if ((autoAmpScored + x) >= 0)
                {
                    autoAmpScored += x;
                }

            }

            else if (itemToChange.ToLower().Contains("autoampmiss"))
            {
                if ((autoAmpMissed + x) >= 0)
                {
                    autoAmpMissed += x;
                }
            }

            else if (itemToChange.ToLower().Contains("autospeakerscore"))
            {
                if ((autoSpeakerScored + x) >= 0)
                {
                    autoSpeakerScored += x;
                }

            }

            else if (itemToChange.ToLower().Contains("autospeakermiss"))
            {
                if ((autoSpeakerMissed + x) >= 0)
                {
                    autoSpeakerMissed += x;
                }
            }

            else if (itemToChange.ToLower().Contains("teleunampspeaker"))
            {
                if ((teleUnampSpeakerScored + x) >= 0)
                {
                    teleUnampSpeakerScored += x;
                }
            }

            else if (itemToChange.ToLower().Contains("teleampspeaker"))
            {
                if ((teleAmpSpeakerScored + x) >= 0)
                {
                    teleAmpSpeakerScored += x;
                }
            }

            else if (itemToChange.ToLower().Contains("telespeakermiss"))
            {
                if ((teleSpeakerMissed + x) >= 0)
                {
                    teleSpeakerMissed += x;
                }
            }

            else if (itemToChange.ToLower().Contains("teleampscore"))
            {
                if ((teleAmpScored + x) >= 0)
                {
                    teleAmpScored += x;
                }
            }

            else if (itemToChange.ToLower().Contains("teleampmiss"))
            {
                if ((teleAmpMissed + x) >= 0)
                {
                    teleAmpMissed += x;
                }
            }

            lblAutoAmp.Text = "Scored: " + autoAmpScored + "\nMisses: " + autoAmpMissed;
            lblAutoSpeaker.Text = "Scored: " + autoSpeakerScored + "\nMisses: " + autoSpeakerMissed;
            lblTeleSpeaker.Text = "Scored (Unamp.): " + teleUnampSpeakerScored + "\nScored (Amp.): " + teleAmpSpeakerScored + "\nMisses: " + teleSpeakerMissed;
            lblTeleAmp.Text = "Scored: " + teleAmpScored + "\nMisses: " + teleAmpMissed;
        }

        #endregion

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (filePath != "" && txtInfoMatchNumber.Text.Length > 0 && txtInfoTeam.Text.Length > 0)
            {

            #region Notes Formatting
            // notes output should be correct for CSV formatting, with literal interpretation
            string notes = txtNotes.Text.Replace("\"", @"""""");
            notes = "\"" + notes + "\"";
            // entries will be surrounded on each side with " and every quotation mark will be replaced with 2 quotation marks for preventing seperation of values
            // fixes comma issue, in a CSV file. Stack overflow explains this pretty well
            // https://stackoverflow.com/questions/4617935/is-there-a-way-to-include-commas-in-csv-columns-without-breaking-the-formatting
            #endregion
            string scoutName = txtInfoScouterName.Text.Replace(","," ");
            string matchNumber = txtInfoMatchNumber.Text;
            string position = cmbInfoPosition.Text;
            string teamNumber = txtInfoTeam.Text;
            string robotTaxiCheck = btnAutoTaxiCheck.Tag.ToString();
            string stageTrapCheck = btnStageTrap.Tag.ToString();
            string parkClimbCheck = cmbStageParkClimb.Text;
            string autoAmpAccuracy = (Math.Round(100 * (Convert.ToDouble(autoAmpScored) / Convert.ToDouble(autoAmpScored + autoAmpMissed)), 2)).ToString() + "%";
            string autoSpeakerAccuracy = (Math.Round(100 * (Convert.ToDouble(autoSpeakerScored) / Convert.ToDouble(autoSpeakerMissed + autoSpeakerScored)), 2)).ToString() +"%";
            string teleAmpAccuracy = (Math.Round(100 * (Convert.ToDouble(teleAmpScored) / Convert.ToDouble(teleAmpMissed + teleAmpScored)), 2)).ToString() + "%";
            string teleAmplifiedSpeakerAccuracy = (Math.Round(100 * (Convert.ToDouble(teleAmpSpeakerScored) / Convert.ToDouble(teleAmpSpeakerScored + teleUnampSpeakerScored + teleSpeakerMissed)), 2)).ToString() + "%";
            string teleUnamplifiedSpeakerAccuracy = (Math.Round(100 * (Convert.ToDouble(teleUnampSpeakerScored) / Convert.ToDouble(teleAmpSpeakerScored + teleUnampSpeakerScored + teleSpeakerMissed)), 2)).ToString() + "%";
            string teleSpeakerMissPercentage = (Math.Round(100 * (Convert.ToDouble(teleSpeakerMissed) / Convert.ToDouble(teleAmpSpeakerScored + teleUnampSpeakerScored + teleSpeakerMissed)), 2)).ToString() + "%";

            //if (filePath != "")
            //{
                string line = $@"{scoutName},{teamNumber},{position},{matchNumber},{robotTaxiCheck},{autoAmpScored},{autoAmpMissed},{autoAmpAccuracy},{autoSpeakerScored},{autoSpeakerMissed},{autoSpeakerAccuracy},";
                line += $@"{teleAmpScored},{teleAmpMissed},{teleAmpAccuracy},{teleUnampSpeakerScored},{teleAmpSpeakerScored},{teleSpeakerMissed},{teleUnamplifiedSpeakerAccuracy},{teleAmplifiedSpeakerAccuracy},{teleSpeakerMissPercentage},";
                line += $@"{parkClimbCheck},{stageTrapCheck},{notes}";
                StreamWriter writer = new StreamWriter(filePath, true);
                writer.WriteLine(line);
                writer.Close();
                #region Reset Values for Next Output
                //checkbox for taxi
                btnAutoTaxiCheck.BackColor = Color.Red;
            btnAutoTaxiCheck.Text = "X";
            btnAutoTaxiCheck.Tag = "False";

            //checkbox for stage trap
            btnStageTrap.BackColor = Color.Red;
            btnStageTrap.Text = "X";
            btnStageTrap.Tag = "False";

            //resets text for next entry
            txtNotes.Text = "";
            txtInfoMatchNumber.Text = "";
            txtInfoTeam.Text = "";
            cmbStageParkClimb.SelectedIndex = 0;

            //resets every variable to default values
            autoAmpScored = 0;
            autoAmpMissed = 0;
            autoSpeakerScored = 0;
            autoSpeakerMissed = 0;
            teleAmpMissed = 0;
            teleAmpScored = 0;
            teleSpeakerMissed = 0;
            teleUnampSpeakerScored = 0;
            teleAmpSpeakerScored = 0;

            //updates the labels to register the change
            lblAutoAmp.Text = "Scored: " + autoAmpScored + "\nMisses: " + autoAmpMissed;
            lblAutoSpeaker.Text = "Scored: " + autoSpeakerScored + "\nMisses: " + autoSpeakerMissed;
            lblTeleSpeaker.Text = "Scored (Unamp.): " + teleUnampSpeakerScored + "\nScored (Amp.): " + teleAmpSpeakerScored + "\nMisses: " + teleSpeakerMissed;
            lblTeleAmp.Text = "Scored: " + teleAmpScored + "\nMisses: " + teleAmpMissed;
                #endregion
            
            } else if (txtInfoTeam.Text.Length==0){
                MessageBox.Show("You do not have a team selected. Please fill the team in");
            } else if (txtInfoMatchNumber.Text.Length == 0)
            {
                MessageBox.Show("You do not have a team selected. Please fill the match number in");
            }
            else
            {
                MessageBox.Show("WARNING! YOU DO NOT HAVE A FILE SELECTED! PLEASE CHOOSE A FILE LOCATION TO SAVE TO OR CREATE A NEW FILE!");
                MessageBox.Show("This round's data has not been saved, but will not be cleared until properly entered.");
            }
        }
    }
}
