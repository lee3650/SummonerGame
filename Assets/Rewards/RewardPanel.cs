using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] GameObject RollButton; 
    [SerializeField] TextMeshProUGUI RollText; 
    [SerializeField] TextMeshProUGUI RollDescriptionText;

    [SerializeField] TextMeshProUGUI QualityText;
    [SerializeField] TextMeshProUGUI OddsText;
    [SerializeField] TextMeshProUGUI RewardDescriptionText;
    [SerializeField] GameObject CloseButton;
    [SerializeField] RewardViewModel RewardViewModel; //I'm definitely not doing this right 

    float SpinThreshold;

    public void Show(float spinThreshold)
    {
        SpinThreshold = spinThreshold;
        UpdateRollDescriptionText();
        RollButton.SetActive(true);
        RollText.gameObject.SetActive(true);
        RollText.text = "";

        RewardDescriptionText.gameObject.SetActive(false);
        QualityText.gameObject.SetActive(false);
        OddsText.gameObject.SetActive(false);
        CloseButton.SetActive(false);

        gameObject.SetActive(true);
    }

    public void RollButtonPressed()
    {
        RollButton.gameObject.SetActive(false);
        StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        //we do this so it displays instantly, right. 
        float random = Random.Range(0, 100f);
        RollText.text = string.Format("{0}", random);
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.05f * (i + 1)); //maybe we slow it down, eh? 
            random = Random.Range(0, 100f);
            RollText.text = string.Format("{0}", random);
        }

        if (random <= SpinThreshold)
        {
            Won();
        } 
        else
        {
            Lost();
        }

        CloseButton.SetActive(true);
    }

    void Won()
    {
        RewardViewModel.WonSpin();
    }

    void Lost()
    {
        RewardDescriptionText.gameObject.SetActive(true);
        RewardDescriptionText.text = "You got: Nothing. Better luck next time.";
    }

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

    void UpdateRollDescriptionText()
    {
        SetRollDescriptionText(SpinThreshold);
    }

    void SetRollDescriptionText(float necessaryValue)
    {
        RollDescriptionText.text = "Must be less than " + necessaryValue;
    }
}
