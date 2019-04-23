using Cardiff.Model.Layers.Contracts;
using Cardiff.Model.NeuralUnits;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cardiff.Model.Layers
{
    class LSTM : ILayer<LstmGateUnit>
    {
        public IReadOnlyList<LstmGateUnit> Neurons => throw new NotImplementedException();

        public void Link(ILayer<NeuralUnit> layer)
        {
            throw new NotImplementedException();
        }
    }
}
