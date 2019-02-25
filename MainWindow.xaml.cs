using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace SQLiteCRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
        SQLiteConnection con = new SQLiteConnection($"Data Source = C:\\Users\\{username}\\Desktop\\DB.sqlite3");

        public MainWindow()
        {
            InitializeComponent();
            
        }
        
        private void CreateDBBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(@"C:\Users\Damian\Desktop\DB.sqlite3"))
            {                
                SQLiteConnection.CreateFile($"C:\\Users\\{username}\\Desktop\\DB.sqlite3");
           
                con.Open();

                string createTable = "create table Data( name varchar(20), email varchar(30))";

                SQLiteCommand command = new SQLiteCommand(createTable, con);
                command.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("DB Created", "Info", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("DB Exist", "Info", MessageBoxButton.OK);

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();

                string add = $"insert into Data( name, email) values ('{NameTxtBox.Text}', '{EmailTxtBox.Text}')";

                SQLiteCommand command = new SQLiteCommand(add, con);
                command.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info", MessageBoxButton.OK);
            }
        }

        private void Loadbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();

                string query = "select rowid,name,email from Data";

                SQLiteCommand command = new SQLiteCommand(query, con);
                command.ExecuteNonQuery();

                SQLiteDataAdapter dataad = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable("Data");
                dataad.Fill(dt);
                Display.ItemsSource = dt.DefaultView;
                dataad.Update(dt);

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info", MessageBoxButton.OK);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                con.Open();

                string del = $"delete from Data where name ='{NameTxtBox.Text}' and email='{EmailTxtBox.Text}'";
               
                SQLiteCommand command = new SQLiteCommand(del, con);
                    command.ExecuteNonQuery();
               
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info", MessageBoxButton.OK);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            EditStackPanel.Visibility = Visibility;           
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();

                string update = $"update Data set name='{EditNameTxtBox.Text}', email='{EditEmailTxtBox.Text}' where name='{NameTxtBox.Text}'and email='{EmailTxtBox.Text}'";

                SQLiteCommand command = new SQLiteCommand(update, con);
                command.ExecuteNonQuery();

                con.Close();
                EditStackPanel.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Info", MessageBoxButton.OK);
            }
        }
    }
}
