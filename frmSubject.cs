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
    public partial class frmSubject : Form
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
        Class1 subjects = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmSubject(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void frmSubject_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToLongDateString();
            try
            {
                DataTable dt = subjects.GetData("SELECT * FROM tbl_subjects ORDER BY SubjectCode");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on subject load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = subjects.GetData("SELECT * FROM tbl_subjects " +
                    " WHERE SubjectCode LIKE '%" + txtSearch.Text + "%' OR SubjectDescription LIKE '%" +
                    txtSearch.Text + "%' ORDER BY SubjectCode");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on txtsearch textchanged", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            frmSubject_Load(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewSubject newSubject = new frmNewSubject();
            newSubject.ShowDialog();
            frmSubject_Load(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string editSubjectCode = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editSubjectDescription = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string editUnit = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editCourse = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string editPreRequisite1 = dataGridView1.Rows[row].Cells[4].Value.ToString();
            string editPreRequisite2 = dataGridView1.Rows[row].Cells[5].Value.ToString();
            string editPreRequisite3 = dataGridView1.Rows[row].Cells[6].Value.ToString();
            string editPreRequisite4 = dataGridView1.Rows[row].Cells[7].Value.ToString();
            frmUpdateSubject updateaccount = new frmUpdateSubject(editSubjectCode,
                editSubjectDescription, editUnit, editCourse, editPreRequisite1, 
                editPreRequisite2,editPreRequisite3, editPreRequisite4, username);
            updateaccount.ShowDialog();
            frmSubject_Load(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete Subject?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    subjects.executeSQL("DELETE FROM tbl_subjects WHERE SubjectCode = '" + selectedUser + "'");
                    if (subjects.rowAffected > 0)
                    {
                        MessageBox.Show("Subject Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        subjects.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                        DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                        DateTime.Now.ToShortTimeString() +
                        "', 'Deleted a student with SubjectCode of " + selectedUser + "', '" +
                        username + "', 'Subject Management')");
                        frmSubject_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                MessageBox.Show(ex.Message, "Error on datagrid cell click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void frmSubject_MouseDown(object sender, MouseEventArgs e)
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