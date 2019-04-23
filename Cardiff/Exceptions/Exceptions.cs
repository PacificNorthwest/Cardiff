using System;
using System.Collections.Generic;
using System.Text;

namespace Cardiff.Exceptions
{
    public class CardiffException : Exception
    {
        public CardiffException(string message) : base(message) { }
    }

    public class InputException : CardiffException
    {
        public InputException(string message) : base(message) { }
    }

    public class OutputExceptin : CardiffException
    {
        public OutputExceptin(string message) : base(message) { } 
    }

    public class IncompatibleInputSizeException : InputException
    {
        public IncompatibleInputSizeException() : base("Incompatible input size") { }
    }

    public class UnableToFindInputException : InputException
    {
        public UnableToFindInputException() : base("Unable to find input layer") { }
    }

    public class IncompatibleOutputSizeException : OutputExceptin
    {
        public IncompatibleOutputSizeException() : base("Incompatible output size") { }
    }

    public class InvalidFittingAlgorithmException : CardiffException
    {
        public InvalidFittingAlgorithmException() : base("Fitting algorithm is supposed to implement IFittingAlgorithm interface") { }
    }
}
