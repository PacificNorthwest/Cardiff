using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cardiff.Algorithms.Activation;
using Cardiff.Model.Layers.Contracts;

namespace Cardiff.Model.NeuralUnits
{
    class BiasUnit : NeuralUnit
    {
        public BiasUnit(ActivationFunction func) : base(func) { }

        public override void Link(ILayer<NeuralUnit> layer)
        {
            foreach (NeuralUnit neuron in layer.Neurons.Where(n => !typeof(BiasUnit).IsInstanceOfType(n)))
            {
                Synapse synapse = new Synapse(this, neuron);
                synapse.End.InputSynapses.Add(synapse);
                this.OutputSynapses.Add(synapse);
            }
        }

        protected override void ProcessData() => this._output.Value = 1;
    }
}
