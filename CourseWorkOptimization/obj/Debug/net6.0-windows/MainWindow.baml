   M S B A M L   `   `   `  ����i  eChartDirector.Net.Desktop.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=2db5fc05de12e204YGclr-namespace:ChartDirector;assembly=ChartDirector.Net.Desktop.ControlsChartDirector  5   	   Q MCourseWorkOptimization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:CourseWorkOptimizationCourseWorkOptimization 5   
   '   !CourseWorkOptimization.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   achartDirectorGclr-namespace:ChartDirector;assembly=ChartDirector.Net.Desktop.Controls     5   	   R NOxyPlot.Wpf, Version=2.1.2.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0Y UOxyPlot.Wpf.Shared, Version=2.1.2.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0"wpfhttp://oxyplot.org/wpf   5   	   2local$clr-namespace:CourseWorkOptimization   5   
     � Title$  
MainWindow��5
   	   $	��294��6   $	��398��6)    � WindowStartupLocation$ CenterScreen=�65      MyWindowStyle#��[  6Z   .��5      � .+�5   
   ��  	 Title#��[ 6   $0��*Математическая модель:��6/   $
��Left=�6P   $��	19,30,0,0q�5      $	��Top=�6#   �� 5   
   $(��"Входные параметры:��6   $
��Left=�6-   $��
15,105,0,0q�6H   $	��Top=�6\   �� 5   
   $��alpha:��6   $
��Left=�6!   $��
25,134,0,0q�6<   $	��Top=�6P    � ToolTip$4 .Расход реакционной массы��5      $��52��63   �� 5   
   -   5         ��Name$ GTextBox��5      $
��Left=�6$   $��
77,138,0,0q�6?    �� TextWrapping$
 Wrap=�6S   $��2��6g   $	��Top=�5      $��74��6*   o� 5   
   $��28��6   $	��Top=�6   .@�5      m�  � Cursor$
 Hand��6   $'��!Построить графики��6%   .@�5      �� -   5         ��Name$ Create2DChartButton��5      $��2D график��66   6`   �� 5      -   5        $ Create3DChartButton��5      $��3D график��66   6`   5      �� 5      $��О моделе��6   $��Center=�6(   $��Center=�6E   �� 5      -   5        $��
Выйти��5      $��Center=�6%   $��Center=�6B   6]   5      �� 5   
   $��/Image1.png��6   $��35��6%   $	��339��64   $	��339��6C   $��19,62,40,171q�6O   $
}�True��5       J� SnapsToDevicePixels$
 True��6"    �� StretchDirection$ DownOnly=�5      5      5       