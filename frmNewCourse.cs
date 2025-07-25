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

namespace CS311_3B_2023_Database
{
    public partial class frmNewCourse : Form
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
        Class1 newcourse = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmNewCourse()
        {
            InitializeComponent();
        }

        public void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(txtCourseID.Text))
            {
                errorProvider1.SetError(txtCourseID, "Course ID is empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtCourseDescription.Text))
            {
                errorProvider1.SetError(txtCourseDescription, "Course Description is empty");
                errorCount++;
            }
            try
            {
                DataTable dt = newcourse.GetData("SELECT * FROM tbl_course WHERE courseID = '" + txtCourseID.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtCourseID, "Course ID is already in use");
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validate exist courseID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorCount == 0)
            {
                try
                {
                    newcourse.executeSQL("INSERT INTO tbl_course VALUES ('" + txtCourseID.Text + "', '" 
                        + txtCourseDescription.Text + "')");
                    if (newcourse.rowAffected > 0)
                    {
                        MessageBox.Show("New course added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtCourseID.Clear();
            txtCourseDescription.Clear();
            txtCourseID.Focus();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmNewCourse_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}