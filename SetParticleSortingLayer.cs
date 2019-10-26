using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParticleSortingLayer : MonoBehaviour {
    public string sortingLayerName;
    public int sortingOrder;

	// Use this for initialization
	void Start () {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Renderer particleRenderer = particleSystem.GetComponent<Renderer>();
        particleRenderer.sortingLayerName = sortingLayerName;
        particleRenderer.sortingOrder = sortingOrder;
	}
	
}
