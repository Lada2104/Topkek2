using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace prokekt
{
    public partial class Form3 : Form
    {
        string site = Form4.site;
        string find = Form4.find;
        int skip = Form4.skip;
        bool entire = Form4.entire;
        
        string head, body, allT;
        string HTML_Code = "";
        public Form3()
        {
            InitializeComponent();            
            comboBox1.Items.Add("Yandex");
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form f2 = new Form2();
            f2.Show();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {         
                site = "http://news.yandex.ru";
                Connect con = new Connect();
                HTML_Code = con.Con(site);
                ////////////////////////////
                search src1 = new search();
                find = "<h2 class=\"story>";
                skip = 1;
                entire = false;
                head = src1.ser(HTML_Code, skip, entire, find);     //HEAD
                ///////////////////////////
                search src = new search();
                find = "<div class=\"story__text\">";
                skip = 0;
                entire = true;
                body = src.ser(HTML_Code, skip, entire, find);      //BODY
                allT = head + body;                                   //Заголовок + новость
                ///////////////////////////
                piece_of_news pon = new piece_of_news();
                richTextBox1.Text = pon.PoN(HTML_Code, allT);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }

    }
}


//public class myForm: Form
//{
//public string site{get; set; }
//}


    public class Connect : search
    {
        public Connect()
        {

        }
        public string Con(string sURL)
        {
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);
            string sLine1 = "";
            for (int i = 0; i <= 3; i++)
                sLine1 += objReader.ReadToEnd();
            return sLine1;
        }
    }

    public class search : piece_of_news
    {
        string check = "";
        int i = 0;
        int j;
        int k = 0;
        int Count = 1;
        string story = "";
        string a;
        public search()
        {

        }
        public string ser(string sLine1, int skip, bool entire, string find)
        {
            for (i = 0; i < sLine1.Length; i++)
            {
                if (sLine1[i] == '<')
                {
                    j = i;
                    while (sLine1[j] != '>')
                    {
                        check += sLine1[j];
                        j++;
                    }
                    check += ">";
                    i = j - 1;
                    if (entire == false && find.Length < check.Length)
                    {
                        for (int iter = 0; iter < find.Length - 1; iter++) //-1 чтобы не брать последнюю скобку ">"
                        {
                            a += check[iter];
                        }
                        check = a + ">";
                        a = "";
                    }
                }
                else
                {
                    if (check != "")
                    {
                        if (check == find && Count < 6)
                        {
                            k = i + 1;
                            for (int iter = 0; iter <= skip; iter++)
                            {
                                if (skip == iter)
                                {
                                    Count++;

                                    while (sLine1[k] != '<')
                                    {
                                        story += sLine1[k];
                                        k++;
                                    }
                                }
                                else
                                {
                                    while (sLine1[k] != '>')
                                    {
                                        k++;
                                    }
                                    k++;
                                    check = "";
                                }
                                check = "";

                            }
                            story += "\n";
                        }
                        else
                        {
                            check = "";
                        }
                    }
                }
            } return (story);
        }
    }

    public class piece_of_news 
    {
        public piece_of_news()
        {
        }
        public string PoN(string HTML_Code, string allT)
        {
            int N = 10;    
            int i = 1;
            int iter = 0;
            int iter1 = iter + 1;            
            string copy = "";
            int n = N / 2;
            int[] arr = new int[N];
            iter = n;
            iter = 1;
            for (i = 0; i < N; i++)
            {
                arr[i] = iter;
                arr[i + 1] = iter + n;
                i++;
                iter++;
            }
            i = 0;
            int i1 = 0;
            int i2 = 0;
            int k = 0;
            iter = 0;
            iter1 = 0;
            //////////////////////////////////////////////////////
            copy = "";
            for (int kek = 0; kek < N; kek++)
            {
                for (iter = 0; iter < allT.Length; iter++)
                {
                    if (allT[iter] + i2 == '\n')
                    {
                        if (arr[i1] == i + 1)
                        {
                            k = iter;
                            while (k != 0)
                            {
                                if (allT[k - 1] != allT[iter])
                                {
                                    k--;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            for (k = k; k <= iter; k++)
                            {
                                copy += allT[k];
                            }                            
                            i1++;
                            iter++;
                            i2 = 1;

                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                i = 0;
                i2 = 0;
                copy += "\n";
            }
            return copy;
        }       
    } 