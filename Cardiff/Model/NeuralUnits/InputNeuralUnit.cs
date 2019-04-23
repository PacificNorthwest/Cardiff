using Cardiff.Algorithms.Activation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cardiff.Model.NeuralUnits
{
    public class InputNeuralUnit : NeuralUnit
    {
        public InputNeuralUnit() : base(Activation.Blank) { }

        public void ProcessInput(double value) => this._output.Value = value;
    }
}
