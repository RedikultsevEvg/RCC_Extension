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
using RDBLL.Common.Service;

namespace RDUIL.WinForms
{
    public partial class frmOpening : Form
    {
        private OpeningPlacing _openingPlacing;
        public frmOpening(OpeningPlacing openingPlacing)
        {
            //InitializeComponent();
            //_openingPlacing = openingPlacing;
            //int counter = 0;
            //foreach (OpeningType openingType in _openingPlacing.Wall.Level.ParentMember.OpeningTypeList)
            //{
            //    cbOpeningTypes.Items.Add(openingType.FullName());
            //    if (ReferenceEquals(openingType, _openingPlacing.OpeningType)) { cbOpeningTypes.SelectedIndex = counter; }
            //    counter++;
            //}
            //tbWall.Text = _openingPlacing.Wall.Name;
            //nudLeft.Minimum = Convert.ToDecimal(_openingPlacing.OpeningType.Width / 2);
            //nudLeft.Maximum = Convert.ToDecimal(_openingPlacing.Wall.GetLength() - _openingPlacing.OpeningType.Width / 2);
            //nudLeft.Value = Convert.ToDecimal(_openingPlacing.Left);
            //cbOverrideBottom.Checked = _openingPlacing.OverrideBottom;
            //nudBottom.Maximum = Convert.ToDecimal(_openingPlacing.Wall.GetHeight() - _openingPlacing.OpeningType.Height);
            //nudBottom.Value = Convert.ToDecimal(_openingPlacing.Bottom);
        }

        private void cbOverrideBottom_CheckedChanged(object sender, EventArgs e)
        {
            nudBottom.Enabled = cbOverrideBottom.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //_openingPlacing.OpeningType = _openingPlacing.Wall.Level.ParentMember.OpeningTypeList[cbOpeningTypes.SelectedIndex];
            //_openingPlacing.Left = Convert.ToDouble(nudLeft.Value);
            //_openingPlacing.OverrideBottom = cbOverrideBottom.Checked;
            //_openingPlacing.Bottom = Convert.ToDouble(nudBottom.Value);
            //ProgrammSettings.IsDataChanged = true;
        }
    }
}
