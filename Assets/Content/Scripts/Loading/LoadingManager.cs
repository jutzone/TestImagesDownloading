using UnityEngine;
using System.Threading;


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
                _instance.cancelTokenSource = new CancellationTokenSource();
                _instance.token = _instance.cancelTokenSource.Token;
            }
            return _instance;
        }
        private set
        {
            Instance = value;
        }
    }
    public CancellationTokenSource cancelTokenSource;
    CancellationToken token;
}
