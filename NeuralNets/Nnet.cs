namespace Nnet
{
    /// <summary>
    /// Neural NETwork
    /// </summary>
    public sealed class Nnet
    {
        private readonly Layr[] arlayr;

        private int Count => arlayr.Length;

        internal Nnet(Layr[] arlayr) { this.arlayr = arlayr; }

        public Vector Evaluate(Vector vInput)
        {
            foreach (Layr layr in arlayr)
                vInput = layr.Evaluate(vInput);

            return vInput;
        }

        public void Train(Vector vExpected)
        {
            Layr layr = arlayr[arlayr.Length - 1];
            Vector vAdjWeights = layr.Backpropagation0(vExpected);

            for (int n = arlayr.Length - 2; n >= 0; n--)
                vAdjWeights = arlayr[n].Backpropagation(vAdjWeights);
        }

        public Matrix[] AugWeight
        {
            get
            {
                Matrix[] arm = new Matrix[arlayr.Length];

                for (int n = 0; n < Count; n++)
                    arm[n] = arlayr[n].AugWeight;

                return arm;
            }
        }
    }
}
