using UnityEngine;

public class ColorChangeReticle : MonoBehaviour
{
    public GameObject rayLine;
    public GameObject reticle;
    Renderer capsuleColor;
    Renderer liner;
    // Start is called before the first frame update
    void Start()
    {
        capsuleColor = reticle.GetComponent<Renderer>();
        liner = rayLine.GetComponent<Renderer>();
    }

    void Update()

    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "test2")
        {
            capsuleColor.material.color = new Color32(0, 0, 255, 122);
            liner.material.color = new Color32(255, 0, 0, 122);

        }
        else
        {
            capsuleColor.material.color = Color.blue;
            liner.material.color = Color.blue;
        }
    }
}
