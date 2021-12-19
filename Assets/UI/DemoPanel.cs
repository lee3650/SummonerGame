using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPanel : MonoBehaviour
{
    public void Wishlist()
    {
        System.Diagnostics.Process.Start("https://store.steampowered.com/app/1811940/Archipelago/");
    }

    public void Survey()
    {
        System.Diagnostics.Process.Start("https://docs.google.com/forms/d/e/1FAIpQLSdnLDSjYqWpvGJtTTqkpiosMlCCrSeur8tsnZcHzdagJvFXhw/viewform?usp=sf_link");
    }
}
