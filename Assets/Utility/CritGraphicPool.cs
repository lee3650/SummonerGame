using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritGraphicPool : MonoBehaviour, IResettable
{
    [SerializeField] SpriteRenderer critGraphic;

    private static float maxlife = 0.5f;

    private static List<SpriteRenderer> CritGraphics = new List<SpriteRenderer>();
    private static List<float> lifetime = new List<float>();
    private static SpriteRenderer CritGraphic;

    void Awake()
    {
        CritGraphic = critGraphic;
    }

    private void Update()
    {
        AnimateCrits();        
    }

    private static void AnimateCrits()
    {
        for (int i = 0; i < CritGraphics.Count; i++)
        {
            if (lifetime[i] > 0)
            {
                lifetime[i] -= Time.deltaTime;
                if (lifetime[i] < 0)
                {
                    CritGraphics[i].gameObject.SetActive(false);
                } else
                {
                    if ((int)(lifetime[i] * 10) % 2 == 0)
                    {
                        CritGraphics[i].color = Color.red;
                    } else
                    {
                        CritGraphics[i].color = Color.white;
                    }
                }
            }
        }
    }

    public static void ShowCrit(Vector2 position)
    {
        for (int i = 0; i < CritGraphics.Count; i++)
        {
            if (lifetime[i] < 0)
            {
                lifetime[i] = maxlife;
                CritGraphics[i].transform.position = position;
                CritGraphics[i].enabled = true;
                CritGraphics[i].gameObject.SetActive(true);
                return;
            }
        }

        SpriteRenderer newGraphic = Instantiate(CritGraphic, position, Quaternion.Euler(Vector3.zero));
        CritGraphics.Add(newGraphic);
        lifetime.Add(maxlife);
    }

    public void ResetState()
    {
        CritGraphics = new List<SpriteRenderer>();
        lifetime = new List<float>();
        CritGraphic = null;
    }
}
