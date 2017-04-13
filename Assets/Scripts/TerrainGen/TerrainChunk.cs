using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk {

	public int X { get; private set; }

    public int Z { get; private set; }

    private Terrain Terrain { get; set; }

    private TerrainChunkSettings Settings { get; set; }

    private NoiseProvider NoiseProvider { get; set; }

    private TerrainGen generator { get; set; }

    public TerrainChunk(TerrainChunkSettings S, NoiseProvider NP, int XX, int ZZ, TerrainGen gen)
    {
        Settings = S;
        NoiseProvider = NP;
        X = XX;
        Z = ZZ;
        this.generator = gen;
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
        var draftmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];
        var midmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];
        var heightmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];

        // Set the base heightmap to 10%
        for (var zRes = 0; zRes < Settings.HeightmapResolution; zRes++)
        {
            for (var xRes = 0; xRes < Settings.HeightmapResolution; xRes++)
            {
                //heightmap[zRes, xRes] = NoiseProvider.GetValue(xRes, zRes);
                draftmap[zRes, xRes] = 0.5f;
            }
        }

        var lastZeroDeriv = 0;

        // Set the nearby X's to allow the canyon feel
        for (var xRes = 1; xRes < Settings.HeightmapResolution; xRes++)
        {
            var zRes = NoiseProvider.GetValue(xRes);

            // if Z is outside bounds, trash it. Hopefully this is temp....
            if (zRes < 0 || zRes > Settings.HeightmapResolution - 1)
            {
                if (zRes != -1)
                {
                    Debug.Log("Z is " + zRes + " which is outside bounds, trashing.");
                    generator.TrashIt();
                    GameObject.Destroy(Terrain);
                    return draftmap;
                }
                continue;
            }

			var lastzRes = NoiseProvider.GetValue (xRes - 1);
            // Find derivative
            var zResDeriv = NoiseProvider.GetDerivative(xRes);
            if (Mathf.Abs(zResDeriv) < 0.1f)
            {
                lastZeroDeriv = xRes;
                //Debug.Log("I has deriv less than 0.01f at " + xRes + "!");
            }
            // Now loop through every nearby and calculate the height via distance formula
            for (int curX = xRes - 40; curX <= xRes + 40; curX++)
            {
                if (curX > Settings.HeightmapResolution - 2 || curX < 0 || curX < lastZeroDeriv)
                {
                    //Debug.Log("CurX : " + curX + " and lastZeroDeriv : " + lastZeroDeriv);
                    continue;
                }
				for (int curZ = zRes - 40; curZ <= zRes + 40; curZ++)
				{
                    if (curZ > Settings.HeightmapResolution - 2 || curZ < 0) continue;
					var distance1 = Mathf.Sqrt(Mathf.Pow(curX - (xRes - 1), 2) + Mathf.Pow(curZ - lastzRes, 2));
					var distance2 = Mathf.Sqrt(Mathf.Pow(curX - xRes, 2) + Mathf.Pow(curZ - zRes, 2));
					if (distance1 > distance2)
                    {
                        draftmap[curZ, curX] = Mathf.Clamp(Mathf.Pow(distance2 / 60.0f, 2) * 0.5f + (0.015f - (Random.value * 0.04f)), 0.05f, 0.5f);
                    }
                }
            }
        }

        // Set any spot on the line to 5%
        for (var xRes = 0; xRes < Settings.HeightmapResolution; xRes++)
        {
            var zRes = NoiseProvider.GetValue(xRes);
            if (zRes < 0 || zRes > Settings.HeightmapResolution - 1) continue;
            // Find derivative
            var zResDeriv = NoiseProvider.GetDerivative(xRes);
            // Set the values of the function to the river bed.
            if (Mathf.Abs(zResDeriv) < 0.1f)
            {
                for (int tempzRes = -17; tempzRes <= 17; tempzRes++)
                {
                    if (zRes + tempzRes > Settings.HeightmapResolution - 2 || zRes + tempzRes < 0) continue;
                    draftmap[zRes + tempzRes, xRes] = 0.05f;
                }
            } else
            {
                for (int tempzRes = -15 - (((int)Mathf.Abs(zResDeriv * 5))); tempzRes <= 15 + (((int)Mathf.Abs(zResDeriv * 5))); tempzRes++)
                {
                    if (zRes + tempzRes > Settings.HeightmapResolution - 2 || zRes + tempzRes < 0) continue;
                    draftmap[zRes + tempzRes, xRes] = 0.05f;
                }
            }
        }

        // Apply small smoothing algorithm to make the theme seem more.... natural, smooth 1
        for (int z = 1; z < Settings.HeightmapResolution - 1; z++)
        {
            for (int x = 1; x < Settings.HeightmapResolution - 1; x++)
            {
                midmap[z, x] = (
                    draftmap[z - 1, x - 1] +
                    draftmap[z, x - 1] +
                    draftmap[z + 1, x - 1] +
                    draftmap[z - 1, x] +
                    draftmap[z, x] +
                    draftmap[z + 1, x] +
                    draftmap[z - 1, x + 1] +
                    draftmap[z, x + 1] +
                    draftmap[z + 1, x + 1]) / 9;
            }
        }

        // Apply small smoothing algorithm to make the theme seem more.... natural, smooth 2
        for (int z = 1; z < Settings.HeightmapResolution - 1; z++)
        {
            for (int x = 1; x < Settings.HeightmapResolution - 1; x++)
            {
                heightmap[z, x] = (
                    midmap[z - 1, x - 1] +
                    midmap[z, x - 1] +
                    midmap[z + 1, x - 1] +
                    midmap[z - 1, x] +
                    midmap[z, x] +
                    midmap[z + 1, x] +
                    midmap[z - 1, x + 1] +
                    midmap[z, x + 1] +
                    midmap[z + 1, x + 1]) / 9;
            }
        }
        return heightmap;
    }

}
