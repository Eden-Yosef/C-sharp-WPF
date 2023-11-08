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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Path = System.IO.Path;
using Microsoft.Win32;
using System.IO;
using Image = System.Windows.Controls.Image;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ProjectExams
{
    /// <summary>
    /// Interaction logic for Teacher.xaml
    /// </summary>
    public partial class Teacher : Window
    {
        IExamsRepository repo;
        public Teacher(IExamsRepository repo)
        {
            InitializeComponent();
            this.repo = repo;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            repo.LoadData("AppData/exams.json");
            this.listBoxExams.ItemsSource = repo.Exams;
            this.listBoxQue.ItemsSource = repo.Questions;
            listBoxQue.DisplayMemberPath = "Name";


        }

        private void listBoxExams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (this.listBoxExams.SelectedItem is Exam s)
            {
                this.txtId.Text = s.ID;
                this.txtName.Text = s.Name;
                this.nameTeacher.Text = s.Teacher;
                this.beginningTime.Text = s.StartTime.ToString();
                this.totalTime.Text = s.TotalTime.ToString();
                this.date.Text = s.Date.ToString("d");
                this.listBoxQue.ItemsSource= s.Questions;

                List<int> ints = new List<int>();
                for(int i=0; i< s.Grades.Count; i++)
                {
                    ints.Add(s.Grades[i][0]);
                }

                int total = s.Grades.Count;
                double average = ints.Average();
                int highest = ints.Max();
                int lowest = ints.Min();

                this.totalGrades.Content = total.ToString();
                this.averageGrade.Content = average.ToString();
                this.highestGrade.Content = highest.ToString();
                this.lowestGrade.Content = lowest.ToString();

                
            }

        }

        int iNoName = 1;

        private void BtnAddExam_Click(object sender, RoutedEventArgs e)
        {
            Exam s = new Exam { Name = "NoExam_" + iNoName };
            this.repo.AddExam(s);
            iNoName++;

            this.listBoxExams.ItemsSource = this.repo.Exams;
            SetSelectedByIndex(this.listBoxExams.Items.Count - 1);
            SetSelectedById(s.ID);
            this.repo.UpdateAllExam();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxExams.SelectedItem is Exam s)
            {
                repo.RemoveExam(s.ID);

            }

            this.listBoxExams.ItemsSource = repo.Exams;
            SetSelectedByIndex(0);
            this.repo.UpdateAllExam();

        }

        private void SetSelectedByIndex(int index)
        {
            if (index >= 0 && index < this.listBoxExams.Items.Count)
            {
                this.listBoxExams.SelectedIndex = index;
            }
        }

        private void SetSelectedById(string id)
        {
            for (int i = 0; i < this.listBoxExams.Items.Count; i++)
            {

                Exam? s = listBoxExams.Items[i] as Exam;
                if (s != null)
                {
                    if (s.ID == id)
                        this.listBoxExams.SelectedItem = s;
                }

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxExams.SelectedItem is Exam s)
            {
                s.Name = txtName.Text;
                s.Teacher = nameTeacher.Text;
                s.ID = this.txtId.Text;
                s.StartTime = TimeSpan.Parse(this.beginningTime.Text);
                s.TotalTime = int.Parse(this.totalTime.Text);
                s.Date = DateTime.Parse(this.date.Text);
               


                this.repo.UpdateExam(s);
                this.listBoxExams.ItemsSource = repo.Exams;
                this.SetSelectedById(s.ID);
            }

            this.repo.UpdateAllExam();

        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();
                image.Source = bitmap;


                if (!Directory.Exists("IMAGES"))
                {
                    Directory.CreateDirectory("IMAGES");
                }
                string result = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                System.IO.File.Copy(openFileDialog.FileName, $"IMAGES/{result}.png");

            }

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

       

        private void listBoxQue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBoxQue.SelectedItem is Question q)
            {
                this.Q1.Text = q.Text;
                if (this.listBoxAnswers.SelectedItem is Answer answer)
                {
                    this.al.Text = answer.Name;

                }
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();

                if (!string.IsNullOrEmpty(q.Img))
                {
                    bitmap.UriSource = new Uri(q.Img);
                    bitmap.EndInit();
                }
                else
                {
                    bitmap.UriSource = new Uri("pack://application:,,,/noimage.png");
                }

                int[] arr = new int[q.Answers.Count];

                for (int i = 0; i < q.Answers.Count; i++)
                {
                    arr[i] = i+1;
                }
                this.comboBox.ItemsSource = arr;
                this.comboBox.Text = q.CorrectAnswerIndex.ToString();


                this.image.Source = bitmap;
                this.listBoxAnswers.ItemsSource = q.Answers.ToArray();
                listBoxAnswers.DisplayMemberPath = "Name";

            }

        }

        private void btnSaveQ_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxQue.SelectedItem is Question q)
            {
                if(image.Source != null)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(image.Source.ToString());
                    bitmap.EndInit();
                    image.Source = bitmap;
                    q.Img = image.Source.ToString();
                }

                q.Text = Q1.Text;
                q.CorrectAnswerIndex = comboBox.SelectedIndex+1;

                this.repo.UpdateQuestion(q);

            }
            this.repo.UpdateAllExam();

        }


        

        private void addQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxExams.SelectedItem is Exam s)
            {
                int n = s.Questions.Count + 1;
                Question q = new Question { Name = "Question_" + n };
                this.repo.AddQuestion(q);
                s.Questions.Add(q);
               // iNoQue++;
                this.listBoxQue.ItemsSource = s.Questions.ToArray();
                listBoxQue.DisplayMemberPath = "Name";
            }


            this.repo.UpdateAllExam();

        }


        private void addAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxQue.SelectedItem is Question q)
            {
                int n = q.Answers.Count + 1;
                Answer answer = new Answer { Name = "Answer_" + n };
                q.Answers.Add(answer);
                this.repo.UpdateQuestion(q);
                this.listBoxAnswers.ItemsSource = q.Answers.ToArray();
                listBoxAnswers.DisplayMemberPath = "Name";

            }
            this.repo.UpdateAllExam();

        }

        private void listBoxAnswer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (this.listBoxAnswers.SelectedItem is Answer answer)
            {
                this.al.Text = answer.Name;
            }
            
        }

        private void SetSelectedByIndexQ(int index)
            {
                if (index >= 0 && index < this.listBoxQue.Items.Count)
                {
                    this.listBoxQue.SelectedIndex = index;
                }
            }

            private void SetSelectedByIdQ(string Name)
            {
                for (int i = 0; i < this.listBoxQue.Items.Count; i++)
                {

                    Question? q = listBoxQue.Items[i] as Question;
                    if (q != null)
                    {
                        if (q.ID == Name)
                            this.listBoxQue.SelectedItem = q;
                    }

                }
            }

        private void btnRemoveQ_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxExams.SelectedItem is Exam s)
            {
                if (this.listBoxQue.SelectedItem is Question q)
                {
                    repo.RemoveQuestion(q.ID);
                    s.Questions.Remove(q);
                }
                this.listBoxQue.ItemsSource = s.Questions;

            }

            SetSelectedByIndex(0);
            this.repo.UpdateAllExam();

        }

        private void RemoveAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBoxQue.SelectedItem is Question q)
            {
                if (this.listBoxAnswers.SelectedItem is Answer answer)
                {
                    q.Answers.Remove(answer);
                    this.repo.UpdateQuestion(q);
                    this.listBoxAnswers.ItemsSource = q.Answers.ToArray();
                    listBoxAnswers.DisplayMemberPath = "Name";
                    this.repo.UpdateAllExam();
                }

            }

            SetSelectedByIndex(0);

        }

        private void saveAnswer_Click(object sender, RoutedEventArgs e)
        {

            if (this.listBoxQue.SelectedItem is Question q)
            {
                if (this.listBoxAnswers.SelectedItem is Answer answer)
                {
                    q.Answers.Remove(answer);
                    Answer ans = new Answer(this.al.Text, false);
                    q.Answers.Add(ans);
                    //this.SetSelectedByIdQ(q.Name);
                }
                this.repo.UpdateQuestion(q);
                this.listBoxAnswers.ItemsSource = q.Answers.ToArray();
                listBoxAnswers.DisplayMemberPath = "Name";
            }
            this.repo.UpdateAllExam();

        }


    }
   

}



