using UnityEngine;

public class ProgressionUnlock : MonoBehaviour
{
    [SerializeField] public bool IsItem;
    [Tooltip("If IsItem is false, this field is ignored")]
    [SerializeField] public GameObject Item;
    [SerializeField] public DisplayRewardData DisplayRewardData;
    [Space(20)]
    [SerializeField] public bool SetGameplayChange;
    [SerializeField] public GameplayChange ChangeToSet;

    private void Awake()
    {
        if (DisplayRewardData != null)
        {
            DisplayRewardData.IsItem = IsItem;
        }
    }
}
