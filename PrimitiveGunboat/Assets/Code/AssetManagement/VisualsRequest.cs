using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualsRequest : MonoBehaviour
{
    // this is imitation of asset loading for demonstration purposes only
    // in actual app this list could contain links to anchor transforms,
    // asset or prefab names etc.
   [SerializeField]
   private List<GameObject> assetsToLoad; 
   private bool isLoadingRequested;

   public void RequestLoad(IAssetDispenser assetDispenser)
    {
        if (isLoadingRequested)
        {
            return;
        }
        PrepareForLoadingImitation();       
        assetDispenser.LoadAsync(assetsToLoad, OnAssetOrComponentLoaded);
        isLoadingRequested = true;
    }

    private void PrepareForLoadingImitation()
    {
        foreach (var asset in assetsToLoad)
        {
            asset.SetActive(false);
        }
    }

    private void OnAssetOrComponentLoaded(GameObject loadedObject)
    {
        var loadedImitation = assetsToLoad.FirstOrDefault(obj => obj == loadedObject);
        // Technically, loadedObject is the same object in heap as loadedImitation and
        // we could activate loadedObject itself with the same result.
        if (loadedImitation)
        {
            loadedImitation.SetActive(true);
        }
        
    }
}
