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
        public GameObject[] InstructionScreens;
        public GameObject RightArrow, LeftArrow;

        int showscreen;
        // Start is called before the first frame update
        void Start()
        {
            btn_MainMenu.onClick.AddListener(MainMenu_OnClick);
        }

        public void MainMenu_OnClick()
        {
            EventManager.Raise<MainMenuUIEvent>(new MainMenuUIEvent{});
        }

        public void OnRightArrow()
        {
            for(int i = 0; i < InstructionScreens.Length; i++)
            {
                InstructionScreens[i].SetActive(false);
            }
            showscreen++;
            InstructionScreens[showscreen].SetActive(true);
            if (showscreen == InstructionScreens.Length - 1)
                RightArrow.SetActive(false);
            LeftArrow.SetActive(true);

        }

        public void OnLeftArrow()
        {
            for (int i = 0; i < InstructionScreens.Length; i++)
            {
                InstructionScreens[i].SetActive(false);
            }
            showscreen--;
            InstructionScreens[showscreen].SetActive(true);

            if (showscreen == 0)
                LeftArrow.SetActive(false);
            RightArrow.SetActive(true);
        }
    }
}
