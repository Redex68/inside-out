using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class QuestionParser
{
    public static List<Question> ParseQuestionsFromFile(string filePath)
    {
        List<Question> questions = new List<Question>();

        string[] lines = File.ReadAllLines("./Assets/" + filePath);

        for (int i = 0; i < lines.Length; i += 6)
        {
            string text = lines[i].Substring(3).Trim();

            List<string> answers = new List<string>();
            for (int j = i + 1; j <= i + 4; j++)
            {
                string answer = lines[j].Substring(4).Trim();
                answers.Add(answer);
            }

            Dictionary<string, int> answer2index = new Dictionary<string,int>
            {
                { "a" , 0 },
                { "b" , 1 },
                { "c" , 2 },
                { "d" , 3 },
            };

            string solution = answers[answer2index[lines[i + 5].Trim()]];

            Question question = new Question(text, answers, solution);
            questions.Add(question);
        }

        return questions;
    }
}
