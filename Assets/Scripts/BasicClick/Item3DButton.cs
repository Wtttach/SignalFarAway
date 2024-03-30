using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Item3DButton : Item3DInteractive 
{
    // ---------------------------Move Paras------------------------
    public enum ButtonMoveAxis { XAxis, YAxis, ZAxis }
    public ButtonMoveAxis moveAxis = ButtonMoveAxis.YAxis;
    public float moveDist = -0.05f;
    public float moveSpeed = 0.025f;

    // ---------------------------Audio Source------------------------
    AudioSource buttonPress;

    // ---------------------------Coroutines--------------------------
    protected IEnumerator InteractAnim(Vector3 axis)
    {
        var originalPos = transform.localPosition;
        var targetPos = transform.localPosition + axis * moveDist;

        while ((transform.localPosition - targetPos).magnitude > 0.001f)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        while ((transform.localPosition - originalPos).magnitude > 0.001f)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition,originalPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        SetNotActive();
    }

    protected override void ItemAct()
    {
        if (IsActive()) return;

        buttonPress.Play();
        SetActive();
        switch(moveAxis)
        {
            case ButtonMoveAxis.XAxis: StartCoroutine(InteractAnim(Vector3.right)); break;
            case ButtonMoveAxis.YAxis: StartCoroutine(InteractAnim(Vector3.up)); break;
            case ButtonMoveAxis.ZAxis: StartCoroutine(InteractAnim(Vector3.forward)); break;
        }

        m_ItemClicked.Invoke();
    }

    private void Start()
    {
        buttonPress = GetComponent<AudioSource>();
    }
}
