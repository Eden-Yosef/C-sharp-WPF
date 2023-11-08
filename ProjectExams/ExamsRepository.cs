using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectExams
{
    public class ExamsRepository : IExamsRepository
    {
        private List<Exam> _exams;
        private List<Question> _question;


        public ExamsRepository()
        {
            _exams = new List<Exam>();
            _question = new List<Question>();

        }
        public Question[] Questions
        {
            get { return _question.ToArray(); }

        }

        public Exam[] Exams
        {
            get { return _exams.ToArray(); }

        }

        public void AddExam(Exam exam)
        {
            this._exams.Add(exam);

        }

        public void AddQuestion(Question question)
        {
            this._question.Add(question);

        }

        public void UpdateExam(Exam exam)
        {
            int indexFound = this._exams.FindIndex(e => e.ID == exam.ID);
            if (indexFound >= 0)
            {
                this._exams[indexFound] = exam;

            }

        }

        public void UpdateQuestion(Question question)
        {
            int indexFound = this._question.FindIndex(q => q.Name == question.Name);
            if (indexFound >= 0)
            {
                this._question[indexFound] = question;

            }

        }

        public void LoadData(string text)
        {
            if (text != String.Empty)
            {
                string examsText = File.ReadAllText(text);
                var examsList = JsonSerializer.Deserialize<Exam[]>(examsText);

                foreach (Exam item in examsList)
                {
                    AddExam(item);
                }
            }
        }

        public void UpdateAllExam()
        {
            //Student[] students = repo.Students;
            List<Exam> exams = Exams.ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStudentsString = JsonSerializer.Serialize<List<Exam>>(exams, options);

            if (!Directory.Exists(""))
            {
                Directory.CreateDirectory("AppData");

            }
            Directory.CreateDirectory("AppData");
            File.WriteAllText("AppData/exams.json", jsonStudentsString);

        }

      
        public void RemoveExam(string id)
        {
            int indexFound = this._exams.FindIndex(e => e.ID == id);
            if (indexFound >= 0)
            {
                this._exams.RemoveAt(indexFound);
            }
        }

        public void RemoveQuestion(string name)
        {
            int indexFound = this._question.FindIndex(e => e.Name == name);
            if (indexFound >= 0)
            {
                this._question.RemoveAt(indexFound);
            }
        }
    }

}
