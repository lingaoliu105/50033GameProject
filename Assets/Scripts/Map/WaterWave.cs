using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterWave : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    private Spline spline;
    private List<Vector2> splinePos = new ();
    public int pointNum;
    public float speed = 30;
    public float currentAngle = 0;
    public float strength = 10;
    public float radDis = 180;
    public float n = 1f;
    private void Start()
    {
        spriteShapeController = GetComponent<SpriteShapeController>();
        spline = spriteShapeController.spline;
        var topLeft = spline.GetPosition(1);
        var topRight = spline.GetPosition(2);
        for (int i = 0; i < pointNum; i++)
        {
            spline.InsertPointAt(2+i,topLeft + (topRight.x - topLeft.x)/ (pointNum+1)*(i+1)* Vector3.right);
        }
        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            if (i >= 2 && i < spline.GetPointCount() - 2)
            {
                spline.SetTangentMode(i,ShapeTangentMode.Continuous);
                spline.SetLeftTangent(i,new Vector3(-n,0,0));
                spline.SetRightTangent(i,new Vector3(n,0,0));
                
            }
            splinePos.Add(spline.GetPosition(i));

        }
    }

    private void Update()
    {
        currentAngle += speed * Time.deltaTime;
        if (currentAngle > 360)
        {
            currentAngle -= 360;
        }

        int n = spline.GetPointCount();
        float prevAngle =currentAngle * Mathf.Deg2Rad;
        for (int i = 2; i < n-2; i++)
        {
            float newAngle = prevAngle + radDis * Mathf.Deg2Rad;
            float dh = Mathf.Sin(newAngle) * strength;
            spline.SetPosition(i,splinePos[i] + new Vector2(0,dh));
            prevAngle = newAngle;

        }
        
    }
}