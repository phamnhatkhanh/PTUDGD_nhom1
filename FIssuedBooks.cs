using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace WFQuanlithuvien
{
    public partial class FIssuedBooks : Form
    {
        public FIssuedBooks()
        {
            InitializeComponent();
        }
        SqlConnection connetFissedBook;
        string connetDbandVisual= @"Data Source=.\QUANLYTHUVIEN;Initial Catalog=QuanLyThuVien;Integrated Security=True";
        SqlCommand Interactive;
        SqlDataAdapter FillData = new SqlDataAdapter();
        DataTable TableBookissue=new DataTable();
        void LoadDt()
        {
            Interactive = connetFissedBook.CreateCommand();
            Interactive.CommandText = "SELECT * FROM dbo.Book_Issues";
            FillData.SelectCommand = Interactive;
            TableBookissue.Clear();
            FillData.Fill(TableBookissue);
            dtgvIBooks.DataSource = TableBookissue;
        }
        private void FIssuedBooks_Load(object sender, EventArgs e)
        {
            connetFissedBook = new SqlConnection(connetDbandVisual);
            connetFissedBook.Open();
            LoadDt();
            connetFissedBook.Close();
        }
        private void dtgvIBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i= dtgvIBooks.CurrentRow.Index;
            txtBk_Issue_id.Text = dtgvIBooks.Rows[i].Cells[0].Value.ToString();
            Return_dt.Text= dtgvIBooks.Rows[i].Cells[1].Value.ToString();
            Issue_dt.Text= dtgvIBooks.Rows[i].Cells[2].Value.ToString();
            txtBookID.Text= dtgvIBooks.Rows[i].Cells[3].Value.ToString();
            txtUserID.Text= dtgvIBooks.Rows[i].Cells[4].Value.ToString();
            txtDel.Text = dtgvIBooks.Rows[i].Cells[5].Value.ToString();
        }
        private void btuDel_Click(object sender, EventArgs e)
        {
            connetFissedBook.Open();
            Interactive = connetFissedBook.CreateCommand();
            Interactive.CommandText = "DELETE dbo.Book_Issues WHERE User_id_='"+txtUserID.Text.ToString()+"'";
            Interactive.ExecuteNonQuery();
            LoadDt();
            connetFissedBook.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            connetFissedBook.Open();
            Interactive = connetFissedBook.CreateCommand();
            Interactive.CommandText = "INSERT INTO dbo.Book_Issues VALUES('" + Issue_dt.Text + "', '" + Return_dt.Text + "','" + txtBookID.Text + "','" + txtUserID.Text + "','" + txtDel.Text + "')";
            Interactive.ExecuteNonQuery();
            LoadDt();
            connetFissedBook.Close();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            connetFissedBook.Open();
            Interactive = connetFissedBook.CreateCommand();
            Interactive.CommandText = "UPDATE dbo.Book_Issues SET Date_Of_Issue='"+Issue_dt.Text+ "',Date_Of_Return='" + Return_dt.Text + "',Book_Id='" + txtBookID.Text + "',User_id_='" + txtUserID.Text + "',Bki_Delete='" + txtDel.Text + "' WHERE Bk_Issue_Id='" + txtBk_Issue_id.Text + "'";
            Interactive.ExecuteNonQuery();
            LoadDt();
            connetFissedBook.Close();
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            connetFissedBook.Open();
            Interactive = connetFissedBook.CreateCommand();
            Interactive.CommandText = "SELECT *FROM dbo.Book_Issues WHERE User_id_='"+txtUserID.Text+"'";
            SqlDataReader SearchResults= Interactive.ExecuteReader();
            DataTable ResultsTable = new DataTable();
            ResultsTable.Load(SearchResults);
            dtgvIBooks.DataSource  = ResultsTable;
            connetFissedBook.Close();
        }
    }
}
