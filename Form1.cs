// This is a personal academic project. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba4CryptV2
{
    public partial class Form1 : Form
    {
        int multInv = -1;
        int g = -1, p;
        int x, y, k;
        int a, b;
        Random rand = new Random();

        String originText, cryptedText, decryptedText;

        public class TestClass
        {
            public string name { get; set; }

            public TestClass(string n)
            {
                name = n;
            }
        }

        int VozvVStep(int a, int x, int m)
        {

            int b, result = 1;
            b = a % m;

            while (x > 0)
            {
                if ((x & 1) == 1)
                {
                    result = result * b;
                    result = result % m;
                }
                b = b * b;
                b = b % m;
                x = x >> 1;

            }
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            originText = null;
            cryptedText = null;
            decryptedText = null;
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
        }

        int Ymnoz(int a, int b, int n)
        {

            int sum = 0;
            for (int i = 0; i < b; i++)
            {
                sum += a;
                if (sum >= n)
                {
                    sum -= n;
                }
            }
            return sum;

        }


        bool multi(int n, int m)
        {
            int v1 = 0, v2 = m;
            int p1 = 1, p2 = n;
            int q = v2 / p2;



            while (p2 != 1)
            {
                int temp1 = p1;
                int temp2 = p2;

                p1 = v1 - p1 * q;
                p2 = v2 - p2 * q;

                v1 = temp1;
                v2 = temp2;

                q = v2 / p2;

            }

            int mm = p1;

            if (mm < 0)
            {
                mm = mm + m;
            }

            multInv = mm;
            return true;
        }

        public int PoiskP()
        {
            Random random = new Random();
            int p = 0;
            bool f = false;

            do
            {
                p = random.Next(1000, 2500);

                for (int i = 2; i != p; i++)
                {
                    if (i == p - 1)
                    {
                        f = PoiskG(p, p - 1);
                        break;
                    }

                    if (p % i == 0)
                    {
                        break;
                    }
                }
            }
            while (f == false);
            return p;
        }

        public bool PoiskG(int p, int g_)
        {
            bool f = false;

            List<BigInteger> number = new List<BigInteger>();

            BigInteger integer = ((BigInteger.Pow(g_, 1)) % p);
            number.Add(integer);

            for (int i = 2; i != p; i++)
            {
                integer = BigInteger.Pow(g_, i) % p;
                for (int j = 0; j != i - 1; j++)
                {
                    if (number[j] == integer)
                    {
                        g_--;
                        number.Clear();
                        i = 1;
                        integer = BigInteger.Pow(g_, 1) % p;
                        number.Add(integer);
                        break;
                    }

                    if ((j == i - 2) && (number[j] != integer))
                    {
                        number.Add(integer);
                    }
                }
            }
            g = g_;
            f = true;
            return f;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (textBox1.Text == " " || textBox2.Text == " ") return;

            originText = null;
            cryptedText = null;
            decryptedText = null;
            textBox3.Text = null;
            textBox4.Text = null;

            try
            {
                x = Convert.ToInt32(textBox1.Text);
                originText = textBox2.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            p = PoiskP();

            if (x >= p)
            {
                MessageBox.Show("Неправильно выбрано число x.");
                return;
            }

            y = VozvVStep(g, x, p);


            Console.WriteLine("p = " + p);
            Console.WriteLine("g = " + g);
            Console.WriteLine("y = " + y);

            if (originText.Length > 0)
            {
                char[] temp = new char[originText.Length - 1];
                temp = originText.ToCharArray();

                for (int i = 0; i <= originText.Length - 1; i++)
                {
                    int m = (int)temp[i];
                    if (m > 0)
                    {
                        k = rand.Next() % (p - 2) + 1;

                        Console.WriteLine((i + 1) + " пара k = " + k);
                        a = VozvVStep(g, k, p);
                        b = Ymnoz(VozvVStep(y, k, p), m, p);
                        cryptedText = cryptedText + a + ' ' + b + ' ';
                    }

                }
                textBox3.Text = cryptedText;
            }

            if (cryptedText == null) return;
            string[] strA = cryptedText.Split(' ');

            if (strA.Length > 0)
            {

                //for (int i = 0; i < strA.Length - 1; i += 2)
                for (int i = 0; i < strA.Length - 1;)
                {

                    a = Convert.ToInt32(strA[i]);
                    b = Convert.ToInt32(strA[i + 1]);

                    if ((a != 0) && (b != 0))
                    {

                        multi(VozvVStep(a, x, p), p);
                        int dec = Ymnoz(b, multInv, p);
                        char m = (char)dec;
                        decryptedText = decryptedText + m;

                    }
                }
                textBox4.Text = decryptedText;

            }


        }
    }
}
