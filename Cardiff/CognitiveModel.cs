using Cardiff.Exceptions;
using Cardiff.Fit.Attributes;
using Cardiff.Model;
using Cardiff.Model.Layers;
using Cardiff.Model.Layers.Contracts;
using Cardiff.Model.NeuralUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Cardiff
{
    public class CognitiveModel
    {
        private const double MIN_WEIGHT = -1.0;
        private const double MAX_WEIGHT = 1.0;

        public List<ILayer<NeuralUnit>> Layers { get; set; } = new List<ILayer<NeuralUnit>>();

        /// <summary>
        /// Cognitive model constructor
        /// </summary>
        /// <param name="layers">List of neural network layers</param>
        public CognitiveModel(List<ILayer<NeuralUnit>> layers)
        {
            this.Layers = layers;
        }

        /// <summary>
        /// Initialize neural network layers with connections and weights
        /// </summary>
        public void Compile()
        {
            this.LinkLayers();
            this.PopulateRandomWeights();
        }

        /// <summary>
        /// Process input data
        /// </summary>
        /// <param name="input">An array of double-encoded data</param>
        /// <returns></returns>
        public double[] ProcessData(double[] input)
        {
            if (this.Layers.First() is Input inputLayer)
            {
                inputLayer.InputData(input);
                return this.Layers.Last().Neurons.Select(n => n.Output.Value).ToArray();
            }
            else throw new UnableToFindInputException();
        }

        /// <summary>
        /// Fit the neural network
        /// </summary>
        /// <param name="trainingSets">A tuple containing input training data and expected output data</param>
        /// <param name="fittingAlgorithm">A fitting algorithm to use</param>
        /// <param name="epochsCount">Number of epochs to train</param>
        /// <param name="learningRate">Learning rate</param>
        /// <param name="momentum">Momentum</param>
        /// <param name="evaluationMetricsPipeline">A pipeline for a evaluation metrics flow</param>
        public void Fit(List<(double[] input, double[] expectedOutput)> trainingSets, Fit.Algorithms fittingAlgorithm, int epochsCount, double? learningRate = default, double? momentum = default, Subject<double> evaluationMetricsPipeline = null)
        {
            if (fittingAlgorithm.GetType().GetMember(fittingAlgorithm.ToString()).First().GetCustomAttributes(typeof(FittingAlgorithmAttribute), false).FirstOrDefault() is FittingAlgorithmAttribute attr)
                attr.GetAlgorithm().Fit(this.Layers, trainingSets, epochsCount, learningRate, momentum, evaluationMetricsPipeline);
        }

        /// <summary>
        /// Link layers with synapses
        /// </summary>
        private void LinkLayers()
        {
            for (int i = 0; i < this.Layers.Count - 1; i++)
                this.Layers[i].Link(this.Layers[i + 1]);

            foreach (NeuralUnit unit in this.Layers.Skip(1).SelectMany(l => l.Neurons))
                unit.SubscribeToInput();
        }

        /// <summary>
        /// Generate randow weights for synapses
        /// </summary>
        private void PopulateRandomWeights()
        {
            Random rand = new Random();
            foreach (Synapse synapse in this.Layers.SelectMany(l => l.Neurons.SelectMany(n => n.OutputSynapses)))
                synapse.SetWeight(rand.NextDouble() * (MAX_WEIGHT - MIN_WEIGHT) + MIN_WEIGHT);
        }
    }
}
