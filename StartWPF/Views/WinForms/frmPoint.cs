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
using RDBLL.Common.Geometry;
using RDBLL.Common.Interfaces.Geometry.Points;

namespace RDUIL.WinForms
{
    public partial class frmPoint : Form
    {
        private IPoint2D _point2D;

        public frmPoint(IPoint2D point2D)
        {
            InitializeComponent();
            _point2D = point2D;
            throw new NotImplementedException();
            //tbCoord.Text = _point2D.PointText();
            nudCoord_X.Value = Convert.ToDecimal(_point2D.X);
            nudCoord_Y.Value = Convert.ToDecimal(_point2D.Y);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _point2D.X = Convert.ToDouble(nudCoord_X.Value);
            _point2D.Y = Convert.ToDouble(nudCoord_Y.Value);
            ProgrammSettings.IsDataChanged = true;
        }
    }
}
