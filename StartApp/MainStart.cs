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
using RCC_Extension.BLL.Geometry;
using RCC_Extension.BLL.WallAndColumn;
using RCC_Extension.BLL.Service;
using RCC_Extension.UI;

namespace StartApp
{
    public partial class frmMain : Form
    {
        private BuildingSite _buildingSite;
        private Building _building;

        public frmMain()
        {
            InitializeComponent();
            _buildingSite = new BuildingSite();
            _building = new Building(_buildingSite);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var detailObjectList = new DetailObjectList("Levels", _building, _building.LevelList, false);
            frmDetailList DetailForm = new frmDetailList(detailObjectList);
            DetailForm.Show();
            
        }
    }
}
