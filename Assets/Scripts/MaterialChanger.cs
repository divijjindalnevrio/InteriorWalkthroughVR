using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MaterialChanger : MonoBehaviour
{    
    [System.Serializable]
    public class RendererMaterialPair
    {
        public Renderer renderer;
        public int materialIndex;
    }
    
    public List<RendererMaterialPair> rendererMaterialPairs = new List<RendererMaterialPair>();
    public Material material;
    
    // Call this function with a specific material

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ChangeMaterial);
    }

    public void ChangeMaterial()
    {
        Debug.Log("ChangeMaterial");
        if (rendererMaterialPairs != null && material != null)
        {
            foreach (RendererMaterialPair pair in rendererMaterialPairs)
            {
                if (pair.renderer != null)
                {
                    Debug.Log($"Changing material for renderer: {pair.renderer.name} at index: {pair.materialIndex}");
                    
                    if(pair.renderer.materials.Length == 1)
                    {
                        Debug.Log("ChangeMaterial3");
                        pair.renderer.material = material;
                    }
                    else
                    {
                        Debug.Log("ChangeMaterial4");
                        Material[] materials = pair.renderer.materials;
                        if (pair.materialIndex < materials.Length)
                        {
                            materials[pair.materialIndex] = material;
                            pair.renderer.materials = materials;
                        }
                        else
                        {
                            Debug.LogWarning($"Material index {pair.materialIndex} is out of range for renderer {pair.renderer.name}");
                        }
                    }
                }
            }
        }
    }
}
