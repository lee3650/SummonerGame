using UnityEngine;

public class EnableOnWaveEnds : MonoBehaviour, IWaveNotifier
{
    [SerializeField] bool EnableOnLevelEnd = false;
    [SerializeField] bool DisableOnLevelEnd = false;
    [SerializeField] NextWaveFunctionMonitor NextWaveFunctionMonitor;
    private void Awake()
    {
        WaveSpawner.NotifyWhenWaveEnds(this);
    }

    public void OnWaveEnds()
    {
        if (EnableOnLevelEnd)
        {
            if (NextWaveFunctionMonitor.DoesNextWaveButtonStartNextLevel())
            {
                gameObject.SetActive(true);
            }
        }
        else if (DisableOnLevelEnd)
        {
            if (NextWaveFunctionMonitor.DoesNextWaveButtonStartNextLevel())
            {
                gameObject.SetActive(false);
            } else
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
