using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour {

    public int resolution;
    public int cuts;
    public int size;
    public int height;
    public int canyonwidth;
    public int smoothlevel;
    public Texture2D FlatTexture;
    public Texture2D SteepTexture;
    public GameObject[] enemies;
    public GameObject coin;
    private float enemyFrequency = 0.20f;
    private float coinFrequency = 0.015f;

    private float timesincetrash;
    private bool trashing;
    private TerrainChunk terrain;

	// Use this for initialization
	void Start () {
        if (Data_Manager.difficulty > 0)
        {
            enemyFrequency = enemyFrequency / Data_Manager.difficulty;
        }
        else enemyFrequency = 1.0f;
        remakeMesh();
    }
	
	// Update is called once per frame
	void Update ()
    {
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
        var noiseProvider = new NoiseProvider(cuts, resolution);
        terrain = new TerrainChunk(resolution, size, height, noiseProvider, canyonwidth, smoothlevel, this, FlatTexture, SteepTexture, enemies, enemyFrequency, coin, coinFrequency);
        terrain.CreateTerrain();
    }
}
