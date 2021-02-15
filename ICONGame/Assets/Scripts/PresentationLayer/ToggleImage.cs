using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleImage : MonoBehaviour
{

    Toggle m_Toggle;
    public Image img_On;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Toggle = GetComponent<Toggle>();
        //img_On = GetComponentInChildren<Image>();

        if (img_On != null && m_Toggle != null)
        {
            img_On.gameObject.SetActive(m_Toggle.isOn);
        }    

        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        Debug.Log("New Value : " + m_Toggle.isOn);
        if (img_On != null)
            img_On.gameObject.SetActive(m_Toggle.isOn);
    }
}
