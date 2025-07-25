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
    public partial class frmNewAccount : Form
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
        private int errorCount;
        Class1 newaccount = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmNewAccount(String username)
        {
            InitializeComponent();
            this.username = username;
        }
        public void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Username is empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is empty");
                errorCount++;
            }
            else if (txtPassword.Text.Length < 6)
            {
                errorProvider1.SetError(txtPassword, "Password length should be atleast 6 characters");
                errorCount++;
            }
            if (cmbUsertype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbUsertype, "Select usertype");
                errorCount++;
            }
            try 
            {
                DataTable dt = newaccount.GetData("SELECT * FROM tbl_accounts WHERE username = '" + txtUsername.Text + "'");
                if(dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtUsername, "Username is already in use");
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validate exist username", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            validateForm();
            if(errorCount == 0)
            {
                try
                {
                    newaccount.executeSQL("INSERT INTO tbl_accounts VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" +
                        cmbUsertype.Text.ToUpper() + "', '" + txtEmail.Text + "', 'ACTIVE', '" + username + "', '" +
                        DateTime.Now.ToShortDateString() + "')");
                    if(newaccount.rowAffected > 0)
                    {
                        MessageBox.Show("New account added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            cmbUsertype.SelectedIndex = -1;
            txtUsername.Focus();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmNewAccount_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}