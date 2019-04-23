using System;
using System.Collections.Generic;
using System.Text;

namespace Cardiff.Model.Layers.Contracts
{
    public interface ILayer<out T> where T : NeuralUnits.NeuralUnit
    {
        IReadOnlyList<T> Neurons { get; }

        /// <summary>
        /// Link layer to an other layer
        /// </summary>
        /// <param name="layer"></param>
        void Link(ILayer<NeuralUnits.NeuralUnit> layer);
    }

    public interface ILayer : ILayer<NeuralUnits.NeuralUnit> { }
}
