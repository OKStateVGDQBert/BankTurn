using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseProvider {

    private Vector2[] points;
    private int size;
	private Matrix solution;

    public NoiseProvider(int splits, int max)
    {
        points = new Vector2[((int)(Mathf.Pow(2, splits-1))) + 1];
        size = max;
        findPoints(splits);
        splinePoints();
    }

    public int GetValue(int x)
    {
        int i = 0;
        if (x < points[0].x)
            return -1;
        while (x > points[i + 1].x)
        {
            if (i == points.Length - 2)
                return -1;
            i++;
        }
        return (int)(solution[(i * 3) + 2, 0] + solution[(i * 3) + 1, 0] * x + solution[i * 3, 0] * x * x);
    }

    public float GetDerivative(int x)
    {
        int i = 0;
        if (x < points[0].x)
            return -1;
        while (x > points[i + 1].x)
        {
            if (i == points.Length - 2)
                return -1;
            i++;
        }
        return (float)(solution[(i * 3) + 1, 0] + 2 * solution[i * 3, 0] * x);
    }

    public float GetValue(int x, int z)
    {
		int i = 0;
		if (x < points [0].x)
			return 0.1f;
		while (x > points[i + 1].x)
        {
			if (i == points.Length - 2)
				return 0.1f;
			i++;
        }
		float targetZ = (float) (solution [(i * 3) + 2, 0] + solution [(i * 3) + 1, 0] * x + solution [i * 3, 0] * x * x);
		float targetZDeriv = (float)(solution [(i * 3) + 1, 0] + 2 * solution [i * 3, 0] * x);
		if (Mathf.Abs (z - targetZ) < 15 + Mathf.Abs (5 * targetZDeriv)) {
			return 0.05f;
		} else
			//return 0.05f + Mathf.Pow(Mathf.Abs (z - targetZ)*0.01f,2);
		return 0.1f;
    }

    private void findPoints( int splits)
    {
        float x = 0;
        float y = 0;
        x = Random.value * 10f;
        y = size/2 + Random.value * size/6;
        points[0] = new Vector2((int)x, (int)y);
        x = size - (Random.value * 10f);
		y = size/2 + Random.value * size/6;
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
        y = size/2 + Random.value * size/6;
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
		Matrix matrix = new Matrix (3 * (points.Length - 1), 3 * (points.Length - 1));
		Matrix right = Matrix.ZeroMatrix(3 * (points.Length - 1), 1);

        for (int i = 0; i < points.Length-1; i++)
        {
            matrix[i * 2, i * 3] = points[i].x * points[i].x;
            matrix[i * 2, i * 3 + 1] = points[i].x;
            matrix[i * 2, i * 3 + 2] = 1;
			right [i * 2, 0] = points [i].y;
            matrix[i * 2 + 1, i * 3] = points[i+1].x * points[i+1].x;
            matrix[i * 2 + 1, i * 3 + 1] = points[i+1].x;
			matrix[i * 2 + 1, i * 3 + 2] = 1;
			right [i * 2 + 1, 0] = points [i + 1].y;
        }

        for (int i = 0; i < points.Length-2; i++)
        {
            matrix[((points.Length - 1) * 2) + i, i * 3] = 2 * points[i + 1].x;
            matrix[((points.Length - 1) * 2) + i, i * 3 + 1] = 1;
            matrix[((points.Length - 1) * 2) + i, (i + 1) * 3] = -2 * points[i + 1].x;
            matrix[((points.Length - 1) * 2) + i, (i + 1) * 3 + 1] = -1;
        }

        matrix[3 * (points.Length - 1) - 1, 0] = 1;

		//solution = matrix.SolveWith (right);
		solution = matrix.Invert() * right;
    }
}
