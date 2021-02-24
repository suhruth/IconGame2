using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;
using System;


namespace PRESENTATION
{
    public class UISignIn : UIScreen
    {
        public InputField input_UserName;
        public InputField input_CompanyName;
        public Button btn_Submit;

        public Text txt_ErrorMsg;


        // Start is called before the first frame update
        void Start()
        {
            if (btn_Submit != null)
                btn_Submit.onClick.AddListener(SubmitButton_OnClick);

            EventManager.Listen<SignInStatusEvent>(OnSignInStatuEvent);
        }
        private bool ValidateInputFields()
        {
            if (txt_ErrorMsg != null)
                txt_ErrorMsg.text = "";

            if (input_UserName.text.Length == 0)
            {
                txt_ErrorMsg.text = "Enter User Name";
                return false;
            }
            if (input_CompanyName.text.Length == 0)
            {
                txt_ErrorMsg.text = "Enter Your Mail Id";
                return false;
            }
            string s = input_CompanyName.text;
            if (!s.Contains("@"))
            {
                txt_ErrorMsg.text = "Enter Your Mail Id";
                return false;
            }

            return true;
        }

        public void SubmitButton_OnClick()
        {
            if (txt_ErrorMsg == null)
            {
                Debug.LogError("Error Msg field is Invalid");
                return;
            }
            if (input_UserName == null)
            {
                Debug.LogError("Invalid UserName Input field");
                return;
            }

            if (input_CompanyName == null)
            {
                Debug.LogError("Invalid Company Name Input field");
                return;
            }

            if (!ValidateInputFields())
                return;

            Debug.Log("Submit Sign In on btn Click : " + input_UserName.text + "  " + input_CompanyName.text);
            EventManager.Raise<SubmitSignInCredentialsEvent>(new SubmitSignInCredentialsEvent { userName = input_UserName.text, email = input_CompanyName.text });
        }

        public void OnSignInStatuEvent(IEventBase obj)
        {
            if (obj is SignInStatusEvent)
            {
                SignInStatusEvent signIn = (SignInStatusEvent)obj;
                if (signIn.status)
                {
                    Debug.Log("Submit on successfull Sign In : " + input_UserName.text + "  " + input_CompanyName.text);
                    EventManager.Raise<SubmitUserCredentialsEvent>(new SubmitUserCredentialsEvent { userName = input_UserName.text, companyName = input_CompanyName.text });
                }
                else if (txt_ErrorMsg != null)
                        txt_ErrorMsg.text = signIn.Msg;
            }
        
        }

    }
}
