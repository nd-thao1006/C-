using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NguyenDinhThaoForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            showdata();
        }
        ServiceReference1.Service1Client objService = new ServiceReference1.Service1Client(); // Add service reference  
        private void showdata()  // To show the data in the DataGridView  
        {
            DataSet ds = new DataSet();
            ds = objService.SelectUserDetails();
            dataGridView1.DataSource = ds.Tables[0]; dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceReference1.UserDetails objuserdetail;

            if (string.IsNullOrWhiteSpace(txtt.Text) ||
                string.IsNullOrWhiteSpace(txthp.Text) ||
                string.IsNullOrWhiteSpace(txtk.Text) ||
                string.IsNullOrWhiteSpace(txtl.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            int Maxe;
            if (int.TryParse(txtm.Text, out Maxe))
            {
                objuserdetail = new ServiceReference1.UserDetails
                {
                    getMaxe = Maxe,
                    getTenxe = txtt.Text,
                    getSoluong = txthp.Text,
                    getLoaixe = txtk.Text,
                };
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã sách hợp lệ.");
                return;
            }

            // Thực hiện kiểm tra xem mã sách đã tồn tại hay chưa
            bool exists = objService.CheckIfMatpExists(MaSV);

            if (exists)
            {
                MessageBox.Show("Mã tác phẩm đã  tồn tại vui lòng chọn mã khác.");
                return;
            }

            // Nếu mã sách chưa tồn tại, thực hiện chèn dữ liệu vào cơ sở dữ liệu
            string message = objService.InsertUserDetails(objuserdetail);
            MessageBox.Show("Thêm thành công."); // Hiển thị thông báo kết quả

            // Hiển thị dữ liệu sau khi thêm
            showdata();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            ServiceReference1.UserDetails objuserdetail = new ServiceReference1.UserDetails();
            objuserdetail.getMaxe = (int)dataGridView1.CurrentRow.Cells[0].Value;
            objuserdetail.getTenxe = txtt.Text;
            objuserdetail.getSoluong = txthp.Text;
            objuserdetail.getLoaixe = txtk.Text;

            objService.UpdateRegistrationTable(objuserdetail); // To Update the Data  
            showdata();
            txtt.Text = "";
            txthp.Text = "";
            txtk.Text = "";
            txtl.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem DataGridView có hàng dữ liệu hay không
            if (dataGridView1.Rows.Count > 0)
            {
                // Hiển thị hộp thoại xác nhận xóa
                DialogResult result = MessageBox.Show("Bạn có muốn xóa?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Nếu người dùng chọn Yes
                if (result == DialogResult.Yes)
                {
                    // Lấy mã nhân viên từ hàng được chọn trong DataGridView
                    int Maxe = (int)dataGridView1.CurrentRow.Cells[0].Value;

                    // Tạo đối tượng UserDetails để xóa
                    ServiceReference1.UserDetails objuserdetail = new ServiceReference1.UserDetails
                    {
                        getMaxe = Maxe
                    };

                    // Thực hiện xóa dữ liệu
                    objService.DeleteUserDetails(objuserdetail);

                    // Hiển thị dữ liệu sau khi xóa
                    showdata();
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           this.Close();
        }
        private DataTable originalData;
        private void Form_Load(object sender, EventArgs e)
        {
            // Lưu trữ dữ liệu ban đầu từ DataGridView vào biến originalData
            originalData = (DataTable)dataGridView1.DataSource;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

            // Lấy chuỗi nhập từ TextBox
            string searchText = textBox6.Text.Trim();

            // Kiểm tra nếu chuỗi nhập là "0"
            if (searchText == "0")
            {
                // Hiển thị dữ liệu hiện tại trên DataGridView
                return;
            }

            // Gọi phương thức tìm kiếm từ dịch vụ WCF
            try
            {
                DataSet searchResult = objService.SearchEmployeeDetails(searchText);

                // Kiểm tra xem có kết quả tìm kiếm hay không
                if (searchResult != null && searchResult.Tables.Count > 0 && searchResult.Tables[0].Rows.Count > 0)
                {
                    // Hiển thị kết quả tìm kiếm trên DataGridView
                    dataGridView1.DataSource = searchResult.Tables[0];
                }
                else
                {
                    // Nếu không có kết quả tìm kiếm, khôi phục dữ liệu gốc
                    dataGridView1.DataSource = originalData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tìm kiếm: " + ex.Message);
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtm.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtt.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txthp.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtk.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtl.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }
    }
}
