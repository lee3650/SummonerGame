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
        Income = PlayerIncome.GetIncome(); 
        Expenses = 0f;

        List<Summon> summons = PlayerSummoner.GetSummons();

        foreach (Summon s in summons)
        {
            Income += s.GetIncome();
            Expenses += s.GetMaintenanceFee();
        }
    }

    void UpdateText()
    {
        IncomeText.text = "Income: " + Income;
        ExpensesText.text = "Expenses: " + Expenses;
        NetIncomeText.text = "Net Income: " + (Income - Expenses);
    }
}