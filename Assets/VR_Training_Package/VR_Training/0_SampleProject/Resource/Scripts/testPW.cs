using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace VRKeyboard.Utils
{
    public class testPW : MonoBehaviour
    {
        public bool pwValue = false;
        public testID testid;
        public Button pwButton;
        public GameObject placeholder;

        void Start()
        {
            ThisButton();
        }

        void ThisButton()
        {
            pwButton.onClick.AddListener(ChangeValue);

        }

        public void ChangeValue()
        {
            pwValue = true;
            testid.idValue = false;
            placeholder.SetActive(false);
            testid.placeholder.SetActive(true);
        }
    }
}

