using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Lab2_Pankov_Quiz
{
    public class Question : QuizItem, IEnumerable
    {
        public List<Answer> Answers { get; private set; }
        public bool Multichoice { get; private set; }
        public string Text { get; private set; }

        public Question(string text, bool multichoice)
        {
            Multichoice = multichoice;
            Text = text;
            Answers = new List<Answer>();
        }

        public void SelectAnswer(int idx)
        {
            SelectAnswer(Answers[idx]);
        }

        public void SelectAnswer(Answer a)
        {
            if (!Multichoice)
                foreach (Answer ans in Answers)
                    ans.Selected = false;
            a.Selected = true;
        }

        public void UnselectAnswer(int idx)
        {
            UnselectAnswer(Answers[idx]);
        }

        public void UnselectAnswer(Answer a)
        {
            a.Selected = false;
        }

        public override bool IsAnsweredCorrectly()
        {
            return Answers.All(x => x.Selected == x.Correct);
        }

        public override bool IsAnswered()
        {
            return Answers.Any(x => x.Selected);
        }

        public override int GetCorrectAnswerCount()
        {
            return IsAnsweredCorrectly() ? 1 : 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Answers.GetEnumerator();
        }

        public Answer this[int idx]
        {
            get { return Answers[idx]; }
        }

        public override int GetQuestionCount()
        {
            return 1;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Question)) return false;
            return ((Question)obj == this);
        }

        public override bool Equals(QuizItem other)
        {
            return (Text == ((Question)other).Text &&
                Answers.Count == ((Question)other).Answers.Count &&
                Answers.All(x => ((Question)other).Answers.Any(y => y == x)));
        }
    }
}
