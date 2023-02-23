using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CoreController : MonoBehaviour
{
    [SerializeField] private Button loadButton, cancelButton;
    [SerializeField] private Image[] images;
    private string[] urls;

    private void Start()
    {
        urls = new string[images.Length];
        Debug.Log(images.Length);
        for (int i = 0; i < urls.Length; i++)
        {
            urls[i] = $"https://picsum.photos/id/{UnityEngine.Random.Range(0, 100)}/200/300";
        }
        ShowWhenReady();
        //LoadingManager.Instance.Console();
        //ShowAllInOne();
    }

    async void ShowAllInOne()
    {
        List<Texture> tex = new List<Texture>();
        tex = await LoadingManager.Instance.AllInOneDownloading(urls);
        while (tex == null)
        {
            await Task.Yield();
        }
        Debug.Log(tex.Count);
        for (int j = 0; j < images.Length; j++)
        {
            images[j].sprite = Sprite.Create((Texture2D)tex[j], new Rect(0.0f, 0.0f, tex[j].width, tex[j].height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
    async void ShowWOneByOne()
    {
        Texture[] texs = new Texture[urls.Length];
        for (int j = 0; j < urls.Length; j++)
        {
            texs[j] = await LoadingManager.Instance.ShowWhenOneByOneloading(urls[j]);
            while (texs[j] == null)
            {
                await Task.Yield();
            }
            images[j].sprite = Sprite.Create((Texture2D)texs[j], new Rect(0.0f, 0.0f, texs[j].width, texs[j].height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
    async void ShowWhenReady()
    {
        var indexes = new List<int>();
        while (indexes.Count < urls.Length)
        {
            TextureWithIndex twi = await LoadingManager.Instance.ShowWhenReadyloading(urls);
            while (twi == null)
            {
                await Task.Yield();
            }
            images[twi._index].sprite = Sprite.Create((Texture2D)twi._texture, new Rect(0.0f, 0.0f, twi._texture.width, twi._texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            if (!indexes.Contains(twi._index))
            {
                indexes.Add(twi._index);
            }
        }
    }


}
