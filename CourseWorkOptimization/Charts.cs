using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using ChartDirector;
using OxyPlot;
using OxyPlot.Series;

namespace CourseWorkOptimization;

public class Charts
{
    private readonly Algorithm _algorithm;
    private readonly WPFChartViewer _viewer3D;
    
    
    public Charts(WPFChartViewer viewer3D)
    {
        _viewer3D = viewer3D;
        _algorithm = new Algorithm();
        _viewer3D.MouseMoveChart += WPFChart3D_MouseMoveChart;
        _viewer3D.ViewPortChanged += WPFChart3D_ViewPortChanged;
        _viewer3D.MouseUp += WPFChart3D_MouseUpChart;
       

    }

    public string GetName()
    {
        return "Contour Chart";
    }

    //Number of charts produced in this demo module
    public int GetNoOfCharts()
    {
        return 1;
    }

    //Main code for creating chart.
    //Note: the argument chartIndex is unused because this demo only has 1 chart.
    public void createChart(WPFChartViewer viewer)
    {
        double[] dataX =
        {
            -3, -2, -1, 0, 1, 2, 3
        };
        double[] dataY =
        {
            -2, -1, 0, 1, 2, 3, 4, 5, 6
        };
            double[] dataZ = new double[dataX.Length * dataY.Length];
            for (int yIndex = 0; yIndex < dataY.Length; ++yIndex) {
                double y = dataY[yIndex];
                for (int xIndex = 0; xIndex < dataX.Length; ++xIndex) {
                    double x = dataX[xIndex];
                    //double t = Math.Pow((x - y), 2) + Math.Pow((x + y - 10), 2) / 9;
                    dataZ[yIndex * dataX.Length + xIndex] = _algorithm.GetS(x, y);
                    //dataZ[yIndex * dataX.Length + xIndex] = t;
                }
            }

            // Create a SurfaceChart object of size 720 x 600 pixels
            SurfaceChart c = new SurfaceChart(600, 600);

            // Set the center of the plot region at (330, 290), and set width x depth x height to
            // 360 x 360 x 270 pixels
            c.setPlotRegion(330, 290, 380, 380, 300);

            // Set the data to use to plot the chart
            c.setData(dataX, dataY, dataZ);

            // Spline interpolate data to a 80 x 80 grid for a smooth surface
            c.setInterpolation(80, 80);

            // Set the view angles
            c.setViewAngle(m_elevationAngle, m_rotationAngle);
            
            // Check if draw frame only during rotation
            if (m_isDragging)
                c.setShadingMode(Chart.RectangularFrame);

            // Add a color axis (the legend) in which the left center is anchored at (660, 270). Set
            // the length to 200 pixels and the labels on the right side.
            c.setColorAxis(650, 270, Chart.Left, 200, Chart.Right);

            // Set the x, y and z axis titles using 10 points Arial Bold font
            c.xAxis().setTitle("X", "Arial Bold", 15);
            c.yAxis().setTitle("Y", "Arial Bold", 15);

            // Set axis label font
            c.xAxis().setLabelStyle("Arial Bold", 10);
            c.yAxis().setLabelStyle("Arial Bold", 10);
            c.zAxis().setLabelStyle("Arial Bold", 10);
            c.colorAxis().setLabelStyle("Arial Bold", 10);
            

            // Output the chart
            viewer.Chart = c;

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("", "",
                "title='<*cdml*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}'");
    }
    
    // 3D view angles
    private double m_elevationAngle = 30;
    private double m_rotationAngle = 45;

    // Keep track of mouse drag
    private int m_lastMouseX = -1;
    private int m_lastMouseY = -1;
    private bool m_isDragging = false;

    private void WPFChart3D_MouseMoveChart(object sender, MouseEventArgs e)
    {
        _viewer3D.updateViewPort(true, false);
        int mouseX = _viewer3D.ChartMouseX;
        int mouseY = _viewer3D.ChartMouseY;

        // Drag occurs if mouse button is down and the mouse is captured by the m_ChartViewer
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            if (m_isDragging)
            {
                m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
                m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
                _viewer3D.updateViewPort(true, false);
            }
                 
            m_lastMouseX = mouseX;
            m_lastMouseY = mouseY;
            m_isDragging = true;
        }
             
             
    }
         
    private void WPFChart3D_ViewPortChanged(object sender, WPFViewPortEventArgs e)
    {
        if (e.NeedUpdateChart)
            createChart((WPFChartViewer)sender);
    }
    private void WPFChart3D_MouseUpChart(object sender, MouseEventArgs e)
    {
        m_isDragging = false;
        _viewer3D.updateViewPort(true, false);
    }
    public void CreateChart(WPFChartViewer viewer)
    {
        // The x and y coordinates of the grid
        double[] dataX =
        {
            -3, -2, -1, 0, 1, 2, 3
        };
        double[] dataY =
        {
            -2, -1, 0, 1, 2, 3, 4, 5, 6
        };

        // The values at the grid points. In this example, we will compute the values using the
        // formula z = x * sin(y) + y * sin(x).
        var dataZ = new double[dataX.Length * dataY.Length];
        for (var yIndex = 0; yIndex < dataY.Length; ++yIndex)
        {
            var y = dataY[yIndex];
            for (var xIndex = 0; xIndex < dataX.Length; ++xIndex)
            {
                var x = dataX[xIndex];
                dataZ[yIndex * dataX.Length + xIndex] = _algorithm.GetS(x, y);
            }
        }

        // Create a XYChart object of size 600 x 500 pixels
        var c = new XYChart(600, 500);

        // Add a title to the chart using 15 points Arial Bold Italic font
        c.addTitle("z = x * sin(y) + y * sin(x)      ", "Arial Bold Italic", 15);

        // Set the plotarea at (75, 40) and of size 400 x 400 pixels. Use semi-transparent black
        // (80000000) dotted lines for both horizontal and vertical grid lines
        c.setPlotArea(75, 40, 400, 400, -1, -1, -1, c.dashLineColor(unchecked((int)0x80000000),
            Chart.DotLine), -1);

        // Set x-axis and y-axis title using 12 points Arial Bold Italic font
        c.xAxis().setTitle("X-Axis Title Place Holder", "Arial Bold Italic", 12);
        c.yAxis().setTitle("Y-Axis Title Place Holder", "Arial Bold Italic", 12);

        // Set x-axis and y-axis labels to use Arial Bold font
        c.xAxis().setLabelStyle("Arial Bold");
        c.yAxis().setLabelStyle("Arial Bold");

        // When auto-scaling, use tick spacing of 40 pixels as a guideline
        c.yAxis().setTickDensity(40);
        c.xAxis().setTickDensity(40);

        // Add a contour layer using the given data
        var layer = c.addContourLayer(dataX, dataY, dataZ);

        // Move the grid lines in front of the contour layer
        c.getPlotArea().moveGridBefore(layer);

        // Add a color axis (the legend) in which the top left corner is anchored at (505, 40).
        // Set the length to 400 pixels and the labels on the right side.
        var cAxis = layer.setColorAxis(505, 40, Chart.TopLeft, 400, Chart.Right);

        // Add a title to the color axis using 12 points Arial Bold Italic font
        cAxis.setTitle("Color Legend Title Place Holder", "Arial Bold Italic", 12);

        // Set color axis labels to use Arial Bold font
        cAxis.setLabelStyle("Arial Bold");
        _algorithm.Calculate();
        _algorithm.Nesterov();
        var addLineLayer = c.addLineLayer(_algorithm.List.Select(tuple => tuple.SecondElement).ToArray());
        addLineLayer.setXData(_algorithm.List.Select(tuple => tuple.FirstElement).ToArray());
        addLineLayer.setLineWidth(2);
        c.getPlotArea().moveGridBefore(addLineLayer);
        var addLineLayer2 = c.addLineLayer(_algorithm.ListNesterov.Select(tuple => tuple.SecondElement).ToArray());
        addLineLayer2.setXData(_algorithm.ListNesterov.Select(tuple => tuple.FirstElement).ToArray());
        addLineLayer2.setLineWidth(2);
        c.getPlotArea().moveGridBefore(addLineLayer2);
        // Output the chart
        viewer.Chart = c;
       

        //include tool tip for the chart
        viewer.ImageMap = c.getHTMLImageMap("clickable", "",
            "title='<*cdml*><*font=bold*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}''");
    }
}