using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.Events;
using DG.Tweening;
public class Card : MonoBehaviour, ICard
{
    [SerializeField] private Image innerImage, outerImage;
    private bool _isHide = true;
    public bool IsHide
    {
        get
        {
            return _isHide;
        }
        set
        {
            if (_isHide != value)
            {
                _isHide = value;
                if (value == true)
                    HideCard();
                else
                    ShowCard();
            }
        }
    }
    public UnityEvent<int> CardLoaded;
    public UnityEvent<int> CardReversed;
    public void DisposeContent()
    {
        innerImage.sprite = null;
    }

    public void ShowCard()
    {
        Vector3 startPos = transform.position;
        innerImage.DOColor(Color.white, 4f);
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f);
        transform.DOMove(new Vector3(startPos.x + 25, startPos.y + 25, startPos.z), 1f);
        transform.DORotate(new Vector3(0, 90, 0), 1f).OnComplete(() =>
        {
            outerImage.gameObject.SetActive(false);
            CardReversed.Invoke(transform.GetSiblingIndex());
            transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
            transform.DOMove(startPos, 1f);
            transform.DORotate(new Vector3(0, 180, 0), 1f);//.OnComplete(() => CardReversed.Invoke(transform.GetSiblingIndex()));
        });

    }
    public void HideCard()
    {
        Vector3 startPos = transform.position;
        innerImage.DOColor(Color.black, 0.5f);
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
        transform.DOMove(new Vector3(startPos.x + 25, startPos.y + 25, startPos.z), 0.2f);
        transform.DORotate(new Vector3(0, 90, 0), 0.2f).OnComplete(() =>
        {
            outerImage.gameObject.SetActive(true);
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
            transform.DOMove(startPos, 0.2f);
            transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        });
    }

    public void LoadContent()
    {
        var url = $"https://picsum.photos/id/{UnityEngine.Random.Range(0, 100)}/1080/1920";
        Task task = LoadImage(url, LoadingManager.Instance.cancelTokenSource.Token);
    }


    public async Task LoadImage(string url, CancellationToken token)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        var result = request.SendWebRequest();
        while (!result.isDone)
        {
            await Task.Yield();
        }
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            LoadContent();
        }
        else
        {
            var tex = ((DownloadHandlerTexture)request.downloadHandler).texture;

            if (!token.IsCancellationRequested)
            {
                innerImage.sprite = Sprite.Create((Texture2D)tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                CardLoaded.Invoke(transform.GetSiblingIndex());
            }
            else
                Debug.Log("Loading Canceled");
        }
    }

    private void OnDisable()
    {
        CardLoaded.RemoveAllListeners();
        CardReversed.RemoveAllListeners();
    }
}
