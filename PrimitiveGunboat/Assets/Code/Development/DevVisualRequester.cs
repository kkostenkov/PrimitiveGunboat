﻿#if UNITY_EDITOR
using UnityEngine;

public class DevVisualRequester : MonoBehaviour
{
    void OnGUI()
    {
        if (GUILayout.Button("Press to load"))
        {
            var assetDispenser = FindObjectOfType<AssetDispenser>();
            
            var allThatNeedLoading = FindObjectsOfType<VisualsRequest>();
            foreach (var inNeed in allThatNeedLoading)
            {
                inNeed.RequestLoad(assetDispenser);
            }
        }
    }
}
#else
This script will not sneak to build
#endif