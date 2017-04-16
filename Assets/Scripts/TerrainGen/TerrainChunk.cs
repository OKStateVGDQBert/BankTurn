using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk {

    private int canyonwidth { get; set; }

    private Terrain Terrain { get; set; }

    private int HeightMapResolution { get; set; }

    private int AlphaMapResolution { get; set; }

    private int Height { get; set; }

    private int Length { get; set; }

    private int smoothlevel { get; set; }

    private NoiseProvider NoiseProvider { get; set; }

    private TerrainGen generator { get; set; }

    private Texture2D flatText { get; set; }

    private Texture2D steepText { get; set; }


    public TerrainChunk(int hmr, int l, int h, NoiseProvider NP, int cwidth, int smooth, TerrainGen gen, Texture2D flat, Texture2D steep)
    {
        HeightMapResolution = hmr;
        AlphaMapResolution = hmr;
        Height = h;
        Length = l;
        NoiseProvider = NP;
        canyonwidth = cwidth;
        smoothlevel = smooth;
        generator = gen;
        flatText = flat;
        steepText = steep;
    }

    public void CreateTerrain()
    {
        var terrainData = new TerrainData();
        terrainData.heightmapResolution = HeightMapResolution;
        terrainData.alphamapResolution = AlphaMapResolution;

        var heightmap = GetHeightmap();
        if (heightmap.GetLength(0) == 0)
        {
            Debug.Log("Z is outside bounds, trashing.");
            generator.TrashIt();
            return;
        }
        terrainData.SetHeights(0, 0, heightmap);
        terrainData.size = new Vector3(Length, Height, Length);
        applyTextures(terrainData);

        var newTerrainGameObject = Terrain.CreateTerrainGameObject(terrainData);
        newTerrainGameObject.transform.position = new Vector3(0, 0, 0);
        Terrain = newTerrainGameObject.GetComponent<Terrain>();
        Terrain.Flush();
    }

    private float[,] GetHeightmap()
    {
        var draftmap = new float[HeightMapResolution, HeightMapResolution];
        var heightmap = new float[HeightMapResolution, HeightMapResolution];

        // Set the base heightmap to 10%
        for (int zRes = 0; zRes < HeightMapResolution; zRes++)
        {
            for (int xRes = 0; xRes < HeightMapResolution; xRes++)
            {
                draftmap[zRes, xRes] = 0.5f;
            }
        }

        // Store the last zero derivative x value outside the loop to preserve scope.
        float lastZeroDeriv = 0.0f;
        
        for (int xRes = 1; xRes < HeightMapResolution; xRes++)
        {
            int zRes = NoiseProvider.GetValue(xRes);
            
            // If we get a z value that is out of bounds, send an empty float to signal trash map.
            if (zRes < 0 || zRes > HeightMapResolution - 1)
            {
                if (zRes != -1)
                {
                    return new float[0,0];
                }
                continue;
            }

            // Get the z value for x-1 and store it for distance work.
			var lastzRes = NoiseProvider.GetValue (xRes - 1);

            // Find derivative
            var zResDeriv = NoiseProvider.GetDerivative(xRes);

            // If the derivative is near 0, set the last zero derivative to the current x.
            if (Mathf.Abs(zResDeriv) < 0.1f)
            {
                lastZeroDeriv = xRes;
            }

            // Now loop through every nearby point and calculate the height via distance formula
            for (int curX = xRes - canyonwidth; curX <= xRes + canyonwidth; curX++)
            {
                // If out of bounds, or to the right of the last zero derivative, skip.
                // Drawing on the right of the zero derivative causes strange artifacts.
                if (curX > HeightMapResolution - 2 || curX < 0 || curX < lastZeroDeriv)
                {
                    continue;
                }
				for (int curZ = zRes - canyonwidth; curZ <= zRes + canyonwidth; curZ++)
				{
                    if (curZ > HeightMapResolution - 2 || curZ < 0) continue;
                    // Distance formula for the current point and the last point, if we are closer this point, then draw, if not then leave alone.
					var distance1 = Mathf.Sqrt(Mathf.Pow(curX - (xRes - 1), 2) + Mathf.Pow(curZ - lastzRes, 2));
					var distance2 = Mathf.Sqrt(Mathf.Pow(curX - xRes, 2) + Mathf.Pow(curZ - zRes, 2));
                    if (distance1 > (distance2 + 0.35f))
                    {
                        // Formula based on the cube of the distance formula to give a concave shape. Also apply some noise.
                        draftmap[curZ, curX] = Mathf.Clamp(Mathf.Pow(distance2 / canyonwidth, 3) * 0.5f + (0.015f - (Random.value * 0.04f)) + 0.05f, 0.05f, 0.5f);
                    }
                }
            }
        }

        // Set any spot on the line to 5%
        for (var xRes = 0; xRes < HeightMapResolution; xRes++)
        {
            var zRes = NoiseProvider.GetValue(xRes);
            if (zRes < 0 || zRes > HeightMapResolution - 1) continue;
            // Find derivative
            var zResDeriv = NoiseProvider.GetDerivative(xRes);
            // Set the values of the river bed to 5%
            for (int tempzRes = -15 - (((int)Mathf.Abs(zResDeriv * 5))); tempzRes <= 15 + (((int)Mathf.Abs(zResDeriv * 5))); tempzRes++)
            {
                if (zRes + tempzRes > HeightMapResolution - 2 || zRes + tempzRes < 0) continue;
                draftmap[zRes + tempzRes, xRes] = 0.05f;
            }
        }

        heightmap = draftmap;
        // Smoothen the terrain to the desired amount
        for (int i = 0; i < smoothlevel; i++)
        {
            for (int z = 1; z < HeightMapResolution - 1; z++)
            {
                for (int x = 1; x < HeightMapResolution - 1; x++)
                {
                    heightmap[z, x] = (
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
            draftmap = heightmap;
        }
        return heightmap;
    }

    private void applyTextures(TerrainData terrainData)
    {
        var flatSplat = new SplatPrototype();
        var steepSplat = new SplatPrototype();

        flatSplat.texture = flatText;
        steepSplat.texture = steepText;

        terrainData.splatPrototypes = new SplatPrototype[]
        {
        flatSplat,
        steepSplat
        };

        terrainData.RefreshPrototypes();

        var splatMap = new float[terrainData.alphamapResolution, terrainData.alphamapResolution, 2];

        for (var zRes = 0; zRes < terrainData.alphamapHeight; zRes++)
        {
            for (var xRes = 0; xRes < terrainData.alphamapWidth; xRes++)
            {
                var normalizedX = (float)xRes / (terrainData.alphamapWidth - 1);
                var normalizedZ = (float)zRes / (terrainData.alphamapHeight - 1);

                var steepness = terrainData.GetSteepness(normalizedX, normalizedZ) - 60f;
                var steepnessNormalized = Mathf.Clamp(steepness, 0, 1f);

                splatMap[zRes, xRes, 0] = 1f - steepnessNormalized;
                splatMap[zRes, xRes, 1] = steepnessNormalized;
            }
        }

        terrainData.SetAlphamaps(0, 0, splatMap);
    }

}
