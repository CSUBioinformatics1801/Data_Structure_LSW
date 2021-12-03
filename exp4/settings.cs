using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exp4
{
    public partial class settings : Form
    {
        static Label label5 = new Label();
        static Label label11 = new Label();
        public settings(ref Label lb5, ref Label lb11)
        {
            label5 = lb5;
            label11 = lb11;
            InitializeComponent();
        }

        private void settings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = main.Sortmode - 1;
            comboBox2.SelectedIndex = main.Sortby - 1;
            if (main.Ascending == 1) radioButton1.Checked = true;
            else radioButton2.Checked = true;
            comboBox3.SelectedIndex = main.Searchmode - 1;
            comboBox4.SelectedIndex = main.Searchby - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            main.Sortmode = comboBox1.SelectedIndex + 1;
            main.Sortby = comboBox2.SelectedIndex + 1;
            if (this.radioButton1.Checked == true) main.Ascending = 1;
            else main.Ascending = 0;
            main.Searchmode = comboBox3.SelectedIndex + 1;
            main.Searchby = comboBox4.SelectedIndex + 1;
            label5.Text = Program.SMtag[main.Sortmode - 1];
            label11.Text = Program.SDtag[main.Searchmode - 1];
            this.Hide();
        }
    }
}
