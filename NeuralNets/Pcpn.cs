using System.Diagnostics;

namespace Nnet
{
    /// <summary>
    /// PerCePtroN
    /// </summary>
    internal sealed class Pcpn
    {
        private Vector vWeight;
        private Scalar sBias;

        public int cInput => vWeight.Count;

        public Pcpn(Vector vWeight, Scalar sBias)
        {
            this.vWeight = vWeight;
            this.sBias = sBias;
        }

        public Scalar Evaluate(Vector vInput)
        {
            Debug.Assert(vInput.Count == vWeight.Count);
            return vInput * vWeight + sBias;
        }

        public Vector UpdateWeightsAndBias(Scalar sAlpha, Vector vInput, Scalar sSensitivity)
        {
            Vector vOldWeight = new Vector(vWeight);

            Scalar s = sAlpha * sSensitivity;
            for (int i = 0; i < cInput; i++)
                vWeight[i] -= s * vInput[i];

            sBias -= s;

            return vOldWeight;
        }

        public Vector Weight
        {
            get
            {
                int cAugWeight = cInput + 1;
                Scalar[] ars = new Scalar[cAugWeight];

                for (int n = 0; n < cAugWeight - 1; n++)
                    ars[n] = vWeight[n];

                ars[cAugWeight - 1] = sBias;

                return new Vector(ars);
            }
        }
    }
}
