using System;
using System.Runtime.CompilerServices;

namespace CourseWorkOptimization;

public class MyTuple
{
    public MyTuple(double firstElement, double secondElement)
    {
        FirstElement = firstElement;
        SecondElement = secondElement;
    }

    public double FirstElement { get; set; }
    public double SecondElement { get; set; }

    public static MyTuple operator *(double factor, MyTuple tuple) =>
        new(tuple.FirstElement * factor, tuple.SecondElement * factor);
    
    public static MyTuple operator *(MyTuple tuple, double factor) =>
        new(tuple.FirstElement * factor, tuple.SecondElement * factor);
    
    public static MyTuple operator /(MyTuple tuple, double factor) =>
        new(tuple.FirstElement / factor, tuple.SecondElement / factor);
    
    public static MyTuple operator -(MyTuple tuple1, MyTuple tuple2) =>
        new(tuple1.FirstElement - tuple2.FirstElement, tuple1.SecondElement - tuple2.SecondElement);
    
    public static MyTuple operator +(MyTuple tuple1, MyTuple tuple2) =>
        new(tuple1.FirstElement + tuple2.FirstElement, tuple1.SecondElement + tuple2.SecondElement);
    
    public static MyTuple operator -(MyTuple tuple) => -1 * tuple;
    public MyTuple Middle(MyTuple tuple) => (this + tuple) / 2;

    public double Path(MyTuple tuple) => 
        Math.Sqrt(Math.Pow(FirstElement - tuple.FirstElement, 2) + Math.Pow(SecondElement - tuple.SecondElement, 2));

}