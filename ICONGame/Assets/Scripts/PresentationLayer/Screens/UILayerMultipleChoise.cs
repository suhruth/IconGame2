using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;

namespace PRESENTATION
{
    public class UILayerMultipleChoise : UIScreen
    {
        public Text txt_Msg;
        public int layerID = -1;
        float startTime = 0.0f;

        List<ToggleGroup> questions = new List<ToggleGroup>();
        List<Toggle> answers = new List<Toggle>();
        public int PointsPerAnswer = 20;
        public bool winAllForBonus = true;
        public float bonusPerInterval = 5;
        // Start is called before the first frame update
        void Start()
        {
            startTime = 0.0f;
            if (layerID < 0)
                Debug.LogError("Invalid Layer ID");

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

            if (winAllForBonus)
                score += (int)(bonusMultiply * bonusPerInterval);

            return score;
        }

        public void Submit_OnClick()
        {
            int totalScore = 0;
            int correctAnswers = 0;
            bool allQuestionsAnswered = true;
            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i].AnyTogglesOn())
                {
                    Toggle tg = questions[i].GetFirstActiveToggle();
                    if (tg == answers[i])
                    {
                        totalScore += PointsPerAnswer;
                        correctAnswers += 1;
                    }
                }
                else allQuestionsAnswered = false;
            }
             

            if (!allQuestionsAnswered)
            {
                if (txt_Msg != null)
                    txt_Msg.text = "Answer all Questions";
            }
            else
            {
               
                int bonus = CalculateBonusScore();
                totalScore += bonus;

                EventManager.Raise<SubmitLayerScoreEvent>(new SubmitLayerScoreEvent { layerID = this.layerID, correctAns = correctAnswers, timeTaken = (int)startTime, BonusScore = bonus, score = totalScore }) ;
            }
        }
    }
}

