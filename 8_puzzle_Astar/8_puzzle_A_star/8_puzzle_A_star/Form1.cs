using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _8_15_puzzle_A_star
{
    public partial class Form1 : Form
    {
        int[,] Matrix;
        n_puzzle n_puzzle;
        Stack<int[,]> stk;
        Button[,] buttonArray;
        int n;
        int MoveCount = 0;
        int count = 0;
 
        public Form1()
        {
            InitializeComponent();
            Matrix = new int[n, n];
            n_puzzle = new n_puzzle();
            stk = new Stack<int[,]>();
            buttonArray = new Button[n, n];
          
        }

        void LoadPuzzle(int[,] a, Button[,] b)
        {
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(0); j++)
                {
                    if (a[i, j] == 0)
                    {
                        b[i, j].Text = "";
                        b[i, j].BackColor = Color.LightSeaGreen;
                    }
                    else
                    {
                        b[i, j].Text = a[i, j].ToString();
                        b[i, j].BackColor = Color.White;
                    }
                }
        }
        void Init()
        {
            Matrix = n_puzzle.RandomMatrix(n);
            LoadPuzzle(Matrix, buttonArray);
            cbbTocDo.Text = cbbTocDo.Items[0].ToString();


            lbSoLanDiChuyen.Text = "0";
            MoveCount = 0;
            count = 0;
            btnGiai.Enabled = true;
            btnDung.Enabled = false;
            timer1.Enabled = false;
            cbbSize.Enabled = false;
            

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cbbSize.SelectedIndex = 0;
            radioBtnHeuristic1.Checked= true;
            n_puzzle.HeuristicFunction = true;
       
            n = int.Parse(cbbSize.Text);
            InitButtons();

            btnGiai.Enabled = false;
            btnDung.Enabled = false;
        

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (cbbTocDo.Text)
            {
                case "1": timer1.Interval = 2000; break;
                case "2": timer1.Interval = 1200; break;
                case "3": timer1.Interval = 800; break;
                case "4": timer1.Interval = 300; break;
                case "5": timer1.Interval = 100; break;
            }

            int[,] Temp = new int[n, n];

            if (stk.Count != 0)
            {
                Temp = stk.Pop();
                LoadPuzzle(Temp, buttonArray);


                MoveCount++;
                lbSoLanDiChuyen.Text =MoveCount.ToString() + " / " + count.ToString();
                
            }
            else
            {
                timer1.Enabled = false;
                cbbSize.Enabled = true;
                btnChoiMoi.Enabled = true;
            }
                
    

        }

        private async void btnGiai_Click(object sender, EventArgs e)
        {
            btnChoiMoi.Enabled = false;
       
            System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            st.Start();

            // Sử dụng Task.Run để chạy tìm kiếm trong một luồng riêng biệt
            var searchTask = Task.Run(() => n_puzzle.GameSolve(Matrix, n));

            // Wait for the searchTask to complete with a timeout of 5000ms
            await Task.WhenAny(searchTask, Task.Delay(20000));

            // Check if the searchTask completed successfully
            if (searchTask.IsCompleted)
            {
                stk = searchTask.Result;
                stk.Pop();
                st.Stop();
                count = stk.Count;

                if (stk.Count > 0)
                {
                    MessageBox.Show("Tìm thấy lời giải với " + stk.Count + " bước trong thời gian " + st.ElapsedMilliseconds + "ms. Tiếp theo chạy lời giải:");
                    
                    timer1.Enabled = true;
                    btnGiai.Enabled = false;
                    btnDung.Enabled = true;
                    cbbSize.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy lời giải.");
                    btnChoiMoi.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Thời gian tìm kiếm quá lâu > 20000ms.");
                btnChoiMoi.Enabled = true;
            }
        }




        private void btDung_Click(object sender, EventArgs e)
        {
          
            btnDung.Enabled = true;
            btnGiai.Enabled = false;

            if (btnDung.Text=="Tạm dừng")
            {
                btnDung.Text = "Tiếp tục";
                timer1.Enabled = false;

            }
            else if (btnDung.Text=="Tiếp tục")
            {
                btnDung.Text = "Tạm dừng";
                timer1.Enabled = true;
            }
        }

        private void ChoiMoi(object sender, EventArgs e)
        {
            Init();
       
        }

        private void cbbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            n = int.Parse(cbbSize.Text);
            InitButtons();
        }
        private void InitButtons()
        {
            panel3.Controls.Clear();

            // Xác định kích thước của từng ô nút dựa trên panel3 và số lượng ô (n)
            //int buttonSize = panel3.Size.Width / n;
            int buttonSizeW = 200;
            int buttonSizeH = 200;
            if (n==4)
            {
                buttonSizeW = 150;
                buttonSizeH = 150;
            }
  
            int spacing = 5; // Khoảng cách giữa các nút

            int x = 70;
            int y = 15; // Bắt đầu từ vị trí (0, 0)

            // Khởi tạo mảng Mangbt với kích thước n x n
            buttonArray = new Button[n, n];

            // Tạo các nút và sắp xếp chúng trong panel3
            int buttonValue = 1;
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    Button button = new Button();
                    button.Size = new Size(buttonSizeW, buttonSizeH);
                    button.Location = new Point(x, y);
                    button.Font = new Font("Microsoft Sans Serif",n==3? 48F : 30F , FontStyle.Bold, GraphicsUnit.Point, ((byte)(163)));
                    button.BackColor = Color.White;
                    button.ForeColor = SystemColors.MenuHighlight;
                    button.FlatStyle = FlatStyle.Flat;
                    if (buttonValue <= n * n - 1)
                    {
                        button.Text = buttonValue.ToString();
                    }
                    else
                    {
                        button.Text = "";
                        button.BackColor = Color.LightSeaGreen;
                    }
                    // Gán button vào mảng Mangbt
                    buttonArray[row, col] = button;

                    panel3.Controls.Add(button);
                    x += buttonSizeW + spacing; // Thêm khoảng cách vào vị trí x
                    buttonValue++;
                }
                x = 70;
                y += buttonSizeH + spacing; // Thêm khoảng cách vào vị trí y
            }
        }



        private void radioBtnHeuristic1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnHeuristic1.Checked)
            {
                n_puzzle.HeuristicFunction= true;
            }
            else n_puzzle.HeuristicFunction= false;
     
        }

        private void radioBtnHeuristic2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnHeuristic2.Checked)
            {
                n_puzzle.HeuristicFunction = false;
            }
            else n_puzzle.HeuristicFunction = true;
         
        }


    }
}
