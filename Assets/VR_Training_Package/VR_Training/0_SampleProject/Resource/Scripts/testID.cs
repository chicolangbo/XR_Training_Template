using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace VRKeyboard.Utils
{
    public class testID : MonoBehaviour
    {
        public bool idValue = false;
        public testPW testpw;
        public Button idButton;
        public GameObject placeholder;


        void Start()
        {
            ThisButton();
        }

        void ThisButton()
        {
            idButton.onClick.AddListener(ChangeValue);

        }

        public void ChangeValue()
        {
            idValue = true;
            testpw.pwValue = false;
            placeholder.SetActive(false);
            testpw.placeholder.SetActive(true);
        }
    }
}
