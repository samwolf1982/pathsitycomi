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
using System.Drawing.Drawing2D;
using System.Drawing;
using MoreLinq;
using System.Threading;


namespace twelve
{
      
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


     System.Windows.Threading.DispatcherTimer   dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
           

     Random rand = new Random();
        /// <summary>
        /// нечетное количество точек пока
        /// </summary>
        List<PointF> arrPoints = new List<PointF>();
       

        //случайная подстановка для цикла гамильтона
        List<int> arrStartPosition = new List<int>();
        List<int> arrRandom = new List<int>();
        List<int> nextarrRandom = new List<int>();
       float t = 100;     // верхняя грань  для функции всегда декремент 

        double mainValue = 0,tempNumber=0,mainvalueBackUp=0;
        public MainWindow()
        {
 
            InitializeComponent();
            arrPoints.Add(createPoint());
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,5);

        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (t <= 0) dispatcherTimer.Stop();
            t = t - (float)(1 - (0.7 / 0.99));
                String str2 = "";
                // независимый генератор
                double d2 = randFill(ref nextarrRandom);
                str2 = "Текущее значение:  " + mainValue.ToString() + "  " + "\nНовое значение:  " + d2.ToString() + "\n t= " + t.ToString();

                textik.Document.Blocks.Clear();
                textik.Document.Blocks.Add(new Paragraph(new Run(str2)));

                // проверка на то кто больше c предыдущим
                if (mainValue < d2) // если путь правильный идем дальше в глубь
                {
                    //   mainValue = d2;
                    //  return;

                }
                else    // а вдруг он хороший? )) вычисление критической величины и сравнивание ёё с случайной величиной
                {
                    // запоминаем текущее состояние 
                    double P = 0; // критическая величина
                    if ((mainValue - d2) > 0)
                    {
                        P = 100 * Math.Pow(Math.E, -(mainValue - d2) / t);
                        // третий герератор для сравнивания результата с порогом // случайное число от некой границы в даном случае 1-100
                        int temp = rand.Next(1, 100);
                        if (P > temp) //  сделать анализ!!!!!!! (на ранних етапах возможность выбрать плохое решение всегда выше !!!) зависимоть от знаменателя !!
                        {
                            tempNumber = mainValue;
                            //////////////////////////////
                            arrRandom = nextarrRandom;
                           //////////////////////////
                            mainValue = d2;
                            System.Diagnostics.Debug.WriteLine("main=d  M=" + mainValue);

                        }
                        else
                        {
                            if (tempNumber != 0)
                                mainValue = tempNumber;

                            System.Diagnostics.Debug.WriteLine("back ");
                        }

                    }


                }

           // show();
                pic.Children.Clear();

            showMAinPath();



            
        }

        /// <summary>
        /// случайный маршрут
        /// </summary>
        /// <returns></returns>
        private double randFill(ref List<int> arr )
        {
            //случайная длина
        // cлучайно заполняем массив спереди и сзади точка начала и конца (0 12468975 3 0)
            arr = Enumerable.Range(1,arrPoints.Count-1).ToArray().OrderBy(x => rand.Next()).ToList();
            arr.Insert(0, 0);
            arr.Add(0);
            // подсчет суммы
            double sum = 0;
            for (int i = 0;  i < arrPoints.Count-1; i++)
            {
                sum += lengt(arr[i], arr[i+1]);
            }
            // линия возврата
            sum += lengt(arr[arr.Count-1], arr[0]);
            return sum;
        }



 
        /// <summary>
        /// линия из двух точек
        /// </summary>
        /// <param name="begin">первая точка</param>
        /// <param name="end">вторая точка</param>
        /// <returns></returns>
        private Line createLine(int begin,int end)
        { 
            Line  line = new Line();
            line.Stroke = System.Windows.Media.Brushes.Black;
            line.X1 = arrPoints[begin].X;
            line.X2 = arrPoints[end].X;
            line.Y1 = arrPoints[begin].Y;
            line.Y2 = arrPoints[end].Y;

            line.StrokeThickness = 2;
            return line;

        }

        /// <summary>
        /// возврат елипса
        /// </summary>
        /// <param name="begin">первая точка</param>
        /// <param name="end">вторая точка</param>
        /// <returns></returns>
        private Ellipse createCirkle(int begin)
        {

            SolidColorBrush fillBrush = new SolidColorBrush() { Color = Colors.Red };
            SolidColorBrush borderBrush = new SolidColorBrush() { Color = Colors.Black };
           Ellipse e=new  Ellipse()
            {
                Height = 10,
                Width = 10,
                StrokeThickness = 1,
                Stroke = borderBrush,
                Fill = fillBrush
            };
           //Canvas.SetLeft(e, 10);
         //  Canvas.SetTop(e,10);
           Canvas.SetLeft(e,  arrPoints[begin].X-5);
           Canvas.SetTop(e, arrPoints[begin].Y-5);
           return e;

        }

        private PointF createPoint()
        {
            return new PointF(rand.Next(10, 400), rand.Next(10, 400));
        }
/// <summary>
        ///  растояние между двумя  точками
/// </summary>
/// <param name="begin">первая</param>
/// <param name="end">вторая</param>
/// <returns></returns>
        private double lengt(int begin,int end)
        {
            PointF x = arrPoints[begin];
            PointF y = arrPoints[end];
           return Math.Sqrt( Math.Pow( (y.X-x.X),2)+Math.Pow( (y.Y-x.Y),2));
        }
        /// <summary>
        /// добавляет новые точку в массив
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_Click(object sender, RoutedEventArgs e)
        {

            /// только первый раз инициализация
            if (arrPoints.Count == 1 || arrPoints.Count == 0)
            {
                // количество точек
                for (int i = 0; i < 18; i++)
                {

                    arrPoints.Add(createPoint());
                }
            }

            /// только первый раз инициализация
            /// 
            //первя случайная инициализация

            if (arrStartPosition.Count == 0)
            {
              mainvalueBackUp=  randFill(ref arrStartPosition);
                // cоздание копии
                arrRandom = arrStartPosition;
                mainValue = mainvalueBackUp;
                t = 100;
            }
            else
            {
                arrRandom = arrStartPosition;
                mainValue = mainvalueBackUp;
                t = 100;
                nextarrRandom.Clear();
            }
            
          //  mainValue = randFill(ref arrRandom);
            if(dispatcherTimer.IsEnabled==false) dispatcherTimer.Start();
           
           
        }

    

 
        /// <summary>
        /// clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear_Click_1(object sender, RoutedEventArgs e)
        {
            if (dispatcherTimer.IsEnabled == true) dispatcherTimer.Stop();
           // arrLine.Clear();
            arrRandom.Clear();
            nextarrRandom.Clear();
            arrStartPosition.Clear();
           
            textik.Document.Blocks.Clear();
            arrPoints.Clear();
            pic.Children.Clear();

        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (e.Source is TabControl) //if this event fired from TabControl then enter
            {
                if (t1.IsSelected)
                {
                    //if ( FillerX == null ||    FillerX.mainList.Count == 0) return;
                    //curentList = selectfun(1);
                    //curentindex = 0;
                    ////Do your job here
                    //System.Diagnostics.Debug.WriteLine("Tab,change T1");
                    //// инфо про фигурку

                    // curentList = selectfun(1);
                }
  


            }





        }




   

        /// <summary>
        /// рисует все точки и линии по заданому алгоритму из массива  ArrRandom (cамий опитмальный на текущий момент)
        /// </summary>
        private void showMAinPath()
        {
            
            foreach (var item in arrRandom)
            {
                if (item == 0) { Ellipse el = createCirkle(item);
                SolidColorBrush fillBrush = new SolidColorBrush() { Color = Colors.Yellow};
                el.Fill = fillBrush;
                    
                    pic.Children.Add(el); }
                else
                {
                    pic.Children.Add(createCirkle(item));
                }
            }
            for (int i = 0; i < arrRandom.Count - 1; i++)
            {
                Line l = createLine(arrRandom[i], arrRandom[i + 1]);
                pic.Children.Add(l);
            }
            Line l2 = createLine(arrRandom[arrRandom.Count - 1], arrRandom[0]);

            pic.Children.Add(l2);
        }
        private void showPointsArr()
        {
            
    SolidColorBrush blueBrush = new SolidColorBrush();
    blueBrush.Color = Colors.Blue;
    SolidColorBrush blackBrush = new SolidColorBrush();

            //foreach (var item in arrRandom)
            //{
                   
            //}

    
    blackBrush.Color = Colors.Black;
            Ellipse el = new Ellipse();
            el.Width = 200;
            el.Height = 100;

            pic.Children.Add(el);
        }

    }

}
