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

namespace RDUIL.WinForms
{
    public partial class frmOpeningType : Form
    {
        private OpeningType _Opening;

        public frmOpeningType(OpeningType value)
        {
            InitializeComponent();
            _Opening = value;
            tbName.Text = _Opening.Name;
            tbPurpose.Text = _Opening.Purpose;
            nudHeight.Value = Convert.ToDecimal(_Opening.Height);
            nudWidth.Value = Convert.ToDecimal(_Opening.Width);
            nudBottom.Value = Convert.ToDecimal(_Opening.Bottom);
            cbAddEdgeLeft.Checked = _Opening.AddEdgeLeft;
            cbAddEdgeRight.Checked = _Opening.AddEdgeRight;
            cbAddEdgeTop.Checked = _Opening.AddEdgeTop;
            cbAddEdgeBottom.Checked = _Opening.AddEdgeBottom;
            cbMoveVert.Checked = _Opening.MoveVert;
            nudQuantVertLeft.Value = _Opening.QuantVertLeft;
            nudQuantVertRight.Value = _Opening.QuantVertRight;
            nudQuantInclined.Value = _Opening.QuantInclined;
            cbIncTopLeft.Checked = _Opening.AddIncTopLeft;
            cbIncTopRight.Checked = _Opening.AddIncTopRight;
            cbIncBottomLeft.Checked = _Opening.AddIncBottomLeft;
            cbIncBottomRight.Checked = _Opening.AddIncBottomRight;
        }
        
        private void cbMoveVert_CheckedChanged(object sender, EventArgs e)
        {
            lbQuantVertLeft.Enabled = cbMoveVert.Checked;
            lbQuantVertRight.Enabled = cbMoveVert.Checked;
            nudQuantVertLeft.Enabled = cbMoveVert.Checked;
            nudQuantVertRight.Enabled = cbMoveVert.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _Opening.Name = tbName.Text;
            _Opening.Purpose = tbPurpose.Text;
            _Opening.Height = Convert.ToDouble(nudHeight.Value);
            _Opening.Width = Convert.ToDouble(nudWidth.Value);
            _Opening.Bottom = Convert.ToDouble(nudBottom.Value);
            _Opening.AddEdgeLeft = cbAddEdgeLeft.Checked;
            _Opening.AddEdgeRight = cbAddEdgeRight.Checked;
            _Opening.AddEdgeTop = cbAddEdgeTop.Checked;
            _Opening.AddEdgeBottom = cbAddEdgeBottom.Checked;
            _Opening.MoveVert = cbMoveVert.Checked;
            _Opening.QuantVertLeft = Convert.ToInt32(nudQuantVertLeft.Value);
            _Opening.QuantVertRight = Convert.ToInt32(nudQuantVertRight.Value);
            _Opening.QuantInclined = Convert.ToInt32(nudQuantInclined.Value);
            _Opening.AddIncTopLeft = cbIncTopLeft.Checked;
            _Opening.AddIncTopRight = cbIncTopRight.Checked;
            _Opening.AddIncBottomLeft = cbIncBottomLeft.Checked;
            _Opening.AddIncBottomRight = cbIncBottomRight.Checked;
            ProgrammSettings.IsDataChanged = true;
        }
    }
}
