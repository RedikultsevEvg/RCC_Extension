using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDBLL.Entity.RCC.BuildingAndSite;
using System.Xml;
using System.IO;
using win32 = Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RDBLL.Forces;
using RDBLL.Entity.MeasureUnits;


namespace RDBLL.Common.Service
{
    public static class ProgrammSettings
    {
        private static String _filePath;
        private static bool _isDataChanged;
        public static List<ForceParamKind> ForceParamKinds { get; set; } 

        public static BuildingSite BuildingSite { get; set; }
        public static String FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                if (FilePathChanged != null) FilePathChanged(null, EventArgs.Empty);
            }
        }
        public static bool IsDataChanged
        {
            get
            {   return _isDataChanged; }
            set
            {
                _isDataChanged = value;
                if (IsDataChangedChanged != null) IsDataChangedChanged(null, EventArgs.Empty);
            }
        }
        public static void InicializeNew()
        {
            BuildingSite = new BuildingSite();
            BuildingSite.BuildingList.Add(new Building(BuildingSite));
            IsDataChanged = false;
            #region
            MeasureUnit measureUnitForce = new MeasureUnit();
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id=1, UnitName = "Н", AddKoeff= 1.0});
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 2, UnitName = "кН", AddKoeff = 0.001 });
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 3, UnitName = "МН", AddKoeff = 0.000001 });
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 4, UnitName = "кгс", AddKoeff = 1 / 9.81 });
            measureUnitForce.UnitLabels.Add(new MeasureUnitLabel { Id = 5, UnitName = "тс", AddKoeff = 0.001 / 9.81 });
            measureUnitForce.CurrentUnitLabelId = 5;
            MeasureUnit measureUnitMoment = new MeasureUnit();
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 7, UnitName = "Н*м", AddKoeff = 1.0 });
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 8, UnitName = "кН*м", AddKoeff = 0.001 });
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 9, UnitName = "МН*м", AddKoeff = 0.000001 });
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 10, UnitName = "кгс*м", AddKoeff = 1 / 9.81 });
            measureUnitMoment.UnitLabels.Add(new MeasureUnitLabel { Id = 11, UnitName = "тс*м", AddKoeff = 0.001 / 9.81 });
            measureUnitMoment.CurrentUnitLabelId = 11;
            MeasureUnit measureUnitStress = new MeasureUnit();
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 12, UnitName = "Па", AddKoeff = 1.0 });
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 13, UnitName = "кПа", AddKoeff = 0.001 });
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 14, UnitName = "МПа", AddKoeff = 0.000001 });
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 15, UnitName = "кгс/см^2", AddKoeff = 0.00001 / 9.81 });
            measureUnitStress.UnitLabels.Add(new MeasureUnitLabel { Id = 16, UnitName = "тс/м^2", AddKoeff = 0.0001 / 9.81 });
            measureUnitStress.CurrentUnitLabelId = 14;

            ForceParamKinds = new List<ForceParamKind>();
            ForceParamKinds.Add(new ForceParamKind
            {
                Id = 1,
                LongLabel = "Продольная сила N",
                ShortLabel = "N",
                Addition = "Положительному значению силы соответствует направление вдоль оси Z (направлена вертикально вверх)",
                MeasureUnit = measureUnitForce
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Id = 2,
                LongLabel = "Изгибающий момент Mx",
                ShortLabel = "Mx",
                Addition = "За положительное значение момента принят момент против часовой стрелки если смотреть с конца оси X",
                MeasureUnit = measureUnitMoment
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Id = 3,
                LongLabel = "Изгибающий момент My",
                ShortLabel = "My",
                Addition = "За положительное значение момента принят момент против часовой стрелки если смотреть с конца оси Y",
                MeasureUnit = measureUnitMoment
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Id = 4,
                LongLabel = "Поперечная сила Qx",
                ShortLabel = "Qx",
                Addition = "Положительному значению силы соответствует направление вдоль оси X (направлена вправо по плану)",
                MeasureUnit = measureUnitForce
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Id = 5,
                LongLabel = "Поперечная сила Qy",
                ShortLabel = "Qy",
                Addition = "Положительному значению силы соответствует направление вдоль оси Y (направлена вверх по плану)",
                MeasureUnit = measureUnitForce
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Id = 6,
                LongLabel = "Крутящий момент Mz",
                ShortLabel = "Mz",
                Addition = "За положительное значение момента принят момент против часовой стрелки если смотреть с конца оси Z",
                MeasureUnit = measureUnitMoment
            });
            #endregion
        }
        public static void ClearAll()
        {
            BuildingSite.BuildingList.Clear();
        }
        public static bool OpenProjectFromFile()
        {
            try
            {
                win32.OpenFileDialog openFileDialog = new win32.OpenFileDialog();
                openFileDialog.Filter = "XML file (*.xml)|*.xml";
                if (openFileDialog.ShowDialog() == true) FilePath = openFileDialog.FileName; else return false;
                ClearAll();
                InicializeNew();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(FilePath);
                XmlElement xmlRoot = xmlDocument.DocumentElement;
                foreach (XmlNode childNode in xmlRoot.ChildNodes)
                {
                    if (childNode.Name == "BuildingSite") BuildingSite = new BuildingSite(childNode);
                }
                IsDataChanged = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
                return false;
            }
        }
        public static bool SaveProjectToFile(bool InNewFile)
        {
            try
            {
                win32.SaveFileDialog saveFileDialog = new win32.SaveFileDialog();
                saveFileDialog.Filter = "XML file (*.xml)|*.xml";
                if (FilePath == "" || FilePath == null || InNewFile)
                {
                    
                    if (saveFileDialog.ShowDialog() == true)
                        FilePath = saveFileDialog.FileName;
                    else return false;
                }
                XmlTextWriter textWritter = new XmlTextWriter(FilePath, Encoding.UTF8);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("Project");
                textWritter.Close();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(FilePath);
                XmlElement xmlRoot = xmlDocument.DocumentElement;
                XmlElement xmlSite = BuildingSite.SaveToXMLNode(xmlDocument);
                xmlRoot.AppendChild(xmlSite);
                xmlDocument.Save(FilePath);
                IsDataChanged = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
                return false;
            }

        }
        // Raise the change event through this static method
        public static event EventHandler IsDataChangedChanged;
        public static event EventHandler FilePathChanged;
    }
}
