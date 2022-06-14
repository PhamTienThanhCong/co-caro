using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace Caro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Biến kiểm tra xem đi hết bàn cờ chưa
        int SumCount = 0;

        int[,] a;
        Button[,] Button;
        int Cols = 10;
        int Rows = 10;
        string player1 = "";
        string player2 = "";

        public MainWindow(string play1, string play2)
        {
            InitializeComponent();
            player1 = play1;
            player2 = play2;
        }
        
        //private object uiCanVas;
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;

            int btnWidth = 40;
            int btnHeigh = 40;

            a = new int[Cols, Rows];
            Button = new Button[Cols,Rows];

            // tạo bàn cờ và lắng nghe sư kiện
            for(int i=0;i<Rows;i++)
            {
                for(int j=0;j<Cols;j++)
                {
                    Button[i,j] = new Button();
                    Button[i, j].Height = btnHeigh;
                    Button[i, j].Width = btnWidth;
                    Button[i, j].Background = Brushes.White;
                    Button[i, j].FontSize = 20;

                    Button[i, j].Tag = new Tuple<int, int>(i, j);
                    Button[i, j].Click += BtnClick;
                    
                    // thêm các ô nút lên giao diện
                    Canvas.Children.Add(Button[i, j]);
                    Canvas.SetLeft(Button[i,j],80+ j * btnWidth);
                    Canvas.SetTop(Button[i, j],80+ i * btnHeigh);
                }
            }
        }

        
        bool Xturn = true;

        /*click vào các ô giao diện*/
        private void BtnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var tuple = btn.Tag as Tuple<int,int>;

            int i = tuple.Item1;
            int j = tuple.Item2;

            //MessageBox.Show($"{i};{j}");
            if(a[i,j]==0)
            {
                SumCount++;
                if (Xturn)
                {
                    btn.Content = "X";               
                    btn.Foreground = Brushes.Red;
                    a[i, j] = 1;
                }
                else
                {
                    btn.Content = "O";                  
                    btn.Foreground = Brushes.Green;
                    a[i, j] = 2;
                }
                Xturn = !Xturn;

                //kiểm tra thắng thua
                var kt = CheckWin(a, i, j);

                if (kt == 1)
                {
                    MessageBox.Show(player1 + " Won!");
                    Reset();
                }
                if (kt == 2)
                {
                    MessageBox.Show(player2 + " Won!");
                    Reset();
                }

                if(SumCount==Cols*Rows)
                {
                    MessageBox.Show(player1 + " Và " + player2 + " Hòa nhau!");
                    Reset();
                }

                countTime = 10;
                SetTurn();
            }

           
        }

        public void SetTurn()
        {
            if (Xturn)
            {
                turn.Content = "X";
                turn.Foreground = Brushes.Red;
                turnPlayer.Content = player1;
            }
            else
            {
                turn.Content = "O";
                turn.Foreground = Brushes.Green;
                turnPlayer.Content = player2;
            }
        }

        private int CheckWin(int[,] a, int i, int j)
        {
            const int conditionWin = 5;
            int count;
            int di, dj;
            //---------------load theo chiều ngang----------------
            count = 1;
            //load bên trái
            di = 0;
            dj = -1;
            count += Load(di, dj, i, j);
            //Load bên phải
            di = 0;
            dj = 1;
            count += Load(di, dj, i, j);
            
            if(count>=conditionWin)
            {
                return a[i, j];
            }

            //---------------Load theo chiều dọc----------------
            count = 1;
            //Load bên trên
            di = -1;
            dj = 0;
            count += Load(di, dj, i, j);
            //Load bên dưới
            di = 1;
            dj = 0;
            count += Load(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }


            //---------------Load theo đường chéo chính----------------
            count = 1;
            //Load bên trên
            di = -1;
            dj = -1;
            count += Load(di, dj, i, j);
            //Load bên dưới
            di = 1;
            dj = 1;
            count += Load(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }

            //---------------Load theo đường chéo phụ----------------
            count = 1;
            //Load bên trên
            di = -1;
            dj = 1;
            count += Load(di, dj, i, j);
            //Load bên dưới
            di = 1;
            dj = -1;
            count += Load(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }


            return 0;
        }

        /* Hàm load các đường từ vị trí */
        int Load(int di, int dj, int i, int j)
        {
            int count = 0;
            int StartI = i;
            int StartJ = j;

            while (true)
            {
                j += dj;
                i += di;
                if(i>9||i<0||j>9||j<0)
                {
                    break;
                }
                //nếu khác giá trị với start thì không cộng nửa
                if (a[StartI, StartJ] != a[i, j])
                {
                    break;
                }
                else
                    count++;
            }
            return count;
        }

        void Reset()
        {
            for(int i=0;i<Rows;i++)
            {
                for(int j=0;j<Cols;j++)
                {
                    a[i, j] = 0;
                    Button[i, j].Content = "";
                }
            }
            Xturn = true;
            SetTurn();
            countTime = 10;
            time.Text = "10";
            SumCount = 0;
            time.Foreground = Brushes.Blue;
            dt.Stop();
        }

        DispatcherTimer dt = new DispatcherTimer();
        int countTime = 10;
        private void dtTicker(object sender, EventArgs e)
        {
            
            countTime--;
            if(countTime<=5)
            {
                time.Foreground = Brushes.Red;
            }
            else
            {
                time.Foreground = Brushes.Blue;
            }
            time.Text = (countTime).ToString();
            if (countTime==0)
            {
                if (Xturn)
                {
                    MessageBox.Show("O Won!");
                    Reset();
                }
                else
                {
                    MessageBox.Show("X Won!");
                    Reset();
                }
            }
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void TimeOut_Click(object sender, RoutedEventArgs e)
        {
            btn_starttimeOut.Content = "Start With Time Out";
            dt.Start();
        }
    }
}
