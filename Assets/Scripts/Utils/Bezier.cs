using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Bezier {
    public static Vector2 BezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2) {
        Vector2 a = Vector2.Lerp(p0, p1, t);
        Vector2 b = Vector2.Lerp(p1, p2, t);
        return Vector2.Lerp(a, b, t);
    }
    public static Quaternion BezierRotation(float t, Vector2 p0, Vector2 p1, Vector2 p2) {
        Vector2 a = Vector2.Lerp(p0, p1, t);
        Vector2 b = Vector2.Lerp(p1, p2, t);
        Vector2 p = Vector2.Lerp(a, b, t);
        Vector2 dir = b - a;
        return Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}
