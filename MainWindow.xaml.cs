using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace ChM2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<MyTable> result = new List<MyTable>();
            double a = 0, b = 3;
            double h = 0.1;
            double k1, k2, k3, k4;
            int size = (int)((b - a) / h);
            double[] X = new double[size+1];
            double[] Y = new double[size+1];
            double[] Yrk = new double[Y.Length];
            double[] Ya = new double[Y.Length];

            X[0] = 0;
            for (int i = 1; i < X.Length; i++)
            {
                X[i] = X[i - 1] + h;
            }
            /*Поиск точных значений*/
            for (int i = 0; i < X.Length; i++)
            {
                Y[i] = Math.Sin(X[i]);
            }

            /*Метод Рунге-Кутта для y' = sqrt(1-y*y)*/
            for (int i = 0; i < X.Length; i++)
            {
                if (i == 0)
                {
                    Yrk[i] = 0;
                }
                else
                {
                    k1 = Math.Sqrt(1 - Math.Pow(Yrk[i - 1], 2));
                    k2 = Math.Sqrt(1 - Math.Pow(Yrk[i - 1] + h * k1 / 2, 2));
                    k3 = Math.Sqrt(1 - Math.Pow(Yrk[i - 1] + h * k2 / 2, 2));
                    k4 = Math.Sqrt(1 - Math.Pow(Yrk[i - 1] + h * k3, 2));
                    Yrk[i] = Yrk[i - 1] + (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                }
            }

            double[] D = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                D[i] = Math.Abs(Yrk[i] - Y[i]);
                var Dpc = D[i] / (Yrk[i]) * 100;
            }

            /*Метод Адамса для y' = -2*y + 4*x*/
            for (var i = 0; i < X.Length; i++)
            {
                if (i <= 3)
                {
                    Ya[i] = Yrk[i];
                }
                else
                {
                    Ya[i] = Ya[i - 1] + (h / 24) * (9 * (Math.Sqrt(1 - Math.Pow(Y[i], 2))) + 19 * (Math.Sqrt(1 - Math.Pow(Y[i-1], 2))) - 5 * (Math.Sqrt(1 - Math.Pow(Y[i-2], 2))) + (Math.Sqrt(1 - Math.Pow(Y[i-3], 2))));
                }
            }

            double[] Da = new double[X.Length];
            for (var i = 0; i < X.Length; i++)
            {
                Da[i] = Math.Abs(Ya[i] - Y[i]);
                var Dpc = Da[i] / (Ya[i]) * 100;
            }
            
            for(int i = 0; i < X.Length; i++)
            {
                result.Add(new MyTable(X[i], Y[i], Yrk[i], Ya[i], D[i], Da[i]));
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Y",
                    Values = new ChartValues<double>(Y)
                },
                new LineSeries
                {
                    Title = "RK",
                    Values = new ChartValues<double>(Yrk)
                },
                new LineSeries
                {
                    Title = "Adams",
                    Values = new ChartValues<double>(Ya)
                }
            };
            Labels = X.Select(x => x.ToString("F2")).ToArray();
            YFormatter = value => value.ToString("F2");
            DataContext = this;
            grid.ItemsSource = result;

        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


        class MyTable
        {
            public MyTable(double x, double y1, double y2, double y3, double d1, double d2)
            {
                this.X = x.ToString("F3");
                this.Y = y1.ToString("F10");
                this.Yrk = y2.ToString("F10");
                this.Ya = y3.ToString("F10");
                this.D1 = d1.ToString("P3");
                this.D2 = d2.ToString("P3");
            }
            public string X { get; set; }
            public string Y { get; set; }
            public string Yrk { get; set; }
            public string Ya { get; set; }
            public string D1 { get; set; }
            public string D2 { get; set; }
        }


        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            List<MyTable> result = new List<MyTable>();
            double a = Convert.ToDouble(xstart.Text), b = Convert.ToDouble(xfinish.Text);
            double h = Convert.ToDouble(hkrok.Text.ToString());
            double k1, k2, k3, k4;
            int size = (int)((b - a) / h);
            double[] X = new double[size + 1];
            double[] Y = new double[size + 1];
            double[] Yrk = new double[Y.Length];
            double[] Ya = new double[Y.Length];

            X[0] = 0;
            for (int i = 1; i < X.Length; i++)
            {
                X[i] = X[i - 1] + h;
            }
            /*Поиск точных значений*/
            for (int i = 0; i < X.Length; i++)
            {
                Y[i] = Math.Sin(X[i]);
            }

            /*Метод Рунге-Кутта для y' = sqrt(1-y*y)*/
            for (int i = 0; i < X.Length; i++)
            {
                if (i == 0)
                {
                    Yrk[i] = 0;
                }
                else
                {
                    k1 = Math.Sqrt(1 - Math.Pow(Yrk[i - 1], 2));
                    k2 = Math.Sqrt(1 - Math.Pow(Yrk[i - 1] + h * k1 / 2, 2));
                    k3 = Math.Sqrt(1 - Math.Pow(Yrk[i - 1] + h * k2 / 2, 2));
                    k4 = Math.Sqrt(1 - Math.Pow(Yrk[i - 1] + h * k3, 2));
                    Yrk[i] = Yrk[i - 1] + (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                }
            }

            double[] D = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                D[i] = Math.Abs(Yrk[i] - Y[i]);
                var Dpc = D[i] / (Yrk[i]) * 100;
            }

            /*Метод Адамса для y' = -2*y + 4*x*/
            for (var i = 0; i < X.Length; i++)
            {
                if (i <= 3)
                {
                    Ya[i] = Yrk[i];
                }
                else
                {
                    Ya[i] = Ya[i - 1] + (h / 24) * (9 * (Math.Sqrt(1 - Math.Pow(Y[i], 2))) + 19 * (Math.Sqrt(1 - Math.Pow(Y[i - 1], 2))) - 5 * (Math.Sqrt(1 - Math.Pow(Y[i - 2], 2))) + (Math.Sqrt(1 - Math.Pow(Y[i - 3], 2))));
                }
            }

            double[] Da = new double[X.Length];
            for (var i = 0; i < X.Length; i++)
            {
                Da[i] = Math.Abs(Ya[i] - Y[i]);
                var Dpc = Da[i] / (Ya[i]) * 100;
            }

            for (int i = 0; i < X.Length; i++)
            {
                result.Add(new MyTable(X[i], Y[i], Yrk[i], Ya[i], D[i], Da[i]));
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Y",
                    Values = new ChartValues<double>(Y)
                },
                new LineSeries
                {
                    Title = "RK",
                    Values = new ChartValues<double>(Yrk)
                },
                new LineSeries
                {
                    Title = "Adams",
                    Values = new ChartValues<double>(Ya)
                }
            };
            Labels = X.Select(x => x.ToString("F2")).ToArray();
            YFormatter = value => value.ToString("F2");
            DataContext = this;
            grid.ItemsSource = result;
        }

    }
}