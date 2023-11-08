using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace ProjectExams
{
    /// <summary>
    /// Interaction logic for PageExam.xaml
    /// </summary>
    public partial class PageExam : Window
    {
        IExamsRepository repo;
        string id;
        string id_stu;

        public PageExam(IExamsRepository repo, string id, string idd)
        {
            InitializeComponent();
            this.repo = repo;
            this.id = id;
            this.id_stu = idd;




        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            repo.LoadData("AppData/exams.json");
            this.listBoxQue.ItemsSource = this.repo.Exams.FirstOrDefault(x => x.ID == id).Questions;
            listBoxQue.DisplayMemberPath = "Name";
        }

        private void listBoxExams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxQue.SelectedItem is Question q)
            {
                this.listBoxQue.ItemsSource = this.repo.Exams.FirstOrDefault(x => x.ID == id).Questions;
                if (q.Img != "")
                {
                    qImg.Visibility = Visibility.Visible;
                    qText.Visibility = Visibility.Hidden;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(q.Img.ToString());
                    bitmap.EndInit();
                    img.Source = bitmap;
                }
                else
                {
                    qImg.Visibility = Visibility.Hidden;
                    qText.Visibility = Visibility.Visible;

                    qText.Text = q.Text;

                }

                for (int i = 1; i <= 10; i++)
                {
                    CheckBox cb = (CheckBox)FindName($"myCheckBox{i}");
                    cb.Visibility = Visibility.Hidden;
                }

                int n = q.Answers.Count;
                for(int i=1; i<=n; i++)
                {
                    CheckBox cb = (CheckBox)FindName($"myCheckBox{i}");
                    cb.Visibility = Visibility.Visible;
                    cb.Content = q.Answers[i-1].Name;
                }

            }



        }

        private void myCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (listBoxQue.SelectedItem is Question q)
            {

                for(int i=1; i<=q.Answers.Count; i++)
                {
                    CheckBox cb = (CheckBox)FindName($"myCheckBox{i}");
                    Predicate<Question> questionPredicate = (Question que) => que == q;
                    this.repo.Exams.FirstOrDefault(x => x.ID == id).Questions.Find(questionPredicate).Answers[i-1].isRandomized = (bool)cb.IsChecked;
                    this.repo.UpdateAllExam();
                }
                
            }
        }


        private void EndExam_Click(object sender, RoutedEventArgs e)
        {
            int n = this.repo.Exams.FirstOrDefault(x => x.ID == id).Questions.Count;
            int isTrue = n;
            int num = 100 / n;
            for(int i=0; i<n; i++)
            {
                int m = this.repo.Exams.FirstOrDefault(x => x.ID == id).Questions[i].CorrectAnswerIndex;
                if(this.repo.Exams.FirstOrDefault(x => x.ID == id).Questions[i].Answers[m-1].isRandomized == false)
                {
                    isTrue--;
                }

            }

            int[] grade = new int[2];
            grade[0] = (isTrue * num);
            grade[1] = int.Parse(this.id_stu);
            this.repo.Exams.FirstOrDefault(x => x.ID == id).Grades.Add(grade);

            for (int i=0; i<n; i++)
            {
                int numAns = this.repo.Exams.FirstOrDefault(x => x.ID == id).Questions[i].Answers.Count;
                for (int j=0; j<numAns; j++)
                {
                    this.repo.Exams.FirstOrDefault(x => x.ID == id).Questions[i].Answers[j].isRandomized = false;

                }
            }

            this.repo.UpdateAllExam();
            endExam endexam = new endExam(new ExamsDictionaryRepository() , grade[0]);
            this.Close();
            endexam.Show();
        }


    }
    
}



