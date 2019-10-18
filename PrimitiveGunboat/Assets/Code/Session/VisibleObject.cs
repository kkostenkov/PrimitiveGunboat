using UnityEngine;

public class VisibleObject : MonoBehaviour
{
    [SerializeField]
    private VisualsRequest visualsRequest;
    public VisualsRequest VisualsRequest { get { return visualsRequest; } }
}
