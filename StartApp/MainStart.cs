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
         public frmMain()
        {
            InitializeComponent();
            ProgrammSettings.buildingSite = new BuildingSite();
            ProgrammSettings.building = new Building(ProgrammSettings.buildingSite);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var detailObjectList = new DetailObjectList("Levels", ProgrammSettings.building, ProgrammSettings.building.LevelList, false);
            frmDetailList DetailForm = new frmDetailList(detailObjectList);
            DetailForm.Show();
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ProgrammSettings.SaveProjectToFile("C:\\Repos\\Project.xml");
        }
    }
}
