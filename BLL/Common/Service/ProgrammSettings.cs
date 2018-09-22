﻿using System;
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


namespace RDBLL.Common.Service
{
    public class ProgrammSettings
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
            ForceParamKinds = new List<ForceParamKind>();
            ForceParamKinds.Add(new ForceParamKind
            {
                Label = "Продольная сила N",
                UnitLabel = "Н",
                Addition = "Положительному значению силы соответствует направление вдоль оси Z (направлена вертикально вверх)"
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Label = "Изгибающий момент Mx",
                UnitLabel = "Н*м",
                Addition = "За положительное значение момента принят момент против часовой стрелки если смотреть с конца оси X"
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Label = "Изгибающий момент My",
                UnitLabel = "Н*м",
                Addition = "За положительное значение момента принят момент против часовой стрелки если смотреть с конца оси Y"
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Label = "Поперечная сила Qx",
                UnitLabel = "Н",
                Addition = "Положительному значению силы соответствует направление вдоль оси X (направлена вправо по плану)"
            });
            ForceParamKinds.Add(new ForceParamKind
            {
                Label = "Поперечная сила Qy",
                UnitLabel = "Н",
                Addition = "Положительному значению силы соответствует направление вдоль оси Y (направлена вверх по плану)"
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