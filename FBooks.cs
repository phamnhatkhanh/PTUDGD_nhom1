using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFQuanlithuvien
{
    public partial class FBooks : Form
    {
        QuanLyThuVien1Entities db = new QuanLyThuVien1Entities();
        List<int> idToDelete = new List<int>();
        List<Book> listbook = new List<Book>();
        int sizeOfBook;
        List<int> a= new List<int>();
        List<int> b= new List<int>();
        bool checkSave = true;

        public FBooks()
        {
            InitializeComponent();
            idToDelete.Clear();
            loadListDelete();
            LoadBookDatas();
            listbook = db.Books.ToList();
            //sizeOfBook = listbook[listbook.Count-1].Book_Id;
            sizeOfBook = listbook.Count;
            LoadCbCategory();
        }
        private void saveDatas()
        {
            var list = db.Books.ToList();
            if (a.Count!=0)
            {a.Sort();
                for (int i = 0; i < a.Count; i++)
                {
                    db.Books.Add(listbook[a[i] - 1]);
                    MessageBox.Show(i.ToString());
                }
            }
            list = db.Books.ToList();
            if (b.Count != 0)
            {b.Sort();
                for (int i = 0; i < b.Count; i++)
                {
                    list[b[i]] = listbook[b[i]];
                }
            }
            checkSave = true;
            db.SaveChanges();

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
            idToDelete.Sort();
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
        private void LoadBook()
        {
            dtgvBooks.Rows.Clear();

            for (int i = 0; i < listbook.Count; i++)
            {
                Book item = listbook[i];
                if (item.BK_delete == true) continue;
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

        //Hàm Add Sách==========================================//
        private void Add()
        {
            Book newBook = new Book();
            newBook.Book_Name = txtBookName.Text;
            newBook.Author_Name = txtAuthorName.Text;
            newBook.Year_of_release = int.Parse(txtYearOfRelease.Text);

            Category category = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault();
            newBook.category_id = category.category_id;

            newBook.BK_delete = false;
            listbook.Add(newBook);
            //db.Books.Add(newBook);
            sizeOfBook += 1;
            a.Add(sizeOfBook);
            checkSave = false;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //loadListDelete();
            try
            {
                if ((string.IsNullOrWhiteSpace(txtBookName.Text) && string.IsNullOrWhiteSpace(txtAuthorName.Text) && string.IsNullOrWhiteSpace(txtYearOfRelease.Text)))
                {
                    MessageBox.Show("Chưa nhập dữ liệu");
                }
                else if (!idToDelete.Any()) Add();
                else
                {
                    update(idToDelete[0]);
                    idToDelete.RemoveAt(0);
                }

                //db.SaveChanges();

                LoadBook();
            }
            catch (Exception)
            {
                MessageBox.Show("Chưa nhập dữ liệu");
            }
        }

        //Hàm Find Sách==========================================//
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtBookID.Text != null && !string.IsNullOrWhiteSpace(txtBookID.Text))
            {
                int idd = int.Parse(txtBookID.Text);
                dtgvBooks.Rows.Clear();
                var list = from book in db.Books where book.Book_Id == idd select book;

                foreach (Book item in list)
                {
                    if (item.BK_delete == true) continue;
                    int categoryid = (int)item.category_id;
                    Category category = db.Categories.Where(w => w.category_id == categoryid).SingleOrDefault();

                    string[] row = { item.Book_Id + "", item.Book_Name, item.Author_Name,
                    category.category_name, item.Year_of_release + "" };
                    dtgvBooks.Rows.Add(row);
                }
            }
            else if (txtBookName.Text != null && !string.IsNullOrWhiteSpace(txtBookName.Text))
            {
                dtgvBooks.Rows.Clear();
                var list = from book in db.Books where book.Book_Name == txtBookName.Text select book;

                foreach (Book item in list)
                {
                    if (item.BK_delete == true) continue;
                    int categoryid = (int)item.category_id;
                    Category category = db.Categories.Where(w => w.category_id == categoryid).SingleOrDefault();

                    string[] row = { item.Book_Id + "", item.Book_Name, item.Author_Name,
                    category.category_name, item.Year_of_release + "" };
                    dtgvBooks.Rows.Add(row);
                }
            }
            else if (txtAuthorName.Text != null && !string.IsNullOrWhiteSpace(txtAuthorName.Text))
            {
                dtgvBooks.Rows.Clear();
                var list = from book in db.Books where book.Author_Name == txtAuthorName.Text select book;

                foreach (Book item in list)
                {
                    if (item.BK_delete == true) continue;
                    int categoryid = (int)item.category_id;
                    Category category = db.Categories.Where(w => w.category_id == categoryid).SingleOrDefault();

                    string[] row = { item.Book_Id + "", item.Book_Name, item.Author_Name,
                    category.category_name, item.Year_of_release + "" };
                    dtgvBooks.Rows.Add(row);
                }
            }
            else if (cbbCategory.Text != null && !string.IsNullOrWhiteSpace(cbbCategory.Text))
            {
                dtgvBooks.Rows.Clear();
                int idCategory = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault().category_id;
                var list = from book in db.Books where book.category_id == idCategory select book;

                foreach (Book item in list)
                {
                    if (item.BK_delete == true) continue;
                    int categoryid = (int)item.category_id;
                    Category category = db.Categories.Where(w => w.category_id == categoryid).SingleOrDefault();

                    string[] row = { item.Book_Id + "", item.Book_Name, item.Author_Name,
                    category.category_name, item.Year_of_release + "" };
                    dtgvBooks.Rows.Add(row);
                }
            }
            else if (txtYearOfRelease.Text != null && !string.IsNullOrWhiteSpace(txtYearOfRelease.Text))
            {
                dtgvBooks.Rows.Clear();
                int yearBook = int.Parse(txtYearOfRelease.Text);
                var list = from book in db.Books where book.Year_of_release == yearBook select book;

                foreach (Book item in list)
                {
                    if (item.BK_delete == true) continue;
                    int categoryid = (int)item.category_id;
                    Category category = db.Categories.Where(w => w.category_id == categoryid).SingleOrDefault();

                    string[] row = { item.Book_Id + "", item.Book_Name, item.Author_Name,
                    category.category_name, item.Year_of_release + "" };
                    dtgvBooks.Rows.Add(row);
                }
            }
            else
            {
                MessageBox.Show("Khong tim thay hoac chua nhap du lieu");
            }

        }

        //Hàm Delete Sách==========================================//
        private void btnDelete_Click(object sender, EventArgs e)
        {
            /*int idd = int.Parse(txtBookID.Text);
            idToDelete.Add(idd);

            Book newBook = db.Books.Where(w => w.Book_Id == idd).SingleOrDefault();
            newBook.BK_delete = true;

            db.SaveChanges();
            LoadBookDatas();*/
            int idd = int.Parse(txtBookID.Text);
            idToDelete.Add(idd);

            Book newBook = listbook.Where(w => w.Book_Id == idd).SingleOrDefault();
            newBook.BK_delete = true;

            checkSave = false;
            LoadBook();
            a.Add(idd);

            
        }

        //Hàm Update Sách==========================================//
        private void update(int id)
        {
            try
            {
                Book newBook = listbook.Where(w => w.Book_Id == id).SingleOrDefault();
                newBook.Book_Name = txtBookName.Text;
                newBook.Author_Name = txtAuthorName.Text;
                newBook.Year_of_release = int.Parse(txtYearOfRelease.Text);

                Category category = db.Categories.Where(w => w.category_name == cbbCategory.Text).SingleOrDefault();
                newBook.category_id = category.category_id;
                newBook.BK_delete = false;
                b.Add(id);
                checkSave = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Chưa nhập dữ liệu");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {

            update(int.Parse(txtBookID.Text));
            //db.SaveChanges();
            LoadBook();
        }

        //Hàm CellClick Sách==========================================//
        private void dtgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dtgvBooks.CurrentRow.Index;

            txtBookID.Text = dtgvBooks.Rows[i].Cells[0].Value.ToString();
            txtBookName.Text = dtgvBooks.Rows[i].Cells[1].Value.ToString();
            txtAuthorName.Text = dtgvBooks.Rows[i].Cells[2].Value.ToString();

            cbbCategory.Text = dtgvBooks.Rows[i].Cells[3].Value.ToString();
            txtYearOfRelease.Text = dtgvBooks.Rows[i].Cells[4].Value.ToString();
        }

        //Closing
        private void FBooks_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(checkSave==false)
            {
                DialogResult dialogResult = MessageBox.Show("Sure", "Some Title", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveDatas();
                }
                
            }
        }
    }
}
