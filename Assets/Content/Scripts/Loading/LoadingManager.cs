using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Threading;
using System;

public enum LoadingType
{
    AllInOne,
    OneByOne,
    ShowWhenReady

}

public class TextureWithIndex
{
    public TextureWithIndex(Texture texture, int index)
    {
        this._texture = texture;
        this._index = index;
    }
    public Texture _texture;
    public int _index;
}
public class LoadingManager
{
    private static LoadingManager _instance;

    public static LoadingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LoadingManager();
            }
            return _instance;
        }
        set
        {
            Instance = value;
        }
    }

    public async Task<List<Texture>> AllInOneDownloading(string[] urls)
    {
        List<Texture> textures = new List<Texture>();

        var tasks = new Task[urls.Length];

        for (int i = 0; i < urls.Length; i++)
        {
            Debug.Log(urls[i]);
            tasks[i] = LoadOneImage(urls[i], textures);
        }
        await Task.WhenAll(tasks);
        return textures;
    }

    public async Task<Texture> ShowWhenOneByOneloading(string url)
    {
        List<Texture> textures = new List<Texture>();
        var task = new Task[1];
        task[0] = LoadOneImage(url, textures);
        await Task.WhenAll(task);
        return textures[0];
    }

    public async Task<TextureWithIndex> ShowWhenReadyloading(string[] urls)
    {
        List<Texture> textures = new List<Texture>();
        List<Task> tasks = new List<Task>();

        foreach(string url in urls)
        {
            Task task = LoadOneImage(url, textures);
            tasks.Add(task);
        }
        var completedTask = await Task.WhenAny(tasks);
        Debug.Log(tasks.IndexOf(completedTask) + " completed");
        return new TextureWithIndex(textures[0], tasks.IndexOf(completedTask));
    }

    public async Task LoadOneImage(string url, List<Texture> texList = null)
    {
        Debug.Log("start task");
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        var result = request.SendWebRequest();
        while (!result.isDone)
        {
            await Task.Yield();
        }
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            //return null;
        }
        else
        {
            Debug.Log("loading complete");
            texList.Add(((DownloadHandlerTexture)request.downloadHandler).texture);
            while (!texList.Contains(((DownloadHandlerTexture)request.downloadHandler).texture))
            {
                await Task.Yield();
            }
        }
    }

    public void LoadImages(string[] urls, LoadingType typeOfLoading = LoadingType.AllInOne)
    {
        switch (typeOfLoading)
        {
            case LoadingType.AllInOne:
                break;
        }
    }

}
