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
    public partial class frmUpdateCourse : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End
        private string editCourseID, editCourseDescription, username;
        Class1 updatecourse = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");

        public frmUpdateCourse(string editCourseID, string editCourseDescription, string username)
        {
            InitializeComponent();
            this.editCourseID = editCourseID;
            this.editCourseDescription = editCourseDescription;
            this.username = username;
        }
        private void frmUpdateCourse_Load(object sender, EventArgs e)
        {
            txtCourseID.Text = editCourseID;
            txtCourseDescription.Text = editCourseDescription;
            txtCourseID.ReadOnly = true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Course Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    updatecourse.executeSQL("UPDATE tbl_course SET courseDescription = '" + txtCourseDescription.Text +
                        "' WHERE courseID = '" + txtCourseID.Text + "'");
                    if (updatecourse.rowAffected > 0)
                    {
                        MessageBox.Show("Course Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updatecourse.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                            DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                            DateTime.Now.ToShortTimeString() +
                            "', 'Updated a course with course ID of " + txtCourseID.Text + "', '" +
                            username + "', 'Course Management')");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on save button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void frmUpdateCourse_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}