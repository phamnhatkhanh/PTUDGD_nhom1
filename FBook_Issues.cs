using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WFQuanlithuvien
{
    public partial class FIssuedBooks : Form
    {
        QuanLyThuVien1Entities db = new QuanLyThuVien1Entities();
        List<int> idToDelete = new List<int>();
        List<Book_Issues> listbook = new List<Book_Issues>();
        int sizeOfBook;
        List<int> a = new List<int>();
        List<int> b = new List<int>();
        bool checkSave = true;

        public FIssuedBooks()
        {
            InitializeComponent();
            loadListDelete();
            LoadBookDatas();
            listbook = db.Book_Issues.ToList();
            sizeOfBook = listbook.Count;

        }
        private void saveDatas()
        {
            var list = db.Book_Issues.ToList();
            if (a.Count != 0)
            {
                a.Sort();
                for (int i = 0; i < a.Count; i++)
                {
                    db.Book_Issues.Add(listbook[a[i] - 1]);
                    
                }
            }
            list = db.Book_Issues.ToList();
            if (b.Count != 0)
            {
                b.Sort();
                for (int i = 0; i < b.Count; i++)
                {MessageBox.Show(i.ToString());
                    list[b[i]] = listbook[b[i]];
                }
            }
            checkSave = true;
            db.SaveChanges();

        }
        private void loadListDelete()
        {
            var list = db.Book_Issues.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                Book_Issues item = list[i];
                if (item.Bki_Delete == true) idToDelete.Add(item.Bk_Issue_Id);
            }
            idToDelete.Sort();

        }
        private void LoadBook()
        {
            dtgvIssue_Books.Rows.Clear();

            for (int i = 0; i < listbook.Count; i++)
            {
                Book_Issues item = listbook[i];
                if (item.Bki_Delete == true) continue;

                string[] row = { item.Bk_Issue_Id + "", item.Book_Id + "", item.User_id_ + "",
                        item.Date_Of_Issue+ "", item.Date_Of_Return + "" };
                dtgvIssue_Books.Rows.Add(row);
            }

        }
        private void LoadBookDatas()
        {
            dtgvIssue_Books.Rows.Clear();
            var list = db.Book_Issues.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                Book_Issues item = list[i];
                if (item.Bki_Delete == true) continue;

                string[] row = { item.Bk_Issue_Id + "", item.Book_Id + "", item.User_id_ + "",
                        item.Date_Of_Issue+ "", item.Date_Of_Return + "" };
                dtgvIssue_Books.Rows.Add(row);
            }

        }
        private void FIssueBooks(object sender, EventArgs e)
        {

        }

        //Hàm Add Issue Books==========================================//
        private void Add()
        {
            Book_Issues newBook_Issues = new Book_Issues();
            newBook_Issues.Book_Id = int.Parse(txtBookID.Text);
            newBook_Issues.User_id_ = int.Parse(txtUserID.Text);

            DateTime dt = dtpDate_Of_Issue.Value.Date;
            newBook_Issues.Date_Of_Issue = dt;
            DateTime dt1 = dtpDate_Of_Return.Value.Date;
            if (ChBReturn.Checked == true)
            {
                newBook_Issues.Date_Of_Return = dt1;
            }

            newBook_Issues.Bki_Delete = false;
            //db.Book_Issues.Add(newBook_Issues);
            listbook.Add(newBook_Issues);
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
                if (!idToDelete.Any()) Add();
                else
                {
                    MessageBox.Show(idToDelete[0].ToString());
                    update(idToDelete[0]);
                    idToDelete.RemoveAt(0);
                }
                LoadBook();
                //db.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Chưa nhập dữ liệu");
            }

            
        }

        //Hàm Delete Issue Books==========================================//
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idd = int.Parse(txtBK_Issue_ID.Text);
            idToDelete.Add(idd);

            Book_Issues newBook_Issues = listbook.Where(w => w.Bk_Issue_Id == idd).SingleOrDefault();
            newBook_Issues.Bki_Delete = true;

            //db.SaveChanges();
            checkSave = false;
            LoadBook();
            b.Add(idd);
        }

        //Hàm Update Issue Books==========================================//
        private void update(int id)
        {
            try
            {
                Book_Issues newBook_Issues = listbook.Where(w => w.Bk_Issue_Id == id).SingleOrDefault();
                newBook_Issues.Book_Id = int.Parse(txtBookID.Text);
                newBook_Issues.User_id_ = int.Parse(txtUserID.Text);

                DateTime dt = dtpDate_Of_Issue.Value.Date;
                newBook_Issues.Date_Of_Issue = dt;
                DateTime dt1 = dtpDate_Of_Return.Value.Date;
                newBook_Issues.Date_Of_Return = dt1;
                newBook_Issues.Bki_Delete = false;
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
            int n = int.Parse(txtBK_Issue_ID.Text);
            if (listbook[n - 1].Bki_Delete == false) MessageBox.Show("Không tìm thấy ID");
            update(n);
            //db.SaveChanges();
            LoadBook();
        }

        //Hàm Find Issue Books==========================================//
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtBK_Issue_ID.Text != null && !string.IsNullOrWhiteSpace(txtBK_Issue_ID.Text))
            {
                int idd_issue = int.Parse(txtBK_Issue_ID.Text);
                dtgvIssue_Books.Rows.Clear();
                var list = from book_Issues in db.Book_Issues where book_Issues.Bk_Issue_Id == idd_issue select book_Issues;

                foreach (Book_Issues item in list)
                {
                    if (item.Bki_Delete == true) continue;

                    string[] row = { item.Bk_Issue_Id + "", item.Book_Id + "", item.User_id_ + "",
                    item.Date_Of_Issue + "", item.Date_Of_Return + "" };
                    dtgvIssue_Books.Rows.Add(row);
                }
            }
            else if (txtBookID.Text != null && !string.IsNullOrWhiteSpace(txtBookID.Text))
            {
                int idd_book = int.Parse(txtBookID.Text);
                dtgvIssue_Books.Rows.Clear();
                var list = from book_Issues in db.Book_Issues where book_Issues.Book_Id == idd_book select book_Issues;

                foreach (Book_Issues item in list)
                {
                    if (item.Bki_Delete == true) continue;

                    string[] row = { item.Bk_Issue_Id + "", item.Book_Id + "", item.User_id_ + "",
                    item.Date_Of_Issue + "", item.Date_Of_Return + "" };
                    dtgvIssue_Books.Rows.Add(row);
                }
            }
            else if (txtUserID.Text != null && !string.IsNullOrWhiteSpace(txtUserID.Text))
            {
                int idd_user = int.Parse(txtUserID.Text);
                dtgvIssue_Books.Rows.Clear();
                var list = from book_Issues in db.Book_Issues where book_Issues.User_id_ == idd_user select book_Issues;

                foreach (Book_Issues item in list)
                {
                    if (item.Bki_Delete == true) continue;

                    string[] row = { item.Bk_Issue_Id + "", item.Book_Id + "", item.User_id_ + "",
                    item.Date_Of_Issue + "", item.Date_Of_Return + "" };
                    dtgvIssue_Books.Rows.Add(row);
                }
            }
            else if (dtpDate_Of_Issue.Text != null && !string.IsNullOrWhiteSpace(dtpDate_Of_Issue.Text))
            {
                DateTime dt = DateTime.Parse(dtpDate_Of_Issue.Text);
                dtgvIssue_Books.Rows.Clear();
                var list = from book_Issues in db.Book_Issues where book_Issues.Date_Of_Issue >= dt select book_Issues;

                foreach (Book_Issues item in list)
                {
                    if (item.Bki_Delete == true) continue;

                    string[] row = { item.Bk_Issue_Id + "", item.Book_Id + "", item.User_id_ + "",
                    item.Date_Of_Issue + "", item.Date_Of_Return + "" };
                    dtgvIssue_Books.Rows.Add(row);
                }
            }
            else if (dtpDate_Of_Return.Text != null && !string.IsNullOrWhiteSpace(dtpDate_Of_Return.Text))
            {
                DateTime dt = DateTime.Parse(dtpDate_Of_Return.Text);
                dtgvIssue_Books.Rows.Clear();
                var list = from book_Issues in db.Book_Issues where book_Issues.Date_Of_Return >= dt select book_Issues;

                foreach (Book_Issues item in list)
                {
                    if (item.Bki_Delete == true) continue;

                    string[] row = { item.Bk_Issue_Id + "", item.Book_Id + "", item.User_id_ + "",
                    item.Date_Of_Issue + "", item.Date_Of_Return + "" };
                    dtgvIssue_Books.Rows.Add(row);
                }
            }
            else
            {
                MessageBox.Show("chua nhap du lieu");
            }
        }

        //Hàm CellClick Issue Books==========================================//
        private void dtgvIssue_Books_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dtgvIssue_Books.CurrentRow.Index;

            txtBK_Issue_ID.Text = dtgvIssue_Books.Rows[i].Cells[0].Value.ToString();
            txtBookID.Text = dtgvIssue_Books.Rows[i].Cells[1].Value.ToString();
            txtUserID.Text = dtgvIssue_Books.Rows[i].Cells[2].Value.ToString();

            //DateTime dt = dtpDate_Of_Issue.Value.Date;
            dtpDate_Of_Issue.Text = dtgvIssue_Books.Rows[i].Cells[3].Value.ToString();
            dtpDate_Of_Return.Text = dtgvIssue_Books.Rows[i].Cells[4].Value.ToString();

        }

        //Closing
        private void FIssuedBooks_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkSave == false)
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
