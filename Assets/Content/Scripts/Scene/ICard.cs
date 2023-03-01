using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public bool IsHide
    {
        get;
        set;
    }
    public void ShowCard();
    public void HideCard();
    public void LoadContent();
    public void DisposeContent();
}
