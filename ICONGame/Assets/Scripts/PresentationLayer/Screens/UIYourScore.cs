using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;

namespace PRESENTATION
{
    public class UIYourScore : UIScreen
    {
        public List<Text> layerID = new List<Text>();
        public List<Text> correctAnswer = new List<Text>();
        public List<Text> timeTaken = new List<Text>();
        public List<Text> points = new List<Text>();
        public List<Text> bonusPoints = new List<Text>();
        public List<Text> layerTotalPoints = new List<Text>();
       
        // Start is called before the first frame update
        void Start()
        {
            EventManager.Listen<SetPlayerScoreEvent>(OnPlayerScoreEvent);
             EventManager.Raise<GetPlayerScoreEvent>(new GetPlayerScoreEvent());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPlayerScoreEvent(IEventBase obj)
        {
            if (obj is SetPlayerScoreEvent)
            {
                SetPlayerScoreEvent ps = (SetPlayerScoreEvent)obj;
                for (int i = 0; i < ps.layers.Count; i++)
                {
                    if (layerID.Count > i) layerID[i].text = i.ToString();
                    if (correctAnswer.Count > i) correctAnswer[i].text = ps.layers[i].correctAns.ToString();
                    if (timeTaken.Count > i) timeTaken[i].text = ps.layers[i].TimeTaken.ToString();
                    if (points.Count > i) points[i].text = (ps.layers[i].TotalPoints - ps.layers[i].bonusPoints).ToString();
                    if (bonusPoints.Count > 1) bonusPoints[i].text = ps.layers[i].bonusPoints.ToString();
                    if (layerTotalPoints.Count > 1) layerTotalPoints[i].text = ps.layers[i].TotalPoints.ToString();
                }
            }
        }
    }
}
