
using UnityEngine;

class MathUtilsExt
{
    public static bool V3Equal(Vector3 a, Vector3 b)
    {
        float dis = Vector3.Distance(a, b);
        bool ret = dis < 0.05;
        return ret;
    }
}