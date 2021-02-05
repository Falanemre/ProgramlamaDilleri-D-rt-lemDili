using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace progDillerBüt
{

    public partial class Form1 : Form
    {
    
        public static int evaluate(string expression) //programlama dilimde kod ekranı kısmındaki bütün kodlar birleştirilir ve hesaplancıcak olan string infix biçiminde birleştirilir ve buraya değer olarak verilir burdada sonuç işlem önceliğine göre bulunur ve program tamamlanır.
        {
            char[] tokens = expression.ToCharArray();

            // Stack for numbers: 'values' 
            Stack<int> values = new Stack<int>();

            // Stack for Operators: 'ops' 
            Stack<char> ops = new Stack<char>();

            for (int i = 0; i < tokens.Length; i++)
            {
                // Current token is a whitespace, skip it 
                if (tokens[i] == ' ')
                {
                    continue;
                }

                // Current token is a number, 
                // push it to stack for numbers 
                if (tokens[i] >= '0' && tokens[i] <= '9')
                {
                    StringBuilder sbuf = new StringBuilder();

                    // There may be more than 
                    // one digits in number 
                    while (i < tokens.Length &&
                            tokens[i] >= '0' &&
                                tokens[i] <= '9')
                    {
                        sbuf.Append(tokens[i++]);
                    }
                    values.Push(int.Parse(sbuf.ToString()));

                    // Right now the i points to 
                    // the character next to the digit,
                    // since the for loop also increases 
                    // the i, we would skip one 
                    //  token position; we need to 
                    // decrease the value of i by 1 to
                    // correct the offset.
                    i--;
                }

                // Current token is an opening 
                // brace, push it to 'ops' 
                else if (tokens[i] == '(')
                {
                    ops.Push(tokens[i]);
                }

                // Closing brace encountered, 
                // solve entire brace 
                else if (tokens[i] == ')')
                {
                    while (ops.Peek() != '(')
                    {
                        values.Push(applyOp(ops.Pop(),
                                         values.Pop(),
                                        values.Pop()));
                    }
                    ops.Pop();
                }

                // Current token is an operator. 
                else if (tokens[i] == '+' ||
                         tokens[i] == '-' ||
                         tokens[i] == '*' ||
                         tokens[i] == '/')
                {

                    // While top of 'ops' has same 
                    // or greater precedence to current 
                    // token, which is an operator. 
                    // Apply operator on top of 'ops' 
                    // to top two elements in values stack 
                    while (ops.Count > 0 &&
                             hasPrecedence(tokens[i],
                                         ops.Peek()))
                    {
                        values.Push(applyOp(ops.Pop(),
                                         values.Pop(),
                                       values.Pop()));
                    }

                    // Push current token to 'ops'. 
                    ops.Push(tokens[i]);
                }
            }

            // Entire expression has been 
            // parsed at this point, apply remaining 
            // ops to remaining values 
            while (ops.Count > 0)
            {
                    values.Push(applyOp(ops.Pop(),
                                 values.Pop(),
                                values.Pop()));
            }

            // Top of 'values' contains 
            // result, return it 
            return values.Pop();
        }

        // Returns true if 'op2' has 
        // higher or same precedence as 'op1', 
        // otherwise returns false. 
        public static bool hasPrecedence(char op1,     // infix çözümünün devamı.
                                         char op2)
        {
            if (op2 == '(' || op2 == ')')
            {
                return false;
            }
            if ((op1 == '*' || op1 == '/') &&
                   (op2 == '+' || op2 == '-'))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // A utility method to apply an 
        // operator 'op' on operands 'a'  
        // and 'b'. Return the result. 
        public static int applyOp(char op,        // infix çözümünün devamı.
                                int b, int a)
        {
            switch (op)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    if (b == 0)
                    {
                        throw new
                        System.NotSupportedException(
                               "Cannot divide by zero");
                    }
                    return a / b;
            }
            return 0;
        }

        int derleyici = 0;  // eğer sıfır olursa derşeyici sorunsuz olmazsa program çalışmaz hatalar derleyici kısmında görünür.
        int buton = 0;    // derleyicide = bulduktan sonra bazı kodların çalışmaması için eklenmiştir.
        string atılcakDegIsmı; //for döngüsüyle değişken ismi char olarak toplanır (=) görününce toplanma durur ve "degIsmı" listesine eklenir.
        string atılcakDeg;     //for döngüsüyle değişken değeri char olarak toplanır (;) görününce toplanma durur ve integer değere dönüştürülüp "degerler" listesine eklenir.
        static int sayac = 0;   
        static int ısSayac = 0; // işaret sayaacı
        List<double> degerler = new List<double>();       // programlama dilinde tanımlanan değerler tutulur.
        List<string> degIsmı = new List<string>();  // programlama dilinde tanımlanan değişken isimleri tutulur.
        List<string> ısaret = new List<string>();   // programlama dilinde tanımlanan işaretler tutulur.
        string sonucc = "";   //en sonunda çıkan sonuç budur messageBox ile gösterilir.
        int noktalıVırgulSay = 0;  //derleyicide kontrol amaçlı noktalı virgül sayar.
        int essittirSay = 0;  // derleyicide kontrol amaçlı eşittir sayar.
        int derleIsaretKontrol = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = ""; // başlarken derleyiciyi temizlyorum çünkü daha önceki hatalar kalıyor.
            string a = "";
            a = richTextBox1.Text.ToString();
            for (int i = 0; i < a.Length; i++)//kodların derlendiği ve çalıştırıldığı döngü burada iflerle girilen kodlar kontrol ediliyor ve doğru listelere ekleniyor.
            {
                char karakter = a[i];
                if (karakter == ';') //for ile noktalı virgül aranıyor eğer noktalı virgül bulunursa değişken ismi ve değeri ayıklanıp belirtilen liste yapılarına ekleniyor.
                {
                    noktalıVırgulSay++;
                    degIsmı.Add(atılcakDegIsmı);
                    bool karr = true;
                    for (int m = 0; m < atılcakDeg.Length; m++)
                    {
                        if (atılcakDeg[m].ToString() =="0" || atılcakDeg[m].ToString() == "1" || atılcakDeg[m].ToString() == "2" || atılcakDeg[m].ToString() == "3" || atılcakDeg[m].ToString() == "4" || atılcakDeg[m].ToString() == "5" || atılcakDeg[m].ToString() == "6" || atılcakDeg[m].ToString() == "7" || atılcakDeg[m].ToString() == "8" || atılcakDeg[m].ToString() == "9")
                        {
                            karr = true;
                        }
                        else
                        {
                            karr = false;
                            break;
                        }
                    }
                    if (karr == true)
                    {
                        degerler.Add(Convert.ToDouble(atılcakDeg));
                    }
                    else
                    {
                        derleyici++;
                        richTextBox2.Text = richTextBox2.Text + (derleyici + ".) Değişken yanlış lütfen düzeltiniz.\n");
                        label4.Text = derleyici.ToString();
                    }
                    sonucc = sonucc + atılcakDeg.ToString();
                    sayac++;
                    atılcakDegIsmı = "";
                    atılcakDeg = "";
                    buton = 0;
                    derleIsaretKontrol = 0;
                    continue;
                    
                }
                if (karakter =='=') // burada eşittir aranıyor eğer eşittir görünürse programlama dilimdeki değişken tanımlamasına göre(a=2;) eşittirden sonra değer geliyor bu yüzden kontrol ediliyor.
                {
                    buton = 1;
                    essittirSay++;
                    continue;
                }
                if(buton==1)
                {
                    atılcakDeg += a[i];
                }
                if (karakter =='+' || karakter=='-'|| karakter=='*' || karakter == '/') // kod ekranı kısmında yazan kodlar derlenir ve işlemler işlem dizisine eklenir.
                {
                        string ısaretAt = karakter.ToString();
                        ısaret.Add(ısaretAt);
                        sonucc = sonucc + ısaretAt;
                        ısSayac++;
                        i = i + 2;
                        derleIsaretKontrol++;
                }
                if (buton == 0)
                {
                    try
                    {
                        atılcakDegIsmı += a[i];
                    }
                    catch
                    {
                        derleyici++;
                        richTextBox2.Text = richTextBox2.Text + (derleyici + ".) eksik değişken tanımladınız lütfen düzeltin.\n");
                        label4.Text = derleyici.ToString();
                    }
                }
                
            }
            for (int i = 0; i < sonucc.Length; i++)//derleyici 0 a bölünme hatası.
            {
                if(sonucc[i].ToString()=="/" && sonucc[i + 1].ToString() == "0")
                {
                    derleyici++;
                    richTextBox2.Text =richTextBox2.Text + (derleyici+ ".) '0' a bölme hatası lütfen paydadaki değişkeni düzeltin.\n");
                    label4.Text = derleyici.ToString();
                }
            }
            for (int i = 0; i < degIsmı.Count; i++)// derşeyici değişken ismi hatası (aynı isimden varsa).
            { 
                for (int j = i+1; j < degIsmı.Count; j++)
                {
                    string bır = degIsmı[i].Replace("\n", "").Replace("\r", "");
                    string ıkı = degIsmı[j].Replace("\n", "").Replace("\r", "");
                    if(bır == ıkı)
                    {
                        derleyici++;
                        richTextBox2.Text =richTextBox2.Text + (derleyici + ".) Aynı değişken birden fazla kullanılmış.\n");
                        label4.Text = derleyici.ToString();
                    }
                }
            }
            if (degerler.Count <=1)
            {
                derleyici++;
                richTextBox2.Text = richTextBox2.Text + (derleyici + ".) Değişken eksik lütfen düzeltiniz.\n");
                label4.Text = derleyici.ToString();
            }
           if (ısaret.Count==0) //işlem girilmemişse derleyici ekranına yazar.
            {
                derleyici++;
                richTextBox2.Text = richTextBox2.Text + (derleyici + ".) Lütfen işlem giriniz.\n");
                label4.Text = derleyici.ToString();
            }
            if (essittirSay > noktalıVırgulSay)//derleyici noktalı virgül ve eşittir hatası.
            {
                derleyici++;
                richTextBox2.Text = richTextBox2.Text + (derleyici + ".) Noktalı virgül unutulmuştur lütfen kontrol edin.\n");
                label4.Text = derleyici.ToString();
                essittirSay = 0;
                noktalıVırgulSay = 0;
            }
            else if (essittirSay < noktalıVırgulSay)
            {
                derleyici++;
                richTextBox2.Text = richTextBox2.Text + (derleyici + ".) Eşittir unutulmuştur lütfen kontrol edin.\n");
                label4.Text = derleyici.ToString();
                essittirSay = 0;
                noktalıVırgulSay = 0;
            }
            else { }
            if (ısaret.Count+1 != degerler.Count)
            {
                derleyici++;
                richTextBox2.Text = richTextBox2.Text + (derleyici + ".) eksik yada fazla işlem girdiniz lütfen düzeltin.\n");
                label4.Text = derleyici.ToString();
            }
            else
            {

            }
            if (derleyici==0) // burasıda derleyiciden geçen kod sorunsuz ise çalıştırılıcak ve derleyici tekrar kod yazılması için sıfırlanıcaktır.
            {
                richTextBox2.Text = "Başarıyla çalıştırılmıştır.";
                MessageBox.Show(evaluate(sonucc).ToString(), "SONUÇ");
                ısSayac = 0;
            }
            else
            {
                derleyici = 0;
            }
            sonucc = "";  // burada yeniden kod yazılması için önceki kodlardan kalan bilgiler sıfırlanıyor.
            degIsmı.Clear();
            degerler.Clear();
            ısaret.Clear();
            sayac = 0;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        int panelButon = 0;
        private void Button1_Click(object sender, EventArgs e)
        {
            if(panelButon % 2 == 0)
            {
                richTextBox3.Visible = true;
                panelButon++;
            }
            else
            {
                richTextBox3.Visible = false;
                panelButon++;
                if(panelButon == 2) { panelButon = 0; }
            }
        }
    }
}
