using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NguyenDinhThao{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public DataSet SelectUserDetails()
        {
            SqlConnection con = new SqlConnection(@"Data Source=NIT;Initial Catalog=QL_Benxe;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from QL", con);
            SqlDataAdapter dta = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            dta.Fill(ds);
            cmd.ExecuteNonQuery();
            con.Close();
            return ds;

        }
        public string InsertUserDetails(UserDetails userInfo)
        {
            string Message;
            SqlConnection con = new SqlConnection(@"Data Source=NIT;Initial Catalog=QL_Benxe;Integrated Security=True");
            con.Open();
            SqlCommand sqlCommand = new SqlCommand("insert into QL(Maxe,Tenxe,Soluong,Loaixe) values(@Maxe,@Tenxe,@Soluong,@Loaixe)", con);
            SqlCommand cmd = sqlCommand;
            cmd.Parameters.AddWithValue("@Maxe", userInfo.getMaxe);
            cmd.Parameters.AddWithValue("@Tenxe", userInfo.getTenxe);
            cmd.Parameters.AddWithValue("@Soluong", userInfo.getSoluong);
            cmd.Parameters.AddWithValue("Loaixe", userInfo.getLoaixe);


            int result = cmd.ExecuteNonQuery();
            if (result == 1)
            {
                Message = userInfo.getMaxe + " Details inserted successfully";
            }
            else
            {
                Message = userInfo.getMaxe + " Details not inserted successfully";
            }
            con.Close();
            return Message;
        }
        public void UpdateRegistrationTable(UserDetails userInfo)
        {
            SqlConnection con = new SqlConnection(@"Data Source=NIT;Initial Catalog=QL_Benxe;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("update SV set Maxe=@Maxe,Tenxe=@Tenxe,Sloluong=@Soluong,Loaixe=@Loaixe where Maxe=@Maxe", con);
            cmd.Parameters.AddWithValue("@Maxe", userInfo.getMaxe);
            cmd.Parameters.AddWithValue("@Tenxe", userInfo.getTenxe);
            cmd.Parameters.AddWithValue("@Soluong", userInfo.getSoluong);
            cmd.Parameters.AddWithValue("Loaixe", userInfo.getLoaixe);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public bool DeleteUserDetails(UserDetails userInfo)
        {
            SqlConnection con = new SqlConnection(@"Data Source=NIT;Initial Catalog=QL_Benxe;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from QL where Maxe=@Maxe", con);
            cmd.Parameters.AddWithValue("@Maxe", userInfo.getMaxe);
            cmd.ExecuteNonQuery();
            con.Close();
            return true;
        }
        public DataSet UpdateUserDetails(UserDetails userInfo)
        {
            SqlConnection con = new SqlConnection(@"Data Source=NIT;Initial Catalog=QLSV;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from tp where Maxe=@Maxe", con);
            cmd.Parameters.AddWithValue("@Maxe", userInfo.getMaxe);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            cmd.ExecuteNonQuery();
            con.Close();
            return ds;
        }
        public DataSet SearchEmployeeDetails(string searchText)
        {
            DataSet result = new DataSet();
            using (SqlConnection connection = new SqlConnection(@"Data Source=NIT;Initial Catalog=QL_Benxe;Integrated Security=True"))
            {
                string query = "SELECT * FROM QL WHERE Maxe LIKE @searchText OR Tenxe LIKE @searchText OR Soluong LIKE @searchText OR Loaixe LIKE @searchText";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(result);
            }
            return result;
        }
        public bool CheckIfMatpExists(int ma)
        {
            // Thực hiện truy vấn SQL để kiểm tra xem mã sách có tồn tại trong cơ sở dữ liệu hay không
            using (SqlConnection connection = new SqlConnection(@"Data Source=NIT;Initial Catalog=QL_Benxe;Integrated Security=True"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Ql WHERE Maxe = @Maxe";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Maxe", ma);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
