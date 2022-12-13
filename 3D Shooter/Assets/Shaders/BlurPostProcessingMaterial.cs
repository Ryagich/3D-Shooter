using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "CustomPostProcessingMaterials",
    menuName = "CustomPostProcessingMaterials")]
public class BlurPostProcessingMaterial : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;

    //---Accessing the data from the Pass---
    static BlurPostProcessingMaterial _instance;

    public static BlurPostProcessingMaterial Instance
    {
        get
        {
            if (_instance) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            _instance = Resources.LoadAll<BlurPostProcessingMaterial>("")[0];
            return _instance;
        }
    }
}