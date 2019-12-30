using RDBLL.Common.Service;
using RDBLL.Entity.MeasureUnits;
using RDBLL.Entity.Results.NDM;
using RDBLL.Entity.SC.Column;
using RDBLL.Forces;
using RDBLL.Processors.SC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;


namespace RDUIL.WPF_Windows
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WndSteelColumnBase : Window
    {
        private SteelBase _steelColumnBase;

        public WndSteelColumnBase(SteelBase steelColumnBase)
        {
            InitializeComponent();
            _steelColumnBase = steelColumnBase;
            this.DataContext = _steelColumnBase;
            tbWidthMeasure.Text = MeasureUnitConverter.GetUnitLabelText(0);
            tbLengthMeasure.Text = MeasureUnitConverter.GetUnitLabelText(0);
            tbThicknessMeasure.Text = MeasureUnitConverter.GetUnitLabelText(0);
            tbSteelStrengthMeasure.Text = MeasureUnitConverter.GetUnitLabelText(3);
            tbConcreteStrengthMeasure.Text = MeasureUnitConverter.GetUnitLabelText(3);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.DialogResult = OK;
                ProgrammSettings.IsDataChanged = true;
                _steelColumnBase.IsActual = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Некорректные данные :" + ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnForces_Click(object sender, RoutedEventArgs e)
        {
            wndForces wndForces = new wndForces(_steelColumnBase);
            wndForces.ShowDialog();
        }
        private void BtnParts_Click(object sender, RoutedEventArgs e)
        {
            WndSteelBasePart wndSteelBasePart = new WndSteelBasePart(_steelColumnBase);
            wndSteelBasePart.ShowDialog();
        }

        private void BtnBolts_Click(object sender, RoutedEventArgs e)
        {
            wndSteelBaseBolts wndSteelBaseBolts = new wndSteelBaseBolts(_steelColumnBase);
            wndSteelBaseBolts.ShowDialog();
        }

        private void BtnStresses_Click(object sender, RoutedEventArgs e)
        {
            //Коллекция комбинаций нагрузок и значений по прямоугольникам
            List<LoadCaseRectangleValue> loadCaseRectangleValues = SteelBaseProcessor.GetRectangleValues(_steelColumnBase);
            //Здесь необходимо вызывать окно с построением изополей и передавать в него коллекцию комбинаций и значений

            //Коллекция комбинаций нагрузок и значений по прямоугольникам
            //List<LoadCaseRectangleValue> loadCaseRectangleValues = SteelBaseProcessor.GetRectangleValues(_steelColumnBase);
            //Здесь необходимо вызывать окно с построением изополей и передавать в него коллекцию комбинаций и значений
            //======================   Имитируем  SteelBaseProcessor.GetRectangleValues(_steelColumnBase)   ==========================
            //List<LoadCaseRectangleValue> loadCaseRectangleValues = new List<LoadCaseRectangleValue>();

            //---------------- ИзоПоле №1 <Serge>  -----------
            LoadCaseRectangleValue loadCaseRectangleValue = new LoadCaseRectangleValue();
            List<LoadSet> loadSet = new List<LoadSet>();
            LoadSet ls = new LoadSet { Id = 1, Name = "Serge" };
            loadSet.Add(ls);

            loadCaseRectangleValue.LoadCase = ls;

            List<RectangleValue> RectangleValues = new List<RectangleValue>();
            string sLine;

            // Read the file and display it line by line. 
            this.Title = "Ждём-с... Read test00.txt";
            System.IO.StreamReader file = new System.IO.StreamReader(@"test00.txt");
            while ((sLine = file.ReadLine()) != null)
            {
                RectangleValue aRV = new RectangleValue();
                int iSt = sLine.IndexOf("#");
                if (iSt >= 0)
                {
                    int iFin = sLine.IndexOf("#", iSt + 1);
                    string cX = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dX = Convert.ToDouble(cX);
                    iSt = iFin;
                    iFin = sLine.IndexOf("#", iSt + 1);
                    string cY = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dY = Convert.ToDouble(cY);
                    iSt = iFin;
                    iFin = sLine.IndexOf("#", iSt + 1);
                    string cW = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dW = Convert.ToDouble(cW);
                    iSt = iFin;
                    iFin = sLine.IndexOf("#", iSt + 1);
                    string cL = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dL = Convert.ToDouble(cL);
                    iSt = iFin;
                    iFin = sLine.IndexOf("#", iSt + 1);
                    string cV = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dV = Convert.ToDouble(cV);

                    aRV.CenterX = dX; aRV.CenterY = dY; aRV.Width = dW; aRV.Length = dL; aRV.Value = dV;

                    RectangleValues.Add(aRV);
                }
            }
            file.Close();
            loadCaseRectangleValue.RectangleValues = RectangleValues;
            //-------------------------------------------------
            //   Заносим в список
            loadCaseRectangleValues.Add(loadCaseRectangleValue);
            //---------------- ИзоПоле №2 <Clause>  -----------
            this.Title = "Ждём-с... Read test01.txt";
            loadCaseRectangleValue = new LoadCaseRectangleValue();
            loadSet = new List<LoadSet>();
            ls = new LoadSet { Id = 2, Name = "Clause" };
            loadSet.Add(ls);

            loadCaseRectangleValue.LoadCase = ls;

            RectangleValues = new List<RectangleValue>();
            // Read the file and display it line by line.  
            file = new System.IO.StreamReader(@"test01.txt");
            while ((sLine = file.ReadLine()) != null)
            {
                RectangleValue aRV = new RectangleValue();
                int iSt = sLine.IndexOf("#");
                if (iSt >= 0)
                {
                    int iFin = sLine.IndexOf("#", iSt + 1);
                    string cX = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dX = Convert.ToDouble(cX);
                    iSt = iFin;
                    iFin = sLine.IndexOf("#", iSt + 1);
                    string cY = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dY = Convert.ToDouble(cY);
                    iSt = iFin;
                    iFin = sLine.IndexOf("#", iSt + 1);
                    string cW = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dW = Convert.ToDouble(cW);
                    iSt = iFin;
                    iFin = sLine.IndexOf("#", iSt + 1);
                    string cL = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dL = Convert.ToDouble(cL);
                    iSt = iFin;
                    iFin = sLine.IndexOf("#", iSt + 1);
                    string cV = sLine.Substring(iSt + 1, iFin - iSt - 1);
                    double dV = Convert.ToDouble(cV);

                    aRV.CenterX = dX; aRV.CenterY = dY; aRV.Width = dW; aRV.Length = dL; aRV.Value = dV;

                    RectangleValues.Add(aRV);
                }
            }
            file.Close();
            loadCaseRectangleValue.RectangleValues = RectangleValues;
            //-------------------------------------------------
            //   Заносим в список
            loadCaseRectangleValues.Add(loadCaseRectangleValue);
            //-------------------------------------------------
            // MessageBox.Show("<"+loadCaseRectangleValues.Count.ToString()+">");
            //======================================================================================
            Window wndIzos = new wndIzo(loadCaseRectangleValues);
            wndIzos.Show();
        }
    }

    internal class wndIzo : Window
    {
        private List<LoadCaseRectangleValue> loadCaseRectangleValues;

        public wndIzo(List<LoadCaseRectangleValue> loadCaseRectangleValues)
        {
            this.loadCaseRectangleValues = loadCaseRectangleValues;
            string writePath = "CollectionView.txt", text = "";
            double width_ = 0, height_ = 0, minV = -1, maxV = -1;

            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(text);
            }

            foreach (LoadCaseRectangleValue loadCaseRectangleValue in loadCaseRectangleValues)
            {
                text = "Id = " + loadCaseRectangleValue.LoadCase.Id.ToString() + ", Name = " + loadCaseRectangleValue.LoadCase.Name + ContentStringFormat;
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
                this.Title = text;
                text = ".................................................................................";
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }

                List<RectangleValue> listRV = loadCaseRectangleValue.RectangleValues;
                foreach (RectangleValue RectangleValues in listRV)
                {
                    text = "X = " + RectangleValues.CenterX.ToString() + "; Y = " + RectangleValues.CenterY.ToString() + "; W = " + RectangleValues.Width.ToString() +
                                "; L = " + RectangleValues.Length.ToString() + "; V = " + RectangleValues.Value.ToString();
                    using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }
                    if (width_ < RectangleValues.CenterX) width_ = RectangleValues.CenterX;
                    if (height_ < RectangleValues.CenterY) height_ = RectangleValues.CenterY;
                    if (minV > RectangleValues.Value || minV == -1) minV = RectangleValues.Value;
                    if (maxV < RectangleValues.Value || maxV == -1) maxV = RectangleValues.Value;
                }
                text = ".................................................................................";
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
                text = "minV = " + minV.ToString() + ", MaxV = " + maxV.ToString() + ", W = " + width_.ToString() + ", H = " + height_.ToString();
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
                text = "*********************************************************************************";
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
            }
            MessageBox.Show("QWERTY");
        }
    }
}
