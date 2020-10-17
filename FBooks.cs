using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFQuanlithuvien
{
    public partial class FBooks : Form
    {
        QuanLyThuVienEntities db = new QuanLyThuVienEntities();
        List<int> idToDelete = new List<int>();
        public FBooks()
        {
            InitializeComponent();
            idToDelete.Clear();
            loadListDelete();
            LoadBookDatas();
            LoadCbCategory();

        }

        private void LoadCbCategory()
        {
            List<Category> listCategory = db.Categories.ToList();

            foreach (Category item in listCategory)
            {
                cbbCategory.Items.Add(item.category_name);
            }
        }
        private void loadListDelete()
        {
            var list = db.Books.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                Book item = list[i];
                if (item.BK_delete == true) idToDelete.Add(item.Book_Id);
            }
        }
        private void LoadBookDatas()
        {
            dtgvBooks.Rows.Clear();
            var list = db.Books.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                Book item = list[i];
                if (item.BK_delete == true) continue;
                int categoryid = (int)item.category_id;
                Category category = db.Categories.Where(w => w.category_id == categoryid).SingleOrDefault();

                string[] row = { item.Book_Id + "", item.Book_Name, item.Author_Name,
                    category.category_name, item.Year_of_release + "" };
                dtgvBooks.Rows.Add(row);
            }

        }
        /*private void LoadBookDatasFind(Book db)
        {
            dtgvBooks.Rows.Clear();
            var list = db.Books.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                Book item = list[i];
                int categoryid = (int)item.category_id;
                Category category = db.Categories.Where(w => w.category_id == categoryid).SingleOrDefault();

                string[] row = { item.Book_Id + "", item.Book_Name, item.Author_Name,
                    category.category_name, item.Year_of_release + "" };
                dtgvBooks.Rows.Add(row);
            }

        }*/

        private void FBooks_Load(object sender, EventArgs e)
        {

        }
        private void Add()
        {
            Book newBook = new Book();
            newBook.Book_Name = txtBookName.Text;
            newBook.Author_Name = txtAuthorName.Text;
            newBook.Year_of_release = int.Parse(txtYearOfRelease.Text);

            Category category = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault();
            newBook.category_id = category.category_id;

            db.Books.Add(newBook);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //loadListDelete();
            if (!idToDelete.Any()) Add();
            else
            {
                idToDelete.Sort();
                update(idToDelete[0]);
            }

            db.SaveChanges();

            LoadBookDatas();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idd = int.Parse(txtBookID.Text);
            idToDelete.Add(idd);

            Book newBook = db.Books.Where(w => w.Book_Id == idd).SingleOrDefault();
            newBook.BK_delete = true;

            db.SaveChanges();
            LoadBookDatas();
        }
        private void update(int id)
        {
            Book newBook = db.Books.Where(w => w.Book_Id == id).SingleOrDefault();
            newBook.Book_Name = txtBookName.Text;
            newBook.Author_Name = txtAuthorName.Text;
            newBook.Year_of_release = int.Parse(txtYearOfRelease.Text);

            Category category = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault();
            newBook.category_id = category.category_id;
            newBook.BK_delete = false;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {

            update(int.Parse(txtBookID.Text));
            db.SaveChanges();
            LoadBookDatas();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            /*//Book newBook = new Book();
            //newBook.Book_Name = txtBookName.Text;
            //newBook.Author_Name = txtAuthorName.Text;
            //newBook.Year_of_release = int.Parse(txtYearOfRelease.Text);
            string Name = txtBookName.Text;
            Category category = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault();
            //newBook.category_id = category.category_id;

            Book findBook = db.Books.Where(w => w.Book_Name == Name).SingleOrDefault();
            dtgvBooks.Columns.*/

        }

     

        /*private void dtgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgvBooks.CurrentCell.ColumnIndex.Equals(5) && e.RowIndex != -1)
            {
                if (dtgvBooks.CurrentCell != null && dtgvBooks.CurrentCell.Value != null)
                {
                    MessageBox.Show(dtgvBooks.CurrentCell.Value.ToString());
                }
            }
        }*/
    }
}
