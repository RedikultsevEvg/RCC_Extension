using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RCC_Extension.BLL.WallAndColumn;

namespace RCC_Extension.UI.Forms
{
    public partial class frmOpening : Form
    {
        private OpeningPlacing _openingPlacing;
        public frmOpening(OpeningPlacing openingPlacing)
        {
            InitializeComponent();
            _openingPlacing = openingPlacing;
            int counter = 0;
            foreach (OpeningType openingType in _openingPlacing.Wall.Level.Building.OpeningTypeList)
            {
                cbOpeningTypes.Items.Add(openingType.Name + " - "+ openingType.Purpose+Convert.ToString(openingType.Width)+"*"+ Convert.ToString(openingType.Height) + "(h)");
                if (ReferenceEquals(openingType, _openingPlacing.OpeningType)) { cbOpeningTypes.SelectedIndex = counter; }
                counter++;
            }
            tbWall.Text = _openingPlacing.Wall.Name;
            nudLeft.Minimum = _openingPlacing.OpeningType.Width / 2;
            nudLeft.Maximum = _openingPlacing.Wall.GetLength() - _openingPlacing.OpeningType.Width / 2;
            nudLeft.Value = _openingPlacing.Left;
            cbOverrideBottom.Checked = _openingPlacing.OverrideBottom;
            nudBottom.Maximum = _openingPlacing.Wall.GetHeight() - _openingPlacing.OpeningType.Height;
            nudBottom.Value = _openingPlacing.Bottom;
        }

        private void cbOverrideBottom_CheckedChanged(object sender, EventArgs e)
        {
            nudBottom.Enabled = cbOverrideBottom.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _openingPlacing.OpeningType = _openingPlacing.Wall.Level.Building.OpeningTypeList[cbOpeningTypes.SelectedIndex];
            _openingPlacing.Left = nudLeft.Value;
            _openingPlacing.OverrideBottom = cbOverrideBottom.Checked;
            _openingPlacing.Bottom = nudBottom.Value;
        }
    }
}
