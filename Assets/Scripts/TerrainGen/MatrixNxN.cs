using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixNxN {

    public float[,] values { get; set; }
    private int size;

    public MatrixNxN(int insize)
    {
        this.size = insize;
        this.values = new float[insize, insize];
    }

    public MatrixNxN(float[,] invalues)
    {
        this.size = invalues.GetLength(0);
        this.values = invalues;
    }

    public MatrixNxN GausElim()
    {
        for (int c = 0; c < values.GetLength(1)-1; c++)
        {
            for (int r = c; r < values.GetLength(0) - 1; r++)
            {
                float modifier = values[r + 1, c] / values[c, c];
                if (values[r + 1, c] == 0) continue;
                values[r + 1, c] = 0;
                for (int nc = c+1; nc < values.GetLength(1); nc++)
                {
                    values[r + 1, nc] = values[r + 1, nc] - values[c, nc] * modifier;
                }
            }
        }
        return new MatrixNxN(values);
    }

    public override string ToString()
    {
        string tostring = "";
        for (int r = 0; r < values.GetLength(0); r++)
        {
            for (int c = 0; c < values.GetLength(0); c++)
            {
                tostring += values[r , c] + " ";
            }
            tostring += "\n";
        }
        return tostring;
    }


}
