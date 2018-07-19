using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RCC_Extension.BLL.Building;
using RCC_Extension.BLL.Geometry;
using RCC_Extension.BLL.WallAndColumn;
using RCC_Extension.UI;

namespace StartApp
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BuildingSite buildingSite = new BuildingSite();
            buildingSite.Name = "Мой объект";
            buildingSite.BuildingList = new List<Building>();

            Building building = new Building();
            building.Name = "Мое здание";

            Level level = new Level();
            level.Name = "Этаж 1";
            level.FloorLevel = 0;
            level.Height = 3000;
            level.TopFloorThickness = 200;
            level.BasePoint = new Point3D(0,0,0);
            level.Quant = 1;

            Wall wall = new Wall(new Point2D(0,0), new Point2D(6,0));
            wall.Name = "Новая стена";
            

            buildingSite.BuildingList.Add(building);
            building.LevelList = new List<Level>();
            building.LevelList.Add(level);
            level.WallList = new List<Wall>();
            level.WallList.Add(wall);



            frmDetailList WallForm = new frmDetailList("Wall");
            WallForm.Show();
            
        }
    }
}
