using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_AngleController : MonoBehaviour
{
    public Tool_AngleBase toolAngle;
    public ToolDistanceController toolDisCont;
    public List<GameObject> leftHand, rightHand;
    
    void Start()
    {
        
    }

    public void ShowLeftHand()
    {
        return;
        for (int i = 0; i < leftHand.Count; i++)
        {
            if (leftHand[i])
                leftHand[i].SetActive(true);
            if (rightHand[i])
                rightHand[i].SetActive(false);
        }
    }


    public void AdjustProgressUI(GameObject t,PartsID part)
    {   
        if (part.partType == EnumDefinition.PartsType.PARTS_SLOT)
        {
            switch (part.id)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 300:
                case 301:
                case 302:
                case 303:
                case 304:
                case 400:
                case 401:
                case 402:
                case 403:
                case 404:
                    t.transform.localPosition = new Vector3(-1.2f, 0, -0.05f);
                    break; 

                case 14:
                case 15:
                case 314:
                case 315:
                    t.transform.localPosition = new Vector3(0, 2.86f, -0.05f);
                    break;

                case 23:
                case 24:
                case 25:
                case 323:
                case 324:
                case 325:
                    t.transform.localPosition = new Vector3(0, 1, -0.05f);
                    break;

                case 9:
                case 309:
                    t.transform.localPosition = new Vector3(0, 4, -0.05f);
                    break;
                case 409:
                case 509:
                    t.transform.localPosition = new Vector3(0, -4, -0.05f);
                    break;
                case 88:
                case 89:
                    t.transform.localPosition = new Vector3(0, 4, -0.05f); 
                    break;
                case 90:
                case 290:
                case 91:
                case 291:
                    t.transform.localPosition = new Vector3(0, -5, -0.05f);
                    break;
                case 270:
                case 271:
                case 272:
                case 273:
                case 274:
                case 374:
                case 375:
                    t.transform.localPosition = new Vector3(3, 0, -0.05f);
                    break;
                case 345:
                case 349:
                    t.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 10f);        //BG UI
                    t.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 10f);        //PROCESS Ui

                    t.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0, 0, -2.51f);        //BG UI        
                    t.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0, 0, -2.51f);        //PROCESS UI                    
                    break;
                case 354:
                    t.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 10f);        //BG UI
                    t.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 10f);        //PROCESS Ui

                    t.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0, 0, -3f);        //BG UI        
                    t.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0, 0, -3f);        //PROCESS UI                    
                    break;

                case 240:   //고전압 차단
                case 241:
                case 242:
                case 243:
                case 244:
                case 245:
                case 246:
                    t.transform.localPosition = new Vector3(3, 0, -0.05f);
                    break;
                case 207:  
                case 208:
                case 209:
                case 210:
                case 211:
                case 212:
                    t.transform.localPosition = new Vector3(0, 2.4f, 0);
                    break;
                case 213:
                case 214:
                    t.transform.localPosition = new Vector3(0, 2f, 0);
                    t.transform.localScale = new Vector3(20f, 20f, 20f);
                    break;
                case 336:
                case 337:
                case 338:
                case 339:
                    t.transform.localPosition = new Vector3(0, 1f, 0);                    
                    break;
                case 226:
                    t.transform.localPosition = new Vector3(0, -2f, 0);
                    break;
                case 626:
                    t.transform.localPosition = new Vector3(0, 0.7f, 0);
                    t.transform.localScale = new Vector3(10f, 10f, 10f);
                    break;
                case 279:
                case 280:
                    t.transform.localPosition = new Vector3(2f, -0.1f, -0.087f);
                    t.transform.localScale = new Vector3(4f, 4f, 4f);
                    break;
                case 679:
                case 680:
                    t.transform.localPosition = new Vector3(2f, -0.1f, -0.087f);
                    t.transform.localScale = new Vector3(20f, 20f, 20f);
                    break;
                case 281:
                case 282:
                case 283:
                case 284:
                case 285:
                case 286:
                case 287:
                case 288:
                case 275:
                case 276:
                case 388:
                case 390:
                case 391:
                    t.transform.localPosition = new Vector3(2f, -0.1f, -0.087f);
                    t.transform.localScale = new Vector3(20f, 20f, 20f);
                    break;
                case 248:
                case 251:
                    t.transform.localPosition = new Vector3(3f, 0, 0);
                    break;
                case 249:
                case 250:
                case 252:
                case 253:
                case 254:
                case 255:
                case 256:
                case 257:
                case 259:
                case 260:
                case 261:
                case 262:
                    t.transform.localPosition = new Vector3(2.2f, 0, 0);                    
                    break;
            }
        }

    }

}
