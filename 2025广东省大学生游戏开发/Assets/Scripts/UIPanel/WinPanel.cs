using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : BasePanel
{
    public void SetImg(Sprite sprite)
    {
        GetControl<Image>("winIMG").sprite = sprite;
    }
}
