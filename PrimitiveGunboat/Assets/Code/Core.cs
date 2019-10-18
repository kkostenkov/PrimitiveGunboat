using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] 
    private SessionController sessionController;

    [SerializeField] 
    private UiCore uiCore;

    [SerializeField]
    private AssetDispenser assetDispenser;
    

    void Start()
    {
        sessionController.Initialize(assetDispenser);
        uiCore.Initialize(sessionController);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


}
