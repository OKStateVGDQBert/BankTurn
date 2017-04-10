using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk {

	public int X { get; private set; }

    public int Z { get; private set; }

    private Terrain Terrain { get; set; }

    private TerrainChunkSettings Settings { get; set; }

    private NoiseProvider NoiseProvider { get; set; }

    public TerrainChunk(TerrainChunkSettings S, NoiseProvider NP, int XX, int ZZ)
    {
        Settings = S;
        NoiseProvider = NP;
        X = XX;
        Z = ZZ;
    }

    public void CreateTerrain()
    {
        var terrainData = new TerrainData();
        terrainData.heightmapResolution = Settings.HeightmapResolution;
        terrainData.alphamapResolution = Settings.AlphamapResolution;

        var heightmap = GetHeightmap();
        terrainData.SetHeights(0, 0, heightmap);
        terrainData.size = new Vector3(Settings.Length, Settings.Height, Settings.Length);

        var newTerrainGameObject = Terrain.CreateTerrainGameObject(terrainData);
        newTerrainGameObject.transform.position = new Vector3(X * Settings.Length, 0, Z * Settings.Length);
        Terrain = newTerrainGameObject.GetComponent<Terrain>();
        Terrain.Flush();
    }

    public float[,] GetHeightmap()
    {
        var heightmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];

        for (var zRes = 0; zRes < Settings.HeightmapResolution; zRes++)
        {
            for (var xRes = 0; xRes < Settings.HeightmapResolution; xRes++)
            {
                heightmap[zRes, xRes] = NoiseProvider.GetValue(xRes, zRes);
                //heightmap[zRes, xRes] = 0;
            }
        }
        return heightmap;
    }

}
