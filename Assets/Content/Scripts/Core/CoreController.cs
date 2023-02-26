using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public enum ShowType
{
    AllAtOnce,
    OneByOne,
    ShowWhenReady,
    OneByOneBeauty
}

public class CoreController : MonoBehaviour
{
    [SerializeField] private Card[] cards;
    public delegate void VoidDelegate();
    public static VoidDelegate SelectShowType, StartLoading, CancelLoading;
    private List<int> loadingResultsContainer;
    private List<int> reversedContainer;
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
        reversedContainer = new List<int>();
        foreach (Card card in cards)
        {
            card.CardLoaded.AddListener(increaseLoadedCardsContainer);
            card.CardReversed.AddListener(increaseReversedCardsContainer);
        }
    }

    private void startLoading()
    {
        counter = 0;
        RefreshContainers();
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
        LoadingManager.Instance.cancelTokenSource.Dispose();
        LoadingManager.Instance.cancelTokenSource = new System.Threading.CancellationTokenSource();
    }

    private void increaseReversedCardsContainer(int index)
    {
        reversedContainer.Add(index);
    }
    private async void increaseLoadedCardsContainer(int index)
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
                        cards[counter].IsHide = false;
                        counter++;
                    }
                }
                break;
            case ShowType.ShowWhenReady:
                loadingResultsContainer.Add(index);
                cards[index].IsHide = false;
                break;
            case ShowType.OneByOneBeauty:
                loadingResultsContainer.Add(index);
                if (loadingResultsContainer.Count >= cards.Length)
                {
                    for (int i = 0; i < cards.Length; i++)
                    {
                        cards[i].IsHide = false;
                        while (reversedContainer.Count - 1 < i)
                        {
                            await Task.Yield();
                        }
                    }
                }
                break;
        }
    }
    public void RefreshContainers()
    {
        loadingResultsContainer = new List<int>();
        reversedContainer = new List<int>();
    }
}
