using UnityEngine;

public class EnableOnWaveEnds : MonoBehaviour, IWaveNotifier
{
    private void Awake()
    {
        WaveSpawner.NotifyWhenWaveEnds(this);
    }

    public void OnWaveEnds()
    {
        gameObject.SetActive(true);
    }
}
