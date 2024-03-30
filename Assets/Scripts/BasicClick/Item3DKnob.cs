using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3DKnob : Item3DInteractive
{
    // -----------------------------Rotate Paras-----------------------------
    public enum KnobRotateAxis { XAxis, YAxis, ZAxis };
    public int knobLevels = 5;
    private int currentLevel;
    public KnobRotateAxis rotateAxis;
    public float rotateAngle = 25f;
    public float rotateSpeed = 30f;
    Quaternion originalRotation;

    IEnumerator rotateAnim;

    // -----------------------------Audio Source-----------------------------
    AudioSource knobSource;

    // -----------------------------Coroutines-------------------------------
    protected IEnumerator InteractAnim(Vector3 axis)
    {
        if (currentLevel <= 0)
        {
            transform.localRotation = originalRotation;
            SetNotActive();
            yield break;
        }

        float currentAngle = 0;
        while (currentAngle < Mathf.Abs(rotateAngle))
        {
            currentAngle += rotateSpeed * Time.deltaTime;
            transform.localRotation *= Quaternion.Euler(axis * rotateSpeed * Time.deltaTime * (rotateAngle/Mathf.Abs(rotateAngle)));
            yield return null;
        }
        SetNotActive();

        Debug.Log(transform.localRotation);
    }

    protected override void ItemAct()
    {
        if (IsActive()) return;

        currentLevel--;
        knobSource.Play();

        SetActive();

        
        switch(rotateAxis)
        {
            case KnobRotateAxis.XAxis: rotateAnim = InteractAnim(Vector3.right); break;
            case KnobRotateAxis.YAxis: rotateAnim = InteractAnim(Vector3.up); break;
            case KnobRotateAxis.ZAxis: rotateAnim = InteractAnim(Vector3.forward); break;
        }

        if (rotateAnim != null)
            StartCoroutine(rotateAnim);

        if (currentLevel <= 0) currentLevel = knobLevels;

        Debug.Log(currentLevel);
        m_ItemClicked.Invoke();
    }

    public void ResetKnob()
    {
        currentLevel = -1;
        SetNotActive();

        if (rotateAnim != null)
            StopCoroutine(rotateAnim);

        switch (rotateAxis)
        {
            case KnobRotateAxis.XAxis: transform.localRotation = originalRotation; break;
            case KnobRotateAxis.YAxis: transform.localRotation = originalRotation; break;
            case KnobRotateAxis.ZAxis: transform.localRotation = originalRotation; break;
        }

        currentLevel = knobLevels;
    }

    #region MonoBehavior
    private void Start()
    {
        currentLevel = knobLevels;
        originalRotation = transform.localRotation;
        Debug.Log(transform.localRotation);
        knobSource = GetComponent<AudioSource>();
    }
    #endregion
}