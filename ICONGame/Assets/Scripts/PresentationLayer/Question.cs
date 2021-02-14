using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    private LineRenderer lr;


    public Answer correctAnswer;
    private Answer selectedAnswer;
    public Answer Selected {  get { return selectedAnswer; } set { selectedAnswer = value; } }
    private int score;
    public int Score {  get { return score; } set { score = value; } }

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lr != null && selectedAnswer != null)
        {
            if (Vector3.Distance(gameObject.transform.position, selectedAnswer.gameObject.transform.position) < .1f)
            {
                lr.SetPosition(0, gameObject.transform.position);
                lr.SetPosition(1, selectedAnswer.gameObject.transform.position);
            }
        }
        
    }
}
