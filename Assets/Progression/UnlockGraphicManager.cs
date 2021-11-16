using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockGraphicManager : MonoBehaviour
{
    [SerializeField] UnlockGraphic[] unlockGraphics;
    [SerializeField] ProgressionManager ProgressionManager;
    [SerializeField] Sprite letterSprite;

    private void Start()
    {
        for (int i = 0; i <= unlockGraphics.Length; i++)
        {
            ProgressionUnlock[] unlocks = ProgressionManager.GetUnlocksFromLevel(i);
            if (unlocks.Length != 0)
            {
                ProgressionUnlock u = unlocks[0];
                Sprite s = GetSprite(u);
                unlockGraphics[i - 1].SetGraphic(s, u.DisplayRewardData, i);
            } else
            {
                if (i > 0)
                {
                    unlockGraphics[i - 1].gameObject.SetActive(false);
                }
            }
        }
    }

    private Sprite GetSprite(ProgressionUnlock data)
    {
        if (data.IsItem)
        {
            SummonWeapon s;
            if (data.Item.TryGetComponent<SummonWeapon>(out s))
            {
                return s.GetSummonSprite();
            }

            return data.Item.GetComponent<SpriteRenderer>().sprite;
        }

        return letterSprite;
    }
}
