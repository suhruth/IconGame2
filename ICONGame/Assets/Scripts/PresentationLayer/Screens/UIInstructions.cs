using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;

namespace PRESENTATION
{
    public class UIInstructions : UIScreen
    {
        public Button btn_MainMenu;

        // Start is called before the first frame update
        void Start()
        {
            btn_MainMenu.onClick.AddListener(MainMenu_OnClick);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MainMenu_OnClick()
        {
            EventManager.Raise<MainMenuUIEvent>(new MainMenuUIEvent{});
        }
    }
}
