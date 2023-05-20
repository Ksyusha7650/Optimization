using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using static System.Double;

namespace CourseWorkOptimization;

public class Algorithm
{
    private readonly double _A = 1;
    private readonly double _alpha = 1;
    private readonly double _beta = 1;
    private readonly double _G = 2;
    private readonly double _mu = 1;
    private readonly int _N = 2;
    public List<MyTuple> List = new(), ListNesterov = new();
    private double _learningRate = 0.01, _momentum = 0.8;

    public double GetS(double T1, double T2)
    {
        if (Check(T1, T2))
        {
            return _alpha * _G * _mu * (Math.Pow(T2 - T1, _N) + Math.Pow(_beta * _A - T1, _N));
        }

        return double.NaN;
        /*return _alpha * _G * _mu * (Math.Pow(T2 - T1, _N) + Math.Pow(_beta * _A - T1, _N));*/
    }

    private bool Check(double x, double y)
    {
        return (x is >= -3 and <= 3) && (y is >= -2 and <= 6) && (x - y >= -3);
    }

    private double DerivativeT1(double T1, double T2)
    {
        return -_N * _alpha * _G * _mu * (Math.Pow(T2 - T1, _N - 1) + Math.Pow(_beta * _A - T1, _N - 1));
    }
    
    private double DerivativeT2(double T1, double T2)
    {
        return _N * _alpha * _G * _mu * Math.Pow(T2 - T1, _N - 1);
    }

    private MyTuple Gradient(MyTuple pair)
    {
        return new MyTuple(
            DerivativeT1(pair.FirstElement, pair.SecondElement),
            DerivativeT2(pair.FirstElement, pair.SecondElement));
    }

    public void Calculate()
    {
        var xK = new MyTuple(-3.0, -2.0);
        var path = (List.Count >= 2)? xK.Path(List[^2]): 1;
        while (path > 0.01)
        {
            if (Check(xK.FirstElement, xK.SecondElement))
            {
                List.Add(xK);
                xK -= _learningRate * Gradient(xK);
            }
            else
            {
                while (!Check(xK.FirstElement, xK.SecondElement))
                {
                    xK = xK.Middle(List[^1]);
                }
                List.Add(xK);
            }
            path = List.Count >= 2? xK.Path(List[^2]): 1;
        }
    }

    public void Nesterov()
    {
        var xK = new MyTuple(-3.0, -2.0);
        var yK = new MyTuple(-2.0, -2.0);
        ListNesterov.Add(xK);
        var path = (ListNesterov.Count >= 2)? xK.Path(ListNesterov[^2]): 1;
        while (path > 0.01)
        {
            if (Check(xK.FirstElement, xK.SecondElement))
            {
                xK = yK - _learningRate * Gradient(yK) / Math.Sqrt(ListNesterov.Count);
                yK = xK + _momentum * (xK - ListNesterov[^1]);
                ListNesterov.Add(xK);
            }
            else
            {
                while (!Check(xK.FirstElement, xK.SecondElement))
                {
                    xK = xK.Middle(ListNesterov[^2]);
                }
                ListNesterov.Add(xK);;
            }
            path = (ListNesterov.Count >= 2)? xK.Path(ListNesterov[^2]): 1;
        }
    }
    
    
}