using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
  
public class CameraEffectsManager : MonoBehaviour {
    public EdgeDetection TheEdgeDetection;
    public ColorCorrectionCurves TheColorCorrectionCurves;
    public Bloom TheBloom;
    public NoiseAndScratches TheNoiseAndScratches;
    public ScreenSpaceAmbientOcclusion TheScreenSpaceAmbientOcclusion;
    public ContrastEnhance TheContrastEnhance;
    public VignetteAndChromaticAberration TheVignetteAndChromaticAberration;
    public Antialiasing TheAntialiasing;
    public static CameraEffectsManager PublicAccess;
    // Use this for initialization
	void Start () {
        PublicAccess = this;
	}
	
	// Update is called once per frame
	void Update () {
     
	}
    public void SetOptions(int level)
    {
        switch (level)
        {
            case 1:
                {
                    TheEdgeDetection.enabled = false;
                    TheColorCorrectionCurves.enabled = false;
                    TheBloom.enabled = false;
                    TheNoiseAndScratches.enabled = false;
                    TheScreenSpaceAmbientOcclusion.enabled = false;
                    TheContrastEnhance.enabled = false;
                    TheVignetteAndChromaticAberration.enabled = false;
                    TheAntialiasing.enabled = true;
                    break;
                }
            case 2:
                {
                    TheEdgeDetection.enabled = false;
                    TheColorCorrectionCurves.enabled = true;
                    TheBloom.enabled = false;
                    TheNoiseAndScratches.enabled = false;
                    TheScreenSpaceAmbientOcclusion.enabled = false;
                    TheContrastEnhance.enabled = false;
                    TheVignetteAndChromaticAberration.enabled = true;
                    TheAntialiasing.enabled = true;
                    break;
                }
            case 3:
                {
                    TheEdgeDetection.enabled = false;
                    TheColorCorrectionCurves.enabled = true;
                    TheBloom.enabled = true;
                    TheNoiseAndScratches.enabled = false;
                    TheScreenSpaceAmbientOcclusion.enabled = true;
                    TheContrastEnhance.enabled = true;
                    TheVignetteAndChromaticAberration.enabled = true;
                    TheAntialiasing.enabled = true;
                    break;
                }
            default:
                {
                    TheEdgeDetection.enabled = false;
                    TheColorCorrectionCurves.enabled = false;
                    TheBloom.enabled = false;
                    TheNoiseAndScratches.enabled = false;
                    TheScreenSpaceAmbientOcclusion.enabled = false;
                    TheContrastEnhance.enabled = false;
                    TheVignetteAndChromaticAberration.enabled = false;
                    TheAntialiasing.enabled = false;
                       break;
                }
        }

    }
}
