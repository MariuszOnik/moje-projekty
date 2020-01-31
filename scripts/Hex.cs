using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex 
{
    public int q;
    public int r;
    public int s;

    public Hex parentInPath;

    public Hex()
    {
        q = 0;
        r = 0;
        s = 0;
    }

    public Hex(int a, int b, int c)
    {
        q = a;
        r = b;
        s = c;
    }

    public override string ToString()
    {
        string infoHex = "q: " + q + " r: " + r + " s: " + s;
        return infoHex;
    }

    /*public void Add(Hex b)
    {
        q = q + b.q;
        r = r + b.r;
        s = s + b.s;
    }

    public void Subtract(Hex b)
    {
        q = q - b.q;
        r = r - b.r;
        s = s - b.s;
    }*/

    static public bool EqualHex(Hex a, Hex b)
    {
        if ((a.q == b.q && a.s == b.s && a.r == b.r))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Hex Add(Hex b)
    {
        return new Hex(q + b.q, r + b.r, s + b.s);
    }


    public Hex Subtract(Hex b)
    {
        return new Hex(q - b.q, r - b.r, s - b.s);
    }


    public static Hex operator +(Hex a, Hex b)
    {
        return new Hex(a.q + b.q, a.r + b.r, a.s + b.s);
    }

    public static Hex operator -(Hex a, Hex b)
    {
        return new Hex(a.q - b.q, a.r - b.r, a.s - b.s);
    }

    public int Length()
    {
        return (int)((Mathf.Abs(q) + Mathf.Abs(r) + Mathf.Abs(s)) / 2);
    }


    public int Distance(Hex b)
    {
        return Subtract(b).Length();
    }
    public Dictionary<string, Hex> GetHexInRange(int range, Hex start)
    {
        Dictionary<string, Hex> resut = new Dictionary<string, Hex>();

        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                for (int k = -range; k <= range; k++)
                {
                    if (i + j + k == 0)
                    {
                        Hex temp = new Hex(i, j, k);
                        Hex z = start.Add(temp);
                        if (!EqualHex(z, start))
                        {
                            resut.Add(z.ToString(), z);
                        }

                    }
                }
            }
        }
        //Debug.Log(resut.Count + " Ile hexow w zasiegu");
        return resut;
    }

    static public List<Hex> directionsList = new List<Hex> { new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1) };
    static public Hex[] directionsArray = new Hex[] { new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1) };
}


