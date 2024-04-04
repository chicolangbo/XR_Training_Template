using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swing : MonoBehaviour
{
    public Image image;

    public Image arow;
    public Image right;
    public Image left;
    public Image middle;

    private void Start()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        middle.gameObject.SetActive(false);
    }

    public void SetLeft()
    {
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(false);
        middle.gameObject.SetActive(false);
    }

    public void SetRight()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(true);
        middle.gameObject.SetActive(false);
    }

    public void SetMiddle()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        middle.gameObject.SetActive(true);
    }
}
