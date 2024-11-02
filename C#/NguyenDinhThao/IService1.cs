using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NguyenDinhThao
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string InsertUserDetails(UserDetails userInfo);
        [OperationContract]
        DataSet SelectUserDetails();
        [OperationContract]
        bool DeleteUserDetails(UserDetails userInfo);
        [OperationContract]
        DataSet UpdateUserDetails(UserDetails userInfo);
        [OperationContract]
        void UpdateRegistrationTable(UserDetails userInfo);
        [OperationContract]
        DataSet SearchEmployeeDetails(string searchText);
        [OperationContract]
        bool CheckIfMatpExists(int ma);

    }
    public class UserDetails
    {
        int Maxe;
        string Tenxe;
        string Soluong;
        string Loaixe;
        [DataMember]
        public int getMaxe
        {
            get { return Maxe; }
            set { Maxe = value; }
        }
        [DataMember]
        public string getTenxe
        {
            get { return Tenxe; }
            set { Tenxe = value; }
        }
        [DataMember]
        public string getSoluong
        {
            get { return Soluong; }
            set { Soluong = value; }
        }
        public string getLoaixe
        {
            get { return Loaixe; }
            set { Loaixe = value; }
        }

    }
}
