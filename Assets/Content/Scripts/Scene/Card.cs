using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.Events;

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
    public void DisposeContent()
    {
        innerImage.sprite = null;
    }

    public void ShowCard()
    {
        Debug.Log("show");
        outerImage.gameObject.SetActive(false);
    }
    public void HideCard()
    {
        Debug.Log("hide");
        outerImage.gameObject.SetActive(true);
    }

    public void LoadContent()
    {
        Debug.Log("card. load content");
        var url = $"https://picsum.photos/id/{UnityEngine.Random.Range(0, 100)}/1080/1920";
        Task task = LoadImage(url, LoadingManager.Instance.cancelTokenSource.Token);
    }


    public async Task LoadImage(string url, CancellationToken token)
    {
        Debug.Log("start task");
        Debug.Log(url);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        Debug.Log("request0");
        var result = request.SendWebRequest();
        Debug.Log("request");
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
            Debug.Log("loading complete");
            var tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            innerImage.sprite = Sprite.Create((Texture2D)tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            CardLoaded.Invoke(transform.GetSiblingIndex());
        }
    }
}
