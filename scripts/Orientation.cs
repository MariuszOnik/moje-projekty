using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientation 
{
    public float f0 { get; set; }
    public float f1 { get; set; }
    public float f2 { get; set; }
    public float f3 { get; set; }
    public float b0 { get; set; }
    public float b1 { get; set; }
    public float b2 { get; set; }
    public float b3 { get; set; }
    public float start_angle { get; set; }

    public Orientation(float f0, float f1, float f2, float f3, float b0, float b1, float b2, float b3, float start_angle)
    {
        this.f0 = f0;
        this.f1 = f1;
        this.f2 = f2;
        this.f3 = f3;
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
        this.b3 = b3;
        this.start_angle = start_angle;
    }

}
