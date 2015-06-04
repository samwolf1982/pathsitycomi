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
        Dictionary<Line, double> arrLine = new Dictionary< Line, double>();

        //случайная подстановка для цикла гамильтона
        List<int> arrRandom = new List<int>();
        List<int> nextarrRandom = new List<int>();
       float t = 100;     // верхняя грань  для функции всегда декремент 

        double mainValue = 0,tempNumber=0;
        public MainWindow()
        {
 
            InitializeComponent();
            ///start point
            arrPoints.Add(createPoint());
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0,5);
            //dispatcherTimer.Start();

        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (t <= 0) dispatcherTimer.Stop();
           t = t - 0.1f;
                String str2 = "";
                //FlowDocument flowDoc2 = new FlowDocument(new Paragraph(new Run(str2)));
                //   textik.Document.Blocks.Add(new Paragraph(new Run(str2)));
                // независимый генератор
                double d2 = randFill(ref nextarrRandom);
                str2 = "MainValue:  " + mainValue.ToString() + "  " + "newVal:  " + d2.ToString() + " t= " + t.ToString();
                // flowDoc2 = new FlowDocument(new Paragraph(new Run(str2 + d.ToString())));

                //var sd = textik.Document.Blocks;
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

        private PointF createPoint()
        {
            return new PointF(rand.Next(10, 200), rand.Next(10, 200));
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
        /// добавляет новую точку в массив
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < 50; i++)
            {
                 arrPoints.Add(createPoint());
            }

           
        }
        /// <summary>
        /// test button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void test_Click_1(object sender, RoutedEventArgs e)
        {


            for (int i = 0; i < arrPoints.Count-1; i++)
            {
                for (int j = i+1; j < arrPoints.Count; j++)
                {
                    Line l = createLine(i, j);
                    double d= showLebgth(i, j);
                    //все ветки графа (вес ето длина)
                    arrLine.Add(l, d);
                    pic.Children.Add(l);
                      

                }
            }

            // первое случайное  заполнение  // генератор для начального маршта
            mainValue = randFill(ref arrRandom);
            //       doubled = 99;

           // String str2 = "Общая цена:  " + mainValue.ToString();
                                            //FlowDocument flowDoc2 = new FlowDocument(new Paragraph(new Run(str2)));
         //   textik.Document.Blocks.Add(new Paragraph(new Run(str2)));

           
        }
        /// <summary>
        /// отбражение длины в тексковаом поле между точками
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="str"></param>
        /// <returns>длина между точками</returns>
        private double showLebgth(int a,int b,string str = "")
        {
            double l=lengt(a, b);
            //String str2 = "Длина от "+arrPoints[a].ToString()+" "+arrPoints[b].ToString()+":  "+l.ToString();
            //FlowDocument flowDoc2 = new FlowDocument(new Paragraph(new Run(str2)));          
          //  textik.Document.Blocks.Add(new Paragraph(new Run(str2)));
            return l;

        }

 
        /// <summary>
        /// clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear_Click_1(object sender, RoutedEventArgs e)
        {
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
                if (t10.IsSelected)
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




        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        /// <summary>
        /// случайное заполнение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void randFill_Click(object sender, RoutedEventArgs e)
        {

        //double d1=    randFill(ref arrRandom);
     //       doubled = 99;

            dispatcherTimer.Start();
// MessageBox.Show("finish");
     

          
          


          
            //for (int i = 0; i < sd.Count; i++)
            //{
            //    textik.Document.Blocks.Add(sd.);
            //}
          //  textik.Document.b

          

      
                    

        }



    }

}
