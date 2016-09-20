using System;
using System.Diagnostics;

namespace Nnet
{
    /// <summary>
    /// (m) A mathematical matrix.
    /// Immutable.
    /// </summary>
    public struct Matrix
    {
        private readonly Vector[] arv;

        public Matrix(params Vector[] arv)
        {
            Debug.Assert(arv != null);
            this.arv = arv;
        }

        public Matrix(int c) : this(new Vector[c]) { }

        public int Count => arv.Length;

        public Vector this[int i]
        {
            get { return arv[i]; }
            set
            {
                Debug.Assert(value.Count == arv[i].Count);
                arv[i] = value;
            }
        }
    }

    /// <summary>
    /// (v) A mathematical vector.
    /// Copy safe, though not totally immutable.
    /// </summary>
    public struct Vector
    {
        public static readonly Vector Empty = new Vector(new Scalar[] {});

        private readonly Scalar[] ars;

        public Vector(params Scalar[] ars)
        {
            Debug.Assert(ars != null);
            this.ars = ars;
        }

        public Vector(Vector v)
        {
            ars = new Scalar[v.ars.Length];
            for (int n = 0; n < Count; n++)
                ars[n] = v.ars[n];
        }

        public Vector(int c) : this(new Scalar[c]) { }

        public int Count => ars.Length;

        public Scalar this[int i]
        {
            set { ars[i] = value; }
            get { return ars[i]; }
        }

        public bool IsEmpty => ars.Length == 0;

        public Scalar InnerProduct(Vector v2) => InnerProduct(this, v2);

        public static Matrix OuterProduct(Vector v1, Vector v2)
        {
            Matrix m = new Matrix(v1.Count);

            for (int im = 0; im < m.Count; im++)
            {
                m[im] = new Vector(v2.Count);
                for (int iv = 0; iv < v2.Count; iv++)
                {
                    Vector v = m[im];
                    v[iv] = v1[im] * v2[iv];
                }
            }

            return m;
        }

        public static Scalar InnerProduct(Vector v1, Vector v2)
        {
            Debug.Assert(v1.Count == v2.Count);

            Scalar v = 0;
            for (int i = 0; i < v1.Count; i++)
                v += v1.ars[i] * v2.ars[i];

            return v;
        }

        public static Scalar operator*(Vector v1, Vector v2) => InnerProduct(v1, v2);

        public static Vector operator-(Vector v1, Vector v2)
        {
            Debug.Assert(v1.Count == v2.Count);

            Vector v = new Vector(v1.Count);
            for (int i = 0; i < v1.Count; i++)
                v[i] = v1[i] - v2[i];

            return v;
        }
    }

    /// <summary>
    /// (s) Wrapper around a C# floating point data type so that the code is not dependent on float or double.
    /// Immutable.
    /// </summary>
    public struct Scalar
    {
        private readonly double val;

        private Scalar(double val)
        {
            this.val = val;
        }

        public static implicit operator Scalar(double val) => new Scalar(val);

        public static explicit operator double(Scalar s) => s.val;

        public static Scalar operator*(Scalar s1, Scalar s2) => s1.val * s2.val;
        public static Scalar operator/(Scalar s1, Scalar s2) => s1.val / s2.val;
        public static Scalar operator+(Scalar s1, Scalar s2) => s1.val + s2.val;
        public static Scalar operator-(Scalar s1, Scalar s2) => s1.val - s2.val;

        public static Scalar operator-(Scalar s) => -s.val;

        public static bool operator==(Scalar s1, Scalar s2) => s1.val == s2.val;
        public static bool operator<(Scalar s1, Scalar s2) => s1.val < s2.val;
        public static bool operator>(Scalar s1, Scalar s2) => s1.val > s2.val;

        public static bool operator!=(Scalar s1, Scalar s2) => !(s1 == s2);
        public static bool operator>=(Scalar s1, Scalar s2) => !(s1 < s2);
        public static bool operator<=(Scalar s1, Scalar s2) => !(s1 > s2);

        public override int GetHashCode() => val.GetHashCode();
        public override bool Equals(object obj) => this == (obj as Scalar?);

        public override string ToString() => val.ToString();
    }

    /// <summary>
    /// Wrappers that take Scalar_s around Math methods.
    /// </summary>
    public static class ScalarU
    {
        public static Scalar Exp(Scalar s) => Math.Exp((double) s);

        public static Scalar LogSigmoid(Scalar s) => 1.0 / (1.0 + Exp(-s));
        public static Scalar HyperbolicTangentSigmoid(Scalar s)
        {
            Scalar exp = Exp(s);
            Scalar inv = Exp(-s);
            return (exp - inv) / (exp + inv);
        }
    }
}
