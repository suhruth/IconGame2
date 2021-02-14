using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;

namespace PRESENTATION
{
    public class UILayer : UIScreen
    {
        public List<Question> Questions;
        public List<Answer> Options;
        public Text txt_Msg;
        public int layerID = -1;
        float startTime = 0.0f;
        public bool winAllForBonus = true;
        public float bonusPerInterval = 5;
        public int PointsPerAnswer = 10;
        public void OnEnable()
        {
            startTime = 0.0f;
        }
        // Start is called before the first frame update
        
        void Start()
        {
            startTime = 0.0f;
            if (layerID < 0)
                Debug.LogError("Invalid Layer ID");

            foreach (Question q in Questions)
            {
                if (q.correctAnswer == null)
                {
                    Debug.LogError("Corrent Answer not Initialized in Layer " + layerID);
                    break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            startTime += Time.deltaTime;
        }

        private int CalculateBonusScore()
        {
            int score = 0;
            if (startTime < 1) startTime = 1;
            int bonusMultiply = 30 / (int)startTime;
            if (startTime % 5 > 0) bonusMultiply += 1;
            
            if(winAllForBonus)
                score += (int)(bonusMultiply * bonusPerInterval);

            return score;
        }

        public void SubmitScore_OnClick()
        {
            int correctAnswers = 0;
            bool allQuestionsAnswered = true;
            foreach (Question q in Questions)
            {
                if (q.Selected == null)
                    allQuestionsAnswered = false;
                else
                {
                    q.Score = 0;
                    if (q.correctAnswer == q.Selected)
                    {
                        q.Score = PointsPerAnswer;
                        correctAnswers += 1;
                    }
                }
            }

            if (!allQuestionsAnswered)
            {
                if (txt_Msg != null)
                    txt_Msg.text = "Answer all Questions";
            }
            else
            {
                int totalScore = 0;
                foreach (Question q in Questions)
                    totalScore += q.Score;

                int bonus = CalculateBonusScore();
                totalScore += bonus;

                EventManager.Raise<SubmitLayerScoreEvent>(new SubmitLayerScoreEvent { layerID = this.layerID, correctAns = correctAnswers, timeTaken = (int)startTime, BonusScore = bonus, score = totalScore }); ; 
            }
        }
    }
}
