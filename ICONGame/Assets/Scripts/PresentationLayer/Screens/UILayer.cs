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
        public Button btn_Submit;
        public Text txt_Timer;
        public Text txt_Msg;
        public int layerID = -1;
        float startTime = 0.0f;
        public float bonusPerInterval = 5;
        public int PointsPerAnswer = 10;
        public void OnEnable()
        {
            startTime = 0.0f;
            for (int i = 0; i < Questions.Count; i++)
                Questions[i].Clean();

            for (int i = 0; i < Options.Count; i++)
                Options[i].Clean();
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

            if (btn_Submit != null)
                btn_Submit.onClick.AddListener(SubmitScore_OnClick);
        }
    

        // Update is called once per frame
        void Update()
        {
            startTime += Time.deltaTime;
            if (txt_Timer != null && startTime < 61)
                txt_Timer.text = (((int)startTime).ToString() + " sec");
        }

        private int CalculateBonusScore()
        {
            int score = 0;
            if (startTime < 1) startTime = 1;
            int bonusMultiply = 0;
            if (startTime < 30.0f)
            {
                bonusMultiply = (int)startTime / 5;
                bonusMultiply = 6 - bonusMultiply;
                score += (int)(bonusMultiply * bonusPerInterval);
            }
            return score;
        }

        public void SubmitScore_OnClick()
        {
            Question.Clear();
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

                if(totalScore > 0)
                    totalScore += bonus;
                else
                    bonus = 0;

                EventManager.Raise<SubmitLayerScoreEvent>(new SubmitLayerScoreEvent { layerID = this.layerID, correctAns = correctAnswers, timeTaken = (int)startTime, BonusScore = bonus, score = totalScore }); ; 
            }
        }
    }
}
