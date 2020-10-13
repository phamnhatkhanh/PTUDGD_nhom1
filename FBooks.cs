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
        List<int> idToDelete;
        public FBooks()
        {
            InitializeComponent();
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

        private void LoadBookDatas()
        {
            dtgvBooks.Rows.Clear();
            var list = db.Books.ToList();
            
            for(int i=0; i< list.Count; i++)
            {
                Book item = list[i];
                int categoryid = (int)item.category_id;
                Category category = db.Categories.Where(w => w.category_id == categoryid).SingleOrDefault();

                string[] row = { item.Book_Id + "", item.Book_Name, item.Author_Name,
                    category.category_name, item.Year_of_release + "" };
                dtgvBooks.Rows.Add(row);
            }

        }

        private void FBooks_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            idToDelete.Sort();
            Book newBook = new Book();
            if (idToDelete != null)
            {
                newBook.Book_Id = idToDelete[0];
                idToDelete.RemoveAt(0);
            }
            newBook.Book_Name = lblBookName.Text;
            newBook.Author_Name = lblAuthorName.Text;
            newBook.Year_of_release = int.Parse(lblYearOfRelease.ToString());

            Category category = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault();
            newBook.category_id = category.category_id;

            db.Books.Add(newBook);
            db.SaveChanges();
            LoadBookDatas();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idd = int.Parse(lblBookID.Text);
            idToDelete.Append(idd);
            Book newBook = new Book() { Book_Id = idd };
            db.Books.Remove(newBook);
            db.SaveChanges();
            LoadBookDatas();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Book newBook = new Book();
            int id = int.Parse(lblBookID.Text);
            newBook.Book_Name = lblBookName.Text;
            newBook.Author_Name = lblAuthorName.Text;
            newBook.Year_of_release = int.Parse(lblYearOfRelease.ToString());

            Category category = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault();
            newBook.category_id = category.category_id;

            Book book = db.Books.Where(w => w.Book_Id == id).SingleOrDefault();
            db.SaveChanges();
            LoadBookDatas();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Book newBook = new Book();
            newBook.Book_Name = lblBookName.Text;
            newBook.Author_Name = lblAuthorName.Text;
            newBook.Year_of_release = int.Parse(lblYearOfRelease.ToString());

            Category category = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault();
            newBook.category_id = category.category_id;
            Book findBook=db.Books.Find(newBook);
            dtgvBooks.DataSource = findBook;

        }
    }
}
