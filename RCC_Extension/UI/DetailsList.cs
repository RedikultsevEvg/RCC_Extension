using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCC_Extension.UI
{
    public partial class frmDetailList : Form
    {
        private String FormType;

        public frmDetailList(String Type)
        {
            InitializeComponent();
            FormType = Type;

            lvDetails.Columns.Clear();

            List<String> ColumnName = new List<String>();
            List<Int32> ColumnWidth = new List<Int32>();

            switch (FormType) //
            {
                case "Buildings": //Создаем окно для списка уровней
                    {
                        this.Text = "Здания";
                        List<String> _ColumnName = new List<String>() { "Наименование"};
                        List<Int32> _ColumnWidth = new List<Int32>() { 600 };
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        break;
                    }

                case "Levels": //Создаем окно для списка уровней
                    {
                        this.Text = "Уровни";
                        List<String> _ColumnName = new List<String>() { "Наименование", "Отметка, м", "Высота, мм", "Перекрытие, мм", "Кол-во, шт." };
                        List<Int32> _ColumnWidth = new List<Int32>() { 100, 100, 100, 100, 100 };
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        break;
                    }

                case "Walls": //Создаем окно для списка стен
                {
                        this.Text = "Стены";
                        List<String> _ColumnName = new List<String>() { "Марка", "Тип", "Толщина, мм", "Высота, мм", "Длина, мм", "S_брутто, кв.м", "S_нетто, кв.м", "V_брутто, куб.м", "V_нетто, куб.м"};
                        List<Int32> _ColumnWidth = new List<Int32>() { 100, 60, 80, 80, 80, 100, 100, 100, 100 };
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        break;
                }

                case "WallTypes": //Создаем окно для списка стен
                    {
                        this.Text = "Типы стен";
                        List<String> _ColumnName = new List<String>() { "Марка", "Толщина, мм", "Шаг верт, мм", "Шаг гор, мм"};
                        List<Int32> _ColumnWidth = new List<Int32>() { 300, 80, 80, 80};
                        ColumnName.AddRange(_ColumnName);
                        ColumnWidth.AddRange(_ColumnWidth);
                        break;
                    }
            }

            foreach (String S in ColumnName)
            {
                lvDetails.Columns.Add(S);
            }

            for (int i = 0; i < lvDetails.Columns.Count; i++)
            {
                lvDetails.Columns[i].Width= ColumnWidth[i];
            }

            for (int i=1; i<lvDetails.Columns.Count; i++)
            {
                lvDetails.Columns[i].TextAlign = HorizontalAlignment.Center;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            /*switch (FormType) //
            {
                case "Buildings":
                    {
                    break;
                    }
                case "Levels":
                    {
                    break;
                    }
                case "Walls":
                    {
                    break;
                    }
                case "WallTypes":
                    {
                    break;
                    }
                    */
            switch (FormType) //
            {
                case "Buildings":
                    {
                        //Добавил комментарий
                        //Еще один комментарий для гита
                        //Next comment
                        break;
                    }
                case "Levels":
                    {
                        break;
                    }
                case "Walls":
                    {
                        break;
                    }
                case "WallTypes":
                    {
                        break;
                    }
            }

        }
    }
}
