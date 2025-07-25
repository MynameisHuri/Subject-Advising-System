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
    public partial class frmCourse : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End
        private string username;
        Class1 courses = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmCourse(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void frmCourse_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToLongDateString();
            try
            {
                DataTable dt = courses.GetData("SELECT * FROM tbl_course ORDER BY courseID");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on course load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = courses.GetData("SELECT * FROM tbl_course " +
                    " WHERE courseID LIKE '%" + txtSearch.Text + "%' OR courseDescription LIKE '%" +
                    txtSearch.Text + "%' ORDER BY courseID");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on txtsearch textchanged", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int row = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = (int)e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on datagrid cell click",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            frmCourse_Load(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewCourse newcourse = new frmNewCourse();
            newcourse.ShowDialog();
            frmCourse_Load(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string editCourseID = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editCourseDescription = dataGridView1.Rows[row].Cells[1].Value.ToString();
            frmUpdateCourse updateCourse = new frmUpdateCourse(editCourseID, editCourseDescription, username);
            updateCourse.ShowDialog();
            frmCourse_Load(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete Course?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    courses.executeSQL("DELETE FROM tbl_course WHERE courseID = '" + selectedUser + "'");
                    if (courses.rowAffected > 0)
                    {
                        MessageBox.Show("Course Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        courses.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                        DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                        DateTime.Now.ToShortTimeString() +
                        "', 'Deleted a course with courseID of " + selectedUser + "', '" +
                        username + "', 'Course Management')");
                        frmCourse_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }
        private void frmCourse_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
