using LM.MathLibrary.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LM.MathLibrary.Extensions;
namespace lm.Comol.Core.Dss.Domain
{
    [Serializable]
    public class dtoAlgorithmInputBase
    {
        public virtual AlgorithmType Type { get; set; }
        public virtual NormalizeTo NormalizeTo { get; set; }
        public String ToString()
        {
            return "Type: " + Type.ToString() + " NormalizeTo:" + NormalizeTo.ToString();
        }
    }
    [Serializable]
    public class dtoAlgorithmInput : dtoAlgorithmInputBase
    {
        public virtual List<double> Weights { get; set; }
        public virtual List<dtoAlgorithmAlternative> Alternatives { get; set; }
        public dtoAlgorithmInput(AlgorithmType type)
        {
            Weights = new List<double>();
            Alternatives = new List<dtoAlgorithmAlternative>();
            Type = type;
            switch (type)
            {
                case AlgorithmType.topsis:
                    NormalizeTo = Domain.NormalizeTo.toOne;
                    break;
            }
        }
        public Boolean HasEmptyValues()
        {
            return Alternatives == null || (Alternatives.Any(a => a.HasEmptyValues));
        }
    }
    [Serializable]
    public class dtoAlgorithmInputFuzzy : dtoAlgorithmInputBase
    {
        public virtual List<TriangularFuzzyNumber> Weights { get; set; }
        public virtual List<dtoAlgorithmAlternativeFuzzy> Alternatives { get; set; }
        public dtoAlgorithmInputFuzzy()
        {
            Weights = new List<TriangularFuzzyNumber>();
            Alternatives = new List<dtoAlgorithmAlternativeFuzzy>();
        }
        public dtoAlgorithmInputFuzzy(AlgorithmType type)
        {
            Weights = new List<TriangularFuzzyNumber>();
            Alternatives = new List<dtoAlgorithmAlternativeFuzzy>();
            Type = type;
        }
        public Boolean HasEmptyValues()
        {
            return Alternatives == null || (Alternatives.Any(a => a.HasEmptyValues));
        }
    }

    [Serializable]
    public class dtoAlgorithmAlternativeBase
    {
        public virtual long Id { get; set; }
        //public virtual NormalizeTo NormalizeTo { get; set; }
        public virtual double Ranking { get; set; }
        public virtual double FinalValue { get; set; }
        public virtual String FinalValueFuzzy { get; set; }
        public virtual Boolean HasEmptyValues { get; set; }
        public virtual Boolean IsFuzzyValue { get; set; }
        public String ToString()
        {
            return "Id: " + Id.ToString() + " FinalValue:" + FinalValue.ToString();
        }
    }

    [Serializable]
    public class dtoAlgorithmAlternative : dtoAlgorithmAlternativeBase
    {
        public virtual List<double> Values { get; set; }
        public dtoAlgorithmAlternative()
        {
            Values = new List<double>();
        }
    }

    [Serializable]
    public class dtoAlgorithmAlternativeFuzzy : dtoAlgorithmAlternativeBase
    {
        public virtual List<TriangularFuzzyNumber> Values { get; protected set; }

        public dtoAlgorithmAlternativeFuzzy()
        {
            IsFuzzyValue = true;
            Values = new List<TriangularFuzzyNumber>();
        }

        public virtual void AddValue(TriangularFuzzyNumber value)
        {
            Values.Add(value);
        }
        public virtual void AddValue(double value)
        {
            Values.Add(value.ToFuzzy());
        }
        public virtual void AddValue(String value)
        {
            Values.Add(value.ToFuzzy());
        }

    }
}