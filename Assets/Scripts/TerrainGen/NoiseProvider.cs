using System.Collections;
using System.Collections.Generic;
using TestMySpline;
using UnityEngine;

public class NoiseProvider {

    private Vector2[] points;
    private int size;
    float[] ys;

    public NoiseProvider(int splits, int max)
    {
        // 2^n-1 + 1 points
        points = new Vector2[((int)(Mathf.Pow(2, splits-1))) + 1];
        size = max;
        findPoints(splits);
        #region CubicSpline
        // This is how the CubicSpline class works
        float[] x = new float[points.Length];
        float[] y = new float[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            x[i] = points[i].x;
            y[i] = points[i].y;
        }
        int n = (int)(points[points.Length-1].x - points[0].x);
        float[] xs = new float[n];

        for (int i = 0; i < n; i++)
        {
            xs[i] = (int)(points[0].x + i);
        }

        CubicSpline spline = new CubicSpline();
        ys = spline.FitAndEval(x, y, xs, 0.0f, 0.0f, true);
        #endregion
    }

    public int GetValue(int x)
    {
        if (x < points[0].x + 1 || x >= points[points.Length - 1].x - 1)
            return -1;
        return (int)(ys[(int)(x - points[0].x)]);
    }

    public float GetDerivative(int x)
    {
        if (x < points[0].x + 1 || x >= points[points.Length - 1].x - 1)
            return 0;
        return (ys[x + 1 - (int)points[0].x] - ys[x - 1 - (int)points[0].x]) / 2;
    }

    private void findPoints( int splits)
    {
        float x = 0;
        float y = 0;
        // Set start point to 10, size/2
        x = 10.0f;
        y = size/2;
        points[0] = new Vector2((int)x, (int)y);
        // Set end point to size-10, size/2
        x = size - 10.0f;
        y = size / 2;
        points[points.Length-1] = new Vector2((int)x, (int)y);
        Vector2[] temp = points;
        points = recursivelyFindPoints(splits, temp);
    }

    private Vector2[] recursivelyFindPoints(int splits, Vector2[] tempPoints)
    {
        // Base case
        if (splits == 1) return tempPoints;
        float x = 0;
        float y = 0;
        // The initial and end values of x
        float initx = tempPoints[0].x;
        float endx = tempPoints[tempPoints.Length - 1].x;
        // Choose an x within -5 to +5 of the midpoint
        // Choose a y anywhere in the middle two 5ths
        x = ((initx+endx)/2f) + 5f-(Random.value * 10f);
        y = size/2 + (size / 5 - (Random.value * size / 2.5f));
        tempPoints[tempPoints.Length / 2] = new Vector2((int)x, (int)y);
        // Left side recursion
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
        // Right side recursion
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
}
