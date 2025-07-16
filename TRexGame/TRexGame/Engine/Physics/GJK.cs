// https://www.youtube.com/watch?v=ajv46BSqcK4

using Microsoft.Xna.Framework;
using TRexGame.Engine.Graphics;

namespace TRexGame.Engine.Physics
{
    public class GJK
    {
        public static void IsColliding(Vector2 position_a, Vector2 size_a, Vector2 postion_b, Vector2 size_b)
        {
            // Get Convex Hull - Calculate Minkowski Difference
            // A ⊖ B = { a + (-b) |a ∈ A, b ∈ B }
            //
            // Properties
            // A and B convex -> A ⊖ B convex
            // if A and B intersect -> (0, 0) ∈ A ⊖ B
            // Statement
            // Triangle (simplex) from points A ⊖ B contains origin => A and B intersect
            // k-Simplex - shape that is guaranteed to enclose a point in k-dimensional space
            // 2-Simplex - triangle
            // 3-Simplex - tetrahedron
        }

        /// <summary>
        /// A support function sB takes a direction ->d and returns point v on the boundary of shape B
        /// "furthest" in direction ->d.
        /// </summary>
        private static void GetSupportPoint()
        {
            // For every point on a convex shape there is a direction, where is the furthest point (supportpoint).

            // support point for shape A - sA(->d) => (x, y) supportpoint
            // support point for shape B - sB(->d) => (x, y) supportpoint
            // support point for C = A ⊖ B
            // sC(->d) = sA(->d) + sB(->d) => (x, y) supportpoint

            // for minkowski difference - opposite direction
            // sC(->d) = sA(->d) - sB(->d * -1) => (x, y) supportpoint

            // A support function sB takes a direction ->d and returns point v on the boundary of the shape B "furthest" in direction ->d.
            // sB(->d) = v = arg max vT->d
            //                v ∈ B       
            // Produces point v that maximizes dot product with ->d

            // for circle
            // sB(->d) = C + r->d

            // by defining support functions we can handle any convex shape
        }
    }
}
