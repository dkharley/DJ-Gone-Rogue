/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{
	public Transform target;
	public int zigs;
	public float speed = 1f;
	public float scale = 1f;
	public Light startLight;
	public Light endLight;
	
	Perlin noise;
	float oneOverZigs;
	
	private Particle[] particles;
	
	void Start()
    {
        float distToTarget = 1.0f;
        if (this.transform.parent.gameObject.GetComponent<Lazer1>() != null)
        {
            distToTarget = this.transform.parent.gameObject.GetComponent<Lazer1>().gapDist;
        }
        else if (this.transform.parent.gameObject.GetComponent<DJ_LaserGun>() != null)
        {
            distToTarget = this.transform.parent.gameObject.GetComponent<DJ_LaserGun>().lengthOfBeam;
        }

        zigs = (int)(20.0f * distToTarget);
        //Debug.Log("Zigs = " + distToTarget);
        oneOverZigs = 1f / (float)zigs;
        GetComponent<ParticleEmitter>().emit = false;

        GetComponent<ParticleEmitter>().Emit(zigs);
        particles = GetComponent<ParticleEmitter>().particles;
	}
	
	void Update ()
	{
        if (noise == null)
        {
            noise = new Perlin();
        }
			
		float timex = Time.time * speed * 0.1365143f;
		float timey = Time.time * speed * 1.21688f;
		float timez = Time.time * speed * 2.5564f;

        float midVal = (float)particles.Length / 2.0f;

		for (int i=0; i < particles.Length; i++)
		{
            Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs * (float)i);
            //Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs);
            Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, timex + position.z),
                                        noise.Noise(timey + position.x, timey + position.y, timey + position.z),
                                        noise.Noise(timez + position.x, timez + position.y, timez + position.z));
            //Vector3 offset = Vector3.up;
            float distToMid = Mathf.Abs((float)i - midVal);
            float val = 2.0f * (midVal - distToMid) / midVal;
            position += (offset * scale * ((float)i * oneOverZigs)) * val;
			
			particles[i].position = position;
			particles[i].color = Color.white;
			particles[i].energy = 1f;
		}
		
		GetComponent<ParticleEmitter>().particles = particles;
		
        //if (particleEmitter.particleCount >= 2)
        //{
        //    if (startLight)
        //        startLight.transform.position = particles[0].position;
        //    if (endLight)
        //        endLight.transform.position = particles[particles.Length - 1].position;
        //}
	}	
}