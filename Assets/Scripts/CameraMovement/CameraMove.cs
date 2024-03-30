using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector2 mousePos;
    public Vector2 nonInterArea;
    public Vector2 rotateBoundX, rotateBoundY;
    public float moveSpeed;


    // Update is called once per frame
    void Update()
    {

        mousePos = Input.mousePosition;
        mousePos.x /= Screen.width;
        mousePos.y /= Screen.height;
        
        mousePos -= new Vector2(0.5f, 0.5f);
        if (mousePos.magnitude > new Vector2(0.5f, 0.5f).magnitude) return;

        Vector3 targetAngles = Vector3.zero;

        if (Mathf.Abs(mousePos.x) > nonInterArea.x)
        {
            if (mousePos.x < 0)
                targetAngles.y = rotateBoundX.x;
            else targetAngles.y = rotateBoundX.y;

            targetAngles.x = transform.eulerAngles.x;
        }
        if(Mathf.Abs(mousePos.y) > nonInterArea.y)
        {
            if (mousePos.y > 0)
                targetAngles.x = rotateBoundY.x;
            else targetAngles.x = rotateBoundY.y;

            targetAngles.y = transform.eulerAngles.y;
        }

        

        if (Mathf.Abs(mousePos.x) > nonInterArea.x || Mathf.Abs(mousePos.y) > nonInterArea.y)
        {
            Vector3 current = transform.eulerAngles;
            if (current.x > 180f) current.x -= 360;
            if (current.y > 180f) current.y -= 360;
            transform.eulerAngles = (Vector3.MoveTowards(current, targetAngles, moveSpeed));
        }
    }
}
