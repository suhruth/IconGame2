using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;

public class LeaderBoardItem : MonoBehaviour
{
    public Text txt_Rank;
    public Text txt_Name;
    public Text txt_Points;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu_OnClick()
    {
        EventManager.Raise<MainMenuUIEvent>(new MainMenuUIEvent { });
    }
}
