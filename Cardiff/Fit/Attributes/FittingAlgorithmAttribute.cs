using Cardiff.Exceptions;
using Cardiff.Fit.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cardiff.Fit.Attributes
{
    class FittingAlgorithmAttribute : Attribute
    {
        private Type _algorithm;
        public IFittingAlgorithm GetAlgorithm() => (IFittingAlgorithm)Activator.CreateInstance(this._algorithm);

        public FittingAlgorithmAttribute(Type algorithm)
        {
            if (typeof(IFittingAlgorithm).IsAssignableFrom(algorithm))
                this._algorithm = algorithm;
            else throw new InvalidFittingAlgorithmException();
        }
    }
}
