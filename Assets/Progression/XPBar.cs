using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
    [SerializeField] Slider XPSlider;
    [SerializeField] TextMeshProUGUI CurrentLevelText;
    [SerializeField] TextMeshProUGUI CurrentXPText;
    
    void Update()
    {
        XPSlider.value = ExperienceManager.GetCurrentLevelPercentage();
        CurrentLevelText.text = string.Format("{0}", ExperienceManager.GetCurrentLevel());
        CurrentXPText.text = string.Format("{0}/{1}", FloatRounder.RoundFloat(ExperienceManager.GetCurrentLevelXP(), 2), ExperienceManager.GetXPToNextLevel(ExperienceManager.GetCurrentLevel()));
    }
}
