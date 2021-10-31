using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncomeDisplay : MonoBehaviour
{
    //well, I guess we could use a panel displayer for this. 
    //Probably not necessary. 

    [SerializeField] Summoner PlayerSummoner;
    [SerializeField] PlayerIncome PlayerIncome; 
    [SerializeField] TextMeshProUGUI IncomeText;
    [SerializeField] TextMeshProUGUI ExpensesText;
    [SerializeField] TextMeshProUGUI NetIncomeText; 

    float Income = 0f;
    float Expenses = 0f;

    string incomeText = "";
    string expensesText = "";

    private void Awake()
    {
        PlayerSummoner.FinancialsChanged += FinancialsChanged;
        PlayerIncome.IncomeChanged += IncomeChanged;
        FinancialsChanged();
    }

    private void FinancialsChanged()
    {
        RecalculateFinancials();
        UpdateText();
    }

    private void IncomeChanged()
    {
        RecalculateFinancials();
        UpdateText();
    }

    void RecalculateFinancials()
    {
        incomeText = "";
        expensesText = "";

        Income = PlayerIncome.GetIncome(); 
        Expenses = 0f;

        List<Summon> summons = PlayerSummoner.GetSummons();

        foreach (Summon s in summons)
        {
            float income = s.GetIncome();
            if (income != 0)
            {
                incomeText += string.Format("{0}: +{1}\n", s.SummonName, income);
            }
            Income += income;

            Expenses += s.GetMaintenanceFee();
        }

        List<Blueprint> bps = BlueprintManager.GetSatisfiedBlueprints();

        foreach (Blueprint b in bps)
        {
            expensesText += string.Format("{0}: -{1}\n", BlueprintManager.GetNameFromBlueprintType(b.BlueprintType), b.MaintenanceFee);
        }
    }

    void UpdateText()
    {
        IncomeText.text = incomeText;
        ExpensesText.text = expensesText;
        NetIncomeText.text = getSignChar((Income - Expenses)) + "" + Mathf.Abs(Income - Expenses);
    }

    private char getSignChar(float num)
    {
        if (Mathf.Sign(num) == 1)
        {
            return '+';
        }
        return '-';
    }
}