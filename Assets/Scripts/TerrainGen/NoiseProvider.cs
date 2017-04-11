using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseProvider {

    private Vector2[] points;
    private int size;

    public NoiseProvider(int splits, int max)
    {
        points = new Vector2[((int)(Mathf.Pow(2, splits-1))) + 1];
        size = max;
        findPoints(splits);
        splinePoints();
    }

    public float GetValue(int x, int z)
    {
        Vector2 nearest1 = points[0];
        Vector2 nearest2 = points[1];
        Vector2 nearest3 = points[2];
        int i = 3;
        bool found = false;
        while (!found)
        {
            if (i >= points.Length)
            {
                found = true;
                break;
            }
            if (Mathf.Abs(x - nearest1.x) > Mathf.Abs(x - points[i].x))
            {
                nearest1 = nearest2;
                nearest2 = nearest3;
                nearest3 = points[i];
                i++;
            }
            else found = true;
        }
        Matrix4x4 matA = Matrix4x4.identity;
        matA[0, 0] = 1;
        matA[1, 0] = 1;
        matA[2, 0] = 1;
        matA[0, 1] = nearest1.x;
        matA[1, 1] = nearest2.x;
        matA[2, 1] = nearest3.x;
        matA[0, 2] = nearest1.x * nearest1.x;
        matA[1, 2] = nearest2.x * nearest2.x;
        matA[2, 2] = nearest3.x * nearest3.x;
        Matrix4x4 matB = Matrix4x4.zero;
        matB[0, 0] = nearest1.y;
        matB[1, 0] = nearest2.y;
        matB[2, 0] = nearest3.y;
        Matrix4x4 solved = matA.inverse * matB;
        float targetZ = solved[0, 0] + solved[1, 0] * x + solved[2, 0] * x * x;
        if (Mathf.Abs(z - targetZ) > 5) return 0.1f;
        else return 0.05f;
    }

    private void findPoints( int splits)
    {
        float x = 0;
        float y = 0;
        x = Random.value * 10f;
        y = Random.value * size;
        points[0] = new Vector2((int)x, (int)y);
        x = size - (Random.value * 10f);
        y = Random.value * size;
        points[points.Length-1] = new Vector2((int)x, (int)y);
        Vector2[] temp = points;
        points = recursivelyFindPoints(splits, temp);
    }

    private Vector2[] recursivelyFindPoints(int splits, Vector2[] tempPoints)
    {
        if (splits == 1) return tempPoints;
        float x = 0;
        float y = 0;
        float initx = tempPoints[0].x;
        float endx = tempPoints[tempPoints.Length - 1].x;
        x = ((initx+endx)/2f) + 5f-(Random.value * 10f);
        y = Random.value * size;
        tempPoints[tempPoints.Length / 2] = new Vector2((int)x, (int)y);
        Vector2[] temp = new Vector2[(tempPoints.Length / 2) + 1];
        for (int i = 0; i < (tempPoints.Length / 2) + 1; i++)
        {
            temp[i] = tempPoints[i];
        }
        temp = recursivelyFindPoints(splits - 1, temp);
        for (int i = 0; i < (tempPoints.Length / 2) + 1; i++)
        {
            tempPoints[i] = temp[i];
        }
        for (int i = (tempPoints.Length / 2); i < tempPoints.Length; i++)
        {
            temp[i - (tempPoints.Length / 2)] = tempPoints[i];
        }
        temp = recursivelyFindPoints(splits - 1, temp);
        for (int i = 0; i < (tempPoints.Length / 2) + 1; i++)
        {
            tempPoints[i + (tempPoints.Length / 2)] = temp[i];
        }
        return tempPoints;
    }
    
    public void splinePoints()
    {
        MatrixNxN matrix = new MatrixNxN(3*(points.Length-1));

        for (int i = 0; i < points.Length-1; i++)
        {
            matrix.values[i * 2, i * 3] = points[i].x * points[i].x;
            matrix.values[i * 2, i * 3 + 1] = points[i].x;
            matrix.values[i * 2, i * 3 + 2] = 1;
            matrix.values[i * 2 + 1, i * 3] = points[i+1].x * points[i+1].x;
            matrix.values[i * 2 + 1, i * 3 + 1] = points[i+1].x;
            matrix.values[i * 2 + 1, i * 3 + 2] = 1;
        }

        for (int i = 0; i < points.Length-2; i++)
        {
            matrix.values[((points.Length - 1) * 2) + i, i * 3] = 2 * points[i + 1].x;
            matrix.values[((points.Length - 1) * 2) + i, i * 3 + 1] = 1;
            matrix.values[((points.Length - 1) * 2) + i, (i + 1) * 3] = 2 * points[i + 2].x;
            matrix.values[((points.Length - 1) * 2) + i, (i + 1) * 3 + 1] = 1;
        }

        matrix.values[3 * (points.Length - 1) - 1, 0] = 1;

        Debug.Log(matrix);
    }
}
