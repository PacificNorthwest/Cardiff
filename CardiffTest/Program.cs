using System;
using System.Collections.Generic;
using Cardiff;
using Cardiff.Algorithms.Activation;
using Cardiff.Model.Layers;
using Cardiff.Model.Layers.Contracts;
using Cardiff.Model.NeuralUnits;

namespace CardiffTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ILayer<NeuralUnit>> layers = new List<ILayer<NeuralUnit>>
            {
                new Input(2),
                new Dense(2, Activation.Sigmoid),
                new Dense(2, Activation.Sigmoid),
                new Dense(1, Activation.Sigmoid)
            };

            CognitiveModel model = new CognitiveModel(layers);
            model.Compile();

            model.Fit((null, null), Cardiff.Fit.Algorithms.BackProp, 100);
        }
    }
}
