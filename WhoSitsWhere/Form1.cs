using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace WhoSitsWhere
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ClsAutoUpdate update = new ClsAutoUpdate();
            update.checkforupdates();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> list_users_string = new List<string>();
            AD_class AD = new AD_class();
            List<DirectoryEntry> list_users = AD.AD_class_getusers();
            foreach(DirectoryEntry d in list_users)
            {
                list_users_string.Add(d.Name);
            }
            AD.WriteoutHTML(list_users_string);
        }
    }
}
