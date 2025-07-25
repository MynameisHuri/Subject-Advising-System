using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_management;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311_3B_2023_Database
{
    public partial class frmAddStudent : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End
        private int errorCount;
        Class1 adddstudent = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmAddStudent()
        {
            InitializeComponent();
        }

        public void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(txtStudentnumber.Text))
            {
                errorProvider1.SetError(txtStudentnumber, "Student Number is empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtLastname.Text))
            {
                errorProvider1.SetError(txtLastname, "Last Name is empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtFirstname.Text))
            {
                errorProvider1.SetError(txtFirstname, "First Name is empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtMiddlename.Text))
            {
                errorProvider1.SetError(txtMiddlename, "Middle Name is empty");
                errorCount++;
            }
            if (cmbCourse.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbCourse, "Select Course");
                errorCount++;
            }
            if (cmbYear.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbYear, "Select Year");
                errorCount++;
            }
            try
            {
                DataTable dt = adddstudent.GetData("SELECT * FROM tbl_students WHERE StudentNumber = '" + txtStudentnumber.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtStudentnumber, "Student Number is already in use");
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validate exist StudentNumber", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmAddStudent_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = adddstudent.GetData("SELECT * FROM tbl_course ORDER BY courseDescription");
                if (dt.Rows.Count > 0)
                {
                    cmbCourse.DataSource = dt;
                    cmbCourse.DisplayMember = "courseDescription";
                    cmbCourse.ValueMember = "courseID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on selecting courseID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorCount == 0)
            {
                try
                {
                    adddstudent.executeSQL("INSERT INTO tbl_students VALUES ('" + txtStudentnumber.Text + "', '" + txtLastname.Text + "', '" +
                        txtFirstname.Text + "', '"  + txtMiddlename.Text + "', '" + cmbCourse.SelectedValue.ToString() 
                        + "', '" + cmbYear.Text + "')");
                    if (adddstudent.rowAffected > 0)
                    {
                        MessageBox.Show("New student added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on save button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStudentnumber.Clear();
            txtLastname.Clear();
            txtFirstname.Clear();
            txtMiddlename.Clear();
            cmbCourse.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            txtStudentnumber.Focus();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmAddStudent_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
