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
    public partial class frmNewGrade : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        private string studentNumber, StudentName, course;
        private int errorCount;
        Class1 newgrades = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmNewGrade(string StudentNumber, string StudentName, string course)
        {
            InitializeComponent();
            this.studentNumber = StudentNumber;
            this.StudentName = StudentName;
            this.course = course;
        }

        private void frmNewGrade_Load(object sender, EventArgs e)
        {
            txtStudentNumber.Text = studentNumber;
            txtName.Text = StudentName;
            txtCourse.Text = course;
            try
            {
                DataTable dt = newgrades.GetData("SELECT sub.SubjectCode, sub.SubjectDescription " +
                    "FROM tbl_subjects sub WHERE NOT EXISTS (" +
                    "SELECT 1 FROM tbl_grades grd WHERE sub.SubjectCode = grd.SubjectCode " +
                    "AND grd.StudentNumber = '" + txtStudentNumber.Text + "');");
                if (dt.Rows.Count > 0)
                {
                    cmbSubjectCode.DataSource = dt;
                    cmbSubjectCode.DisplayMember = "SubjectCode";
                    cmbSubjectCode.ValueMember = "SubjectDescription";

                    txtDescription.Text = cmbSubjectCode.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on selecting subject code",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void countErrors(Control parentControl)
        {
            foreach (Control c in parentControl.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(c)))
                {
                    errorCount++;
                    c.Focus();
                }
                if (c.HasChildren)
                {
                    countErrors(c);
                }
            }
        }
        private void validateForm()
        {
            errorCount = 0;
            cmbGrade_SelectedIndexChanged(null, EventArgs.Empty);
            countErrors(errorProvider1.ContainerControl);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            errorCount = 0;
            cmbGrade_SelectedIndexChanged(null, EventArgs.Empty);
            countErrors(errorProvider1.ContainerControl);
            validateForm();
            if (errorCount == 0)
            {
                DialogResult dr = MessageBox.Show("Add Grade?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newgrades.executeSQL("INSERT INTO tbl_grades VALUES (" +
                            $"'{txtStudentNumber.Text}', '{cmbSubjectCode.Text}', '{cmbGrade.Text}')");
                        if (newgrades.rowAffected > 0)
                        {
                            MessageBox.Show("New grade added", "Message",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save button",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cmbSubjectCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescription.Text = cmbSubjectCode.SelectedValue.ToString();
        }
        public string primaryKey 
        { 
            get 
            { 
                return cmbSubjectCode.Text; 
            } 
        }

        private void cmbGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGrade.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbGrade, "Select a grade");
                errorCount++;
            }
            else
            {
                errorProvider1.SetError(cmbGrade, "");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbSubjectCode.SelectedIndex = -1;
            cmbGrade.SelectedIndex = -1;
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void frmNewGrade_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}