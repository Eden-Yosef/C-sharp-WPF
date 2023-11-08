using Newtonsoft.Json;
using ProjectExams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace ProjectExams
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient clientApi;
        User user;

        public MainWindow()
        {
            InitializeComponent();

            clientApi = new HttpClient();
            clientApi.BaseAddress = new Uri("https://localhost:7142");
            user = new User();
        }
    

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtUsername.Text != null && this.txtPassword.Name != null)
            {
                GetUser(this.txtUsername.Text);
                if(this.txtUsername.Text == this.user.ID && this.txtPassword.Password == this.user.Password)
                {
                    if(this.user.IsStudent == false)
                    {
                        Teacher teacher = new Teacher(new ExamsDictionaryRepository());
                        this.Close();
                        teacher.Show();
                    }
                    else
                    {

                        Student student = new Student(new ExamsDictionaryRepository(), this.user.ID);
                        this.Close();
                        student.Show();
                    }
                   
                }

            }


        }

        public async Task GetUser(string id)
        {

            HttpResponseMessage response = await clientApi.GetAsync($"api/Users/{id}");
            if (response != null)
            {
                response.EnsureSuccessStatusCode();
                string? dataString = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(dataString);

                if (user != null)
                {
                    this.user = user;

                    return;
                }
            }
            return;
        }


    }
}


