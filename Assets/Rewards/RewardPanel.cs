using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] GameObject OpenButton; 
    [SerializeField] TextMeshProUGUI RollDescriptionText;

    [SerializeField] TextMeshProUGUI QualityText;
    [SerializeField] TextMeshProUGUI OddsText;
    [SerializeField] TextMeshProUGUI RewardDescriptionText;
    [SerializeField] GameObject CloseButton;
    [SerializeField] RewardViewModel RewardViewModel; //I'm definitely not doing this right 
    [SerializeField] TextMeshProUGUI ChestChanceInfo;

    public void Show(float odds)
    {
        OpenButton.SetActive(true);

        RewardDescriptionText.gameObject.SetActive(false);
        QualityText.gameObject.SetActive(false);
        OddsText.gameObject.SetActive(false);
        CloseButton.SetActive(false);

        ChestChanceInfo.text = string.Format("({0}% chance)", odds);

        gameObject.SetActive(true);
    }

    public void OpenButtonPressed()
    {
        OpenButton.gameObject.SetActive(false);
        RewardViewModel.WonSpin();
        CloseButton.gameObject.SetActive(true);
    }

    //this is weird now because it kind of comes out of nowhere. This should really just be one script with the viewmodel and this 
    public void WonReward(Reward reward)
    {
        RewardDescriptionText.text = "You got: " + reward.Description;
        RewardDescriptionText.gameObject.SetActive(true);

        QualityText.gameObject.SetActive(true);
        QualityText.text = string.Format("Quality (lower is better): {0}", reward.Quality);

        OddsText.gameObject.SetActive(true);
        OddsText.text = string.Format("Odds: {0}%", reward.MaxScore - reward.MinScore);
    }

    private void Update()
    {
        if (CloseButton.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape)) 
        {
            Close();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
