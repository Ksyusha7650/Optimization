using OxyPlot;

namespace CourseWorkOptimization;


    public class Point : IDataPointProvider {
        private double _value;
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Value { get { return _value; } set { } }
        public Box Opt { get; set; }
        public DataPoint GetDataPoint() => new DataPoint(X1, X2);

        public Point (double X1, double X2, Box Opt) {
            this.X1 = X1;
            this.X2 = X2;
            this.Opt = Opt;
            _value = Opt.FunctionValue(X1, X2);
        }
    }
