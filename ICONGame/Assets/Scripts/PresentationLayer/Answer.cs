using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Answer : MonoBehaviour, IDropHandler
{
    private Button btn_Ans;
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("From Answer");
        //if (eventData.pointerDrag != null)
        //{
        //    Question ques = eventData.pointerDrag.GetComponent<Question>();
        //    if (ques != null)
        //    {
        //        Debug.Log("From Answer Got Question");
        //        ques.Selected = this;
        //    }
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        btn_Ans = GetComponent<Button>();
        if (btn_Ans != null)
            btn_Ans.onClick.AddListener(OnClick_Answer);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_Answer()
    {
        Question.SetActiveAns(this);
    }
}
