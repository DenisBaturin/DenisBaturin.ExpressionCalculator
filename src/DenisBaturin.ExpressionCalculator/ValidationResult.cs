using System.Collections.Generic;

namespace DenisBaturin.ExpressionCalculator
{
    internal class ValidationResult
    {

        public ValidationResult()
        {
            IsValid = true;
            ErrorMessages = new List<string>();
        }

        public bool IsValid { get; set; }
        public List<string> ErrorMessages { get; set; }

    }
}