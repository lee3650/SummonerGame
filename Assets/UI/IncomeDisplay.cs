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


    float Income = 0f;
    float Expenses = 0f; 

    private void Awake()
    {
        PlayerSummoner.SummonsChanged += SummonsChanged;
        SummonsChanged();
    }

    private void SummonsChanged()
    {
        RecalculateFinancials();
        UpdateText();
    }

    void RecalculateFinancials()
    {
        Income = PlayerIncome.GetIncome(); //this is an example of the problem - this is weird and patchwork 
        Expenses = 0f;

        List<Summon> summons = PlayerSummoner.GetSummons();

        foreach (Summon s in summons)
        {
            //I guess we should do this abstractly. At least it's top down.
            Income += s.GetIncome();
            Expenses += s.GetMaintenanceFee();
        }
    }

    void UpdateText()
    {
        IncomeText.text = "Income: " + Income;
        ExpensesText.text = "Expenses: " + Expenses;
    }
}