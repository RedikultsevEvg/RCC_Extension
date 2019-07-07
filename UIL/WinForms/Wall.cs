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
using RDBLL.Entity.RCC.BuildingAndSite;
using RDBLL.Common.Geometry;
using RDUIL.WinForms;
using RDBLL.Entity.RCC.Reinforcement;
using System.Collections.ObjectModel;

namespace RDUIL.WinForms
{
    public partial class frmWall : Form
    {
        private Wall _wall;
        private Level _level;
        private WallType _wallType;
        private Building _building;
        private ObservableCollection<Level> _levelList;
        private ObservableCollection<WallType> _wallTypeList;
        private Point2D _tmpStartPoint;
        private Point2D _tmpEndPoint;
        private BarSpacingSettings _tmpVertSpacingSetting;
        private BarSpacingSettings _tmpHorSpacingSetting;

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
            _tmpVertSpacingSetting = (BarSpacingSettings)_wall.VertSpacingSetting.Clone();
            _tmpHorSpacingSetting = (BarSpacingSettings)_wallType.HorSpacingSetting.Clone();

            tbName.Text = _wall.Name;
            tbStartCoord.Text = _wall.StartPoint.PointText();
            tbEndCoord.Text = _wall.EndPoint.PointText();
            nudConcreteStartOffset.Value = _wall.ConcreteStartOffset;
            nudConcreteEndOffset.Value = _wall.ConcreteEndOffset;
            cbRewriteHeight.Checked = _wall.ReWriteHeight;
            nudHeight.Value = _wall.Height;
            if (_wall.OverrideVertSpacing) { tbVertSpacing.Text = _wall.VertSpacingSetting.SpacingText(); } else { tbVertSpacing.Text = _wall.WallType.VertSpacingSetting.SpacingText(); }
            if (_wall.OverrideHorSpacing) { tbHorSpacing.Text = _wall.HorSpacingSetting.SpacingText(); } else { tbHorSpacing.Text = _wall.WallType.HorSpacingSetting.SpacingText(); }
            cbOverrideVertSpacing.Checked = _wall.OverrideVertSpacing;
            cbOverrideHorSpacing.Checked = _wall.OverrideHorSpacing;
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
            _wall.WallType = _wallTypeList[cbWallTypes.SelectedIndex];
            _wall.StartPoint = _tmpStartPoint;
            _wall.EndPoint = _tmpEndPoint;
            _wall.OverrideVertSpacing = cbOverrideVertSpacing.Checked;
            _wall.OverrideHorSpacing = cbOverrideHorSpacing.Checked;
            _wall.VertSpacingSetting = _tmpVertSpacingSetting;
            _wall.HorSpacingSetting = _tmpHorSpacingSetting;
            ProgrammSettings.IsDataChanged = true;
        }

        private void cbRewriteHeight_CheckedChanged(object sender, EventArgs e)
        {
            nudHeight.Enabled = ((CheckBox)sender).Checked;
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

        private void cbOverrideVertSpacing_CheckedChanged(object sender, EventArgs e)
        {
            btnVertSpacingSetting.Enabled = ((CheckBox)sender).Checked;
            if (((CheckBox)sender).Checked) { tbVertSpacing.Text = _tmpVertSpacingSetting.SpacingText(); } else { tbVertSpacing.Text = _wall.WallType.VertSpacingSetting.SpacingText(); }
        }

        private void cbOverrideHorSpacing_CheckedChanged(object sender, EventArgs e)
        {
            btnHorSpacingSetting.Enabled = ((CheckBox)sender).Checked;
            if (((CheckBox)sender).Checked) { tbHorSpacing.Text = _tmpHorSpacingSetting.SpacingText(); } else { tbVertSpacing.Text = _wall.WallType.HorSpacingSetting.SpacingText(); }
        }

        private void btnVertSpacingSetting_Click(object sender, EventArgs e)
        {
            var obj = new frmBarSpacingSettings(_tmpVertSpacingSetting);
            obj.ShowDialog();
            if (obj.DialogResult == DialogResult.OK) { tbVertSpacing.Text = _tmpVertSpacingSetting.SpacingText(); }
        }

        private void btnHorSpacingSetting_Click(object sender, EventArgs e)
        {
            var obj = new frmBarSpacingSettings(_tmpHorSpacingSetting);
            obj.ShowDialog();
            if (obj.DialogResult == DialogResult.OK) { tbHorSpacing.Text = _tmpHorSpacingSetting.SpacingText(); }
        }
    }
}
