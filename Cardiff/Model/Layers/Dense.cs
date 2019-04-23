using System.Collections.Generic;
using System.Linq;
using Cardiff.Algorithms.Activation;
using Cardiff.Model.Layers.Contracts;
using Cardiff.Model.NeuralUnits;

namespace Cardiff.Model.Layers
{
    public sealed class Dense : ILayer<NeuralUnit>
    {
        private List<NeuralUnit> _neurons;
        public IReadOnlyList<NeuralUnit> Neurons => this._neurons.AsReadOnly();

        /// <summary>
        /// A dense layer constructor
        /// </summary>
        /// <param name="neuronsCount">Number of neurons to use</param>
        /// <param name="activationFunction">Activation function to use</param>
        /// <param name="useBias">Flag for bian neuron usage</param>
        public Dense(int neuronsCount, ActivationFunction activationFunction, bool useBias = false)
        {
            this._neurons = Enumerable.Range(0, neuronsCount).Select(_ => new NeuralUnit(activationFunction)).ToList();
            if (useBias)
                this._neurons.Add(new BiasUnit(Activation.Blank));
        }

        /// <summary>
        /// Link layer to another layer
        /// </summary>
        /// <param name="layer">Layer to link to</param>
        public void Link(ILayer<NeuralUnit> layer)
        {
            foreach (NeuralUnit neuron in this.Neurons) neuron.Link(layer);
        }
    }
}
