using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
    [SerializeField] Slider XPSlider;
    
    void Update()
    {
        XPSlider.value = ResearchManager.GetCurrentResearchPercent();
    }
}
