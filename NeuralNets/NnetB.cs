using System;
using System.Collections.Generic;

namespace Nnet
{
    /// <summary>
    /// Nnet Builder
    /// </summary>
    public sealed class NnetB
    {
        List<Layrdef> lslayrdef = new List<Layrdef>();

        public NnetB() { }

        public void AddLayer(Vector[] arvWeight, Scalar[] arsBias, Tnsf tnsf)
        {
            int cPcpndef = arvWeight.Length;
            int cInputs = arvWeight[0].Count;

            if (cPcpndef != arsBias.Length)
                throw new ArgumentException($"{nameof(arvWeight)} ({cPcpndef}) and {nameof(arsBias)} ({arsBias.Length}) are differnt lengths");

            if (lslayrdef.Count > 0 && cInputs != lslayrdef[lslayrdef.Count - 1].CPcpn)
                throw new ArgumentException($"Number of inputs ({cInputs}) does not match previous layer's number of outputs ({lslayrdef[lslayrdef.Count - 1].CPcpn})");

            Pcpndef[] arpcpndef = new Pcpndef[cPcpndef];
            for (int i = 0; i < cPcpndef; i++)
            {
                if (cInputs != arvWeight[i].Count)
                    throw new ArgumentException($"Weights vectors differ in rank at [0] and [{i}]");

                arpcpndef[i] = new Pcpndef(arvWeight[i], arsBias[i]);
            }

            lslayrdef.Add(new Layrdef(arpcpndef, tnsf));
        }

        public Nnet Nnet()
        {
            int cLayr = lslayrdef.Count;
            Layr[] arlayr = new Layr[cLayr];

            for (int i = 0; i < cLayr; i++)
                arlayr[i] = lslayrdef[i].Layr();

            return new Nnet(arlayr);
        }

        private struct Layrdef
        {
            private readonly Pcpndef[] arpcpndef;
            private readonly Tnsf tnsf;

            public Layrdef(Pcpndef[] arpcpndef, Tnsf tnsf)
            {
                this.arpcpndef = arpcpndef;
                this.tnsf = tnsf;
            }

            public int CPcpn => arpcpndef.Length;

            public Layr Layr()
            {
                int cPcpn = arpcpndef.Length;
                Pcpn[] arpcpn = new Pcpn[cPcpn];

                for (int i = 0; i < cPcpn; i++)
                    arpcpn[i] = arpcpndef[i].Pcpn();

                return new Layr(arpcpn, tnsf);
            }
        }

        private struct Pcpndef
        {
            private readonly Vector vWeight;
            private readonly Scalar sBias;

            public Pcpndef(Vector vWeight, Scalar sBias)
            {
                this.vWeight = vWeight;
                this.sBias = sBias;
            }

            public Pcpn Pcpn() => new Pcpn(vWeight, sBias);
        }
    }
}
