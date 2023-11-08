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

namespace ProjectExams
{
    /// <summary>
    /// Interaction logic for endExam.xaml
    /// </summary>
    public partial class endExam : Window
    {
        IExamsRepository repo;
        int Grade;

        public endExam(IExamsRepository repo, int grade)
        {
            InitializeComponent();
            this.repo = repo;
            this.Grade = grade;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grade.Content = this.Grade.ToString();
        }
    }
}

