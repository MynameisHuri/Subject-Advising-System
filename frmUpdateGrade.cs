using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_management;

namespace CS311_3B_2023_Database
{
    public partial class frmUpdateGrade : Form
    {
        /// To Move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End

        private string StudentNumber, StudentName, Course, SubjectCode, Description, EditGrade, username;
        Class1 updategrades = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");

        public frmUpdateGrade(string StudentNumber, string StudentName, string Course,
            string SubjectCode, string Description, string EditGrade, string username)
        {
            InitializeComponent();
            this.StudentNumber = StudentNumber;
            this.StudentName = StudentName;
            this.Course = Course;
            this.SubjectCode = SubjectCode;
            this.Description = Description;
            this.EditGrade = EditGrade;
            this.username = username;
        }

        private void frmUpdateGrade_Load(object sender, EventArgs e)
        {
            txtStudentNumber.Text = StudentNumber;
            txtName.Text = StudentName;
            txtCourse.Text = Course;
            txtSubjectCode.Text = SubjectCode;
            txtDescription.Text = Description;
            int gradeindex = cmbGrade.FindString(EditGrade);
            cmbGrade.SelectedIndex = gradeindex;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Update Grade?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    updategrades.executeSQL("UPDATE tbl_grades " +
                            $"SET Grade = '{cmbGrade.Text}' " +
                            $"WHERE StudentNumber = '{txtStudentNumber.Text}' " +
                            $"AND SubjectCode = '{txtSubjectCode.Text}'");
                    if (updategrades.rowAffected > 0)
                    {
                        MessageBox.Show("Grade Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updategrades.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                            DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                            DateTime.Now.ToShortTimeString() +
                            "', 'Updated a grade with StudentNumber of " + txtStudentNumber.Text + "', '" +
                            username + "', 'Grade Management')");
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on save button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public string primaryKey
        {
            get
            {
                return txtSubjectCode.Text;
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateGrade_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}