using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace exp4
{
    public partial class main : Form
    {
        [DllImport("kernel32.dll")]
        extern static short QueryPerformanceCounter(ref long x);
        [DllImport("kernel32.dll")]
        extern static short QueryPerformanceFrequency(ref long x);
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        public static Info[] patientInfo = new Info[10500];
        public static long freq, start, end;
        public static int Sortmode = 1, Sortby = 1, Ascending = 1, Searchmode = 1, Searchby = 1, cnt;
        public static int[] sorted = new int[3];
        static Sort[] sort = new Sort[8] { DirectInsertSort, SheelSort, BubbleSort, QucikSort, SimpleSelectionSort, HeapSort, MergeSort, BinaryInsertSort };
        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }
        public static void Show(DataGridView dgv, int index, int i)
        {
            dgv.Rows[index].Cells[0].Value = patientInfo[i].Id.ToString();
            dgv.Rows[index].Cells[1].Value = patientInfo[i].Sex == 0 ? "男" : "女";
            dgv.Rows[index].Cells[2].Value = patientInfo[i].Age.ToString();
            dgv.Rows[index].Cells[3].Value = patientInfo[i].Date.ToString();
            dgv.Rows[index].Cells[4].Value = patientInfo[i].Charge.ToString();
        }
        /// <summary>
        /// Cmp(Info _info1, Info _info2, int para) is a function that compare two Info. As the parameters change, return value will change.
        /// Default value of para is 0, if no parament was passed, compare mode will be static int Sortmode.
        /// Whether value of static int Ascending is 0 will determine negating return value or not.
        /// </summary>
        /// <param name="info1"></param>
        /// <param name="info2"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static bool Cmp(Info info1, Info info2, int sb = 0)
        {
            bool ans = true;
            sb = (sb == 0) ? Sortby : sb;
            if (sb == 1) ans = Convert.ToInt32(info1.Id) > Convert.ToInt32(info2.Id);
            if (sb == 2) ans = info1.Age > info2.Age;
            if (sb == 3) ans = info1.Date > info2.Date;
            return (Ascending == 0) ? ans : !ans;
        }
        public static bool Equal(Info info1, Info info2, int sb = 0)
        {
            sb = (sb == 0) ? Searchby : sb;
            if (sb == 1) return Convert.ToInt32(info1.Id) == Convert.ToInt32(info2.Id);
            if (sb == 2) return info1.Age == info2.Age;
            if (sb == 3) return info1.Date == info2.Date;
            return false;
        }
        delegate void Sort(ref int swap, int l, int r);
        static void DirectInsertSort(ref int swap, int l, int r)
        {
            for (int i = 0; i < cnt; i++)
            {
                for (int j = i; j > 0 && Cmp(patientInfo[j], patientInfo[j - 1]); j--)
                {
                    Swap(ref patientInfo[j], ref patientInfo[j - 1]);
                    swap++;
                }
            }
        }

        static void SheelSort(ref int swap, int l, int r)
        {
            Info tmp = new Info();
            int k;
            for (int gap = cnt / 2; gap > 0; gap /= 2)
            {
                for(int i = 0; i < gap; i++)
                {
                    for(int j = i + gap; j < cnt; j++)
                    {
                        if(Cmp(patientInfo[j], patientInfo[j - gap]))
                        {
                            tmp = patientInfo[j];
                            k = j - gap;
                            while(k >= 0 && Cmp(tmp, patientInfo[k]))
                            {
                                Swap(ref patientInfo[k + gap], ref patientInfo[k]);
                                swap++;
                                k -= gap;
                            }
                            patientInfo[k + gap] = tmp;
                        }
                    }
                }
            }
        }

        static void BubbleSort(ref int swap, int l, int r)
        {
            for (int i = 0; i < cnt; i++)
            {
                for (int j = 0; j < cnt; j++)
                {
                    if (Cmp(patientInfo[i], patientInfo[j]))
                    {
                        Swap(ref patientInfo[i], ref patientInfo[j]);
                        swap++;
                    }

                }
            }
        }

        static void QucikSort(ref int swap, int l, int r)
        {
            if(l < r)
            {
                int i = l, j = r;
                Info tmp = new Info();
                tmp = patientInfo[i];
                while(i < j)
                {
                    while (i < j && Cmp(tmp, patientInfo[j])) j--;
                    if (i < j)
                    {
                        patientInfo[i++] = patientInfo[j];
                        swap++;
                    }
                    while (i < j && Cmp(patientInfo[i], tmp)) i++;
                    if (i < j)
                    {
                        patientInfo[j--] = patientInfo[i];
                        swap++;
                    }
                }
                patientInfo[i] = tmp;
                swap++;
                QucikSort(ref swap, l, i - 1);
                QucikSort(ref swap, i + 1, r);
            }
        }

        static void SimpleSelectionSort(ref int swap, int l, int r)
        {
            Info tmp = new Info();
            int mi = 0;
            for (int i = 0; i < cnt; i++)
            {
                tmp = patientInfo[i];
                mi = i;
                for (int j = i; j < cnt; j++)
                {
                    if (Cmp(patientInfo[j], tmp))
                    {
                        tmp = patientInfo[j];
                        mi = j;
                    }
                }
                Swap(ref patientInfo[i], ref patientInfo[mi]);
                swap++;
            }
        }

        static void heapify(ref int swap, int l, int r)
        {
            int d = l, s = 2 * d + 1;
            while(s <= r)
            {
                if(s + 1 <= r && Cmp(patientInfo[s], patientInfo[s + 1]))
                {
                    s++;
                }
                if (Cmp(patientInfo[s], patientInfo[d]))
                    return;
                else
                {
                    Swap(ref patientInfo[d], ref patientInfo[s]);
                    swap++;
                    d = s;
                    s = d * 2 + 1;
                }
            }
        }
        static void HeapSort(ref int swap, int l, int r)
        {
            for(int i = cnt / 2 - 1; i >= 0; i--)
            {
                heapify(ref swap, i, cnt - 1);
            }
            for(int i = cnt - 1; i > 0; i--)
            {
                Swap(ref patientInfo[0], ref patientInfo[i]);
                swap++;
                heapify(ref swap, 0, i - 1);
            }
        }

        static void MergeSort(ref int swap, int l, int r)
        {
            if(l < r)
            {
                int mid = l + (r - l) / 2;
                MergeSort(ref swap, l, mid);
                MergeSort(ref swap, mid + 1, r);
                Info[] tpatientInfo = new Info[cnt];
                int i = l, j = mid + 1, tcnt = 0;
                while(i <= mid && j <= r)
                {
                    if(Cmp(patientInfo[i], patientInfo[j]))
                    {
                        tpatientInfo[tcnt++] = patientInfo[i++]; 
                    }
                    else
                    {
                        tpatientInfo[tcnt++] = patientInfo[j++];
                    }
                }
                while (i <= mid) tpatientInfo[tcnt++] = patientInfo[i++];
                while (j <= r) tpatientInfo[tcnt++] = patientInfo[j++];
                tcnt = 0;
                while(l <= r)
                {
                    patientInfo[l++] = tpatientInfo[tcnt++];
                    swap++;
                }
            }
        }

        static void BinaryInsertSort(ref int swap, int l, int r)
        {
            Info tmp = new Info();
            int mid;
            for(int i = 1; i < cnt; i++)
            {
                tmp = patientInfo[i];
                l = 0;
                r = i - 1;
                while(l <= r)
                {
                    mid = l + (r - l) / 2;
                    if(Cmp(tmp, patientInfo[mid]))
                    {
                        r = mid - 1;
                    }
                    else
                    {
                        l = mid + 1;
                    }
                }
                for(int j = i - 1; j >= l; j--)
                {
                    patientInfo[j + 1] = patientInfo[j];
                }
                patientInfo[l] = tmp;
            }
        }
        public main()
        {
            AllocConsole();
            Console.WriteLine("Debug Panel:");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            QueryPerformanceFrequency(ref freq);
            for (int i = 0; i < 3; i++)
                sorted[i] = 0;
            label5.Text = Program.SMtag[Sortmode - 1];
            label11.Text = Program.SDtag[Searchmode - 1];
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.Write("Read File::");
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(fileDialog.FileName))
                {
                    QueryPerformanceCounter(ref start);
                    string line;
                    cnt = -1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        cnt++;
                        if (cnt == 0) continue;
                        int it = 0, sex = 0, age = 0, year = 0, month = 0, day = 0, hour = 0, minute = 0, charge = 0;
                        string id = "";
                        while (line[it] != ',') id += line[it++]; it++;
                        if (line[it++] != '男') sex = 1; it++;
                        while (line[it] != ',') age = age * 10 + (line[it++] - '0'); it++;
                        while (line[it] != '-') year = year * 10 + (line[it++] - '0'); it++;
                        while (line[it] != '-') month = month * 10 + (line[it++] - '0'); it++;
                        while (line[it] != ' ') day = day * 10 + (line[it++] - '0'); it++;
                        while (line[it] != ':') hour = hour * 10 + (line[it++] - '0'); it++;
                        while (line[it] != ',') minute = minute * 10 + (line[it++] - '0'); it++;
                        while (it != line.Length) charge = charge * 10 + (line[it++] - '0');
                        patientInfo[cnt - 1] = new Info(id, sex, age, year, month, day, hour, minute, charge);
                    }
                    int index;
                    for (int i = 0; i < cnt; i++)
                    {
                        index = dataGridView1.Rows.Add();
                        Show(dataGridView1, index, i);
                    }
                    QueryPerformanceCounter(ref end);
                    var t = (end - start) / (double)freq * 1000;
                    Console.WriteLine("time_uesd:{0}", t);
                    label2.Text = t.ToString("F3");
                    label4.Text = cnt.ToString() + " record(s) imported.";
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cnt = 0;
            dataGridView1.Rows.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index;
            dataGridView1.Rows.Clear();
            for (int i = 0; i < cnt; i++)
            {
                index = dataGridView1.Rows.Add();
                Show(dataGridView1, index, i);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Find find = new Find(ref label2, ref label9, ref label11, ref dataGridView1);
            find.Show();
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryPerformanceCounter(ref start);
            Console.Write("{0}::", Program.SMtag[Sortmode - 1]);
            int swap = 0;
            sort[Sortmode - 1](ref swap, 0, cnt - 1);
            for (int i = 0; i < 3; i++)
                sorted[i] = 0;
            sorted[Sortby - 1] = 1;
            int index;
            dataGridView1.Rows.Clear();
            for (int i = 0; i < cnt; i++)
            {
                index = dataGridView1.Rows.Add();
                Show(dataGridView1, index, i);
            }
            QueryPerformanceCounter(ref end);
            var t = (end - start) / (double)freq * 1000;
            Console.WriteLine("time_uesd:{0}", t);
            label2.Text = t.ToString("F3");
            label8.Text = swap.ToString();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream st;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((st = saveFileDialog1.OpenFile()) != null)
                {
                    using (StreamWriter sw = new StreamWriter(st))
                    {
                        sw.WriteLine("patient_id,sex,age,registering_date,clinic_charge");
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {
                            string str = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            for (int j = 1; j < 5; j++)
                                str += "," + dataGridView1.Rows[i].Cells[j].Value.ToString();
                            sw.WriteLine(str);
                        }
                    }

                }
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings settings = new settings(ref label5, ref label11);
            settings.Show();
        }

    }
}
