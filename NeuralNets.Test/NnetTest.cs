using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nnet;

namespace NnetTest
{
    [TestClass]
    public class NnetTest
    {
        [TestMethod]
        public void NnetBasicTest()
        {
            System.Diagnostics.Trace.Listeners.Clear();

            NnetB nnetb = new NnetB();

            nnetb.AddLayer(new [] { new Vector(-0.27), new Vector(-0.41) }, new Scalar[] { -0.48, -0.13 }, Tnsf.tnsfLogSigmoid);
            nnetb.AddLayer(new [] { new Vector(0.09, -0.17) }, new Scalar[] { 0.48 }, Tnsf.tnsfLinear);

            Nnet.Nnet nnet = nnetb.Nnet();

            Scalar s = 1.0;
            Vector vOut = nnet.Evaluate(new Vector(s));
            nnet.Train(new Vector(Func(s)));
            Matrix[] augweight = nnet.AugWeight;
        }

        private Scalar Func(Scalar s) => (1.0 + Math.Sin(Math.PI * 0.25 * (double) s));
    }
}
