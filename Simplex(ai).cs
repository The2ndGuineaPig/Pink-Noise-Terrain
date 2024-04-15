public class SimplexNoise
{
    private static readonly int[] Perm = new int[512]
    {
        // Permutation table
        // ...
    };

    private static readonly int[] Grad3 = new int[12]
    {
        // Gradient vectors for 3D
        // ...
    };

    private static readonly double F3 = 1.0 / 3.0;

    private static int FastFloor(double x)
    {
        return x > 0 ? (int)x : (int)x - 1;
    }

    private static double Dot(int g, double x, double y, double z)
    {
        return Grad3[g * 3] * x + Grad3[g * 3 + 1] * y + Grad3[g * 3 + 2] * z;
    }

    private static double Noise(double xin, double yin, double zin)
    {
        double n0, n1, n2, n3; // Noise contributions from the four corners

        // Skew the input space to determine which simplex cell we're in
        double s = (xin + yin + zin) * F3; // Very nice and simple skew factor for 3D
        int i = FastFloor(xin + s);
        int j = FastFloor(yin + s);
        int k = FastFloor(zin + s);

        double t = (i + j + k) * F3;
        double X0 = i - t; // Unskew the cell origin back to (x,y,z) space
        double Y0 = j - t;
        double Z0 = k - t;
        double x0 = xin - X0; // The x,y,z distances from the cell origin
        double y0 = yin - Y0;
        double z0 = zin - Z0;

        // Determine which simplex we are in
        int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
        int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

        if (x0 >= y0)
        {
            if (y0 >= z0)
            {
                i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; // X Y Z order
            }
            else if (x0 >= z0)
            {
                i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; // X Z Y order
            }
            else
            {
                i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; // Z X Y order
            }
        }
        else
        {
            if (y0 < z0)
            {
                i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; // Z Y X order
            }
            else if (x0 < z0)
            {
                i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; // Y Z X order
            }
            else
            {
                i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; // Y X Z order
            }
        }

        // Calculate the contribution from the three corners
        double x1 = x0 - i1 + F3; // Offsets for second corner in (x,y,z) coords
        double y1 = y0 - j1 + F3;
        double z1 = z0 - k1 + F3;
        double x2 = x0 - i2 + 2.0 * F3; // Offsets for third corner in (x,y,z) coords
        double y2 = y0 - j2 + 2.0 * F3;
        double z2 = z0 - k2 + 2.0 * F3;
        double x3 = x0 - 1.0 + 3.0 * F3; // Offsets for last corner in (x,y,z) coords
        double y3 = y0 - 1.0 + 3.0 * F3;
        double z3 = z0 - 1.0 + 3.0 * F3;

        // Calculate the contribution from the four corners
        int gi0 = Perm[i + Perm[j + Perm[k]]] % 12;
        int gi1 = Perm[i + i1 + Perm[j + j1 + Perm[k + k1]]] % 12;
        int gi2 = Perm[i + i2 + Perm[j + j2 + Perm[k + k2]]] % 12;
        int gi3 = Perm[i + 1 + Perm[j + 1 + Perm[k + 1]]] % 12;

        double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;
        if (t0 < 0)
        {
            n0 = 0.0;
        }
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * Dot(gi0, x0, y0, z0);
        }

        double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
        if (t1 < 0)
        {
            n1 = 0.0;
        }
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * Dot(gi1, x1, y1, z1);
        }

        double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
        if (t2 < 0)
        {
            n2 = 0.0;
        }
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * Dot(gi2, x2, y2, z2);
        }

        double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
        if (t3 < 0)
        {
            n3 = 0.0;
        }
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * Dot(gi3, x3, y3, z3);
        }

        // Add contributions from each corner to get the final noise value.
        // The result is scaled to return values in the range [-1,1].
        return 32.0 * (n0 + n1 + n2 + n3);
    }
}

