using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseFlowField))]

public class AudioFlowField : MonoBehaviour {

    NoiseFlowField noiseFlowField;
    public AudioPeer audioPeer;

    [Header("Speed")]
    public bool useSpeed;
    public Vector2 moveSpeedMinMax, rotateSpeedMinMax;

    [Header("Scale")]
    public bool useScale;
    public Vector2 scaleMinMax;

    [Header("Material")]
    public Material material;
    public Material[] audioMaterial;

    public bool useColor1;
    public string colorName1;
    public Gradient gradient1;
    private Color[] color1;
    [Range(0f,1f)]
    public float colorThreshold1;
    public float colorMultyplier1;

    public bool useColor2;
    public string colorName2;
    public Gradient gradient2;
    private Color[] color2;
    [Range(0f, 1f)]
    public float colorThreshold2;
    public float colorMultyplier2;

    public int band;

    // Use this for initialization
    void Start () {
        audioPeer = GameObject.FindObjectOfType<AudioPeer>();
        noiseFlowField = GetComponent<NoiseFlowField>();

        audioMaterial = new Material[8];
        color1 = new Color[8];
        color2 = new Color[8];

        for(int i = 0; i < 8; i++)
        {
            color1[i] = gradient1.Evaluate((1f / 8f) * i);
            color2[i] = gradient2.Evaluate((1f / 8f) * i);
            audioMaterial[i] = new Material(material);
        }

        int countBand = 0;
        for (int i = 0; i < noiseFlowField.amountParticles; i++)
        {
            band = countBand % 8;
            noiseFlowField.particleMeshRenderer[i].material = audioMaterial[band];
            noiseFlowField.particles[i].audioband = band;

            countBand++;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(useSpeed)
        {
            noiseFlowField.particleMoveSpeed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, audioPeer._AmplitudeBuffer);
            noiseFlowField.particleRotateSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audioPeer._AmplitudeBuffer);
        }

        for(int i = 0; i < noiseFlowField.amountParticles; i++)
        {
            if(useScale)
            {
                float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, audioPeer._audioBandBuffer[noiseFlowField.particles[i].audioband]);
                noiseFlowField.particles[i].transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        for(int i = 0; i < noiseFlowField.staticParticles.Count; i++)
        {
            float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, audioPeer._audioBandBuffer[noiseFlowField.staticParticles[i].audioband]);
            noiseFlowField.staticParticles[i].transform.localScale = new Vector3(scale, scale, scale);
        }

        for(int i = 0; i < 8; i++)
        {
            if(useColor1)
            {
                if(audioPeer._audioBandBuffer[i] > colorThreshold1)
                {
                    audioMaterial[i].SetColor(colorName1, color1[i] * audioPeer._audioBandBuffer[i] * colorMultyplier1);
                }
                else
                {
                    audioMaterial[i].SetColor(colorName1, color1[i] * 0f);
                }
            }

            if(useColor2)
            {
                if(audioPeer._audioBand[i] > colorThreshold2)
                {
                    audioMaterial[i].SetColor(colorName2, color2[i] * audioPeer._audioBand[i] * colorMultyplier2);
                }
                else
                {
                    audioMaterial[i].SetColor(colorName2, color2[i] * 0f);
                }
            }
        }
	}
}
