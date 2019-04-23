using System;
using System.Collections.Generic;
using System.Text;
using Cardiff.Algorithms.Activation;
using Cardiff.Model.Layers.Contracts;

namespace Cardiff.Model.NeuralUnits
{
    class ConvolutionalUnit : NeuralUnit
    {
        public ConvolutionalUnit(ActivationFunction func) : base(func) { }

        protected override void ProcessData()
        {
            base.ProcessData();
        }

        public override void Link(ILayer<NeuralUnit> layer)
        {
            base.Link(layer);
        }
    }
}
