using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeLightningWarmtoCold : MonoBehaviour
{
    public List<Light> lights;
    public List<Light> ceilingLights;
    
    [Header("Temperature Settings")]
    public float warmTemperatureCeiling = 2800f;
    public float warmTemperature = 3500f;
    public float coldTemperature = 6500f;
    public float transitionDuration = 2f;

    [Header("Lightmap Settings - EXR Files")]
    [Tooltip("Drag your warm lightmap .exr files here")]
    public List<Texture2D> warmLightmapEXRs;
    [Tooltip("Drag your cold lightmap .exr files here")]
    public List<Texture2D> coldLightmapEXRs;
    
    [Header("Optional: Direction Maps")]
    [Tooltip("Optional: Direction lightmaps (usually _dir suffix)")]
    public List<Texture2D> warmDirectionEXRs;
    public List<Texture2D> coldDirectionEXRs;

    [Header("Debug")]
    public bool debugLightmapChanges = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TurnLightsWarm()
    {
        StartCoroutine(LerpToTemperatureWithSeparateValues(warmTemperature, warmTemperatureCeiling));
    }

    public void TurnLightsCold()
    {
        StartCoroutine(LerpToTemperatureWithSeparateValues(coldTemperature, coldTemperature));
    }
    
    /// <summary>
    /// Changes the lightmaps using EXR files from inspector
    /// </summary>
    /// <param name="isWarm">True for warm lightmaps, false for cold lightmaps</param>
    public void ChangeLightmaps(bool isWarm)
    {
        List<Texture2D> targetColorEXRs = isWarm ? warmLightmapEXRs : coldLightmapEXRs;
        List<Texture2D> targetDirectionEXRs = isWarm ? warmDirectionEXRs : coldDirectionEXRs;
        
        if (targetColorEXRs == null || targetColorEXRs.Count == 0)
        {
            Debug.LogWarning($"No {(isWarm ? "warm" : "cold")} lightmap EXR files assigned!");
            return;
        }
        
        // Create new lightmap data array
        LightmapData[] newLightmaps = new LightmapData[targetColorEXRs.Count];
        
        for (int i = 0; i < targetColorEXRs.Count; i++)
        {
            newLightmaps[i] = new LightmapData();
            
            // Assign color lightmap (main EXR)
            newLightmaps[i].lightmapColor = targetColorEXRs[i];
            
            // Assign direction lightmap if available
            if (targetDirectionEXRs != null && i < targetDirectionEXRs.Count && targetDirectionEXRs[i] != null)
            {
                newLightmaps[i].lightmapDir = targetDirectionEXRs[i];
            }
        }
        
        // Apply the lightmaps
        LightmapSettings.lightmaps = newLightmaps;
        
        if (debugLightmapChanges)
        {
            Debug.Log($"Applied {newLightmaps.Length} {(isWarm ? "warm" : "cold")} lightmaps");
            for (int i = 0; i < newLightmaps.Length; i++)
            {
                Debug.Log($"Lightmap {i}: {targetColorEXRs[i]?.name}");
            }
        }
    }
    
    /// <summary>
    /// Utility function to get current lightmap info
    /// </summary>
    [ContextMenu("Debug Current Lightmaps")]
    public void DebugCurrentLightmaps()
    {
        LightmapData[] current = LightmapSettings.lightmaps;
        Debug.Log($"Current lightmap count: {current.Length}");
        
        for (int i = 0; i < current.Length; i++)
        {
            string colorName = current[i].lightmapColor ? current[i].lightmapColor.name : "null";
            string dirName = current[i].lightmapDir ? current[i].lightmapDir.name : "null";
            Debug.Log($"Lightmap {i}: Color={colorName}, Direction={dirName}");
        }
    }
    
    IEnumerator LerpToTemperatureWithSeparateValues(float targetTempLights, float targetTempCeiling)
    {
        // Determine if we're going to warm or cold based on target temperature
        bool isGoingToWarm = targetTempLights <= warmTemperature;
        
        // Change lightmaps at the beginning of the transition
        //ChangeLightmaps(isGoingToWarm);
        
        // Get starting temperatures
        float[] startTemps = new float[lights.Count + ceilingLights.Count];
        int index = 0;
        
        foreach (Light light in lights)
        {
            startTemps[index] = light.colorTemperature;
            index++;
        }
        foreach (Light light in ceilingLights)
        {
            startTemps[index] = light.colorTemperature;
            index++;
        }
        
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            index = 0;
            
            foreach (Light light in lights)
            {
                light.colorTemperature = Mathf.Lerp(startTemps[index], targetTempLights, t);
                index++;
            }
            foreach (Light light in ceilingLights)
            {
                light.colorTemperature = Mathf.Lerp(startTemps[index], targetTempCeiling, t);
                index++;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure final temperature is set exactly
        foreach (Light light in lights)
        {
            light.colorTemperature = targetTempLights;
        }
        foreach (Light light in ceilingLights)
        {
            light.colorTemperature = targetTempCeiling;
        }
    }
}