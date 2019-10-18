using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Personally, I would look at Addressables instead.
public class AsyncLoader
{
    private Queue<LoadRequest> requests = new Queue<LoadRequest>();
    private MonoBehaviour coroutineHost;
    private Coroutine loadingCoroutine;

    public AsyncLoader(MonoBehaviour mbHost)
    {
        coroutineHost = mbHost;
    }

    internal void Load(List<GameObject> assetsToLoad, Action<GameObject> cb)
    {
        var req = new LoadRequest()
        {
            Callback = cb,
        };
        foreach (var assetRequest in assetsToLoad)
        {
            req.AssetRequests.Enqueue(assetRequest);
        }
        requests.Enqueue(req);

        if (loadingCoroutine == null)
        {
            loadingCoroutine = coroutineHost.StartCoroutine(Loading());
        }
    }

    internal IEnumerator Loading()
    {
        while (requests.Count > 0)
        {
            var request = requests.Dequeue();
            foreach (var assetRequest in request.AssetRequests)
            {
                // "loading" of one asset per frame happens here
                yield return null;
                if (Settings.ImitateLongerLoadingTime)
                {
                    yield return new WaitForSeconds(0.1f);    
                }
                request.Callback?.Invoke(assetRequest);
            }
        }
        loadingCoroutine = null;
    }

    private class LoadRequest
    {
        public Queue<GameObject> AssetRequests = new Queue<GameObject>();
        public Action<GameObject> Callback;
    }
}
