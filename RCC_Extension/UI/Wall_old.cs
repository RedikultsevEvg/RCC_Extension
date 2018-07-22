using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RCC_Extension.BLL;
using RCC_Extension.UI;
using RCC_Extension.BLL.WallAndColumn;


namespace RCC_Extension
{
    public partial class frmWallCrossSection : Form
    {
        List<Opening> OpeningList = new List<Opening>();

        public decimal WallThickness
        {
            get { return this.nUDThickness.Value; }
            set { this.nUDThickness.Value = value; }
        }



        public frmWallCrossSection()
        {
            InitializeComponent();
        }

        private void frmWallCrossSection_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Обработчик нажатия ОК
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            tcWall.SelectedIndex = 4;

            string s;
            s = "Толщина стены "+nUDThickness.Value+"мм" + Environment.NewLine;
            s += "Высота стены " + nUDHeight.Value + "мм" + Environment.NewLine;
            s += "Длина стены " + nUDLength.Value + "мм" + Environment.NewLine;
            s += Environment.NewLine;

            s += "Объем бетона " + nUDThickness.Value * nUDHeight.Value * nUDLength.Value/1000000000 + "куб.м" + Environment.NewLine;
            s += Environment.NewLine;

            decimal VertLen;
            VertLen = nUDHeight.Value + nUDSlabThickness.Value + nUDVertLap.Value;
            if (cbVertRoudLen.Checked) VertLen = nUDVertBaseLength.Value / (Math.Floor(nUDVertBaseLength.Value / VertLen));

            s += "Длина вертикальных стержней " + VertLen + "мм" + Environment.NewLine;

            decimal VertQuant, VertDistance;
            VertQuant = 0;
            VertDistance = nUDLength.Value - 2*nUDVertCover.Value;
            if (cbVertAddLeft.Checked)
            {
                VertDistance -= (nUDVertQuantLeft.Value-1) * nUDVertStepLeft.Value + nUDVertStep.Value;
                VertQuant += nUDVertQuantLeft.Value;
            }

            if (cbVertAddRight.Checked)
            {
                VertDistance -= (nUDVertQuantRight.Value-1) * nUDVertStepRight.Value + nUDVertStep.Value;
                VertQuant += nUDVertQuantRight.Value;
            }

            VertQuant += Math.Floor(VertDistance / nUDVertStep.Value)+1;
            s += "Количество вертикальных стержней 2*" +VertQuant +"=" + 2*VertQuant + "шт." + Environment.NewLine;

            int HorQuant;
            decimal HorDistance;
            HorQuant = 0;
            HorDistance = nUDHeight.Value - 2 * nUDHorCover.Value;
            HorQuant += Convert.ToInt32(Math.Ceiling(HorDistance / nUDHorStep.Value)) + 1;
            s += "Количество горизонтальных стержней 2*" + HorQuant +"=" +2*HorQuant + "шт." + Environment.NewLine;
            s += Environment.NewLine;

            s += "Суммарная длина вертикальных стержней " + 2*VertLen*VertQuant/1000 + "м" + Environment.NewLine;
            s += "Суммарная длина горизонтальных стержней "
                + 2*HorQuant*nUDLength.Value*Math.Round((1+nUDHorLap.Value/nUDHorBaseLenth.Value),3)/1000
                + "м" + Environment.NewLine;
            s += Environment.NewLine;

            int ShearLeftQuant=0, ShearRightQuant=0;
            if (cbAddEdgeLeft.Checked)
            {
                ShearLeftQuant = HorQuant;
                s += "Количество обрамляющих стержней левого торца " + ShearLeftQuant + "шт." + Environment.NewLine;
            }

            if (cbAddEdgeRight.Checked)
            {
                ShearRightQuant = HorQuant;
                s += "Количество обрамляющих стержней правого торца " + ShearRightQuant + "шт." + Environment.NewLine;
            }

            if (cbAddEdgeLeft.Checked || cbAddEdgeRight.Checked)
            {
                s += "Итого " + (ShearLeftQuant+ ShearRightQuant) + "шт." + Environment.NewLine;
                s += Environment.NewLine;
            }

            int ShearQuant, ShearVertQuant, ShearHorQuant;
            ShearVertQuant = Convert.ToInt32(Math.Floor(nUDHeight.Value / nUDShearVertStep.Value) + 1);
            ShearHorQuant = Convert.ToInt32(Math.Floor(nUDLength.Value / nUDShearHorStep.Value) + 1);
            ShearQuant = ShearVertQuant* ShearHorQuant;
            s += "Количество поперечных стержней " + ShearVertQuant +"*" + ShearHorQuant + "=" + ShearQuant + "шт." + Environment.NewLine;


            tbInfo.Text = s;

        }

        private void tbpReinforcement_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmOpenings OpeningsForm = new frmOpenings(OpeningList);
            OpeningsForm.ShowDialog();
        }
    }
}
