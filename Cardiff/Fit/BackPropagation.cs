using Cardiff.Fit.Contracts;
using Cardiff.Model;
using Cardiff.Model.Layers;
using Cardiff.Model.Layers.Contracts;
using Cardiff.Model.NeuralUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Cardiff.Fit
{
    class Backpropagation : IFittingAlgorithm
    {
        private const double DEFAULT_LEARNING_RATE = 0.01;
        private const double DEFAULT_MOMENTUM = 0.5;

        private Dictionary<NeuralUnit, double> _delta = new Dictionary<NeuralUnit, double>();
        private Dictionary<Synapse, double> _weightShift = new Dictionary<Synapse, double>();

        /// <summary>
        /// Fit the neural network
        /// </summary>
        /// <param name="trainingSets">A tuple containing input training data and expected output data</param>
        /// <param name="fittingAlgorithm">A fitting algorithm to use</param>
        /// <param name="epochsCount">Number of epochs to train</param>
        /// <param name="learningRate">Learning rate</param>
        /// <param name="momentum">Momentum</param>
        /// <param name="evaluationMetricsPipeline">A pipeline for a evaluation metrics flow</param>
        public void Fit(List<ILayer<NeuralUnit>> layers, List<(double[] input, double[] expectedOutput)> trainingSets, int epochsCount, double? learningRate = default, double? momentum = default, Subject<double> evaluationMetricsPipeline = null)
        {
            if (trainingSets.Any(s => s.expectedOutput.Length != layers.Last().Neurons.Count))
                throw new Exceptions.IncompatibleOutputSizeException();

            for (int i = 0; i < epochsCount; i++)
            {
                foreach (var set in trainingSets)
                {
                    this.ClearSynapses(layers);
                    this.ProcessSet(layers, set, learningRate ?? DEFAULT_LEARNING_RATE, momentum ?? DEFAULT_MOMENTUM, evaluationMetricsPipeline);
                }
            }
        }

        /// <summary>
        /// Process single training set
        /// </summary>
        /// <param name="layers">List of neural network layers</param>
        /// <param name="set">A tuple containing input training data and expected output data</param>
        /// <param name="learningRate">Learning rate</param>
        /// <param name="momentum">Momentum</param>
        /// <param name="evaluationMetricsPipeline">A pipeline for a evaluation metrics flow</param>
        private void ProcessSet(List<ILayer<NeuralUnit>> layers, (double[] input, double[] expectedOutput) set, double learningRate, double momentum, Subject<double> evaluationMetricsPipeline = null)
        {
            if (layers.First() is Input input)
                input.InputData(set.input);
            else throw new Exceptions.UnableToFindInputException();

            for (int i = 0; i < layers.Last().Neurons.Count; i++)
            {
                NeuralUnit unit = layers.Last().Neurons[i];
                this._delta[unit] = Math.DeltaOutput(set.expectedOutput[i], unit.Output.Value);
            }

            for (int i = layers.Count - 2; i >= 0; i--)
            {
                foreach (NeuralUnit unit in layers[i].Neurons)
                {
                    double sum = unit.OutputSynapses.Sum(s => s.Weight * this._delta[s.End]);
                    this._delta[unit] = Math.DeltaRegular(unit.Output.Value, sum);

                    foreach (Synapse synapse in unit.OutputSynapses)
                    {
                        var grad = Math.SynapseGradient(synapse.Start.Output.Value, this._delta[synapse.End]);
                        this._weightShift.TryGetValue(synapse, out double lastWeightShift);
                        var weightShift = Math.SynapseWeightShift(learningRate, grad, momentum, lastWeightShift);

                        synapse.SetWeight(synapse.Weight + weightShift);
                        this._weightShift[synapse] = weightShift;
                    }
                }
            }

            evaluationMetricsPipeline?.OnNext(Math.MSE(set.expectedOutput, layers.Last().Neurons.Select(n => n.Output.Value).ToArray()));
        }

        /// <summary>
        /// Clear all synapses
        /// </summary>
        /// <param name="layers">List of neural network layers</param>
        private void ClearSynapses(List<ILayer<NeuralUnit>> layers) => layers.SelectMany(l => l.Neurons).SelectMany(n => n.OutputSynapses).ToList().ForEach(s => s.Clear());

        static class Math
        {
            /// <summary>
            /// Compute Mean square error
            /// </summary>
            /// <param name="expectedResult">Expected result</param>
            /// <param name="actualResult">Actual result</param>
            /// <returns>Mean square error</returns>
            public static double MSE(double[] expectedResult, double[] actualResult) => System.Math.Pow(actualResult.Select((result, index) => expectedResult[index] - result).Average(), 2);

            /// <summary>
            /// Compute Output Delta
            /// </summary>
            /// <param name="expectedResult">Expected result</param>
            /// <param name="actualResult">Actual result</param>
            /// <returns>Output Delta</returns>
            public static double DeltaOutput(double expectedResult, double actualResult) => (expectedResult - actualResult) * ((1 - actualResult) * actualResult);

            /// <summary>
            /// Compute Regular Delta
            /// </summary>
            /// <param name="output">Neuron output</param>
            /// <param name="synapseSum">All output synapses values sum</param>
            /// <returns>Regular Delta</returns>
            public static double DeltaRegular(double output, double synapseSum) => ((1 - output) * output) * synapseSum;

            /// <summary>
            /// Compute synapse gradient
            /// </summary>
            /// <param name="outputStart">Start NeuralUnit endpoint output value</param>
            /// <param name="deltaEnd">End NeuralUnit endpoint delta value</param>
            /// <returns>Synapse gradient</returns>
            public static double SynapseGradient(double outputStart, double deltaEnd) => outputStart * deltaEnd;

            /// <summary>
            /// Compute Synapse weight shift
            /// </summary>
            /// <param name="learningRate">Lerning rate</param>
            /// <param name="synapseGradient">Synapse gradient</param>
            /// <param name="momentum">Momentum</param>
            /// <param name="lastShift">Last synapse weight shift</param>
            /// <returns>Synapse weight shift</returns>
            public static double SynapseWeightShift(double learningRate, double synapseGradient, double momentum, double lastShift) => (learningRate * synapseGradient) + (momentum * lastShift);
        }
    }
}
