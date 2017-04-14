using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour {

    public int resolution;
    public int cuts;
    public int size;
    public int height;
    public int canyonwidth;
    private float timesincetrash = 0.0f;
    private bool trashing = false;
    private TerrainChunk terrain;

	// Use this for initialization
	void Start () {
        var settings = new TerrainChunkSettings(resolution, resolution, size, height);
        var noiseProvider = new NoiseProvider(cuts, resolution);
        terrain = new TerrainChunk(settings, noiseProvider, canyonwidth, this);
        terrain.CreateTerrain();
    }
	
	// Update is called once per frame
	void Update () {

        if (trashing) timesincetrash = timesincetrash + Time.deltaTime;
        if (timesincetrash > 10.0f) remakeMesh();
	}

    public void TrashIt()
    {
        trashing = true;
    }

    void remakeMesh()
    {
        trashing = false;
        timesincetrash = 0.0f;
        var settings = new TerrainChunkSettings(resolution, resolution, size, height);
        var noiseProvider = new NoiseProvider(cuts, resolution);
        terrain = new TerrainChunk(settings, noiseProvider, canyonwidth, this);
        terrain.CreateTerrain();
    }
}
