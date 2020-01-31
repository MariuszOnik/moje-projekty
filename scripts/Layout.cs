using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Layout 
{

    public Orientation orientation { get; set; }
    public Vector2 size;
    public Vector2 origin;

    public Layout(Orientation orientation, Vector2 size, Vector2 origin)
    {
        this.orientation = orientation;
        this.size = size;
        this.origin = origin;
    }

    public Vector3 HexToVector3(Hex h)
    {
        Orientation M = orientation;
        float x = (M.f0 * h.q + M.f1 * h.r) * size.x;
        float y = (M.f2 * h.q + M.f3 * h.r) * size.y;
        return new Vector3(x + origin.x,0, y + origin.y);
    }

    public Hex Vector3ToHex(Vector3 p)
    {
        Orientation M = orientation;
        Vector3 pt = new Vector3((p.x - origin.x) / size.x, 0, (p.z - origin.y) / size.y);
        float q = M.b0 * pt.x + M.b1 * pt.z;
        float r = M.b2 * pt.x + M.b3 * pt.z;
        FractionalHex fHex =  new FractionalHex(q, r, -q - r);
        //Debug.Log("FHex : " + fHex);
        return fHex.HexRound();
    }

    static public Orientation pointy = new Orientation(Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f, Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);
    static public Orientation flat = new Orientation(3.0f / 2.0f, 0.0f, Mathf.Sqrt(3.0f) / 2.0f, Mathf.Sqrt(3.0f), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Mathf.Sqrt(3.0f) / 3.0f, 0.0f);

}
