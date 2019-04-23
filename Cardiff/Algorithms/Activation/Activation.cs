using System;
using System.Collections.Generic;
using System.Text;

namespace Cardiff.Algorithms.Activation
{
    public delegate double ActivationFunction(double input);

    public static class Activation
    {
        public static ActivationFunction Sigmoid { get; } = (double input) => 1 / (1 + Math.Exp(-input));
        public static ActivationFunction HyperbolicTangent { get; } = (double input) => (1 - Math.Exp(-input * 2)) / (1 + Math.Exp(-input * 2));
        public static ActivationFunction ReLu { get; } = (double input) => Math.Max(0, input);
        public static ActivationFunction Blank { get; } = (double input) => input;
    }
}
