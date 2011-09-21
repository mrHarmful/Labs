using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab2_Pankov_Quiz
{
    public abstract class QuizItem : IEquatable<QuizItem>
    {
        public abstract bool IsAnswered();
        public abstract bool IsAnsweredCorrectly();
        public abstract int GetCorrectAnswerCount();
        public abstract int GetQuestionCount();
        public abstract bool Equals(QuizItem other);

        public static bool operator ==(QuizItem a, QuizItem b) { return a.Equals(b); }
        public static bool operator !=(QuizItem a, QuizItem b) { return !(a == b); }

    }
}
