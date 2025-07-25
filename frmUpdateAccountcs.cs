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
    public partial class frmUpdateAccountcs : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End
        private string editUsername, editPassword, editType, editEmail, editStatus, username;
        Class1 updateaccount = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmUpdateAccountcs(string editUsername, string editPassword, string editType, 
                                  string editEmail, string editStatus, string username)
        {
            InitializeComponent();
            this.editUsername = editUsername;
            this.editPassword = editPassword;
            this.editType = editType;
            this.editEmail = editEmail;
            this.editStatus = editStatus;
            this.username = username;
        }

        private void frmUpdateAccountcs_Load(object sender, EventArgs e)
        {
            txtUsername.Text = editUsername;
            txtPassword.Text = editPassword;
            if (editType == "ADMINISTRATOR")
            {
                cmbUsertype.SelectedIndex = 0;
            }
            else if (editType == "REGISTRAR")
            {
                cmbUsertype.SelectedIndex = 1;
            }
            else
            {
                cmbUsertype.SelectedIndex = 2;
            }
            txtEmail.Text = editEmail;
            if (editStatus == "ACTIVE")
            {
                cmbStatus.SelectedIndex = 0;
            }
            else
            {
                cmbStatus.SelectedIndex = 1;
            }
            txtUsername.ReadOnly = true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Update Account?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    updateaccount.executeSQL("UPDATE tbl_accounts SET password = '" +
                    txtPassword.Text + "', usertype = '" + cmbUsertype.Text.ToUpper() +
                    "', email = '" + txtEmail.Text + "', status = '" +
                    cmbStatus.Text.ToUpper() + "' WHERE username = '" +
                    txtUsername.Text + "'");
                    if (updateaccount.rowAffected > 0)
                    {
                        MessageBox.Show("Account Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updateaccount.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                            DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                            DateTime.Now.ToShortTimeString() +
                            "', 'Updated a user with username of " + txtUsername.Text + "', '" +
                            username + "', 'Account Management')");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on save button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShow.Checked)
            {
                txtPassword.UseSystemPasswordChar = true;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false;
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void frmUpdateAccountcs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}