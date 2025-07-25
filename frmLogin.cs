using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using ticket_management;

namespace CS311_3B_2023_Database
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        Class1 login = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        private string username, password;
        private int errorCount;

        public void validateUsername()
        {
            if(string.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Input is empty");
            }
            else
            {
                errorProvider1.SetError(txtUsername, "");
                username = txtUsername.Text;
            }
        }
        public void validatePassword()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                errorProvider1.SetError(txtPassword, "Input is empty");
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
                password = txtPassword.Text;
            }
        }

        private void countErrors()
        {
            errorCount = 0;
            foreach(Control c in errorProvider1.ContainerControl.Controls)
            {
                if(!(string.IsNullOrEmpty(errorProvider1.GetError(c))))
                {
                    errorCount++;
                }
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(Convert.ToChar(e.KeyChar) == 13)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToChar(e.KeyChar) == 13)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShow.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '●';
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //input
            validateUsername();
            validatePassword();
            countErrors();
            if (errorCount == 0)
            {
                try
                {
                    DataTable dt = login.GetData("SELECT * FROM tbl_accounts WHERE username = '" + username + "'AND password = '"
                                   + password + "'AND status = 'ACTIVE'");
                    if (dt.Rows.Count > 0)
                    {
                        frmMain mainfrm = new frmMain(txtUsername.Text, dt.Rows[0].Field<string>("usertype"));
                        mainfrm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// Form Design
        private void txtUsername_Click(object sender, EventArgs e)
        {
            txtUsername.BackColor = Color.White;
            panel2.BackColor = Color.White;
            panel3.BackColor = SystemColors.Control;
            txtPassword.BackColor = SystemColors.Control;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.BackColor = Color.White;
            panel3.BackColor = Color.White;
            txtUsername.BackColor = SystemColors.Control;
            panel2.BackColor = SystemColors.Control;
        }
    }
}
