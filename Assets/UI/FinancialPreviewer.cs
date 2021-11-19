using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinancialPreviewer : MonoBehaviour
{
    [SerializeField] ItemSelection ItemSelection;
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] PlaceHomeState HomeState;

    [SerializeField] Color PositiveColor = Color.green;
    [SerializeField] Color NegativeColor = Color.red;

    [SerializeField] TextMeshProUGUI IncomePreview;
    [SerializeField] TextMeshProUGUI BalancePreview;

    private void Update()
    {
        if (ItemSelection.HasItem() || HomeState.IsSelected)
        {
            IFinancialPreviewer preview;

            if (HomeState.IsSelected)
            {
                preview = HomeState.GetHomeTileSummon();
            } else
            {
                preview = ItemSelection.SelectedItem.GetComponent<IFinancialPreviewer>();
            }

            if (preview != null)
            {
                ShowPreviews(preview);
            } else
            {
                HidePreviews();
            }
        } else
        {
            HidePreviews();
        }
    }

    private void HidePreviews()
    {
        IncomePreview.gameObject.SetActive(false);
        BalancePreview.gameObject.SetActive(false);
    }

    private void ShowPreviews(IFinancialPreviewer preview)
    {
        float income = preview.EffectOnIncome(VectorRounder.RoundVector(PlayerInput.GetWorldMousePosition()));

        if (income == 0)
        {
            IncomePreview.text = "";
        } else
        {
            IncomePreview.text = string.Format("{0}{1}", GetSignChar(income), FloatRounder.RoundFloat(Mathf.Abs(income), 2));
        }
        
        if (income > 0)
        {
            IncomePreview.color = PositiveColor;
        } else
        {
            IncomePreview.color = NegativeColor;
        }

        float balance = preview.EffectOnBalance();
        if (balance > 0)
        {
            BalancePreview.text = string.Format("-{0}", FloatRounder.RoundFloat(balance, 2));
        } else
        {
            BalancePreview.text = "";
        }

        IncomePreview.gameObject.SetActive(true);
        BalancePreview.gameObject.SetActive(true);
    }

    private char GetSignChar(float num)
    {
        if (num < 0)
        {
            return '-';
        }
        return '+';
    }

}
