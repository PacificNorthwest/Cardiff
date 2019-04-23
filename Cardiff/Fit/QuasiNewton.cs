using Cardiff.Fit.Contracts;
using Cardiff.Model.Layers.Contracts;
using Cardiff.Model.NeuralUnits;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace Cardiff.Fit
{
    class QuasiNewton : IFittingAlgorithm
    {
        public void Fit(List<ILayer<NeuralUnit>> layers, List<(double[] input, double[] expectedOutput)> trainSets, int epochsCount, double? learningRate = null, double? momentum = null, Subject<double> evaluationMetricsPipeline = null)
        {
            throw new NotImplementedException();
        }
    }
}
