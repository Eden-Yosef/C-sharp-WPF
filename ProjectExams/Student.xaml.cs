using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;


namespace ProjectExams
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : Window
    {
        IExamsRepository repo;
        string id_stu;
        public Student(IExamsRepository repo, string id)
        {
            InitializeComponent();
            this.repo = repo;
            this.id_stu = id;

        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            repo.LoadData("AppData/exams.json");
            List<Exam> listE = new List<Exam>();
            foreach (Exam exam in repo.Exams)
            {
                for(int i=0; i< exam.Grades.Count; i++)
                {
                    if (exam.Grades[i][1].Equals(this.id_stu))
                    {
                        flag = false;
                    }
                }
                if( flag == true)
                {
                    listE.Add(exam);
                }
            }
            this.listBoxExams.ItemsSource = listE;
        }

        private void OnSearchTextChanged(object sender, RoutedEventArgs e)
        {
            string searchText = searchBox.Text.ToLower();
            foreach (var item in listBoxExams.Items)
            {
                ListBoxItem listBoxItem = listBoxExams.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

                if (listBoxItem != null)
                {
                    if ((item as Exam).Name.ToLower().Contains(searchText))
                    {
                        listBoxItem.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        listBoxItem.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void listBoxExams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (this.listBoxExams.SelectedItem is Exam s)
            {
                this.txtName.Text = s.Name;
                this.nameTeacher.Text = s.Teacher;
                this.beginningTime.Text = s.StartTime.ToString();
                this.totalTime.Text = s.TotalTime.ToString();
                this.date.Text = s.Date.ToString("d");

            }

        }


        private void startExam_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxExams.SelectedItem is Exam s)
            {
                PageExam pageExam = new PageExam(new ExamsDictionaryRepository(), s.ID, this.id_stu);
                this.Close();
                pageExam.Show();
            }
            
        }

    }


}