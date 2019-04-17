using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Entites;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для CRUDWindow.xaml
    /// </summary>
    public partial class CRUDWindow : Window
    {
        private string conStr = ConfigurationManager.AppSettings["conStr"];
        private SqlConnection _connect;
        private void updateDT()
        {
            //List<UserModel> userList = null;// _context
                //.Users.Select(u => new UserModel
                //{
                //    Id = u.Id,
                //    FirstName = u.FirstName,
                //    LastName = u.LastName,
                //    Email = u.Email,
                //    Password = u.Password
                //}).ToList();

            

            //DataColumn id = new DataColumn("id", typeof(int));
            ////id.Caption = "hello";
            //DataColumn firstname = new DataColumn("firstname", typeof(string));
            //DataColumn lastname = new DataColumn("lastname", typeof(string));
            //DataColumn email = new DataColumn("email", typeof(string));
            //DataColumn password = new DataColumn("password", typeof(string));
            //dt.Columns.Add(id);
            //dt.Columns.Add(firstname);
            //dt.Columns.Add(lastname);
            //dt.Columns.Add(email);
            //dt.Columns.Add(password);
            //foreach (var user in userList)
            //{
            //    DataRow row = dt.NewRow();
            //    row[0] = user.Id;
            //    row[1] = user.FirstName;
            //    row[2] = user.LastName;
            //    row[3] = user.Email;
            //    row[4] = user.Password;
            //    dt.Rows.Add(row);
            //}
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _connect.Open();
                    SqlCommand cmd = new SqlCommand("ShowValues", _connect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var value = cmd.ExecuteReader();
                    if (value != null)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(value);
                        myDT.ItemsSource = dt.DefaultView;
                    }
                    _connect.Close();
                    scope.Complete();
                }
            }
            catch
            {
                throw new Exception("transaction error");
            }
        }
        public CRUDWindow()
        {
            InitializeComponent();
            _connect = new SqlConnection(conStr);
        }

        private void MyDT_Loaded(object sender, RoutedEventArgs e)
        {
            updateDT();
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            //_context.Users.Add(new User
            //{
            //    FirstName = firstname_txtbx.Text,
            //    LastName = lastname_txtbx.Text,
            //    Email = email_txtbx.Text,
            //    Password = password_txtbx.Text
            //});
            //_context.SaveChanges();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _connect.Open();
                    SqlCommand cmd = new SqlCommand("AddUser", _connect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter;

                    parameter = cmd.Parameters.Add("@fn", SqlDbType.NVarChar, 50);
                    parameter.Value = firstname_txtbx.Text;

                    parameter = cmd.Parameters.Add("@ln", SqlDbType.NVarChar, 50);
                    parameter.Value = lastname_txtbx.Text;

                    parameter = cmd.Parameters.Add("@em", SqlDbType.NVarChar, 50);
                    parameter.Value = email_txtbx.Text;

                    parameter = cmd.Parameters.Add("@pw", SqlDbType.NVarChar, 50);
                    parameter.Value = password_txtbx.Text;

                    int added = cmd.ExecuteNonQuery();
                    MessageBox.Show($"add {added.ToString()} user(s)");

                    _connect.Close();
                    scope.Complete();
                }
            }
            catch
            {
                throw new Exception("transaction error");
            }
            updateDT();

            email_txtbx.Text = "";
            firstname_txtbx.Text = "";
            lastname_txtbx.Text = "";
            password_txtbx.Text = "";

            add_btn.IsEnabled = false;
            update_btn.IsEnabled = true;
        }

        private void MyDT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                user_id_txtbx.Text = dr["id"].ToString();
                firstname_txtbx.Text = dr["firstname"].ToString();
                lastname_txtbx.Text = dr["lastname"].ToString();
                email_txtbx.Text = dr["email"].ToString();
                password_txtbx.Text = dr["password"].ToString();

                add_btn.IsEnabled = false;
                update_btn.IsEnabled = true;
                delete_btn.IsEnabled = true;
            }
        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {

            User upd = null;

            try
            {
                //upd = _context.Users.Where(u => u.Id.ToString() == user_id_txtbx.Text).First();
                
                //upd.FirstName = firstname_txtbx.Text;
                //upd.LastName = lastname_txtbx.Text;
                //upd.Email = email_txtbx.Text;
                //upd.Password = password_txtbx.Text;
                //_context.SaveChanges();

                user_id_txtbx.Text = "";
                email_txtbx.Text = "";
                firstname_txtbx.Text = "";
                lastname_txtbx.Text = "";
                password_txtbx.Text = "";

                myDT.DataContext = null;
                MyDT_Loaded(sender, e);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            updateDT();
            add_btn.IsEnabled = false;
            delete_btn.IsEnabled = false;
            update_btn.IsEnabled = false;
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //_context.Users.Remove(_context.Users.Where(u => u.Id.ToString() == user_id_txtbx.Text).First());
                //_context.SaveChanges();

                user_id_txtbx.Text = "";
                email_txtbx.Text = "";
                firstname_txtbx.Text = "";
                lastname_txtbx.Text = "";
                password_txtbx.Text = "";

                myDT.DataContext = null;
                MyDT_Loaded(sender, e);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            updateDT();
            add_btn.IsEnabled = false;
            delete_btn.IsEnabled = false;
            update_btn.IsEnabled = false;
        }

        private void Resete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //_context.Users.RemoveRange(_context.Users.Where(u => u.Id.ToString() != null));
                //_context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            updateDT();

            add_btn.IsEnabled = false;
            update_btn.IsEnabled = false;
            delete_btn.IsEnabled = false;
        }

        private void Lastname_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            add_btn.IsEnabled = true;
        }

        private void Firstname_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            add_btn.IsEnabled = true;
        }

        private void Email_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            add_btn.IsEnabled = true;
        }

        private void Password_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            add_btn.IsEnabled = true;
        }
    }
}
