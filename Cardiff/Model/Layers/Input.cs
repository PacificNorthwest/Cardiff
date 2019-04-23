using Cardiff.Exceptions;
using Cardiff.Model.Layers.Contracts;
using Cardiff.Model.NeuralUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cardiff.Model.Layers
{
    public class Input : ILayer<InputNeuralUnit>
    {
        private List<InputNeuralUnit> _neurons;
        public IReadOnlyList<InputNeuralUnit> Neurons => this._neurons.AsReadOnly();

        public Input(int neuronsCount)
        {
            this._neurons = Enumerable.Range(0, neuronsCount).Select(_ => new InputNeuralUnit()).ToList();
        }

        public void InputData(double[] input)
        {
            if (this.Neurons.Count != input.Length)
                throw new IncompatibleInputSizeException();

            for (int i = 0; i < this.Neurons.Count; i++)
                this.Neurons[i].ProcessInput(input[i]);
        }

        public void Link(ILayer<NeuralUnit> layer)
        {
            foreach (NeuralUnit neuron in this.Neurons) neuron.Link(layer);
        }
    }
}
