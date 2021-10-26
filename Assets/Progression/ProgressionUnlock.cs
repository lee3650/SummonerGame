using UnityEngine;

public class ProgressionUnlock : MonoBehaviour
{
    [SerializeField] public bool IsItem;
    [Tooltip("If IsItem is false, this field is ignored")]
    [SerializeField] public GameObject Item;
    [SerializeField] public DisplayRewardData DisplayRewardData;
}
