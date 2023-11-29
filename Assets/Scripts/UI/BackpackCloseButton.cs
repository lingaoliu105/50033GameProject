using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackCloseButton : MonoBehaviour,IInteractiveButton
{
    public GameObject backpackPanel;

    public void OnClick()
    {
        backpackPanel.SetActive(false);
    }
}
