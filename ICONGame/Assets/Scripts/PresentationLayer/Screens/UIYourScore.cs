using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;
using System;

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
        public Text txt_totalScore;

        public Button btn_MainMenu, btn_LeaderBoard;
        public Button btn_Quit;
        public AudioClip ac;
        public SoundManager SM;
        [SerializeField] GameObject LeaderboardScreen_MainmenuBtn, LeaderboardScreen_QuitBtn;
        // Start is called before the first frame update
        void Start()
        {
            SM.OnOptionClick(ac);
            btn_MainMenu.onClick.AddListener(MainMenu_OnClick);
            btn_LeaderBoard.onClick.AddListener(OnClick_LeaderBoard);
            btn_Quit.onClick.AddListener(Quit_OnClick);

            //EventManager.Listen<SetPlayerScoreEvent>(OnPlayerScoreEvent);
           //  EventManager.Raise<GetPlayerScoreEvent>(new GetPlayerScoreEvent());
        }
        public void OnEnable()
        {
            EventManager.Listen<SetPlayerScoreEvent>(OnPlayerScoreEvent);
            EventManager.Raise<GetPlayerScoreEvent>(new GetPlayerScoreEvent());
        }


        private void Quit_OnClick()
        {
            //EventManager.Raise<QuitUIEvent>(new QuitUIEvent());
            Application.OpenURL("https://ciscosbsummit2021.marcomarabiamea.com/conference/");
        }

        private void MainMenu_OnClick()
        {
            EventManager.Raise<MainMenuUIEvent>(new MainMenuUIEvent { });
        }
        public void OnClick_LeaderBoard()
        {
            EventManager.Raise<LeaderBoardUIEvent>(new LeaderBoardUIEvent());
            LeaderboardScreen_MainmenuBtn.SetActive(false);
            LeaderboardScreen_QuitBtn.SetActive(true);
        }

        public void OnPlayerScoreEvent(IEventBase obj)
        {
            if (obj is SetPlayerScoreEvent)
            {
                int total = 0;
                SetPlayerScoreEvent ps = (SetPlayerScoreEvent)obj;
                for (int i = 0; i < ps.layers.Count; i++)
                {
                    if (layerID.Count > i) layerID[i].text = "LEVEL " + (i+1).ToString();
                    if (correctAnswer.Count > i) correctAnswer[i].text = ps.layers[i].correctAns.ToString();
                    if (timeTaken.Count > i) timeTaken[i].text = ps.layers[i].TimeTaken.ToString();
                    if (points.Count > i) points[i].text = (ps.layers[i].TotalPoints - ps.layers[i].bonusPoints).ToString();
                    if (bonusPoints.Count > 1) bonusPoints[i].text = ps.layers[i].bonusPoints.ToString();
                    if (layerTotalPoints.Count > 1)
                    {
                        total += ps.layers[i].TotalPoints;
                        layerTotalPoints[i].text = ps.layers[i].TotalPoints.ToString();
                    }
                }
                if (txt_totalScore != null)
                    txt_totalScore.text = total.ToString();
            }
        }
    }
}
