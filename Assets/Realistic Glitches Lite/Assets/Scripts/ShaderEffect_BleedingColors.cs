using System.Collections;
using UnityEngine;

public class ShaderEffect_BleedingColors : MonoBehaviour {

	public float intensity = 3;
	public float shift = 0.5f;
	private Material material;

	// Creates a private material used to the effect
	void Awake ()
	{
		material = new Material( Shader.Find("Hidden/BleedingColors") );
		Invoke("Start_Check", 0.5f);
	}

    void Start_Check()
    {
        if (!(MapData.mapinstance.Boss || MapData.mapinstance.Boss_Reverse)) this.enabled=false;
    }

    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Intensity", intensity);
		material.SetFloat("_ValueX", shift);
		Graphics.Blit (source, destination, material);
	}
}
