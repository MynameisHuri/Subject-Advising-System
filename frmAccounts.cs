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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CS311_3B_2023_Database
{
    public partial class frmAccounts : Form
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
        Class1 accounts = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmAccounts(string username)

        {
            InitializeComponent();
            this.username = username;
        }

        private void frmAccounts_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToLongDateString();
            try
            {
                DataTable dt = accounts.GetData("SELECT username, password, usertype, email, status FROM tbl_accounts WHERE username <> '" + username +
                    "' ORDER BY username");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on accounts load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// Textbox Seach Design 
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT username, password, usertype, email, status FROM tbl_accounts WHERE username <> '" + username +
                    "' AND (username LIKE '%" + txtSearch.Text + "%' OR usertype LIKE '%" + txtSearch.Text + "%' OR status LIKE '%" +
                    txtSearch.Text + "%') ORDER BY username");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on txtsearch textchanged", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            frmAccounts_Load(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewAccount newaccount = new frmNewAccount(username);
            newaccount.ShowDialog();
            frmAccounts_Load(sender, e);
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string editUsername = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editPassword = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string editUsertype = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editEmail = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string editStatus = dataGridView1.Rows[row].Cells[4].Value.ToString();
            frmUpdateAccountcs updateaccount = new frmUpdateAccountcs(editUsername,
                editPassword, editUsertype, editEmail, editStatus, username);
            updateaccount.ShowDialog();
            frmAccounts_Load(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete Account?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    accounts.executeSQL("DELETE FROM tbl_accounts WHERE username = '" + selectedUser + "'");
                    if(accounts.rowAffected > 0)
                    {
                        MessageBox.Show("Account Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        accounts.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                            DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                            DateTime.Now.ToShortTimeString() +
                            "', 'Deleted a user with username of " + selectedUser + "', '" +
                            username + "', 'Account Management')");
                        frmAccounts_Load(sender, e);
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.Value != null)
            {
                e.Value = new String('●', e.Value.ToString().Length);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void frmAccounts_MouseDown(object sender, MouseEventArgs e)
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