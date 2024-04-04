using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CurveLine : MonoBehaviour
{
    public LineRenderer lineRender;
    
    // 0 : start , 1 : middle , 2 : end ( point )
    public List<Transform> linePoints = new List<Transform>();
    public float curvePosY = 0.8f;
    public float lineWidth = 0.1f;
    Vector3 point;

    List<Vector3> line_CurPointList = new List<Vector3>();
    List<Vector3> line_PervPointList = new List<Vector3>();
    List<Vector3> debug_pointList = new List<Vector3>();

    void Start()
    {
        if(gameObject.transform.TryGetComponent<LineRenderer>(out LineRenderer rd))
        {
            lineRender = rd;
        }
        else
        {
            lineRender = gameObject.AddComponent<LineRenderer>();
            lineRender.SetWidth(lineWidth, lineWidth);
        }
    }

    void Update()
    {
        if(linePoints.Count == 3)
        {
            SetMiddlePointPos();

            if (linePoints.Count >= 3 && line_CurPointList.Count <= 0)
            {
                Init();
            }

            if (debug_pointList.Count <= 0)
            {
                CalcPath();
            }

            if (!isPointsSequenceEqua())
            {
                CalcPath();
                Init();
            }

            DrawCurveLine();
        }
    }

    void SetMiddlePointPos()
    {
        
        var x = (linePoints[0].localPosition.x + linePoints[2].localPosition.x) *0.5f;
        var z = (linePoints[0].localPosition.z + linePoints[2].localPosition.z) *0.5f;
        linePoints[1].localPosition = new Vector3(x, curvePosY, z);
    }



    void Init()
    {
        line_CurPointList = GetCurPointPosition();
        line_PervPointList = GetCurPointPosition();
    }

    List<Vector3> GetCurPointPosition()
    {
        return linePoints.Select(s => s.position).ToList();
    }

    void CalcPath()
    {
        //debug_pointList.Clear();
        for (int i = 0; i < 22; i++)
        {
            float index = i;
            var value = index / (float)21 * 100f;
            point = GetBeziercCurve_(linePoints, value * 0.01f);
            try
            {
                debug_pointList[i] = point;
            }
            catch
            {
                debug_pointList.Add(point);
            }
            
        }
    }


    Vector3 GetBezierCurve(Vector3 p0, Vector3 p1, float t)
    {
        return ((1 - t) * p0) + ((t) * p1);
    }

    Vector3 GetBeziercCurve_(List<Transform> list, float t)
    {
        List<Vector3> points = new List<Vector3>();
        List<Vector3> calcPoints = new List<Vector3>();

        for (int i = 0; i < list.Count; i++)
        {
            if (i == list.Count - 1) break;
            var p = GetBezierCurve(list[i].position, list[i + 1].position, t);
            points.Add(p);
        }

        for (int i = 0; i < points.Count; i++)
        {
            if (i == points.Count - 1) break;
            var p = GetBezierCurve(points[i], points[i + 1], t);
            calcPoints.Add(p);
        }

        if (calcPoints.Count <= 1) return calcPoints.LastOrDefault();

        var lastIndex = calcPoints.IndexOf(calcPoints.LastOrDefault());
        return GetBezierCurve(calcPoints[lastIndex - 1], calcPoints[lastIndex], t);
    }

    bool isPointsSequenceEqua()
    {
        return GetCurPointPosition().SequenceEqual(line_PervPointList);
    }

    void DrawCurveLine()
    {
        if(lineRender.positionCount != debug_pointList.Count)
        {
            lineRender.positionCount = debug_pointList.Count;
        }
        if (debug_pointList.Count > 0)
        {
            lineRender.SetPositions(debug_pointList.ToArray());
        }
        
    }
}
