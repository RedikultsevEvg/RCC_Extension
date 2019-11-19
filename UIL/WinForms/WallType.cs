using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.WallAndColumn;
using RDBLL.Entity.RCC.Reinforcement;

namespace RDUIL.WinForms
{
    public partial class frmWallType : Form
    {
        private WallType _wallType;

        private BarSpacingSettings _vertSpacingSetting;
        public BarSpacingSettings _horSpacingSetting;

        public frmWallType(WallType wallType)
        {
            InitializeComponent();
            _wallType = wallType;

            tbName.Text = _wallType.Name;
            nudThickness.Value = Convert.ToDecimal(_wallType.Thickness);
            nudBottomOffset.Value = Convert.ToDecimal(_wallType.BottomOffset);
            nudTopOffset.Value = Convert.ToDecimal(_wallType.TopOffset);

            nudBarTopOffset.Value = Convert.ToDecimal(_wallType.BarTopOffset);
            cbRoundVertToBaseLength.Checked = _wallType.RoundVertToBaseLength;
            nudVertBaseLength.Value = Convert.ToDecimal(_wallType.VertBaseLength);

            cbAddHorLapping.Checked = _wallType.HorLapping;
            nudHorLappingLength.Value = Convert.ToDecimal(_wallType.HorLappingLength);
            nudHorBaseLength.Value = Convert.ToDecimal(_wallType.HorBaseLength);

            _vertSpacingSetting = (BarSpacingSettings)_wallType.VertSpacingSetting.Clone();
            _horSpacingSetting = (BarSpacingSettings)_wallType.HorSpacingSetting.Clone();

            tbVertSpacing.Text = _wallType.VertSpacingSetting.SpacingText();
            tbHorSpacing.Text = _wallType.HorSpacingSetting.SpacingText();
        }

        private void cbAddHorLapping_CheckedChanged(object sender, EventArgs e)
        {
            nudHorLappingLength.Enabled = ((CheckBox)sender).Checked;
            lbHorBaseLength.Enabled = ((CheckBox)sender).Checked;
            nudHorBaseLength.Enabled = ((CheckBox)sender).Checked;
        }

        private void cbAddVertLapping_CheckedChanged(object sender, EventArgs e)
        {
            nudVertBaseLength.Enabled = ((CheckBox)sender).Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _wallType.Name = tbName.Text;
            _wallType.Thickness = Convert.ToDouble(nudThickness.Value);
            _wallType.BottomOffset = Convert.ToDouble(nudBottomOffset.Value);
            _wallType.TopOffset = Convert.ToDouble(nudTopOffset.Value);
            _wallType.BarTopOffset = Convert.ToDouble(nudBarTopOffset.Value);
            _wallType.RoundVertToBaseLength = cbRoundVertToBaseLength.Checked;
            _wallType.VertBaseLength = Convert.ToDouble(nudVertBaseLength.Value);
            _wallType.HorLapping = cbAddHorLapping.Checked;
            _wallType.HorLappingLength = Convert.ToDouble(nudHorLappingLength.Value);
            _wallType.HorBaseLength = Convert.ToDouble(nudHorBaseLength.Value);
            _wallType.VertSpacingSetting = _vertSpacingSetting;
            _wallType.HorSpacingSetting = _horSpacingSetting;
            ProgrammSettings.IsDataChanged = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var obj = new frmBarSpacingSettings(_vertSpacingSetting);
            obj.ShowDialog();
            if (obj.DialogResult == DialogResult.OK) { tbVertSpacing.Text = _vertSpacingSetting.SpacingText(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var obj = new frmBarSpacingSettings(_horSpacingSetting);
            obj.ShowDialog();
            if (obj.DialogResult == DialogResult.OK) { tbHorSpacing.Text = _horSpacingSetting.SpacingText(); }
        }
    }
}
