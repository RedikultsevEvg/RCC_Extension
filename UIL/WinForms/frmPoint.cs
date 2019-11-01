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

namespace RDUIL.WinForms
{
    public partial class frmPoint : Form
    {
        private Point2D _point2D;

        public frmPoint(Point2D point2D)
        {
            InitializeComponent();
            _point2D = point2D;
            tbCoord.Text = _point2D.PointText();
            nudCoord_X.Value = Convert.ToDecimal(_point2D.Coord_X);
            nudCoord_Y.Value = Convert.ToDecimal(_point2D.Coord_Y);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _point2D.Coord_X = Convert.ToDouble(nudCoord_X.Value);
            _point2D.Coord_Y = Convert.ToDouble(nudCoord_Y.Value);
            ProgrammSettings.IsDataChanged = true;
        }
    }
}
