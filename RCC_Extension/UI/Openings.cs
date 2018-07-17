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

namespace RCC_Extension.UI
{
    public partial class frmOpenings : Form
    {
        private List<Opening> InternalOpeningList;

        public frmOpenings(List<Opening> value)
        {
            InitializeComponent();

            InternalOpeningList = value;

            foreach (Opening NewOpening in InternalOpeningList)
            {
                NewListViewItemFromOpening(NewOpening);
            }
        }

        public frmOpenings()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //"добавил комментарий"
            Opening NewOpening = new Opening();
            NewOpening.Name = "Новый проем";
            NewOpening.Purpose = "АР";
            NewOpening.Height = 2200;
            NewOpening.Width = 1000;
            NewOpening.Left = 1000;
            NewOpening.Bottom = 0;
            NewOpening.AddEdgeLeft = true;
            NewOpening.AddEdgeRight = true;
            NewOpening.AddEdgeTop = true;
            NewOpening.AddEdgeBottom = false;

            NewOpening.MoveVert = true;
            NewOpening.QuantVertLeft = 2;
            NewOpening.QuantVertRight = 2;

            InternalOpeningList.Add(NewOpening);
            NewListViewItemFromOpening(NewOpening);
        }

        private void EditListViewItemFromOpening(ListViewItem Item, Opening Opening)
        {
            Item.SubItems.Clear();
            Item.Text = Opening.Name;
            Item.SubItems.Add(Opening.Purpose);
            Item.SubItems.Add(Convert.ToString(Opening.Width));
            Item.SubItems.Add(Convert.ToString(Opening.Height));
            Item.SubItems.Add(Convert.ToString(Opening.Left));
            Item.SubItems.Add(Convert.ToString(Opening.Bottom));
        }

        private ListViewItem NewListViewItemFromOpening(Opening Opening)
        {
            ListViewItem NewItem = new ListViewItem();
            EditListViewItemFromOpening(NewItem, Opening);
            lvOpenings.Items.Add(NewItem);
            return NewItem;
        }

        private void lvOpenings_DoubleClick(object sender, MouseEventArgs e)
        {
            //if (sender = lvOpenings & e.Clicks >= 2)
            //{
            //}
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            //При удалении нескольких объектов учитываем сдвижку номеров в коллекции вызванную удалении
            int i = 0;
            foreach (int j in lvOpenings.SelectedIndices)
            {
                InternalOpeningList.RemoveAt(j-i);
                lvOpenings.Items.RemoveAt(j-i);
                i++;
            }
            
        }

        private void lvOpenings_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            foreach (int i in lvOpenings.SelectedIndices)
            {
                frmOpening OpeningForm = new frmOpening(InternalOpeningList[i]);
                OpeningForm.ShowDialog();
                if (OpeningForm.DialogResult == DialogResult.OK)
                {
                    EditListViewItemFromOpening(lvOpenings.Items[i], InternalOpeningList[i]);
                }
            }
        }

        private void tsbClone_Click(object sender, EventArgs e)
        {
            foreach (int i in lvOpenings.SelectedIndices)
            {
                Opening Opening = (Opening)InternalOpeningList[i].Clone();
                InternalOpeningList.Add(Opening);
                NewListViewItemFromOpening(Opening);
            }
        }
    }
}
