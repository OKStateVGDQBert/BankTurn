using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunkSettings {

    public int HeightmapResolution { get; private set; }
    public int AlphamapResolution { get; private set; }

    public int Length { get; private set; }
    public int Height { get; private set; }

    public TerrainChunkSettings(int HMR, int AMR, int L, int H)
    {
        HeightmapResolution = HMR;
        AlphamapResolution = AMR;
        Length = L;
        Height = H;
    }

}
