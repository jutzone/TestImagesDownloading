using System.Collections.Generic;
using UnityEngine;


public enum ShowType
{
    AllAtOnce,
    OneByOne,
    ShowWhenReady
}

public class CoreController : MonoBehaviour
{
    [SerializeField] private Card[] cards;
    public delegate void VoidDelegate();
    public static VoidDelegate SelectShowType, StartLoading, CancelLoading;
    private List<int> loadingResultsContainer;
    private int counter;
    private static ShowType _showType;
    public static ShowType ShowType
    {
        get
        {
            return _showType;
        }
        set
        {
            _showType = value;
        }
    }

    private void Awake()
    {
        StartLoading = startLoading;
        CancelLoading = cancelLoading;
        loadingResultsContainer = new List<int>();
        foreach (Card card in cards)
        {
            card.CardLoaded.AddListener(IncreaseContainer);
        }
    }

    private void startLoading()
    {
        counter = 0;
        RefreshContainer();
        foreach (Card card in cards)
        {
            card.IsHide = true;
            card.DisposeContent();
            card.LoadContent();
        }
    }
    private void cancelLoading()
    {
        LoadingManager.Instance.cancelTokenSource.Cancel();
    }
    public void IncreaseContainer(int index)
    {
        switch (ShowType)
        {
            case ShowType.AllAtOnce:
                loadingResultsContainer.Add(index);
                if (loadingResultsContainer.Count >= cards.Length)
                {
                    foreach (ICard card in cards)
                    {
                        card.IsHide = false;
                    }
                }
                break;
            case ShowType.OneByOne:
                loadingResultsContainer.Add(index);
                for (int i = 0; i < cards.Length; i++)
                {
                    if (loadingResultsContainer.Contains(counter) && cards[i].IsHide)
                    {
                        Debug.Log(counter + " counter");
                        cards[counter].IsHide = false;
                        counter++;
                    }
                }
                break;
            case ShowType.ShowWhenReady:
                loadingResultsContainer.Add(index);
                cards[index].IsHide = false;
                break;
        }
    }
    public void RefreshContainer()
    {
        loadingResultsContainer = new List<int>();
    }
}
