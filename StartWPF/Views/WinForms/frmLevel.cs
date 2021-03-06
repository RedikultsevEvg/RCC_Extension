﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDBLL.Common.Service;
using RDBLL.Entity.RCC.BuildingAndSite;

namespace RDUIL.WinForms
{
    public partial class frmLevel : Form
    {
        private Level _level;

        public frmLevel(Level level)
        {
            InitializeComponent();
            _level = level;
            tbName.Text = _level.Name;
            nudFlooLevel.Value = Convert.ToDecimal(_level.Elevation);
            nudHeight.Value = Convert.ToDecimal(_level.Height);
            nudTopOffset.Value = Convert.ToDecimal(_level.TopOffset);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _level.Name = tbName.Text;
            _level.Elevation = Convert.ToDouble(nudFlooLevel.Value);
            _level.Height = Convert.ToDouble(nudHeight.Value);
            _level.TopOffset = Convert.ToDouble(nudTopOffset.Value);
            ProgrammSettings.IsDataChanged = true;
        }
    }
}
