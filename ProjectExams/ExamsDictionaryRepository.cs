using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectExams
{
    public class ExamsDictionaryRepository : IExamsRepository
    {
        private Dictionary<string, Exam> _exams;
        private Dictionary<string, Question> _question;
        private Dictionary<string, Answer> _answer;



        public ExamsDictionaryRepository()
        {
            _exams = new Dictionary<string, Exam>();
            _question = new Dictionary<string, Question>();
            _answer = new Dictionary<string, Answer>();

        }

        public Answer[] Answers
        {
            get
            {
                Answer[] answers = new Answer[_answer.Count];
                _answer.Values.CopyTo(answers, 0);
                return answers;
            }
        }

        public Question[] Questions
        {
            get
            {
                Question[] questions = new Question[_question.Count];
                _question.Values.CopyTo(questions, 0);
                return questions;
            }
        }

        public Exam[] Exams
        {
            get
            {
                Exam[] exams = new Exam[_exams.Count];
                _exams.Values.CopyTo(exams, 0);
                return exams;
            }
        }
        public void AddQuestion(Question question)
        {
            this._question.Add(question.ID , question);

        }

        public void AddAnswer(Answer answer)
        {
            this._answer.Add(answer.Name, answer);

        }
        public void AddExam(Exam exam)
        {
            this._exams.Add(exam.ID, exam);

        }

        public void UpdateExam(Exam exam)
        {
            foreach (var key in _exams.Keys)
            {
                if (key == exam.ID)
                {
                    this._exams[key] = exam;
                }
            }

        }

        public void UpdateQuestion(Question question)
        {
            foreach (var key in _question.Keys)
            {
                if (key == question.ID)
                {
                    this._question[key] = question;
                }
            }

        }

        

        public void UpdateAllExam()
        {
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

        public void RemoveExam(string id)
        {
            foreach (var key in _exams.Keys)
            {
                if (key == id)
                {
                    _exams.Remove(key);
                }
            }

        }

        public void RemoveQuestion(string name)
        {
            foreach (var key in _question.Keys)
            {
                if (key == name)
                {
                    _question.Remove(key);
                }
            }

        }

    }
}
