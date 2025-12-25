using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    public List<MyTuple> List = new(), ListNesterov = new(), ListGenetic = new(), ListBox = new();
    private double _learningRate = 0.01, _momentum = 0.8;
    private readonly Random _random = new Random();

    public double GetS(double T1, double T2)
    {
        if (Check(T1, T2))
        {
            return _alpha * _G * _mu * (Math.Pow(T2 - T1, _N) + Math.Pow(_beta * _A - T1, _N));
        }

        return double.NaN;
    }

    private bool Check(double x, double y)
    {
        return (x is >= -3 and <= 3) && (y is >= -2 and <= 6) && (x + 0.5 * y <= 1);
    }

    private double DerivativeT1(double T1, double T2)
    {
        return _N * _alpha * _G * _mu * (Math.Pow(T2 - T1, _N - 1) + Math.Pow(_beta * _A - T1, _N - 1));
    }
    
    private double DerivativeT2(double T1, double T2)
    {
        return -_N * _alpha * _G * _mu * Math.Pow(T2 - T1, _N - 1);
    }

    private MyTuple Gradient(MyTuple pair)
    {
        return new MyTuple(
            DerivativeT1(pair.FirstElement, pair.SecondElement),
            DerivativeT2(pair.FirstElement, pair.SecondElement));
    }

    // Проекция точки в допустимую область (box + линейное ограничение)
    private double Clamp(double x, double a, double b) => Math.Max(a, Math.Min(b, x));

    private MyTuple ProjectToFeasible(MyTuple p, MyTuple center = null)
    {
        // clamp to box
        double t1 = Math.Max(-3.0, Math.Min(3.0, p.FirstElement));
        double t2 = Math.Max(-2.0, Math.Min(6.0, p.SecondElement));

        // if linear constraint satisfied -> ok
        if (t1 + 0.5 * t2 <= 1.0)
            return new MyTuple(t1, t2);

        // if valid center given -> project along center->p to linear boundary
        if (center != null && center.FirstElement + 0.5 * center.SecondElement <= 1.0)
        {
            double cx = Math.Max(-3.0, Math.Min(3.0, center.FirstElement));
            double cy = Math.Max(-2.0, Math.Min(6.0, center.SecondElement));

            double dx = t1 - cx;
            double dy = t2 - cy;
            double denom = dx + 0.5 * dy;
            if (Math.Abs(denom) > 1e-12)
            {
                double lambda = (1.0 - (cx + 0.5 * cy)) / denom;
                lambda = Math.Max(0.0, Math.Min(1.0, lambda));
                double nx = cx + lambda * dx;
                double ny = cy + lambda * dy;
                nx = Math.Max(-3.0, Math.Min(3.0, nx));
                ny = Math.Max(-2.0, Math.Min(6.0, ny));
                if (nx + 0.5 * ny <= 1.0) return new MyTuple(nx, ny);
            }
        }

        // fallback: enforce boundary T2 = 2*(1 - T1)
        double fbT1 = t1;
        double fbT2 = 2.0 * (1.0 - fbT1);
        fbT2 = Math.Max(-2.0, Math.Min(6.0, fbT2));
        if (fbT1 + 0.5 * fbT2 > 1.0) // last resort clamp
        {
            fbT2 = Math.Min(6.0, Math.Max(-2.0, 2.0 * (1.0 - fbT1)));
        }
        return new MyTuple(fbT1, fbT2);
    }

    public double Box(ref double anst1, ref double anst2, ref int countIter)
    {
        ListBox.Clear();
        var rand = _random;
        var complexList = new List<double[]>(); // each entry: [T1, T2, S]

        // initialize 4 feasible vertices
        while (complexList.Count < 4)
        {
            double t1 = -3.0 + rand.NextDouble() * 6.0;   // [-3,3]
            double t2 = -2.0 + rand.NextDouble() * 8.0;   // [-2,6]
            var p = ProjectToFeasible(new MyTuple(t1, t2), null);
            t1 = p.FirstElement; t2 = p.SecondElement;
            if (Check(t1, t2))
            {
                double F = GetS(t1, t2);
                complexList.Add(new double[] { t1, t2, F });
            }
        }

        // find initial best and worst (minimize S)
        int worstIndex = 0, bestIndex = 0;
        double worstValue = complexList[0][2], bestValue = complexList[0][2];
        for (int i = 0; i < complexList.Count; i++)
        {
            if (complexList[i][2] > worstValue) { worstValue = complexList[i][2]; worstIndex = i; }
            if (complexList[i][2] < bestValue) { bestValue = complexList[i][2]; bestIndex = i; }
        }

        double B = double.MaxValue;
        int iterLimit = 2000;
        countIter = 0;

        while (B > 0.01 && iterLimit-- > 0)
        {
            // record current best to ListBox (trajectory like Nesterov)
            ListBox.Add(new MyTuple(complexList[bestIndex][0], complexList[bestIndex][1]));

            countIter++;

            // centroid excluding worst
            double CiT1 = 0.0, CiT2 = 0.0;
            for (int i = 0; i < complexList.Count; i++)
                if (i != worstIndex) { CiT1 += complexList[i][0]; CiT2 += complexList[i][1]; }
            CiT1 /= 3.0; CiT2 /= 3.0;

            // reflection (alpha = 1.3)
            double alpha = 1.3;
            double xt1 = CiT1 + alpha * (CiT1 - complexList[worstIndex][0]);
            double xt2 = CiT2 + alpha * (CiT2 - complexList[worstIndex][1]);

            // project to feasible (prefer projection along centroid)
            var proj = ProjectToFeasible(new MyTuple(xt1, xt2), new MyTuple(CiT1, CiT2));
            xt1 = proj.FirstElement; xt2 = proj.SecondElement;

            double newValue = GetS(xt1, xt2);

            if (newValue <= worstValue)
            {
                // contraction toward centroid
                double c1 = (complexList[worstIndex][0] + CiT1) / 2.0;
                double c2 = (complexList[worstIndex][1] + CiT2) / 2.0;
                var pc = ProjectToFeasible(new MyTuple(c1, c2), new MyTuple(CiT1, CiT2));
                c1 = pc.FirstElement; c2 = pc.SecondElement;
                double cVal = GetS(c1, c2);

                if (cVal <= worstValue)
                {
                    // shrink toward best
                    for (int i = 0; i < complexList.Count; i++)
                    {
                        if (i == bestIndex) continue;
                        double nx = (complexList[i][0] + complexList[bestIndex][0]) / 2.0;
                        double ny = (complexList[i][1] + complexList[bestIndex][1]) / 2.0;
                        var pr = ProjectToFeasible(new MyTuple(nx, ny), new MyTuple(complexList[bestIndex][0], complexList[bestIndex][1]));
                        complexList[i][0] = pr.FirstElement;
                        complexList[i][1] = pr.SecondElement;
                        complexList[i][2] = GetS(pr.FirstElement, pr.SecondElement);
                    }
                }
                else
                {
                    // accept contraction
                    complexList[worstIndex][0] = c1;
                    complexList[worstIndex][1] = c2;
                    complexList[worstIndex][2] = cVal;
                }
            }
            else
            {
                // accept reflection
                complexList[worstIndex][0] = xt1;
                complexList[worstIndex][1] = xt2;
                complexList[worstIndex][2] = newValue;
            }

            // recompute best/worst (minimize S)
            worstIndex = 0; bestIndex = 0;
            worstValue = complexList[0][2]; bestValue = complexList[0][2];
            for (int i = 0; i < complexList.Count; i++)
            {
                if (complexList[i][2] > worstValue) { worstValue = complexList[i][2]; worstIndex = i; }
                if (complexList[i][2] < bestValue) { bestValue = complexList[i][2]; bestIndex = i; }
            }

            // compute simplex "size" B as mean distance to best
            double sumDist = 0.0;
            for (int i = 0; i < complexList.Count; i++)
            {
                double dx = complexList[i][0] - complexList[bestIndex][0];
                double dy = complexList[i][1] - complexList[bestIndex][1];
                sumDist += Math.Sqrt(dx * dx + dy * dy);
            }
            B = sumDist / complexList.Count;
        }

        anst1 = Math.Round(complexList[bestIndex][0], 2);
        anst2 = Math.Round(complexList[bestIndex][1], 2);
        return Math.Round(complexList[bestIndex][2], 2);
    }

    public void GeneticAlgorithm()
    {
        const int populationSize = 30;
        const int generations = 200;
        const double mutationRate = 0.12;
        const double mutationScale = 0.1;

        ListGenetic.Clear();

        // init population feasible
        var population = new List<MyTuple>();
        while (population.Count < populationSize)
        {
            double t1 = -3.0 + _random.NextDouble() * 6.0;
            double t2 = -2.0 + _random.NextDouble() * 8.0;
            var p = ProjectToFeasible(new MyTuple(t1, t2), null);
            if (Check(p.FirstElement, p.SecondElement))
                population.Add(p);
        }

        // evaluate initial best
        MyTuple bestGlobal = population[0];
        double bestVal = GetS(bestGlobal.FirstElement, bestGlobal.SecondElement);
        ListGenetic.Add(new MyTuple(bestGlobal.FirstElement, bestGlobal.SecondElement));

        double path = 1.0;

        for (int gen = 0; gen < generations && path > 0.01; gen++)
        {
            // evaluate population
            var scored = population
                .Select(ind => new { Ind = ind, Val = GetS(ind.FirstElement, ind.SecondElement) })
                .Where(x => !double.IsNaN(x.Val))
                .OrderBy(x => x.Val) // minimize S
                .ToList();

            if (scored.Count == 0) break;

            var genBest = scored.First().Ind;
            var genBestVal = scored.First().Val;

            // update global best
            if (genBestVal < bestVal)
            {
                bestVal = genBestVal;
                bestGlobal = new MyTuple(genBest.FirstElement, genBest.SecondElement);
            }

            // record step (like Nesterov)
            if (ListGenetic.Count == 0 || bestGlobal.Path(ListGenetic[^1]) > 0.01)
                ListGenetic.Add(new MyTuple(bestGlobal.FirstElement, bestGlobal.SecondElement));

            if (ListGenetic.Count >= 2)
                path = ListGenetic[^1].Path(ListGenetic[^2]);

            // selection: top 50% parents
            var parents = scored.Take(Math.Max(2, populationSize / 2)).Select(x => x.Ind).ToList();

            // create new population with elitism (keep top 1)
            var newPop = new List<MyTuple> { new MyTuple(parents[0].FirstElement, parents[0].SecondElement) };

            // crossover + mutation
            int attempts = 0;
            while (newPop.Count < populationSize && attempts++ < populationSize * 50)
            {
                var p1 = parents[_random.Next(parents.Count)];
                var p2 = parents[_random.Next(parents.Count)];

                double alpha = 0.3 + _random.NextDouble() * 0.7;
                double childT1 = alpha * p1.FirstElement + (1 - alpha) * p2.FirstElement;
                double childT2 = alpha * p1.SecondElement + (1 - alpha) * p2.SecondElement;

                if (_random.NextDouble() < mutationRate)
                    childT1 += (_random.NextDouble() * 2.0 - 1.0) * mutationScale;
                if (_random.NextDouble() < mutationRate)
                    childT2 += (_random.NextDouble() * 2.0 - 1.0) * mutationScale;

                // project & clamp
                var child = ProjectToFeasible(new MyTuple(childT1, childT2), bestGlobal);
                if (Check(child.FirstElement, child.SecondElement))
                    newPop.Add(child);
            }

            // if not enough children, refill randomly
            while (newPop.Count < populationSize)
            {
                double t1 = -3.0 + _random.NextDouble() * 6.0;
                double t2 = -2.0 + _random.NextDouble() * 8.0;
                var pr = ProjectToFeasible(new MyTuple(t1, t2), bestGlobal);
                if (Check(pr.FirstElement, pr.SecondElement))
                    newPop.Add(pr);
            }

            population = newPop;
        }
    }


    // ---- исправлённый Calculate ----
    public void Calculate()
    {
        var xK = new MyTuple(0.7, 0.5);
        _learningRate = MainWindow.alpha;

        // инициализируем список первым значением, чтобы корректно брать предыдущий элемент
        List.Clear();
        List.Add(xK);

        double path = 1.0;
        while (path > 0.01)
        {
            // градиентный шаг (без нарушения ограничений)
            var next = xK - _learningRate * Gradient(xK);

            // если вышли за допустимый регион, проецируем на допустимую область через среднее с предыдущим
            if (!Check(next.FirstElement, next.SecondElement))
            {
                // пытаемся найти ближайшую допустимую точку линейной интерполяцией к последнему элементу
                var prev = List[^1];
                int iter = 0;
                while (!Check(next.FirstElement, next.SecondElement) && iter++ < 50)
                {
                    next = next.Middle(prev);
                }
            }

            xK = next;
            List.Add(xK);

            // путь между последними двумя точками
            path = (List.Count >= 2) ? List[^1].Path(List[^2]) : 1.0;
        }
    }

    // ---- исправлённый Nesterov ----
    public void Nesterov()
    {
        // инициализация
        var xK = new MyTuple(0.7, 0.5);
        var yK = new MyTuple(xK.FirstElement, xK.SecondElement); // начать с той же точки
        _learningRate = MainWindow.alpha;
        ListNesterov.Clear();
        ListNesterov.Add(xK);

        double path = 1.0;
        while (path > 0.01)
        {
            // вычисляем предсказанный градиент в yK
            var gradY = Gradient(yK);

            // шаг Нестерова: сначала предсказание, затем коррекция
            var next = yK - (_learningRate * gradY);

            // проекция на допустимую область, если нужно
            if (!Check(next.FirstElement, next.SecondElement))
            {
                // бисекция/интерполяция к последнему допустимому элементу
                var prev = ListNesterov[^1];
                int iter = 0;
                while (!Check(next.FirstElement, next.SecondElement) && iter++ < 50)
                {
                    next = next.Middle(prev);
                }
            }

            // обновление списков
            xK = next;
            ListNesterov.Add(xK);

            // вычисляем y_{k+1} = x_k + momentum * (x_k - x_{k-1})
            if (ListNesterov.Count >= 2)
            {
                var xPrev = ListNesterov[^2];
                yK = xK + _momentum * (xK - xPrev);
            }
            else
            {
                yK = xK;
            }

            // путь между последними двумя точками
            path = (ListNesterov.Count >= 2) ? ListNesterov[^1].Path(ListNesterov[^2]) : 1.0;

            // защитный выход на случай проблем со сходимостью
            if (ListNesterov.Count > 10000) break;
        }
    }

}