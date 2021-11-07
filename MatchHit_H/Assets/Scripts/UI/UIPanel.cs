using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    protected UIManager uiManager;

    public virtual void SetupPanelPosition()
    {
        this.GetComponent<RectTransform>().SetPositionAndRotation(transform.parent.transform.localPosition, Quaternion.identity);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

}
