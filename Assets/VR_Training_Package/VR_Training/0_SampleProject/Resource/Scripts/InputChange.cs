using UnityEngine;
using UnityEngine.EventSystems;

public class InputChange : MonoBehaviour
{
    EventSystem system;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 p = Input.mousePosition;

            Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);


            // Mouse�� �������� Ray cast �� ��ȯ



            RaycastHit hit;

            if (Physics.Raycast(cast, out hit))
            {

                // TO DO ANY THING...
            }// RayCast
        }// Mouse Click

    }

}
