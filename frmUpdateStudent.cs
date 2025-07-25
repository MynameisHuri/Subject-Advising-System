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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311_3B_2023_Database
{
    public partial class frmUpdateStudent : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End
        private string editStudentnumber, editLastname, editFirstname, editMiddlename, editCourse, editYear, username;
        Class1 updatestudent = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");

        public frmUpdateStudent(string editStudentnumber, string editLastname, string editFirstname,
                                string editMiddlename, string editCourse, string editYear, string username)
        {
            InitializeComponent();
            this.editStudentnumber = editStudentnumber;
            this.editLastname = editLastname;
            this.editFirstname = editFirstname;
            this.editMiddlename = editMiddlename;
            this.editCourse = editCourse;
            this.editYear = editYear;
            this.username = username;
        }

        private void frmUpdateStudent_Load(object sender, EventArgs e)
        {
            txtStudentnumber.Text = editStudentnumber;
            txtLastname.Text = editLastname;
            txtFirstname.Text = editFirstname;
            txtMiddlename.Text = editMiddlename;
            try
            {
                DataTable dt = updatestudent.GetData("SELECT * FROM tbl_course ORDER BY courseDescription");
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
            if (cmbYear.Items.Contains(editYear))
            {
                cmbYear.SelectedItem = editYear;
            }
            cmbCourse.SelectedValue = editCourse;
            txtStudentnumber.ReadOnly = true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Update Student?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    updatestudent.executeSQL("UPDATE tbl_students SET Lastname = '" +
                        txtLastname.Text + "', Firstname = '" + txtFirstname.Text + "', Middlename = '" +
                        txtMiddlename.Text + "', Course = '" + cmbCourse.SelectedValue.ToString() + "', Year = '" + cmbYear.Text + "' WHERE StudentNumber = '"
                        + txtStudentnumber.Text + "'");
                    if (updatestudent.rowAffected > 0)
                    {
                        MessageBox.Show("Student Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updatestudent.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                            DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                            DateTime.Now.ToShortTimeString() +
                            "', 'Updated a student with studentnumber of " + txtStudentnumber.Text + "', '" +
                            username + "', 'Student Management')");
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
        private void frmUpdateStudent_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}