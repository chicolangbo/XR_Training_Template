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


            // Mouse의 포지션을 Ray cast 로 변환



            RaycastHit hit;

            if (Physics.Raycast(cast, out hit))
            {

                // TO DO ANY THING...
            }// RayCast
        }// Mouse Click

    }

}
