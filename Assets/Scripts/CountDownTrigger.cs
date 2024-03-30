using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System;

public class CountDownTrigger : MonoBehaviour
{
    [Serializable]
    public class TriggerEvent : UnityEvent { }

    [FormerlySerializedAs("Event")]
    [SerializeField]
    private TriggerEvent m_triggerEvent = new TriggerEvent();

    public TriggerEvent Event { get { return m_triggerEvent; } set { m_triggerEvent = value; } }
    public float countDownTime;

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(countDownTime);

        m_triggerEvent.Invoke();
    }

    private void Start()
    {
        StartCoroutine(CountDown());
    }
}
