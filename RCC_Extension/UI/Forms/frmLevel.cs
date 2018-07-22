using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RCC_Extension.BLL.BuildingAndSite;

namespace RCC_Extension.UI.Forms
{
    public partial class frmLevel : Form
    {
        private Level _level;

        public frmLevel(Level level)
        {
            InitializeComponent();
            _level = level;
            tbName.Text = _level.Name;
            nudFlooLevel.Value = _level.FloorLevel;
            nudHeight.Value = _level.Height;
            nudTopOffset.Value =_level.TopOffset;
            nudQuant.Value = _level.Quant;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _level.Name = tbName.Text;
            _level.FloorLevel = nudFlooLevel.Value;
            _level.Height = nudHeight.Value;
            _level.TopOffset = nudTopOffset.Value;
            _level.Quant = Convert.ToInt32(nudQuant.Value);
        }
    }
}
