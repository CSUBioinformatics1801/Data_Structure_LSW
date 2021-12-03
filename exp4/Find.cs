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

namespace exp4
{
    public partial class Find : Form
    {
        [DllImport("kernel32.dll")]
        extern static short QueryPerformanceCounter(ref long x);
        Label label2 = new Label();
        Label label9 = new Label();
        Label label11 = new Label();
        DataGridView dataGridView1 = new DataGridView();

        public Find(ref Label lb2, ref Label lb9, ref Label lb11, ref DataGridView dgv)
        {
            label2 = lb2;
            label9 = lb9;
            label11 = lb11;
            dataGridView1 = dgv;
            InitializeComponent();
        }

        private void Find_Load(object sender, EventArgs e)
        {
            comboBox3.SelectedIndex = main.Searchmode - 1;
            comboBox4.SelectedIndex = main.Searchby - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QueryPerformanceCounter(ref main.start);
            main.Searchmode = comboBox3.SelectedIndex + 1;
            main.Searchby = comboBox4.SelectedIndex + 1;
            label11.Text = Program.SDtag[main.Searchmode - 1];
            int found = 0, index;
            dataGridView1.Rows.Clear();
            Info tmp = new Info();
            switch (main.Searchby)
            {
                case 1:
                    tmp.Id = textBox1.Text;
                    break;
                case 2:
                    tmp.Age = Convert.ToInt32(textBox1.Text);
                    break;
                case 3:
                    tmp.Date = Convert.ToDateTime(textBox1.Text);
                    break;
            }
            if (main.Searchmode == 1)
            {
                if (main.sorted[main.Searchby - 1] == 0)
                {
                    Console.WriteLine("Unsorted field.");
                    label9.Text = "Unsorted field.";
                }
                else
                {
                    Console.Write("Binary find::");
                    int l = 0, r = main.cnt - 1, mid, ans = -1, i;
                    while (l <= r)
                    {
                        mid = l + (r - l) / 2;
                        if (main.Equal(main.patientInfo[mid], tmp, main.Searchby))
                        {
                            ans = mid;
                            break;
                        }
                        if (main.Cmp(main.patientInfo[mid], tmp, main.Searchby))
                        {
                            l = mid + 1;
                        }
                        else
                        {
                            r = mid - 1;
                        }
                    }
                    i = ans;
                    if(i != -1)
                    {
                        while (i >= 0 && main.Equal(main.patientInfo[ans], main.patientInfo[i], main.Searchby)) i--; i++;
                        while (i < main.cnt && main.Equal(main.patientInfo[ans], main.patientInfo[i], main.Searchby))
                        {
                            index = dataGridView1.Rows.Add();
                            main.Show(dataGridView1, index, i);
                            found++; i++;
                        }
                    }
                    QueryPerformanceCounter(ref main.end);
                    var t = (main.end - main.start) / (double)main.freq * 1000;
                    label2.Text = t.ToString("F3");
                    label9.Text = found == 0 ? ("No record found.") : found.ToString() + " record(s) found";
                }
            }
            else
            {
                Console.Write("Sequential find::");
                for (int i = 0; i < main.cnt; i++)
                {
                    if (main.Equal(main.patientInfo[i], tmp, main.Searchby))
                    {
                        index = dataGridView1.Rows.Add();
                        main.Show(dataGridView1, index, i);
                        found++;
                    }
                }
                QueryPerformanceCounter(ref main.end);
                var t = (main.end - main.start) / (double)main.freq * 1000;
                Console.WriteLine("time_uesd:{0}", t);
                label2.Text = t.ToString("F3");
                label9.Text = found == 0 ? ("No record found.") : found.ToString() + " record(s) found";
            }
        }
    }
}
