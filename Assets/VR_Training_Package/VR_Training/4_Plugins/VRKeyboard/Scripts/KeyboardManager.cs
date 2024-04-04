/***
 * Author: Yunhan Li
 * Any issue please contact yunhn.lee@gmail.com
 ***/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;


namespace VRKeyboard.Utils
{
    public class KeyboardManager : MonoBehaviour
    {
        const string myID = "test";
        const string myPassword = "1234";
        public Text warningText;

        #region Public Variables
        [Header("User defined")]
        [Tooltip("If the character is uppercase at the initialization")]
        public bool isUppercase = false;
        public int maxInputLength;

        [Header("UI Elements")]
        public InputField inputText;
        public InputField id;
        public InputField pw;

        [Header("Essentials")]
        public Transform keys;
        #endregion

        #region Private Variables
        public testID testid;
        public testPW testpw;
        private string Input
        {
            get { return inputText.text; }
            set { inputText.text = value; }
        }
        private Key[] keyList;
        private bool capslockFlag;
        #endregion

        #region Monobehaviour Callbacks
        void Awake()
        {
            keyList = keys.GetComponentsInChildren<Key>();
            //inputText.Select;

            id.text = "test";
            pw.text = "1234"; 
        }//

        void Start()
        {
            foreach (var key in keyList)
            {
                key.OnKeyClicked += GenerateInput;
            }
            capslockFlag = isUppercase;
            CapsLock();
        }

        void Update()
        {
            ChangeIDPW();
        }

        public void ChangeIDPW()
        { 
            if (testid.idValue)
            {
                inputText = id;
            }
            if (testpw.pwValue)
            {
                inputText = pw;
            }
        }


        #endregion

        #region Public Methods
        public void Backspace()
        {
            if (Input.Length > 0)
            {
                Input = Input.Remove(Input.Length - 1);
            }
            else
            {
                return;
            }
        }

        public void Clear()
        {
            Input = "";
        }

        public void CapsLock()
        {
            foreach (var key in keyList)
            {
                if (key is Alphabet)
                {
                    key.CapsLock(capslockFlag);
                }
            }
            capslockFlag = !capslockFlag;
        }

        public void Shift()
        {
            foreach (var key in keyList)
            {
                if (key is Shift)
                {
                    key.ShiftKey();
                }
            }
        }

        public void GenerateInput(string s)
        {
            if (Input.Length > maxInputLength) { return; }
            Input += s;
            SetWarning("",Color.white,false);
        }

        public bool CompareIDAndPassword()
        {
            if (id.text == myID && pw.text == myPassword)
            {
                return true;
            }

            return false;
        }

        public void SetWarning(string warn,Color color,bool enable = true)
        {
        
            warningText.color = color;
            warningText.text = warn;
            warningText.gameObject.SetActive(enable);

        }

        #endregion

    }
}