using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RCC_Extension.BLL.WallAndColumn;
using RCC_Extension.BLL.BuildingAndSite;
using RCC_Extension.BLL.Geometry;
using RCC_Extension.UI.Forms;

namespace RCC_Extension.UI
{
    public partial class frmWall : Form
    {
        private Wall _wall;
        private Level _level;
        private WallType _wallType;
        private Building _building;
        private List<Level> _levelList;
        private List<WallType> _wallTypeList;
        private Point2D _tmpStartPoint;
        private Point2D _tmpEndPoint;

        public frmWall(Wall wall)
        {
            InitializeComponent();
            _wall = wall;
            _level = wall.Level;
            _wallType = _wall.WallType;
            _building = _level.Building;
            _levelList = _building.LevelList;
            _wallTypeList = _building.WallTypeList;
            _tmpStartPoint = (Point2D)_wall.StartPoint.Clone();
            _tmpEndPoint = (Point2D)_wall.EndPoint.Clone();

            tbName.Text = _wall.Name;
            tbStartCoord.Text = _wall.StartPoint.PointText();
            tbEndCoord.Text = _wall.EndPoint.PointText();
            nudConcreteStartOffset.Value = _wall.ConcreteStartOffset;
            nudConcreteEndOffset.Value = _wall.ConcreteEndOffset;
            cbRewriteHeight.Checked = _wall.ReWriteHeight;
            nudHeight.Value = _wall.Height;
            tbVertSpacing.Text = _wall.WallType.VertSpacingSetting.SpacingText();
            tbHorSpacing.Text = _wall.WallType.HorSpacingSetting.SpacingText();
            nudReinforcementStartOffset.Value = _wall.ReiforcementStartOffset;
            nudReinforcementEndOffset.Value = _wall.ReiforcementEndOffset;
            int Counter=0;
            foreach (Level levelItem in _levelList)
            {
                cbLevels.Items.Add(levelItem.Name);
                if (ReferenceEquals(levelItem, _level)) { cbLevels.SelectedIndex = Counter;}
                Counter++;
            }
            Counter = 0;
            foreach (WallType wallTypeItem in _wallTypeList)
            {
                cbWallTypes.Items.Add(wallTypeItem.Name);
                if (ReferenceEquals(wallTypeItem, _wallType)) { cbWallTypes.SelectedIndex = Counter; }
                Counter++;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _wall.Name = tbName.Text;
            _wall.ConcreteStartOffset = nudConcreteStartOffset.Value;
            _wall.ConcreteEndOffset = nudConcreteEndOffset.Value;
            _wall.ReWriteHeight = cbRewriteHeight.Checked;
            _wall.Height = nudHeight.Value;
            _wall.ReiforcementStartOffset = nudReinforcementStartOffset.Value;
            _wall.ReiforcementEndOffset = nudReinforcementEndOffset.Value;

            //_wall.Level = _levelList[cbLevels.SelectedIndex];
            _wall.WallType = _wallTypeList[cbWallTypes.SelectedIndex];
            _wall.StartPoint = _tmpStartPoint;
            _wall.EndPoint = _tmpEndPoint;

        }

        private void cbRewriteHeight_CheckedChanged(object sender, EventArgs e)
        {
            nudHeight.Enabled = ((CheckBox)sender).Checked;
        }

        private void btnOpenings_Click(object sender, EventArgs e)
        {
            var OpeningsForm = new frmOpenings(_wall.OpeningList);
            OpeningsForm.ShowDialog();
        }

        private void btnStartPoint_Click(object sender, EventArgs e)
        {
            frmPoint frmPoint = new frmPoint(_tmpStartPoint);
            frmPoint.ShowDialog();
            if (frmPoint.DialogResult == DialogResult.OK) { tbStartCoord.Text = _tmpStartPoint.PointText(); }
        }

        private void btnEndPoint_Click(object sender, EventArgs e)
        {
            frmPoint frmPoint = new frmPoint(_tmpEndPoint);
            frmPoint.ShowDialog();
            if (frmPoint.DialogResult == DialogResult.OK) { tbEndCoord.Text = _tmpEndPoint.PointText(); }
        }
    }
}
