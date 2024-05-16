using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Question
{
    public string questionText;
    public string[] answers = new string[3];
    public string correctAnswer;

    public Question(string question, string[] ans, string correctAnswer)
    {
        this.questionText = question;
        this.correctAnswer = correctAnswer;
        if (ans.Length == 3)
        {
            this.answers = ans;
        }
        else
        {
            Debug.LogError("Incorrect number of answers provided for question: " + question);
        }

        this.correctAnswer = correctAnswer;
    }
}

public class QuestionList
{
    public List<Question> questions = new List<Question>();
    public List<string> questionText = new List<string>();
    public List<string[]> questionAnswer = new List<string[]>();
    public List<string> correctAnswes = new List<string>();

    public QuestionList()
    {
        questionText.Add("What is 1 + 1?");
        questionText.Add("What is the capital of France?");
        questionText.Add("In what year was the Internet first launched?");
        questionText.Add("What is the national animal of Scotland?");
        questionText.Add("What is the rarest blood type?");
        questionText.Add("What is the chemical element with the highest melting point?");

        questionAnswer.Add(new string[] { "1", "3", "4" });
        questionAnswer.Add(new string[] { "Tokyo", "New York", "Ha Noi" });
        questionAnswer.Add(new string[] { "1970", "2000", "1975" });
        questionAnswer.Add(new string[] { "Dog", "Bird", "Elephant" });
        questionAnswer.Add(new string[] { "O+", "A-", "B+" });
        questionAnswer.Add(new string[] { "Copper", "Gold", "Uranium" });

        correctAnswes.Add("2");
        correctAnswes.Add("Paris");
        correctAnswes.Add("1969");
        correctAnswes.Add("Unicorn");
        correctAnswes.Add("AB-");
        correctAnswes.Add("Tunsten");

        for (int i = 0; i < questionText.Count; i++)
        {
            questions.Add(new Question(questionText[i], questionAnswer[i], correctAnswes[i]));
        }
    }
}