using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnim : MonoBehaviour {

    public Material retroMat;
    public AnimationClip testAnimClip;
    public Animation testAnim;

    public float scrollStatic;
    public float distortionStrength;
    public float materialBrightness;
    private int materialScrollStaticProperty;
    private int materialDistortionStrengthProperty;
    private int materialBrightnessProperty;

    // Start is called before the first frame update
    private void Start() {
        retroMat = GetComponent<MeshRenderer>().sharedMaterial;
        materialScrollStaticProperty = Shader.PropertyToID("SCROLL_STATIC");
        materialDistortionStrengthProperty = Shader.PropertyToID("_Distortion Strength");
        materialBrightnessProperty = Shader.PropertyToID("Vector 1_5660967D");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
