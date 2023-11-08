using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectExams
{
    public interface IExamsRepository
    {
        void AddExam(Exam exam);
        void AddQuestion(Question question);

        void UpdateExam(Exam exam);
        public void UpdateAllExam();
        void UpdateQuestion(Question question);
        public void LoadData(string text);

        void RemoveExam(string id);
        void RemoveQuestion(string name);

        Exam[] Exams { get; }
        Question[] Questions { get; }

    }
}
