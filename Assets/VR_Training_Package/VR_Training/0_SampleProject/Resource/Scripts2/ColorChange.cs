using UnityEngine;

public class ColorChange : MonoBehaviour
{
    Renderer capsuleColor;
    public GameObject rayLine;
    Renderer liner;
    // Start is called before the first frame update
    void Start()
    {
        capsuleColor = gameObject.GetComponent<Renderer>();
        liner = rayLine.GetComponent<Renderer>();
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "test2")
        {
            capsuleColor.material.color = Color.red;
            liner.material.color = Color.red;

        }
        else
        {
            capsuleColor.material.color = Color.blue;
            liner.material.color = Color.blue;
        }
    }
}
