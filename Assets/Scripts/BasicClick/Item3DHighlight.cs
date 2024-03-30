using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Item3DHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject vfx;
    // ---------------------------Coroutines--------------------------
    protected virtual IEnumerator InteractAnim()
    {
        yield return null;
    }

    // --------------------------Iteract functions----------------------

    #region InterfaceRealization
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        vfx.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        vfx.SetActive(false);
    }
    #endregion

    #region MonoBehaviour
    private void Start()
    {
    }
    #endregion
}
