using System.Diagnostics;

namespace Nnet
{
    /// <summary>
    /// LAYeR
    /// </summary>
    internal sealed class Layr
    {
        private readonly Scalar sAlpha = 0.1;

        private readonly Tnsf tnsf;
        private readonly Pcpn[] arpcpn;

        private Vector vOutput;
        private Vector vInput;
        private Vector vPreoutput;
        private Vector vSensitivity;
        private Vector vAdjWeight;

        private int Count => arpcpn.Length;

        public Layr(Pcpn[] arpcpn, Tnsf tnsf)
        {
            this.tnsf = tnsf;
            this.arpcpn = arpcpn;

            vOutput = new Vector(Count);
            vPreoutput = new Vector(Count);
            vSensitivity = new Vector(Count);
            vAdjWeight = new Vector(arpcpn[0].cInput);
        }

        public Vector Evaluate(Vector vInput)
        {
            this.vInput = vInput;

            for (int i = 0; i < Count; i++)
            {
                vPreoutput[i] = arpcpn[i].Evaluate(vInput);
                vOutput[i] = tnsf.Transfer(vPreoutput[i]);;
            }

            return vOutput;
        }

        public Vector Backpropagation0(Vector vExpected)
        {
            Debug.Assert(Count == vExpected.Count);

            for (int i = 0; i < Count; i++)
                vSensitivity[i] = -2.0 * tnsf.Derivative(vOutput[i]) * (vExpected[i] - vOutput[i]);

            return UpdateWeightsAnddBiases();
        }

        public Vector Backpropagation(Vector vAdjRightWeight)
        {
            Debug.Assert(Count == vAdjRightWeight.Count);

            for (int i = 0; i < Count; i++)
                vSensitivity[i] = 0.0;

            for (int i = 0; i < Count; i++)
                vSensitivity[i] = tnsf.Derivative(vOutput[i]) * vAdjRightWeight[i];

            return UpdateWeightsAnddBiases();
        }

        private Vector UpdateWeightsAnddBiases()
        {
            for (int iWeight = 0; iWeight < vAdjWeight.Count; iWeight++)
                vAdjWeight[iWeight] = 0.0;

            for (int iPcpn = 0; iPcpn < Count; iPcpn++)
            {
                Vector vWeight = arpcpn[iPcpn].UpdateWeightsAndBias(sAlpha, vInput, vSensitivity[iPcpn]);
                Debug.Assert(vWeight.Count == vAdjWeight.Count);

                for (int iWeight = 0; iWeight < vWeight.Count; iWeight++)
                    vAdjWeight[iWeight] += vWeight[iWeight] * vSensitivity[iPcpn];
            }

            return vAdjWeight;
        }

        public Matrix AugWeight
        {
            get
            {
                Vector[] arv = new Vector[Count];

                for (int n = 0; n < Count; n++)
                    arv[n] = arpcpn[n].Weight;

                return new Matrix(arv);
            }
        }
    }
}
