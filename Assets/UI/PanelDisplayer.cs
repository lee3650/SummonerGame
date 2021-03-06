using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDisplayer : MonoBehaviour
{
    [SerializeField] RectTransform PanelParent;
    List<GameObject> DisplayedPanels = new List<GameObject>();

    public UIPanel ShowPanel(UIPanel panel, object information)
    {
        UIPanel p = Instantiate(panel, PanelParent);
        p.Show(information);
        DisplayedPanels.Add(p.gameObject);
        return p;
    }

    public void HideAllPanels()
    {
        foreach (GameObject g in DisplayedPanels)
        {
            Destroy(g);
        }

        DisplayedPanels = new List<GameObject>();
    }
}
