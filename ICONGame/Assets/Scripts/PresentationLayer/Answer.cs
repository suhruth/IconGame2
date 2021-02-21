using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Answer : MonoBehaviour, IDropHandler
{
    private Button btn_Ans;
    public Animator anim;
    float pulseTimer = 0.0f;
    public float PulseTime { set { pulseTimer = value; } }

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
    public void Clean()
    {
        if (anim != null)
        {
            anim.SetBool("PlayPulse", false);
            anim.Play("ScaleUPDown", -1, 0f);
            anim.speed = 0;
        }
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
        if (pulseTimer > 0.0f)
        {
            pulseTimer -= Time.deltaTime;
            if (pulseTimer < 0.0f)
            {
                anim.SetBool("PlayPulse", false);
                pulseTimer = 0.0f;
            }
        }

    }

    public void DoPulseAnim()
    {
        anim.SetBool("PlayPulse", true);
        pulseTimer = 1.25f;
        anim.speed = 1;
    }

    public void OnClick_Answer()
    {
        Question.SetActiveAns(this);
    }
}
