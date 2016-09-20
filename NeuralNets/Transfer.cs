namespace Nnet
{
    public abstract class Tnsf
    {
        public abstract Scalar Transfer(Scalar s);
        public abstract Scalar Derivative(Scalar s);

        public static readonly Tnsf tnsfLinear = new TnsfLinear();
        public static readonly Tnsf tnsfSatuatingLinear = new TnsfSatuatingLinear();
        public static readonly Tnsf tnsfSymmetricSaturatingLinear = new TnsfSymmetricSaturatingLinear();
        public static readonly Tnsf tnsfPositiveLinear = new TnsfPositiveLinear();
        public static readonly Tnsf tnsfHardLimit = new TnsfHardLimit();
        public static readonly Tnsf tnsfSymmetricHardLimit = new TnsfSymmetricHardLimit();
        public static readonly Tnsf tnsfLogSigmoid = new TnsfLogSigmoid();
        public static readonly Tnsf tnsfHyperbolicTangentSigmoid = new TnsfHyperbolicTangentSigmoid();

        private class TnsfLinear : Tnsf
        {
            public TnsfLinear() { }
            public override Scalar Transfer(Scalar s) => s;
            public override Scalar Derivative(Scalar s) => 1.0;
        }

        private class TnsfSatuatingLinear : Tnsf
        {
            public TnsfSatuatingLinear() { }
            public override Scalar Transfer(Scalar s) => s >= 1.0 ? 1.0 : (s <= 0.0 ? 0.0 : s);
            public override Scalar Derivative(Scalar s) => s >= 1.0 ? 0.0 : (s <= 0.0 ? 0.0 : 1.0);
        }

        private class TnsfSymmetricSaturatingLinear : Tnsf
        {
            public TnsfSymmetricSaturatingLinear() { }
            public override Scalar Transfer(Scalar s) => s >= 1.0 ? 1.0 : (s <= -1.0 ? -1.0 : s);
            public override Scalar Derivative(Scalar s) => s >= 1.0 ? 0.0 : (s <= 0.0 ? 0.0 : 1.0);
        }

        private class TnsfPositiveLinear : Tnsf
        {
            public TnsfPositiveLinear() { }
            public override Scalar Transfer(Scalar s) => s < 0.0 ? 0.0 : s;
            public override Scalar Derivative(Scalar s) => s < 0.0 ? 0.0 : 1.0;
        }

        private class TnsfHardLimit : Tnsf
        {
            public TnsfHardLimit() { }
            public override Scalar Transfer(Scalar s) => s >= 0.0 ? 1.0 : 0.0;
            public override Scalar Derivative(Scalar s) => 0.0;
        }

        private class TnsfSymmetricHardLimit : Tnsf
        {
            public TnsfSymmetricHardLimit() { }
            public override Scalar Transfer(Scalar s) => s >= 0.0 ? 1.0 : -1.0;
            public override Scalar Derivative(Scalar s) => 0.0;
        }

        private class TnsfLogSigmoid : Tnsf
        {
            public TnsfLogSigmoid() { }
            public override Scalar Transfer(Scalar s) => ScalarU.LogSigmoid(s);
            public override Scalar Derivative(Scalar s) => (1.0 - s) * s;
        }

        private class TnsfHyperbolicTangentSigmoid : Tnsf
        {
            public TnsfHyperbolicTangentSigmoid() { }
            public override Scalar Transfer(Scalar s) => ScalarU.HyperbolicTangentSigmoid(s);
            public override Scalar Derivative(Scalar s) { throw new System.NotImplementedException(); }
        }
    }
}
