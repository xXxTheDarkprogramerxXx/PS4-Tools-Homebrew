using UnityEngine; 
using System.Collections; 
public class ParticleSea : MonoBehaviour {
	public float spacing = 0.25f;
	public ParticleSystem particleSystem;
	private ParticleSystem.Particle[] particlesArray; 
	public int seaResolution = 25; 
	public float noiseScale = 0.2f; 
	public float heightScale = 3f;

	private float perlinNoiseAnimX = 0.01f; 
	private float perlinNoiseAnimY = 0.01f;
	public Gradient colorGradient;

	void GetParticles()
	{
		for(int i = 0; i < seaResolution; i++) { 
			for(int j = 0; j < seaResolution; j++) { 
				float zPos = Mathf.PerlinNoise(i * noiseScale + perlinNoiseAnimX, j * noiseScale + perlinNoiseAnimY); 
				particlesArray[i * seaResolution + j].color = colorGradient.Evaluate(zPos); 
				particlesArray[i * seaResolution + j].position = new Vector3(i * spacing, zPos * heightScale, j * spacing);
			} 
		}
		particleSystem.SetParticles(particlesArray, particlesArray.Length);
	}

	void Update() { 
		for(int i = 0; i < seaResolution; i++) { 
			for(int j = 0; j < seaResolution; j++) { 
				float zPos = Mathf.PerlinNoise(i * noiseScale + perlinNoiseAnimX, j * noiseScale + perlinNoiseAnimY); 
				particlesArray[i * seaResolution + j].color = colorGradient.Evaluate(zPos); 
				particlesArray[i * seaResolution + j].position = new Vector3(i * spacing, zPos * heightScale, j * spacing);
			} 
		} 
		perlinNoiseAnimX += 0.01f; perlinNoiseAnimY += 0.01f;     
		particleSystem.SetParticles(particlesArray, particlesArray.Length); 
	}

	void Start() { 
		particlesArray = new ParticleSystem.Particle[seaResolution * seaResolution]; 
		particleSystem.maxParticles = seaResolution * seaResolution;    
		particleSystem.Emit(seaResolution * seaResolution); 
		particleSystem.GetParticles(particlesArray); 
	}
}
