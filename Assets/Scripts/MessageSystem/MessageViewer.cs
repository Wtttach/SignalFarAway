using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageViewer : MonoBehaviour
{
    public TextMeshProUGUI detailedMessageView;
    public TextMeshProUGUI messageViewTitle;
    public List<string> messageTitles = new List<string>();
    [Multiline]
    public List<string> detailedMessages = new List<string>();

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    { 
    }
    #endregion

    #region APIs
    public void ChangeViewingMessage(int index)
    {
        messageViewTitle.text = messageTitles[index];
        detailedMessageView.text = detailedMessages[index];
    }
    #endregion
}
