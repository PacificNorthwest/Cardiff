using Cardiff.Fit.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cardiff.Fit
{
    public enum Algorithms
    {
        [FittingAlgorithm(typeof(Backpropagation))]BackProp,
        [FittingAlgorithm(typeof(ConjugateGradient))]ConjugateGradient,
        [FittingAlgorithm(typeof(LevenbergMarquardt))]LevenbergMarquardt,
        [FittingAlgorithm(typeof(NewtonsMethod))]Newton,
        [FittingAlgorithm(typeof(QuasiNewton))]QuasiNewton
    }
}
