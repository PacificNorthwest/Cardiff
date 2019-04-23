using Cardiff.Model.NeuralUnits;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace Cardiff.Model
{
    public class Synapse
    {
        public NeuralUnit Start { get; private set; }
        public NeuralUnit End { get; private set; }
        public double Weight { get; private set; }

        public double? Value { get; private set; }
        public event Action ValueReceived;

        /// <summary>
        /// Synapse constructor
        /// </summary>
        /// <param name="start">Start NeuralUnit endpoint</param>
        /// <param name="end">End NeuralUnit endpoint</param>
        public Synapse(NeuralUnit start, NeuralUnit end)
        {
            this.Start = start;
            this.End = end;

            this.Start.Output.Subscribe(this.Transfer);
        }

        /// <summary>
        /// Transfer data to end NeuralUnit endpoint
        /// </summary>
        /// <param name="value"></param>
        private void Transfer(double value)
        {
            this.Value = value * this.Weight;
            this.ValueReceived?.Invoke();
        }

        /// <summary>
        /// Clear the synapse value
        /// </summary>
        public void Clear() => this.Value = null;

        /// <summary>
        /// Set the synapse weight
        /// </summary>
        /// <param name="weight">Weight to set</param>
        public virtual void SetWeight(double weight) => this.Weight = weight;
    }
}
