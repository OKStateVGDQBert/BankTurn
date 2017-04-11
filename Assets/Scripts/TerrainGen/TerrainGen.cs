using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour {

    public int resolution;
    public int cuts;
    public int size;
    public int height;

	// Use this for initialization
	void Start () {
        var settings = new TerrainChunkSettings(resolution, resolution, size, height);
        var noiseProvider = new NoiseProvider(cuts, resolution);
        var terrain = new TerrainChunk(settings, noiseProvider, 0, 0);
        terrain.CreateTerrain();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
