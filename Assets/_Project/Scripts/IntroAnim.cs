using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnim : MonoBehaviour {

    public Material retroMat;

    public float distortionStrength;
    public float materialBrightness;
    public float staticStrength;
    private int materialScrollStaticProperty;
    private int materialDistortionStrengthProperty;
    private int materialBrightnessProperty;
    private int materialStaticStrengthProperty;

    // Start is called before the first frame update
    private void Start() {
        // retroMat = blitAsset.settings.blitMaterial;
        materialDistortionStrengthProperty = Shader.PropertyToID("_DistortionStrength");
        materialBrightnessProperty = Shader.PropertyToID("Vector1_5660967D");
        materialStaticStrengthProperty = Shader.PropertyToID("_StaticStrength");
    }

    // Update is called once per frame
    void Update() {
        retroMat.SetFloat(materialDistortionStrengthProperty, distortionStrength);
        retroMat.SetFloat(materialBrightnessProperty, materialBrightness);
        retroMat.SetFloat(materialStaticStrengthProperty, staticStrength);
    }

    public void ScrollingEnable() {
        retroMat.EnableKeyword("SCROLL_STATIC_ON");
    }

    public void ScrollingDisable() {
        retroMat.DisableKeyword("SCROLL_STATIC_ON");
    }

    public void DestroySelf() {
        Destroy(this);
    }
}
