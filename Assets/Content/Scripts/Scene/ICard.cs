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
    public abstract void ShowCard();
    public abstract void HideCard();
    public abstract void LoadContent();
    public abstract void DisposeContent();
}
