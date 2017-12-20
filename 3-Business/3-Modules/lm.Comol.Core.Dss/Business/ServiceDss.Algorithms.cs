using lm.Comol.Core.Dss.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LM.MathLibrary.Algorithms;
using LM.MathLibrary.Extensions;
namespace lm.Comol.Core.Dss.Business
{
    public partial class ServiceDss
    {
        public List<dtoAlgorithmAlternative> Calculate(dtoAlgorithmInput input)
        {
            List<dtoAlgorithmAlternative> results = input.Alternatives;
            switch (input.Type)
            {
                case AlgorithmType.ahp:
                    break;
                case AlgorithmType.owa:
                    results = CalculateOwa(input);
                    break;
                case AlgorithmType.topsis:
                    results = CalculateTopsis(input);
                    break;
                case AlgorithmType.weightedAverage:
                    results = CalculateWeightedAverage(input);
                    break;
            }
            return results;
        }
        public List<dtoAlgorithmAlternativeFuzzy> Calculate(dtoAlgorithmInputFuzzy input)
        {
            List<dtoAlgorithmAlternativeFuzzy> results = input.Alternatives;
            switch (input.Type)
            {
                case AlgorithmType.ahp:
                    break;
                case AlgorithmType.owa:
                    results = CalculateOwa(input);
                    break;
                case AlgorithmType.topsis:
                    results = CalculateTopsis(input);
                    break;
                case AlgorithmType.weightedAverage:
                    results = CalculateWeightedAverage(input);
                    break;
            }
            return results;
        }

        #region Topsis
            public List<dtoAlgorithmAlternative> CalculateTopsis(dtoAlgorithmInput input, IEnumerable<int> benefits = null, IEnumerable<int> costs = null)
            {
                List<dtoAlgorithmAlternative> results = input.Alternatives;
                double[] weights = input.Weights.ToArray();
                switch(input.NormalizeTo){
                    case NormalizeTo.simple:
                        weights = weights.SimpleNormalize().ToArray();
                        break;
                    case NormalizeTo.standard:
                        weights = weights.Normalize().ToArray();
                        break;
                    case NormalizeTo.toOne:
                        weights = weights.NormalizeTo1().ToArray();
                        break;
                }

                LM.MathLibrary.Algorithms.Topsis tp = new LM.MathLibrary.Algorithms.Topsis(input.Alternatives.Select(a => a.Values.ToArray()), weights, benefits, costs);
                double[] values = tp.Elaborate();
                if (values != null && values.Any()){
                    values = (values.SumToOne()) ? values: values.NormalizeTo1().ToArray();
                    long index = 0;
                    foreach (dtoAlgorithmAlternative alternative in results){
                        alternative.Ranking = values[index];
                        alternative.FinalValue = values[index];
                        alternative.FinalValueFuzzy = new TriangularFuzzyNumber(alternative.FinalValue).ToString();
                        alternative.IsFuzzyValue = false;
                        index++;
                    }
                }
                return results;
            }
            public List<dtoAlgorithmAlternativeFuzzy> CalculateTopsis(dtoAlgorithmInputFuzzy input, IEnumerable<int> benefits = null, IEnumerable<int> costs = null)
            {
                List<dtoAlgorithmAlternativeFuzzy> results = input.Alternatives;

                LM.MathLibrary.Algorithms.FuzzyTopsis tp = new LM.MathLibrary.Algorithms.FuzzyTopsis(input.Alternatives.Select(a => a.Values.ToArray()), input.Weights.ToArray(), benefits, costs);
                double[] values = tp.Elaborate();
                if (values != null && values.Any())
                {
                    //values = (values.SumToOne()) ? values ; values.NormalizeTo1().ToArray();
                    long index = 0;
                    foreach (dtoAlgorithmAlternativeFuzzy alternative in results)
                    {
                        alternative.Ranking = values[index];
                        alternative.FinalValue = values[index];
                        alternative.FinalValueFuzzy = new TriangularFuzzyNumber(alternative.FinalValue).ToString();
                        alternative.IsFuzzyValue = false;
                        index++;
                    }
                }
                return results;
            }
        #endregion

        #region owa
            public List<dtoAlgorithmAlternative> CalculateOwa(dtoAlgorithmInput input)
            {
                List<dtoAlgorithmAlternative> results = input.Alternatives;
                double[] weights = input.Weights.ToArray();
                //switch (input.NormalizeTo)
                //{
                //    case NormalizeTo.simple:
                //        weights = weights.SimpleNormalize().ToArray();
                //        break;
                //    case NormalizeTo.standard:
                        weights = weights.Normalize().ToArray();
                //        break;
                //    case NormalizeTo.toOne:
                //        weights = weights.NormalizeTo1().ToArray();
                //        break;
                //}

                LM.MathLibrary.Algorithms.TriangularFuzzyOWA tp = new LM.MathLibrary.Algorithms.TriangularFuzzyOWA(input.Alternatives.Select(a => a.Values.ToArray()), weights);
                TriangularFuzzyNumber[] values = tp.Elaborate();
                if (values != null && values.Any())
                {
                    long index = 0;
                    foreach (dtoAlgorithmAlternative alternative in results)
                    {
                        if (values[index] != null)
                        {
                            alternative.Ranking = values[index].Ranking();
                            alternative.FinalValue = values[index].ToCrispy();
                            alternative.FinalValueFuzzy = values[index].ToString();
                            alternative.IsFuzzyValue = true;
                        }
                        else
                        {
                            alternative.Ranking = 0;
                            alternative.FinalValue = 0;
                            alternative.FinalValueFuzzy = alternative.FinalValue.ToFuzzy().ToString();
                            alternative.IsFuzzyValue = true;
                        }
                        index++;
                    }
                }
                return results;
            }
            public List<dtoAlgorithmAlternativeFuzzy> CalculateOwa(dtoAlgorithmInputFuzzy input)
            {
                List<dtoAlgorithmAlternativeFuzzy> results = input.Alternatives;

                LM.MathLibrary.Algorithms.TriangularFuzzyOWA tp = new LM.MathLibrary.Algorithms.TriangularFuzzyOWA(input.Alternatives.Select(a => a.Values.ToArray()), input.Weights.ToArray());
                TriangularFuzzyNumber[] values = tp.Elaborate();
                if (values != null && values.Any())
                {
                    //values = (values.SumToOne()) ? values ; values.NormalizeTo1().ToArray();
                    long index = 0;
                    foreach (dtoAlgorithmAlternativeFuzzy alternative in results)
                    {
                        if (values[index] != null)
                        {
                            alternative.Ranking = values[index].Ranking();
                            alternative.FinalValue = values[index].ToCrispy();
                            alternative.FinalValueFuzzy = values[index].ToString();
                            alternative.IsFuzzyValue = true;
                        }
                        else
                        {
                            alternative.FinalValue = 0;
                            alternative.Ranking = 0;
                            alternative.FinalValueFuzzy = alternative.FinalValue.ToFuzzy().ToString();
                            alternative.IsFuzzyValue = true;
                        }
                        index++;
                    }
                
                }
                return results;
            }
        #endregion

        #region WeightedAverage
            public List<dtoAlgorithmAlternative> CalculateWeightedAverage(dtoAlgorithmInput input)
            {
                List<dtoAlgorithmAlternative> results = input.Alternatives;
                double[] weights = input.Weights.ToArray();
                //switch (input.NormalizeTo)
                //{
                //    case NormalizeTo.simple:
                //        weights = weights.SimpleNormalize().ToArray();
                //        break;
                //    case NormalizeTo.standard:
                weights = weights.Normalize().ToArray();
                //        break;
                //    case NormalizeTo.toOne:
                //        weights = weights.NormalizeTo1().ToArray();
                //        break;
                //}

                LM.MathLibrary.Algorithms.TriangularFuzzyWeightedAverage tp = new LM.MathLibrary.Algorithms.TriangularFuzzyWeightedAverage(input.Alternatives.Select(a => a.Values.ToArray()), weights);
                TriangularFuzzyNumber[] values = tp.Elaborate();
                if (values != null && values.Any())
                {
                    long index = 0;
                    foreach (dtoAlgorithmAlternative alternative in results)
                    {
                        if (values[index] != null)
                        {
                            alternative.Ranking = values[index].Ranking();
                            alternative.FinalValue = values[index].ToCrispy();
                            alternative.FinalValueFuzzy = values[index].ToString();
                            alternative.IsFuzzyValue = true;
                        }
                        else
                        {
                            alternative.Ranking = 0;
                            alternative.FinalValue = 0;
                            alternative.FinalValueFuzzy = alternative.FinalValue.ToFuzzy().ToString();
                            alternative.IsFuzzyValue = true;
                        }
                        index++;
                    }
                }
                return results;
            }
            public List<dtoAlgorithmAlternativeFuzzy> CalculateWeightedAverage(dtoAlgorithmInputFuzzy input)
            {
                List<dtoAlgorithmAlternativeFuzzy> results = input.Alternatives;

                LM.MathLibrary.Algorithms.TriangularFuzzyWeightedAverage tp = new LM.MathLibrary.Algorithms.TriangularFuzzyWeightedAverage(input.Alternatives.Select(a => a.Values.ToArray()), input.Weights.ToArray());
                TriangularFuzzyNumber[] values = tp.Elaborate();
                if (values != null && values.Any())
                {
                    //values = (values.SumToOne()) ? values ; values.NormalizeTo1().ToArray();
                    long index = 0;
                    foreach (dtoAlgorithmAlternativeFuzzy alternative in results)
                    {
                        if (values[index] != null)
                        {
                            alternative.Ranking = values[index].Ranking();
                            alternative.FinalValue = values[index].ToCrispy();
                            alternative.FinalValueFuzzy = values[index].ToString();
                            alternative.IsFuzzyValue = true;
                        }
                        else
                        {
                            alternative.Ranking = 0;
                            alternative.FinalValue = 0;
                            alternative.FinalValueFuzzy = alternative.FinalValue.ToFuzzy().ToString();
                            alternative.IsFuzzyValue = true;
                        }
                        index++;
                    }
                }
                return results;
            }
        #endregion
    }
}