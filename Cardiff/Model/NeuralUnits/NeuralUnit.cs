using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive;
using System.Reactive.Subjects;
using Cardiff.Algorithms.Activation;
using Cardiff.Model.Layers.Contracts;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace Cardiff.Model.NeuralUnits
{
    public class NeuralUnit
    {
        protected ReactiveProperty<double> _output = new ReactiveProperty<double>();
        public ReadOnlyReactiveProperty<double> Output => this._output.ToReadOnlyReactiveProperty();

        public List<Synapse> OutputSynapses { get; set; } = new List<Synapse>();
        public List<Synapse> InputSynapses { get; set; } = new List<Synapse>();

        private readonly ActivationFunction _activationFunction;

        /// <summary>
        /// A Neural unit constructor
        /// </summary>
        /// <param name="func">Activation function to use</param>
        public NeuralUnit(ActivationFunction func) { this._activationFunction = func; }

        /// <summary>
        /// Link Neural Unit to another layer
        /// </summary>
        /// <param name="layer">Layer to link to</param>
        public virtual void Link(ILayer<NeuralUnit> layer)
        {
            foreach (NeuralUnit neuron in layer.Neurons)
            {
                Synapse synapse = new Synapse(this, neuron);
                synapse.End.InputSynapses.Add(synapse);
                this.OutputSynapses.Add(synapse);
            }
        }

        /// <summary>
        /// Subscribe to input synapses values
        /// </summary>
        public virtual void SubscribeToInput()
        {
            foreach (Synapse synapse in this.InputSynapses)
                synapse.ValueReceived += () => 
                {
                    if (this.InputSynapses.All(s => s.Value.HasValue))
                        this.ProcessData();
                };
        }

        /// <summary>
        /// Process received data
        /// </summary>
        protected virtual void ProcessData() => this._output.Value = this._activationFunction(this.InputSynapses.Select(s => s.Value ?? default).Sum());
    }
}
