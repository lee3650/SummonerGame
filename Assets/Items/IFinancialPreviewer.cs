using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFinancialPreviewer
{
    float EffectOnBalance();
    float EffectOnIncome(Vector2 position);
}
