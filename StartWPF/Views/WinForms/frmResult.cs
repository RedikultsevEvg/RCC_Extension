using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDBLL.Entity.RCC.WallAndColumn;

namespace RDUIL.WinForms
{
    public partial class frmResult : Form
    {
        private List<Wall> _ObjectlList;
        private List<String> _ColumnName = new List<String>();
        private List<Int32> _ColumnWidth = new List<Int32>();

        public frmResult(List<Wall> objectList)
        {
            InitializeComponent();
            _ObjectlList = objectList;
            List<String> columnName = new List<String>() { "Итого", "Верт.стерж., шт.", "Гор.стерж., м", "Диаг.стерж., шт", "Верт. обрамл., шт.", "Гор. обрамл., шт.", "S_нетто, кв.м", "V_нетто, куб.м" };
            List<Int32> columnWidth = new List<Int32>() { 150, 100, 100, 100, 100, 100, 100, 100};

            _ColumnName = columnName;
            _ColumnWidth = columnWidth;
            foreach (String S in _ColumnName)
            {
                lvResult.Columns.Add(S);
            }
            for (int i = 0; i < lvResult.Columns.Count; i++)
            {
                lvResult.Columns[i].Width = _ColumnWidth[i];
            }
            for (int i = 1; i < lvResult.Columns.Count; i++)
            {
                lvResult.Columns[i].TextAlign = HorizontalAlignment.Center;
            }
            int vertQuant = 0;
            double horLength = 0;
            int incQuant = 0;
            int vertEdgeQuant = 0;
            int horEdgeQuant = 0;
            double sumAreaNetto = 0;
            double sumVolumeNetto = 0;
            foreach (Wall obj in _ObjectlList)
            {
                vertQuant += obj.VertBarQuantity();
                horLength += obj.HorBarLength();
                incQuant += obj.IncBarQuantity();
                vertEdgeQuant +=obj.VertEdgeQuant();
                horEdgeQuant +=obj.HorEdgeQuant();
                sumAreaNetto += obj.GetConcreteAreaNetto();
                sumVolumeNetto += obj.GetConcreteVolumeNetto();
            }
            horLength = (Math.Round(horLength)) / 1000;
            sumAreaNetto = (Math.Round(sumAreaNetto / 1000)) / 1000;
            sumVolumeNetto = (Math.Round(sumVolumeNetto / 1000000)) / 1000;
            ListViewItem NewItem = new ListViewItem();
            lvResult.Items.Add(NewItem);
            NewItem.Text = "Итого";
            NewItem.SubItems.Add(Convert.ToString(vertQuant));
            NewItem.SubItems.Add(Convert.ToString(horLength));
            NewItem.SubItems.Add(Convert.ToString(incQuant));
            NewItem.SubItems.Add(Convert.ToString(vertEdgeQuant));
            NewItem.SubItems.Add(Convert.ToString(horEdgeQuant));
            NewItem.SubItems.Add(Convert.ToString(sumAreaNetto));
            NewItem.SubItems.Add(Convert.ToString(sumVolumeNetto));
        }
    }
}
