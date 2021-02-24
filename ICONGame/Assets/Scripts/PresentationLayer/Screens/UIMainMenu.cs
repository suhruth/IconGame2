using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;

namespace PRESENTATION
{
    public class UIMainMenu : UIScreen
    {
        public Button btn_PlayGame;
        public Button btn_Instructions;
        public Button btn_LeaderBoard;
        //public Button btn_Prizes;
        public Button btn_Quit;

        [SerializeField] GameObject LeaderboardScreen_MainmenuBtn, LeaderboardScreen_QuitBtn;

        // Start is called before the first frame update
        void Start()
        {
            btn_PlayGame.onClick.AddListener(OnClick_PlayGame);
            btn_Instructions.onClick.AddListener(OnClick_Instructions);
            btn_LeaderBoard.onClick.AddListener(OnClick_LeaderBoard);
            //btn_Prizes.onClick.AddListener(OnClick_Prizes);
            btn_Quit.onClick.AddListener(OnClick_Quit);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick_PlayGame() 
        {
            EventManager.Raise<PlayGameUIEvent>(new PlayGameUIEvent());
        }

        public void OnClick_Instructions() 
        {
            EventManager.Raise<InstructionsUIEvent>(new InstructionsUIEvent());
        }

        public void OnClick_LeaderBoard() 
        {
            EventManager.Raise<LeaderBoardUIEvent>(new LeaderBoardUIEvent());
            LeaderboardScreen_MainmenuBtn.SetActive(true);
            LeaderboardScreen_QuitBtn.SetActive(false);
        }
        public void OnClick_Prizes() 
        {
            EventManager.Raise<PrizesUIEvent>(new PrizesUIEvent());
        }
        public void OnClick_Quit() 
        {
            //EventManager.Raise<QuitUIEvent>(new QuitUIEvent());
            Application.OpenURL("https://ciscosbsummit2021.marcomarabiamea.com/thank-you/");
        }
    }
}
