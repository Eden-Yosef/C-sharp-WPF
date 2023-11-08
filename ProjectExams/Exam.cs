using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Media;

namespace ProjectExams
{

    public class Exam
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string? Teacher { get; set; }
        public TimeSpan StartTime { get; set; }
        public int TotalTime { get; set; }
        public List<int[]> Grades { get; set; }

        public List<Question> Questions { get; set; }
        public override string ToString()
        {
            return this.Name;
        }

        public Exam() : this("", DateTime.MinValue, "", TimeSpan.Zero, 0, new List<int[]>(), new List<Question>())
        {
        }
        public Exam(string name, DateTime date, string teacher, TimeSpan startTime, int totalTime, List<int[]> grades, List<Question> questions)
        {
            this.Name = name;
            this.Date = date;
            this.ID = Guid.NewGuid().ToString();
            this.Teacher = teacher;
            this.StartTime = startTime;
            this.TotalTime = totalTime;
            this.Grades = grades;
            this.Questions=questions;
        }

      

    }


    public class Question
    {
        public string ID { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }

        public List<Answer> Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Img { get; set; }

        public Question() : this("", "", new List<Answer>(), 0, "")
        {
        }
        public Question(string name , string text, List<Answer> answers, int correctAnswerIndex, string img)
        {
            this.ID = Guid.NewGuid().ToString();
            this.Name = name;
            this.Text = text;
            this.Answers = answers;
            this.CorrectAnswerIndex = correctAnswerIndex;
            this.Img = img;
        }

        public bool IsRandomized { get; set; }
       
    }


    public class Answer
    {
        public string Name { get; set; }
        public bool isRandomized { get; set; }

        public Answer() : this("", false)
        {
        }

        public Answer(string name, bool isRandomized)
        {
            this.Name = name;
            this.isRandomized = isRandomized;
        }

    }


}
