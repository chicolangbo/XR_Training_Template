using UnityEngine;
using UnityEngine.UI;

public class TEST2 : MonoBehaviour
{
    [SerializeField]
    float eulerAngX;
    [SerializeField]
    float eulerAngY;
    [SerializeField]
    float eulerAngZ;

    public bool actOn = false;
    Transform fboltRot, fleft, pvot, wrenPivot;
    GameObject canvases;
    public Image amountFill;
    public float sumRot, rotSpeed, value2;
    public int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        fboltRot = GameObject.FindWithTag("Bolt").transform;
        fleft = GameObject.FindWithTag("DR").transform;
        wrenPivot = GameObject.FindWithTag("WrenchPivot").transform;
        canvases = GameObject.FindGameObjectWithTag("NutCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (actOn == true)
        {
            boltRotOn2();
        }
    }
    public void TrackingHand()
    {

    }
    public void boltRotOn2()
    {
        actOn = true;
        canvases.SetActive(true);
    }
    // 그랩 시 기능 및 게이지 ON
    public void boltRotOn()
    {
        actOn = true;
        canvases.SetActive(true);
        AxisYLookat();
        AmountCal();
        AmountGage();
    }
    //그랩 안할 시 기능 및 게이지 OFF
    public void boltRotOff()
    {
        actOn = false;
        canvases.SetActive(false);
    }

    // Y축 기준으로 회전하기
    public void AxisYLookat()
    {
        var lookPos = fleft.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 100f);

        eulerAngX = transform.localEulerAngles.x;
        eulerAngY = transform.localEulerAngles.y;
        eulerAngZ = transform.localEulerAngles.z;

        if (eulerAngY > 180f)
        {
            if (eulerAngX > 256f)
                sumRot = (eulerAngX * -1f) + 360f;
            else
                sumRot = -eulerAngX;
            
        }
        else
        {

            if (eulerAngX > 256f)
                sumRot = eulerAngX - 180f;
            else
                sumRot = ((eulerAngX * -1f) + 180f) * -1f;
        }

        if (eulerAngY > 360.0f)
        {
            cnt++;
        }

        /*if (eulerAngY > 180f)
        {
            if (eulerAngX > 256f)
                sumRot = (eulerAngX * -1f) + 360f;
            else
                sumRot = -eulerAngX;
        }
        else
        {

            if (eulerAngX > 256f)
                sumRot = eulerAngX - 180f;
            else
                sumRot = ((eulerAngX * -1f) + 180f) * -1f;
        }

        if (transform.rotation.y < 0)
        {
            sumRot += transform.rotation.y - eulerAngY;
        }*/

    }

    private float remap(float aValue, float oriMin, float oriMax, float newMin, float newMax)
    {
        float normal = Mathf.InverseLerp(oriMin, oriMax, aValue);
        float bValue = Mathf.Lerp(newMin, newMax, normal);
        return bValue;
    }
    //회전 값 받아서 계산하기
    public void AmountCal()
    {
        
    }

    public void AmountGage()
    {
        var t = 0; // 값 미정
        float gageAmount;
        gageAmount = remap(t, 0, 1080f, 0, 1f);
    }
}
