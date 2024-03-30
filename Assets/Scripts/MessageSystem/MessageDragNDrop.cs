using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MessageDragNDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    VerticalLayoutGroup layoutGroup;
    RectTransform rectTransform;
    public float moveSpeed;
    public float popZBias = 3f;
    private Vector3 originalPos;

    int sibling;
    int maxSibling;

    GameObject tempPlace;

    private float popZ;
    // Start is called before the first frame update
    void Start()
    {
        popZ = popZBias + transform.localPosition.z;
        layoutGroup = transform.parent.GetComponent<VerticalLayoutGroup>();
        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        maxSibling = transform.parent.childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region InterfaceRealization
    /// <summary>
    /// Make the message floating when draging
    /// </summary>
    /// <param name="popZ"> height for floating </param>
    /// <returns></returns>

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        tempPlace = GameObject.Instantiate(this.gameObject, transform.parent);
        tempPlace.transform.localScale = Vector3.zero;
        originalPos = transform.position;
        sibling = (int)((layoutGroup.padding.top - transform.localPosition.y) / (layoutGroup.spacing + 100));
        tempPlace.transform.SetSiblingIndex(sibling);

        layoutGroup.enabled = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y,(((transform.position.y - ray.origin.y) / ray.direction.y) * ray.direction + ray.origin).z);
        transform.position = targetPos;

        sibling = (int)((layoutGroup.padding.top - transform.localPosition.y) / (layoutGroup.spacing + 100));
        sibling = Mathf.Min(sibling, maxSibling + 1);
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        transform.SetSiblingIndex(sibling);
        Destroy(tempPlace);
        layoutGroup.enabled = true;
        
        
        if (sibling == maxSibling) transform.localPosition = new Vector3(transform.localPosition.x, -layoutGroup.padding.top - (sibling) * (layoutGroup.spacing + 100), 0);
    }

    #endregion
}
