using UnityEngine;
using System;
using System.Reflection;

public static class PhysicsUtil
{
    // Unity 6 では Rigidbody2D.velocity が非推奨になっていることがあるため、
    // 可能なら linearVelocity プロパティを使い、無ければ velocity を使う。
    public static void SetLinearVelocity(Rigidbody2D rb, Vector2 velocity)
    {
        if (rb == null) return;

        // try linearVelocity property first (Unity6 style)
        var type = typeof(Rigidbody2D);
        var pi = type.GetProperty("linearVelocity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (pi != null && pi.CanWrite)
        {
            try { pi.SetValue(rb, velocity, null); return; } catch { /* ignore */ }
        }

        // fallback to velocity
        try { rb.linearVelocity = velocity; return; }
        catch { /* ignore */ }

        // last resort: use AddForce to approximate
        try { rb.AddForce(velocity - rb.position); }
        catch { /* ignore */ }
    }
}
