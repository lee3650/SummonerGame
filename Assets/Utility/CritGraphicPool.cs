using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritGraphicPool : MonoBehaviour, IResettable
{
    [SerializeField] SpriteRenderer graphic;
    [SerializeField] Sprite critGraphic;
    [SerializeField] Sprite dodgeGraphic;

    private static SpriteRenderer Graphic;

    private static float maxlife = 0.5f;
    private static List<SpriteRenderer> Graphics = new List<SpriteRenderer>();
    private static List<float> lifetime = new List<float>();

    private static Sprite CritGraphic;
    private static Sprite DodgeGraphic;

    void Awake()
    {
        CritGraphic = critGraphic;
        DodgeGraphic = dodgeGraphic;
        Graphic = graphic;
    }

    private void Update()
    {
        AnimateCrits();        
    }

    private static void AnimateCrits()
    {
        for (int i = 0; i < Graphics.Count; i++)
        {
            if (lifetime[i] > 0)
            {
                lifetime[i] -= Time.deltaTime;
                if (lifetime[i] < 0)
                {
                    Graphics[i].gameObject.SetActive(false);
                } else
                {
                    if ((int)(lifetime[i] * 10) % 2 == 0)
                    {
                        Graphics[i].color = Color.red;
                    } else
                    {
                        Graphics[i].color = Color.white;
                    }
                }
            }
        }
    }

    public static void ShowCrit(Vector2 position)
    {
        AddOrShowGraphic(position, CritGraphic);
    }

    public static void ShowDodge(Vector2 position)
    {
        AddOrShowGraphic(position, DodgeGraphic);
    }

    private static void AddOrShowGraphic(Vector2 position, Sprite sprite)
    {
        for (int i = 0; i < Graphics.Count; i++)
        {
            if (lifetime[i] < 0)
            {
                lifetime[i] = maxlife;
                Graphics[i].transform.position = position;
                Graphics[i].sprite = sprite;
                Graphics[i].gameObject.SetActive(true);
                return;
            }
        }

        SpriteRenderer newGraphic = Instantiate(Graphic, position, Quaternion.Euler(Vector3.zero));
        newGraphic.sprite = sprite;
        Graphics.Add(newGraphic);
        lifetime.Add(maxlife);
    }

    public void ResetState()
    {
        Graphics = new List<SpriteRenderer>();
        lifetime = new List<float>();
    }
}
