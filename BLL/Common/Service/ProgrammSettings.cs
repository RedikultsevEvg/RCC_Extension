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
using System.Collections.ObjectModel;
using System.Data;
using RDBLL.Entity.Common.Materials;
using RDBLL.Entity.Common.Materials.MatFactories;
using RDBLL.Entity.Common.Materials.SteelMaterialUsing;
using RDBLL.Entity.MeasureUnits.Factorys;
using RDBLL.Common.Service.DsOperations.Factory;

namespace RDBLL.Common.Service
{
    /// <summary>
    /// Класс хранения основных настроек программы
    /// </summary>
    public static class ProgrammSettings
    {
        private static String _filePath;
        private static bool _isDataChanged;
        private static int _CurrentId;
        private static int _CurrentTmpId;

        /// <summary>
        /// Генератор Id
        /// </summary>
        public static int CurrentId
        {
            get =>  _CurrentId++;
            set  { _CurrentId = value;}
        }
        /// <summary>
        /// Генератор временных Id
        /// </summary>
        public static int CurrentTmpId
        {
            get => _CurrentTmpId++;
            set  {_CurrentTmpId = value;}
        }
        /// <summary>
        /// Перечень датасетов (на будущее если будет предусмотрена обработка нескольких файлов)
        /// </summary>
        public static List<DataSet> DataSets { get; set; }
        /// <summary>
        /// Текущий датасет
        /// </summary>
        public static DataSet CurrentDataSet { get { return DataSets[0]; } }
        public static List<ForceParamKind> ForceParamKinds { get; set; }
        /// <summary>
        /// Единицы измерения
        /// </summary>
        public static ObservableCollection<MeasureUnit> MeasureUnits { get; set; }
        /// <summary>
        /// Строительный объект
        /// </summary>
        public static BuildingSite BuildingSite { get; set; }
        /// <summary>
        /// Путь к рабочему файлу
        /// </summary>
        public static String FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                if (FilePathChanged != null) FilePathChanged(null, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Флаг внесения изменений в файл
        /// </summary>
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
        /// <summary>
        /// Справочник классов стали
        /// </summary>
        public static List<SteelKind> SteelKinds { get; private set; }
        /// <summary>
        /// Справочник классов бетона
        /// </summary>
        public static List<ConcreteKind> ConcreteKinds { get; set; }
        /// <summary>
        /// Справочник классов арматуры
        /// </summary>
        public static List<ReinforcementKind> ReinforcementKinds { get; set; }
        /// <summary>
        /// Метод задания начальных параметров
        /// </summary>
        public static void InicializeNew()
        {
            DataSets = new List<DataSet>();
            DataSet dataSet = DsFactory.GetDataSet();
            DataSets.Add(dataSet);
            BuildingSite = new BuildingSite(true);
            Building building =  new Building(BuildingSite);
            BuildingSite.SaveToDataSet(CurrentDataSet, true);
            IsDataChanged = false;
            CurrentId = 0;
            CurrentTmpId = 1000000;
            ObservableCollection<MeasureUnit> MeasureUnitList = new ObservableCollection<MeasureUnit>();
            List<ForceParamKind> ForceParamKindList = new List<ForceParamKind>();
            UnitFactory.GetMeasureUnits(ref MeasureUnitList, ref ForceParamKindList);
            MeasureUnits = MeasureUnitList;
            ForceParamKinds = ForceParamKindList;
            SteelKinds = MatFactory.GetSteelKinds();
            ConcreteKinds = MatFactory.GetConcreteKinds();
            ReinforcementKinds = MatFactory.GetReinforcementKinds();
        }
        /// <summary>
        /// Очистить коллекцию зданий строительного объекта
        /// </summary>
        public static void ClearAll()
        {
            BuildingSite.Children.Clear();
        }
        /// <summary>
        /// Открыть проект из файла
        /// </summary>
        /// <returns></returns>
        public static bool OpenProjectFromFile()
        {
            try
            {
                win32.OpenFileDialog openFileDialog = new win32.OpenFileDialog();
                openFileDialog.Filter = "XML file (*.xml)|*.xml";
                if (openFileDialog.ShowDialog() == true) FilePath = openFileDialog.FileName; else return false;
                ClearAll();
                DataSets = new List<DataSet>();
                DataSet dataSet = DsFactory.GetDataSet();
                DataSets.Add(dataSet);
                OpenExistDataset(FilePath);
                IsDataChanged = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Сохранение проекта в файл
        /// </summary>
        /// <param name="InNewFile"></param>
        /// <returns></returns>
        public static bool SaveProjectToFile(bool InNewFile = false)
        {
            try
            {
                if (FilePath == "" || FilePath == null || InNewFile)
                {
                    win32.SaveFileDialog saveFileDialog = new win32.SaveFileDialog();
                    saveFileDialog.Filter = "XML file (*.xml)|*.xml";

                    if (saveFileDialog.ShowDialog() == true)
                        FilePath = saveFileDialog.FileName;
                    else return false;
                }
                DataSet dataSet = GetDataSet();
                string path = Path.GetDirectoryName(FilePath);
                //Если директории не существует, создаем ее
                if (! File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                dataSet.WriteXml(FilePath);
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
        /// <summary>
        /// Параметр определяющий, что данные изменились
        /// </summary>
        public static event EventHandler IsDataChangedChanged;
        /// <summary>
        /// Событие изменения пути файла
        /// </summary>
        public static event EventHandler FilePathChanged;
        /// <summary>
        /// Получение датасета
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDataSet()
        {
            DataSet dataSet = DsFactory.GetDataSet();
            DataTable dataTable;
            DataRow dataRow;
            #region Generator
            dataTable = dataSet.Tables["Generators"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { CurrentId };
            dataTable.Rows.Add(dataRow);
            #endregion
            #region Versions
            dataTable = dataSet.Tables["Versions"];
            dataRow = dataTable.NewRow();
            dataRow.ItemArray = new object[] { 1 };
            dataTable.Rows.Add(dataRow);
            #endregion
            #region MeasurementUnits
            dataTable = dataSet.Tables["MeasurementUnits"];
            for (int i = 0; i < MeasureUnits.Count; i++)
            {
                dataRow = dataTable.NewRow();
                dataRow.ItemArray = new object[]
                {
                        MeasureUnits[i].CurrentUnitLabelId
                };
                dataTable.Rows.Add(dataRow);
            }
            #endregion
            BuildingSite.SaveToDataSet(dataSet, true);
            return dataSet;
        }
        /// <summary>
        /// Открыть существующий проект
        /// </summary>
        /// <param name="fileName"></param>
        public static void OpenExistDataset(string fileName)
        {
            DataSet dataSet = DsFactory.GetDataSet();
            DataTable dataTable;
            dataSet.ReadXml(fileName);
            #region Generator
            dataTable = dataSet.Tables["Generators"];
            CurrentId = Convert.ToInt32(dataTable.Rows[0].ItemArray[0]);
            CurrentTmpId = CurrentId + 1000000;
            #endregion
            #region Versions
            #endregion
            #region MeasurementUnits
            dataTable = dataSet.Tables["MeasurementUnits"];
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                int labelId = Convert.ToInt32(dataTable.Rows[i].ItemArray[0]);
                foreach (MeasureUnitLabel measureUnitLabel in MeasureUnits[i].UnitLabels)
                {
                    if (measureUnitLabel.Id == labelId) { MeasureUnits[i].CurrentUnitLabelId=labelId; }
                }
            }
            #endregion
            BuildingSite.OpenFromDataSet(dataSet);
            #region Вложенные объекты
            //Получаем коллекцию грунтов
            BuildingSite.Soils = GetEntity.GetSoils(dataSet, BuildingSite);
            //Получаем коллекцию скважин
            BuildingSite.SoilSections = GetEntity.GetSoilSections(dataSet, BuildingSite);
            //Получаем коллекцию зданий
            GetEntity.GetBuildings(dataSet, BuildingSite);
            #endregion
            DataSets.Clear();
            DataSets.Add(dataSet);
        }
    }
}
