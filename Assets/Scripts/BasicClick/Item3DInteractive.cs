using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Item3DInteractive : MonoBehaviour, IPointerDownHandler
{
    [Serializable]
    public class ItemClickedEvent : UnityEvent { }

    /// --------------------------Click Event--------------------------
    [FormerlySerializedAs("ItemClicked")]
    [SerializeField]
    protected ItemClickedEvent m_ItemClicked = new ItemClickedEvent();
    public ItemClickedEvent ItemClicked
    { 
        get { return m_ItemClicked; } 
        set { m_ItemClicked = value; }
    }

    // ---------------------------Audio Clips-------------------------
    public AudioSource interactSound;

    // ---------------------------Item State--------------------------
    public enum InteractState { active = 0, notActive = 1 }
    protected InteractState State = InteractState.notActive;

    const float pressSpeed = 0.025f;

    // ---------------------------Coroutines--------------------------
    protected virtual IEnumerator InteractAnim()
    {
        yield return null;
    }

    // ---------------------------Interact State-----------------------
    #region InteractState
    protected bool IsActive()
    {
        if (State == InteractState.active) return true;
        else return false;
    }
    protected void SetActive()
    {
        State = InteractState.active;
    }
    protected void SetNotActive()
    {
        State = InteractState.notActive;
    }
    #endregion

    // --------------------------Iteract functions----------------------

    protected virtual void ItemAct()
    {
        if (IsActive()) return;

        if (interactSound != null)
            interactSound.Play();

        SetActive();
        StartCoroutine(InteractAnim());

        m_ItemClicked.Invoke();
        SetNotActive();
    }

    #region InterfaceRealization
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        ItemAct();
    }
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        interactSound = GetComponent<AudioSource>();
    }
    #endregion
}
