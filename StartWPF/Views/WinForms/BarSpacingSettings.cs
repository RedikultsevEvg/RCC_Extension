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
using RDBLL.Entity.RCC.Reinforcement;

namespace RDUIL.WinForms
{
    public partial class frmBarSpacingSettings : Form
    {
        private BarSpacingSettings _barSpacingSettings;

        public frmBarSpacingSettings(BarSpacingSettings barSpacingSettings)
        {
            InitializeComponent();
            _barSpacingSettings = barSpacingSettings;
            nudMainSpacing.Value = Convert.ToDecimal(_barSpacingSettings.MainSpacing);
            cbAddLeftBars.Checked = _barSpacingSettings.AddBarsLeft;
            nudLeftQuant.Value = _barSpacingSettings.AddBarsLeftQuant;
            nudLeftSpacing.Value = Convert.ToDecimal(_barSpacingSettings.AddBarsLeftSpacing);
            cbAddRightBars.Checked = _barSpacingSettings.AddBarsRight;
            nudRightQuant.Value = _barSpacingSettings.AddBarsRightQuant;
            nudRightSpacing.Value = Convert.ToDecimal(_barSpacingSettings.AddBarsRightSpacing);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _barSpacingSettings.MainSpacing = Convert.ToDouble(nudMainSpacing.Value);
            _barSpacingSettings.AddBarsLeft = cbAddLeftBars.Checked;
            _barSpacingSettings.AddBarsLeftQuant = Convert.ToInt32(nudLeftQuant.Value);
            _barSpacingSettings.AddBarsLeftSpacing = Convert.ToDouble(nudLeftSpacing.Value);
            _barSpacingSettings.AddBarsRight = cbAddRightBars.Checked;
            _barSpacingSettings.AddBarsRightQuant = Convert.ToInt32(nudRightQuant.Value);
            _barSpacingSettings.AddBarsRightSpacing = Convert.ToDouble(nudRightSpacing.Value);
            ProgrammSettings.IsDataChanged = true;
        }

        private void cbAddLeftBars_CheckedChanged(object sender, EventArgs e)
        {
            lbLeftQuant.Enabled = ((CheckBox)sender).Checked;
            nudLeftQuant.Enabled = ((CheckBox)sender).Checked;
            lbLeftSpacing.Enabled = ((CheckBox)sender).Checked;
            nudLeftSpacing.Enabled = ((CheckBox)sender).Checked;
        }

        private void cbAddRightBars_CheckedChanged(object sender, EventArgs e)
        {
            lbRightQuant.Enabled = ((CheckBox)sender).Checked;
            nudRightQuant.Enabled = ((CheckBox)sender).Checked;
            lbRightSpacing.Enabled = ((CheckBox)sender).Checked;
            nudRightSpacing.Enabled = ((CheckBox)sender).Checked;
        }
    }
}
