using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractionalHex 
{
    float q { get; set; }
    float r { get; set; }
    float s { get; set; }

    public FractionalHex(float q, float r, float s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
        
    }

    public Hex HexRound()
    {
        int qi = (int)(Mathf.Round(q));
        int ri = (int)(Mathf.Round(r));
        int si = (int)(Mathf.Round(s));
        double q_diff = Mathf.Abs(qi - q);
        double r_diff = Mathf.Abs(ri - r);
        double s_diff = Mathf.Abs(si - s);
        if (q_diff > r_diff && q_diff > s_diff)
        {
            qi = -ri - si;
        }
        else
            if (r_diff > s_diff)
        {
            ri = -qi - si;
        }
        else
        {
            si = -qi - ri;
        }
        return new Hex(qi, ri, si);
    }
}
