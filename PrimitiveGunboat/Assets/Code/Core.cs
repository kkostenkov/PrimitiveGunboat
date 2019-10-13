using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] 
    private SessionController sessionController;

    [SerializeField]
    private AssetDispenser assetDispenser;
    

    void Start()
    {
        sessionController.Initialize(assetDispenser);
        sessionController.RunSession();
    }

}
