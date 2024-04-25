using Godot;
using System;
using System.Collections.Generic;

public partial class Simplex : Godot.Node
{
    public override void _Ready()
	{   
        
        GD.Print("Redty kaldt");
        InitializePermArray();
        double x = 1;
        double y = 5;
        double noiseValue = noise(x, y);
        GD.Print($"Simplex noise at: {noiseValue}");   
	}

    private static int[][] grad3= new int[][]
    {
        new int[] {1,1,0}, new int[] {-1,1,0}, new int[] {1,-1,0},
        new int[] {-1,-1,0}, new int[] {1,0,1}, new int[] {-1,0,1},
        new int[] {1,0,-1}, new int[] {-1,0,-1}, new int[] {0,1,1},
        new int[] {0,-1,1}, new int[] {0,1,-1}, new int[] {0,-1,-1}
    };

    private static int[] p = {151, 160, 137, 91, 90, 15,
                          131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23,
                          190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33,
                          88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
                          77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244,
                          102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
                          135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123,
                          5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42,
                          223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9,
                          129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228,
                          251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107,
                          49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
                          138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180};


    // To remove the need for index wrapping, double the permutation table length
    //private static int perm[] = new int[512];
    private static List<int> perm = new List<int>(512);


    // static { for(int i=0; i<512; i++) perm[i]=p[i & 255]; }
    static void InitializePermArray()
    {
        for (int i = 0; i < 512; i++)
        {
            GD.Print(i);
            perm.Add(p[i & 255]);
        }
    }
    private static int fastfloor(double x)
    {
        GD.Print("fastfloor nået");
        return x > 0 ? (int)x : (int)x - 1;
    }
    private static double dot(int[] g, double x, double y)
    {
        GD.Print("int g" + x + "&" + y);
        return g[0] * x + g[1] * y;
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
        GD.Print("Perm ii: " + perm[ii]);
        int jj = j & 255;
        GD.Print("Hash gradient jj: " + jj);
        GD.Print("Perm ii: " + perm[jj]);
        GD.Print(perm[ii + perm[jj]]);
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
        
        GD.Print("Linje 126, return: " + 70.0 * (n0 + n1 + n2));
        // Add contributions from each corner to get the final noise value.
        // The result is scaled to return values in the interval [-1,1].
        return 70.0 * (n0 + n1 + n2);
    }
}
