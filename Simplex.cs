using Godot;
using System;
using System.Collections.Generic;


public partial class Simplex : Godot.Node
{

    private static int[][] grad3= new int[][]
    {
        new int[] {1,1,0}, new int[] {-1,1,0}, new int[] {1,-1,0},
        new int[] {-1,-1,0}, new int[] {1,0,1}, new int[] {-1,0,1},
        new int[] {1,0,-1}, new int[] {-1,0,-1}, new int[] {0,1,1},
        new int[] {0,-1,1}, new int[] {0,1,-1}, new int[] {0,-1,-1}
    };


    // To remove the need for index wrapping, double the permutation table length
    //private static int perm[] = new int[512];
    static List<int> perm = new List<int>(512);


    //static { for(int i=0; i<512; i++) perm[i]=p[i & 255]; }

    private static int fastfloor(double x)
    {
        return x > 0 ? (int)x : (int)x - 1;
        GD.Print("fastfloor nået");
    }
    private static double dot(int[] g, double x, double y)
    {
        return g[0] * x + g[1] * y;
        GD.Print("int g" + x + "&" + y);
    }

    // 2D simplex noise
    public static double noise(double xin, double yin)
    {
        double n0, n1, n2; // Noise contributions from the three corners
                           // Skew the input space to determine which simplex cell we're in
        double F2 = 0.5 * (Math.Sqrt(3.0) - 1.0);
        //GD.Print("F2: " + F2);
        double s = (xin + yin) * F2; // Hairy factor for 2D
        //GD.Print("s: " + s);
        int i = fastfloor(xin + s);
        GD.Print("i: " + i);
        int j = fastfloor(yin + s);
        GD.Print("j: " + j);
        double G2 = (3.0 - Math.Sqrt(3.0)) / 6.0;
        GD.Print("G2: " + G2);
        double t = (i + j) * G2;
        GD.Print("t: " + t);
        double X0 = i - t; // Unskew the cell origin back to (x,y) space
        GD.Print("X0: " + X0);
        double Y0 = j - t;
        GD.Print("Y0: " + Y0);
        double x0 = xin - X0; // The x,y distances from the cell origin
        GD.Print("x0: " + x0);
        double y0 = yin - Y0;
        GD.Print("y0: " + y0);


        // For the 2D case, the simplex shape is an equilateral triangle.
        // Determine which simplex we are in.
        int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
        if (x0 > y0)
        {
            i1 = 1; j1 = 0;
            GD.Print("linje 48-52");
            } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
        else
        {
            i1 = 0; j1 = 1;
            GD.Print("linje 53-56");
            } // upper triangle, YX order: (0,0)->(0,1)->(1,1)
              // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
              // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
              // c = (3-sqrt(3))/6


        double x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
        GD.Print("x1: " + x1);
        double y1 = y0 - j1 + G2;
        GD.Print("y1: " + y1);
        double x2 = x0 - 1.0 + 2.0 * G2; // Offsets for last corner in (x,y) unskewed coords
        GD.Print("x2: " + x2);
        double y2 = y0 - 1.0 + 2.0 * G2;
        GD.Print("y2: " + y2);


        // Work out the hashed gradient indices of the three simplex corners
        int ii = i & 255;
        GD.Print("Hash gradient ii: " + ii);
        int jj = j & 255;
        GD.Print("Hash gradient jj: " + jj);
        int gi0 = perm[ii + perm[jj]] % 12;
        GD.Print("Hash gradient gi0: " + gi0);
        int gi1 = perm[ii + i1 + perm[jj + j1]] % 12;
        GD.Print("Hash gradient gi1: " + gi1);
        int gi2 = perm[ii + 1 + perm[jj + 1]] % 12;
        GD.Print("Hash gradient gi2: " + gi2);


        // Calculate the contribution from the three corners
        double t0 = 0.5 - x0 * x0 - y0 * y0;
        GD.Print("three corners t0: " + t0);
        if (t0 < 0)
        {
            n0 = 0.0;
            GD.Print("t0<0: n0: " + n0);
        }
        else
        {
            t0 *= t0;
            GD.Print("Linje 94, t0: " + t0);
            n0 = t0 * t0 * dot(grad3[gi0], x0, y0); // (x,y) of grad3 used for 2D gradient
            GD.Print("Linje 96, n0: " + n0);
        }
        double t1 = 0.5 - x1 * x1 - y1 * y1;
        GD.Print("three corners t1: " + t1);
        if (t1 < 0)
        {
            n1 = 0.0;
            GD.Print("t1<0: n1: " + n1);
        }
        else
        {
            t1 *= t1;
            GD.Print("Linje 105, t1: " + t1);
            n1 = t1 * t1 * dot(grad3[gi1], x1, y1);
            GD.Print("Linje 107, n1: " + n1);
        }
        double t2 = 0.5 - x2 * x2 - y2 * y2;
        GD.Print("three corners t2: " + t2);
        if (t2 < 0)
        {
            n2 = 0.0;
            GD.Print("t2<0: n2: " + n2);
        }
        else
        {
            t2 *= t2;
            GD.Print("Linje 117, t2: " + t2);
            n2 = t2 * t2 * dot(grad3[gi2], x2, y2);
            GD.Print("Linje 119, n2: " + n2);
        }


        // Add contributions from each corner to get the final noise value.
        // The result is scaled to return values in the interval [-1,1].
        return 70.0 * (n0 + n1 + n2);
        GD.Print("Linje 126, return: " + 70.0 * (n0 + n1 + n2));
    }
}