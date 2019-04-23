using Cardiff.Model.Layers.Contracts;
using Cardiff.Model.NeuralUnits;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace Cardiff.Fit.Contracts
{
    interface IFittingAlgorithm
    {
        void Fit(List<ILayer<NeuralUnit>> layers, List<(double[] input, double[] expectedOutput)> trainSets, int epochsCount, double? learningRate = default, double? momentum = default, Subject<double> evaluationMetricsPipeline = null);
    }
}
