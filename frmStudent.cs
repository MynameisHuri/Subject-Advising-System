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
    public partial class frmStudent : Form
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
        Class1 students = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public Point mouseLocation;
        public frmStudent(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToLongDateString();
            try
            {
                DataTable dt = students.GetData("SELECT * FROM tbl_students ORDER BY StudentNumber");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on students load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = students.GetData("SELECT * FROM tbl_students " +
                    " WHERE StudentNumber LIKE '%" + txtSearch.Text + "%' OR Lastname LIKE '%" + 
                    txtSearch.Text + "%' OR Course LIKE '%" + txtSearch.Text + "%' ORDER BY StudentNumber");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on txtsearch textchanged", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            frmStudent_Load(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddStudent addStudent = new frmAddStudent(); 
            addStudent.ShowDialog();
            frmStudent_Load(sender, e);
        }
        
        private int row = 0;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string editStudentnumber = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editLastname = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string editFirstname = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editMiddlename = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string editCourse = dataGridView1.Rows[row].Cells[4].Value.ToString();
            string editYear = dataGridView1.Rows[row].Cells[5].Value.ToString();
            frmUpdateStudent updateaccount = new frmUpdateStudent(editStudentnumber,
                editLastname, editFirstname, editMiddlename, editCourse, editYear, username);
            updateaccount.ShowDialog();
            frmStudent_Load(sender, e);
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete Student?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    students.executeSQL("DELETE FROM tbl_students WHERE StudentNumber = '" + selectedUser + "'");
                    if (students.rowAffected > 0)
                    {
                        MessageBox.Show("Student Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        students.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                        DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                        DateTime.Now.ToShortTimeString() +
                        "', 'Deleted a student with StudentNumber of " + selectedUser + "', '" +
                        username + "', 'Student Management')");
                        frmStudent_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = (int)e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on datagrid cell click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void frmStudent_MouseDown(object sender, MouseEventArgs e)
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

        private void lblTime_Click(object sender, EventArgs e)
        {

        }
    }
}