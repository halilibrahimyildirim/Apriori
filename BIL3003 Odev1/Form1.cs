using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BIL3003_Odev1
{
    public struct FrequentItem
    {
        public string name;
        public int minSupportCount;
        public float measure;
        public FrequentItem(string name, int minSupportCount, float measure)
        {
            this.name = name;
            this.minSupportCount = minSupportCount;
            this.measure = measure;
        }
    }
    public partial class Form1 : Form
    {
        static bool[][] tabularMatrix;
        List<string> diffSets;
        List<int> diffSetToAttr;
        string[] attributes;
        int diffSetCount;
        static int dataLength;
        static FileStream fs;
        static StreamWriter sw;
        public Form1()
        {
            InitializeComponent();
        }
        private static string Mod(string[][] data, int attrIndex, int dataLength)//Nominal veri missing value
        {
            int max = 0, i = 0, k = 0;
            string[] x = new string[dataLength];
            for (i = 0; i < dataLength; i++)
            {
                x[i] = data[i][attrIndex];
            }
            Array.Sort(x);
            i = 0;
            List<int> counts = new List<int>();
            while (i < dataLength)
            {
                int say = Array.FindAll(x, delegate (string j) { return j == x[i]; }).Length;
                counts.Add(say);
                if (say > max && x[i] != "?")
                    max = say;
                i += say;
            }
            i = 0;
            while (i < dataLength)
            {
                if (counts[k] == max)
                    return x[i];
                i += counts[k];
                k++;
            }
            return "";
        }
        private static string Xbar(string[][] data, int attrIndex, int dataLength)//Nümerik veri missing value
        {
            int sum = 0, count = 0;
            float avg;
            int i, parsed;
            for (i = 0; i < dataLength; i++)
            {
                if (Int32.TryParse(data[i][attrIndex], out parsed))
                {
                    sum += parsed;
                    count++;
                }
            }
            avg = sum / count;
            return avg.ToString();
        }
        private static string errorUpdate(string k)//6. ve 11. özelliklerde aynı altözellikler söz konusu(No,No) düzeltmek için yapılan fonksiyon,detaylı açıklama word dosyasında var.
        {
            if (k == "No1")
                return "No";
            if (k == "Yes1")
                return "Yes";
            return k;
        }
        private static void DataTransformation(string[][] data, Dictionary<string, int[]> Bins, int attrIndex, int dataLength, string binName)//Eşit aralıklı kutulama
        {
            int i, max = Int32.MinValue, min = Int32.MaxValue;
            int val;
            for (i = 0; i < dataLength; i++)
            {
                val = Int32.Parse(data[i][attrIndex]);
                if (val > max)
                    max = val;
                if (val < min)
                    min = val;
            }
            int n = 3;
            if (max > 1000 && max <= 4000)
                n = 5;
            if (max > 4000 && max <= 7000)
                n = 7;
            if (max > 7000)
                n = 10;
            int width = (Int32)Math.Round((max - min) * 1.0 / n);
            int binStart = min;
            for (i = 0; i < n; i++)
            {
                Bins[binName + (i + 1)] = new int[] { binStart, binStart + width };
                binStart = binStart + width + 1;
            }
        }
        private static float MinSupport(int index)
        {
            int i;
            float sum = 0;
            for (i = 0; i < dataLength; i++)
                sum += (tabularMatrix[index][i] == true) ? 1 : 0; ;
            return sum / dataLength;
        }
        private static float MinSupport(int[] indexes)
        {
            int i, j;
            float sum = 0;
            bool isTrue;
            for (j = 0; j < dataLength; j++)
            {
                isTrue = true;
                for (i = 0; i < indexes.Length; i++)
                {
                    if (!tabularMatrix[indexes[i]][j])
                    {
                        isTrue = false;
                        break;
                    }
                }
                if (isTrue)
                    sum += 1;

            }
            return sum / dataLength;
        }
        private static float Confidence(string numerator, string denominator, List<FrequentItem> frequentItemSets)
        {
            float num = 0, denom = 0;
            //C1,B1 gibi gelen durumları tekrardan B1,C1 gibi yapmak için aşağıdaki bazı işlemler uygulanmıştır
            //Çünkü elimizdeki frequent item setlerde B1,C1 var C1,B1 yok
            string[] tempNumer = numerator.Split(',');
            string[] tempDenom = denominator.Split(',');
            Array.Sort(tempNumer);
            Array.Sort(tempDenom);
            numerator = String.Join(",", tempNumer);
            if (numerator[0] == ',')
            {
                string tempNum = "";
                for (int iter = 1; iter < numerator.Length; iter++)
                    tempNum += numerator[iter];
                numerator = tempNum;
            }
            denominator = String.Join(",", tempDenom);
            foreach (FrequentItem item in frequentItemSets)
            {
                //Bazı özelliklerin alabildiği değerlerden dolayı yukarıdaki öğe sıralama işleminin burada da yapılması gerekti
                string[] tempName = item.name.Split(',');
                Array.Sort(tempName);
                string name = String.Join(",", tempName);
                if (name == denominator)
                    denom = (float)(item.minSupportCount * 1.0 / dataLength);
                if (name == numerator)
                    num = (float)(item.minSupportCount * 1.0 / dataLength);
                if (num != 0 && denom != 0)
                    return num / denom;

            }
            return 0;
        }
        private static float Lift(string numerator, string denominator, List<FrequentItem> frequentItemSets)
        {
            float num = Confidence(numerator, denominator, frequentItemSets);
            foreach (FrequentItem item in frequentItemSets)
                if (item.name == denominator)
                    return (float)(num * 1.0 / (float)(item.minSupportCount * 1.0 / dataLength));
            return 0;
        }
        private static float Leverage(string befMinus, string aftMinus, List<FrequentItem> frequentItemSets)
        {
            float befMin = 0, aftMin = 1;
            foreach (FrequentItem item in frequentItemSets)
            {
                if (item.name == befMinus)
                    aftMin *= (float)(item.minSupportCount * 1.0 / dataLength);
                if (item.name == aftMinus)
                    aftMin *= (float)(item.minSupportCount * 1.0 / dataLength);
                if (item.name == befMinus + "," + aftMinus)
                {
                    befMin = (float)(item.minSupportCount * 1.0 / dataLength);
                    return befMin - aftMin;
                }
            }
            return 0;
        }
        private static void FindRules(List<FrequentItem> frequentItemSets, int measure, float minimumMeasure, ListBox Rules, List<string> diffSets, List<int> diffSetToAttr, string[] attr, Label RulesCount)
        {
            if (frequentItemSets.Count == 0) return;
            int end = frequentItemSets.Count, i, j, k, subIndex, len;
            List<FrequentItem> tempSet = new List<FrequentItem>();
            for (i = 0; i < end; i++)
            {
                subIndex = 0;
                string[] tempArr = frequentItemSets[i].name.Split(',');
                len = (tempArr).Length;
                string[] subSets = new string[(int)(Math.Pow(2, len) - 2)];
                for (j = 0; j < i; j++)//tüm alt kümeleri hesaplamak yerine zaten şu ana kadar hesapladığımız frequent item setlerde arama yapıp orada buluyoruz
                {
                    string[] tempArr2 = frequentItemSets[j].name.Split(',');
                    bool isContain = true;
                    for (k = 0; k < tempArr2.Length; k++)
                    {
                        if (!tempArr.Contains(tempArr2[k]))
                        {
                            isContain = false;
                            break;
                        }
                    }
                    if (isContain)//Subset bulundu
                    {
                        subSets[subIndex] = frequentItemSets[j].name;
                        subIndex++;
                    }
                }
                for (j = 0; j < subIndex; j++)//Tüm subsetler kendi içinde kural oluşturmaya çalışır
                {
                    for (k = 0; k < subIndex; k++)
                    {
                        float measureVal = 0;
                        string[] subK = subSets[k].Split(',');
                        string[] subJ = subSets[j].Split(',');
                        bool isContain = false;
                        foreach (string str1 in subK)//subset[k] ile subset[j] içinde çakışma var mı kontrolü
                        {
                            isContain = false;
                            foreach (string str2 in subJ)
                            {
                                if (str2 == str1)
                                {
                                    isContain = true;
                                    break;
                                }
                            }
                            if (isContain)
                                break;
                        }
                        if (!isContain)//if subsets[k] then subsets[j] kontrolü
                        {
                            string measureName = "";
                            switch (measure)
                            {
                                case 0:
                                    measureVal = Confidence(subSets[j] + "," + subSets[k], subSets[k], frequentItemSets);
                                    measureName = "Confidence";
                                    break;
                                case 1:
                                    measureVal = Lift(subSets[j] + "," + subSets[k], subSets[k], frequentItemSets);
                                    measureName = "Lift";
                                    break;
                                case 2:
                                    measureVal = Leverage(subSets[k], subSets[j], frequentItemSets);
                                    measureName = "Leverage";
                                    break;
                            }
                            if (measureVal >= minimumMeasure)//İkincil ölçütün değeri istenilen sınırın üstündeyse kural oluşturulur
                            {
                                string[] ruleSide1 = subSets[k].Split(',');
                                int ruleSide1Length = ruleSide1.Length;
                                string temp = "IF ";
                                string name;
                                for (int iter = 0; iter < ruleSide1Length - 1; iter++)
                                {
                                    name = errorUpdate(ruleSide1[iter]);
                                    temp += attr[diffSetToAttr[diffSets.IndexOf(ruleSide1[iter])]] + " = " + name + " AND ";
                                }
                                name = errorUpdate(ruleSide1[ruleSide1Length - 1]);
                                temp += attr[diffSetToAttr[diffSets.IndexOf(ruleSide1[ruleSide1Length - 1])]] + " = " + name;
                                temp += " THEN ";
                                string[] ruleSide2 = subSets[j].Split(',');
                                int ruleSide2Length = ruleSide2.Length;
                                for (int iter = 0; iter < ruleSide2Length - 1; iter++)
                                {
                                    name = errorUpdate(ruleSide2[iter]);
                                    temp += attr[diffSetToAttr[diffSets.IndexOf(ruleSide2[iter])]] + " = " + name + " AND ";
                                }
                                name = errorUpdate(ruleSide2[ruleSide2Length - 1]);
                                temp += attr[diffSetToAttr[diffSets.IndexOf(ruleSide2[ruleSide2Length - 1])]] + " = " + name;
                                bool isDup = false;
                                foreach (FrequentItem item in tempSet)//Daha önceden eklenilmiş bir kural mı kontrolü
                                {
                                    isDup = true;
                                    int befSupp = item.name.IndexOf("   Support");
                                    for (int iter = 0; iter < befSupp; iter++)
                                        if (item.name[iter] != temp[iter])
                                        {
                                            isDup = false;
                                            break;
                                        }
                                    if (isDup)
                                        break;
                                }
                                if (isDup)
                                    continue;
                                temp += "   " + "Support:" + (1.0 * frequentItemSets[i].minSupportCount / dataLength) + "   " + measureName + ":" + Math.Round(measureVal, 3);
                                tempSet.Add(new FrequentItem(temp, frequentItemSets[i].minSupportCount, measureVal));
                            }
                        }
                    }
                }
            }
            tempSet = tempSet.OrderBy(item => item.measure).ThenBy(item => item.minSupportCount).ToList();
            int start1 = tempSet.Count - 1;
            sw.WriteLine("Rules" + "      Rules Count=" + (start1 + 1));
            for (i = start1; i >= 0; i--)//Kuralları yazdırır
            {
                sw.WriteLine(tempSet[i].name);
                Rules.Items.Add(tempSet[i].name);
            }
            RulesCount.Text += start1 + 1;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string filePath = "";
            string fileName = "ISBSG-raw-data-v2.txt";
            string checkName = "";
            bool exit = false;
            do//Dosya seçim işlemi
            {
                try
                {
                    OpenFileDialog file = new OpenFileDialog();
                    file.Title = "Data Dosyasını seçiniz";
                    var res = file.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        filePath = file.FileName;
                        checkName = file.SafeFileName;
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        exit = true;
                        break;
                    }
                    if (checkName != fileName)
                        throw new FileNameException("Dosyanın adını değiştirdiyseniz bu hata ile karşılaşıyor olabilirsiniz");
                    else
                        break;
                }
                catch (FileNameException error)
                {
                    MessageBox.Show(error.Message, "Doğru dosyayı seçiniz");
                }
            } while (true);
            if (exit)//Openfiledialogda çıkış yapılmak istenirse
                Application.Exit();
            string[] lines = File.ReadAllLines(filePath);//data okunur
            attributes = lines[0].Split(',');
            dataLength = lines.Length - 1;
            int attrLength = attributes.Length;
            string[][] data = new string[dataLength][];
            string[] missingReplace = new string[attrLength];
            int i, j;
            for (i = 0; i < dataLength; i++)//data parçalanır
            {
                data[i] = lines[i + 1].Split(',');
            }
            bool[] isNumeric = new bool[] { false, true, true, true, false, false, false, false, false, false, false, false };//Nümerik veriler belirlenir
            //(Resource Level sayısal bir özellik olsa bile ortalaması yerine modunun alınması daha doğru olacak gibi geldi)
            for (j = 0; j < attrLength; j++)//Kayıp verilerin hangi değerler ile doldurulması gerektiği bulunur
            {
                if (!isNumeric[j])
                    missingReplace[j] = Mod(data, j, dataLength);
                else
                    missingReplace[j] = Xbar(data, j, dataLength);
            }
            for (i = 0; i < dataLength; i++)
                for (j = 0; j < attrLength; j++)//kayıp veriler doldurulur
                    if (data[i][j] == "?")
                        data[i][j] = missingReplace[j];
            for (i = 0; i < dataLength; i++)//errorUpdate
                data[i][10] += "1";
            Dictionary<string, int[]>[] Bins = new Dictionary<string, int[]>[3];//Yeni nominal değerlerin nümerik aralıklarını tutmak için
            string[] binNames = new string[] { "A", "B", "C" };
            int numTemp;
            string[][] numAttrTemps = new string[3][];//Nümerik verileri dönüştürmeden önce yedeklemek için
            for (j = 1; j < 4; j++)//Nümerik veriler nominal hale getirilir
            {
                Bins[j - 1] = new Dictionary<string, int[]>();
                DataTransformation(data, Bins[j - 1], j, dataLength, binNames[j - 1]);
                numAttrTemps[j - 1] = new string[dataLength];
                for (i = 0; i < dataLength; i++)
                {
                    numAttrTemps[j - 1][i] = data[i][j];//Nümerik veriler yedeklenir

                    foreach (KeyValuePair<string, int[]> bin in Bins[j - 1])
                    {
                        numTemp = Int32.Parse(data[i][j]);

                        if (numTemp >= bin.Value[0] && numTemp <= bin.Value[1])//değerin hangi aralığın içinde kaldığı bulunur
                        {
                            data[i][j] = bin.Key;
                            break;
                        }
                    }
                }
            }
            for (j = 1; j < 4; j++)//Yeni nominal verilerin adları ve aralıkları yazdırılır
            {
                BinRanges.Items.Add(attributes[j]);
                foreach (KeyValuePair<string, int[]> bin in Bins[j - 1])
                    BinRanges.Items.Add("- " + bin.Key + "  [" + bin.Value[0] + "," + bin.Value[1] + "]");
                BinRanges.Items.Add("\n");
            }
            diffSets = new List<string>();
            diffSetToAttr = new List<int>();
            diffSetCount = 0;
            for (i = 0; i < attrLength; i++)
            {
                for (j = 0; j < dataLength; j++)
                {
                    if (!diffSets.Contains(data[j][i]))//Kaç farklı özellik olduğu bulunur ve depolanır
                    {
                        diffSets.Add(data[j][i]);
                        diffSetToAttr.Add(i);
                        diffSetCount++;
                    }
                }
            }
            tabularMatrix = new bool[diffSetCount][];
            for (i = 0; i < diffSetCount; i++)
                tabularMatrix[i] = new bool[dataLength];
            for (i = 0; i < dataLength; i++)
            {
                for (j = 0; j < attrLength; j++)//Tabular matrisi doldurulur(var-yok matrisi)
                {
                    tabularMatrix[diffSets.IndexOf(data[i][j])][i] = true;
                }
            }
            FolderBrowserDialog results = new FolderBrowserDialog();
            string resultFilePath = "";
            resultFilePath = results.SelectedPath;
            fs = new FileStream(resultFilePath + "Results.txt", FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs);
            MessageBox.Show("Minimum support değerini çok küçük girerseniz ciddi bekleme süreleri ile karşılaşabilirsiniz.\n(Tavsiye edilen en düşük minimum support değeri ~=0.5)", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void Start_Click(object sender, EventArgs e)
        {
            sw.WriteLine("");
            FreqItemSets.Items.Clear();
            Rules.Items.Clear();
            RulesCount.Text = "Rules Count = ";
            float minSupport = -1;
            float measure = -1;
            int measureValidation = -1;
            bool minSupValidation = false;
            float val1;
            List<FrequentItem> frequentItemSets = new List<FrequentItem>();
            try//Minimum support değeri okunur
            {
                if (float.TryParse(support.Text, out val1))
                    minSupport = val1;
                else
                    throw new FormatException("Giriş formatınız doğru değildir");
                if (minSupport < 0 || minSupport > 1)
                    throw new ValueRangeException("Girilen minimum support değeri [0,1] aralığında olmalıdır.(Nokta ile giriş yaptıysanız bu hata ile karşılaşıyor olabilirsiniz)");
                else
                    minSupValidation = true;
            }
            catch (ValueRangeException error)
            {
                support.Text = "";
                MessageBox.Show(error.Message, "Doğru aralıkta minimum support değeri giriniz");
            }
            catch (FormatException error)
            {
                support.Text = "";
                MessageBox.Show(error.Message, "Sayı giriniz");
            }
            if (minSupValidation)
            {
                try//İkincil ölçüm birimi okunur
                {
                    if (float.TryParse(measureValue.Text, out val1))
                        measure = val1;
                    else
                        throw new FormatException("Giriş formatınız doğru değildir");
                    if (measures.SelectedIndex == 0)
                        if ((measure < 0 || measure > 1))
                            throw new ValueRangeException("Girilen confidence değeri [0,1] aralığında olmalıdır.(Nokta ile giriş yaptıysanız bu hata ile karşılaşıyor olabilirsiniz)");
                        else
                            measureValidation = 0;
                    else if (measures.SelectedIndex == 1)
                        if (measure < 0)
                            throw new ValueRangeException("Girilen lift değeri [0,∞] aralığında olmalıdır.(Nokta ile giriş yaptıysanız bu hata ile karşılaşıyor olabilirsiniz)");
                        else
                            measureValidation = 1;
                    else if (measures.SelectedIndex == 2)
                        if (measure < -0.25 || measure > 0.25)
                            throw new ValueRangeException("Girilen leverage değeri [-0.25,0.25] aralığında olmalıdır.(Nokta ile giriş yaptıysanız bu hata ile karşılaşıyor olabilirsiniz)");
                        else
                            measureValidation = 2;
                    else
                        throw new SelectionException("3 ölçüden 1 tanesini seçmeniz gerekmektedir.");

                }
                catch (ValueRangeException error)
                {
                    measureValue.Text = "";
                    MessageBox.Show(error.Message, "Seçilen ölçünün değerini doğru giriniz");
                }
                catch (SelectionException error)
                {
                    MessageBox.Show(error.Message, "Ölçü seçiniz");
                }
                catch (FormatException error)
                {
                    measureValue.Text = "";
                    MessageBox.Show(error.Message, "Sayı giriniz");
                }
                if (measureValidation != -1)
                {
                    sw.WriteLine("Frequent Item Sets and Counts");
                    int i, j, startIndex = 0;
                    for (i = 0; i < diffSetCount; i++)
                    {
                        float val = MinSupport(i);
                        if (val >= minSupport)//Minimum support değerinden büyük 1 elemanlı frequent item setler bulunur
                            frequentItemSets.Add(new FrequentItem(diffSets[i], (int)(val * dataLength), 0));
                    }
                    int freqItemCount = frequentItemSets.Count();
                    int subSetCount1Elem = freqItemCount;
                    while (startIndex != freqItemCount)//Bir döngüde hiç bir frequent item set bulamadıysa durma koşulu
                    {
                        string[] temp;
                        int tempLen;
                        for (i = startIndex; i < freqItemCount; i++)//Minimum support değerinden büyük 1 den fazla elemanlı frequent item setler bulunur
                        {
                            temp = frequentItemSets[i].name.Split(',');
                            tempLen = temp.Length;
                            int[] indexes = new int[tempLen + 1];
                            for (j = 0; j < subSetCount1Elem; j++)
                            {
                                if (!frequentItemSets[i].name.Contains(frequentItemSets[j].name))//Aynı özellik yoksa çalışır
                                {
                                    bool breaker = false;
                                    foreach (FrequentItem item in frequentItemSets)//A1,B1 = B1,A1  durumlarını silmek için
                                    {
                                        bool isSame = true;
                                        string[] tempArr = item.name.Split(',');
                                        string[] tempArr2 = (frequentItemSets[i].name + "," + frequentItemSets[j].name).Split(',');
                                        int len1 = tempArr.Length;
                                        int len2 = tempArr2.Length;
                                        if (len1 != len2)
                                            continue;
                                        Array.Sort(tempArr);
                                        Array.Sort(tempArr2);
                                        for (int iter = 0; iter < len1; iter++)
                                            if (tempArr[iter] != tempArr2[iter])
                                            {
                                                isSame = false;
                                                break;
                                            }
                                        if (isSame)
                                        {
                                            breaker = true;
                                            break;
                                        }

                                    }
                                    if (breaker)//A1,B1 ve B1,A1 gelince atlamak için
                                        continue;
                                    for (int p = 0; p < tempLen; p++)//Birden fazla özellik olduğundan indisleri dizide tutarız
                                    {
                                        indexes[p] = diffSets.IndexOf(temp[p]);
                                    }
                                    indexes[tempLen] = diffSets.IndexOf(frequentItemSets[j].name);
                                    float val = MinSupport(indexes);
                                    if (val >= minSupport)
                                        frequentItemSets.Add(new FrequentItem(frequentItemSets[i].name + "," + frequentItemSets[j].name, (int)(val * dataLength), 0));

                                }
                            }
                        }
                        startIndex = freqItemCount;
                        freqItemCount = frequentItemSets.Count();
                    }
                    for (i = 0; i < freqItemCount; i++)
                    {
                        string name="";
                        string[] tempName = frequentItemSets[i].name.Split(',');
                        for (j = 0; j < tempName.Length - 1; j++)
                            name += errorUpdate(tempName[j])+",";
                        name += errorUpdate(tempName[tempName.Length - 1]);
                        FreqItemSets.Items.Add(name + "        " + frequentItemSets[i].minSupportCount);
                        sw.WriteLine(name + "        " + frequentItemSets[i].minSupportCount);
                    }
                    FindRules(frequentItemSets, measureValidation, measure, Rules, diffSets, diffSetToAttr, attributes, RulesCount);//Kurallar bulunur
                }
            }
        }
    }
}
