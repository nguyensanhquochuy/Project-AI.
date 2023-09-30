using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_15_puzzle_A_star
{
    public class Node
    {

        public int[,] puzzleMatrix; // mảng 2 chiều lưu trữ ma trận
        public int heuristicCost;   // //ước lượng chi phí đến đích
        public int index;           // chỉ số của node
        public int parent;          // cha của node, dùng để truy vết kết quả
        public int fn;          // chi phí đi đến node đó   
        
    }
    public class n_puzzle
    {
        private int Index = 0;//chỉ số của Node sẽ tăng sau mỗi lần sinh ra 1 node
        private int fn = 0;// Sau mỗi lần sinh ra các node thì chi phí các node tăng 1 đơn vị,
                           // tức node con sẽ có chi phí lớn hơn node cha 1 đơn vị
        private bool heuristicFunction; // dùng để lựa chọn hàm Heuristic
        public bool HeuristicFunction
        {
            get { return heuristicFunction; }
            set { heuristicFunction = value; }
        }

        public Stack<int[,]> GameSolve(int[,] Matrix, int n)
        {

            Stack<int[,]> stkResult = new Stack<int[,]>();

            List<Node> Close = new List<Node>();
            List<Node> Open = new List<Node>();

            //khai báo và khởi tạo cho node đầu tiên
            Node Node = new Node();
            Node.puzzleMatrix = Matrix;
 
            Node.heuristicCost = heuristicFunction==true ? CountMisplacedPieces(Matrix) : CalculateTotalManhattanDistance(Matrix);


            Node.index = 0;
            Node.parent = -1;
            Node.fn = 0;
            //cho trạng thái đầu tiên vào Open;
            Open.Add(Node);

            int t = 0;
            while (Open.Count != 0)
            {
                #region chọn node tốt nhất trong tập Open và chuyển nó sang Close
                Node = new Node();
                Node = Open[t];
                Open.Remove(Node);
                Close.Add(Node);
                #endregion

                //nếu node có chi phí ước lượng là 0, tức là đích thì thoát
      
                if (Node.heuristicCost == 0) break;
                else
                {
                    //sinh hướng đi của node hiện tại
                    List<Node> lstHuongDi = new List<Node>();
                    lstHuongDi = GenerateMoves(Node);

                    for (int i = 0; i < lstHuongDi.Count; i++)
                    {
                        //hướng đi không thuộc Open và Close
                        if (!IsDuplicateNode(lstHuongDi[i], Open) && !IsDuplicateNode(lstHuongDi[i], Close))
                        {
                            Open.Add(lstHuongDi[i]);
                        }
                        else
                        {   //nếu hướng đi thuộc Open
                            if (IsDuplicateNode(lstHuongDi[i], Open))
                            {
                                /*nếu hướng đi đó tốt hơn thì sẽ được cập nhật lại, 
                                ngược lại thì sẽ không cập nhật*/
                                IsBetterCost(lstHuongDi[i], Open);
                            }
                            else
                            {
                                //nếu hướng đi thuộc Close
                                if (IsDuplicateNode(lstHuongDi[i], Close))
                                {
                                    /*nếu hướng đi đó tốt hơn thì sẽ được cập nhật lại, 
                                    ngược lại thì sẽ không cập nhật và chuyển từ Close sang Open*/
                                    if (IsBetterCost(lstHuongDi[i], Close))
                                    {
                                        Node temp = new Node();
                                        temp = GetDuplicateNodeFromClose(lstHuongDi[i], Close);
                                        Close.Remove(temp);
                                        Open.Add(temp);
                                    }
                                }
                            }
                        }

                    }

                    //chọn vị trí có phí tốt nhất trong Open
                    t = GetBestPositionInOpen(Open);
                }

            }

            //truy vết kết quả trong tập Close
            stkResult = RetrieveSolutionPath(Close);

            return stkResult;
        }

        //truy vết kết quả đường đi trong tập Close
        Stack<int[,]> RetrieveSolutionPath(List<Node> Close)
        {
            Stack<int[,]> solutionPath = new Stack<int[,]>();

            int t = Close[Close.Count - 1].parent;
            Node temp = new Node();
            solutionPath.Push(Close[Close.Count - 1].puzzleMatrix);

            while (t != -1)
            {
                for (int i = 0; i < Close.Count; i++)
                {
                    if (t == Close[i].index)
                    {
                        temp = Close[i];
                        break;
                    }
                }

                solutionPath.Push(temp.puzzleMatrix);
                t = temp.parent;
            }

            return solutionPath;
        }

        /// <summary>
        /// hàm sinh ra các hướng đi từ một node sinh ra các node con
        /// </summary>
        /// <param name="node">node Cha</param>
        /// <returns>danh sách các hướng đi</returns>
        List<Node> GenerateMoves(Node node)
        {
            int n = node.puzzleMatrix.GetLength(0);//lấy số hàng của ma trận

            List<Node> listMoves = new List<Node>();

            #region  Xác định vị trí mảnh trống, có giá trị là 0
            int h = 0;
            int c = 0;
            bool ok = false;
            for (h = 0; h < n; h++)
            {
                for (c = 0; c < n; c++)
                    if (node.puzzleMatrix[h, c] == 0)
                    {
                        ok = true;
                        break;
                    }

                if (ok) break;
            }

            #endregion


            Node Temp = new Node();
            Temp.puzzleMatrix = new int[n, n];
            //Copy mảng Ma trận sang mảng ma trận tạm
            Array.Copy(node.puzzleMatrix, Temp.puzzleMatrix, node.puzzleMatrix.Length);

            fn++;// tăng chi phí của node con lên 1 đơn vị

            #region Xét các hướng đi theo 4 hướng: trên, dưới, phải, trái 
            //xét hàng ngang bắt đầu từ hàng thứ 2 trở đi
            if (h > 0 && h <= n - 1)
            {
                // thay đổi hướng đi của ma trận
                Temp.puzzleMatrix[h, c] = Temp.puzzleMatrix[h - 1, c];
                Temp.puzzleMatrix[h - 1, c] = 0;

                //cập nhật lại thông số của node
                Temp.heuristicCost = heuristicFunction==true ? CountMisplacedPieces(Temp.puzzleMatrix) : CalculateTotalManhattanDistance(Temp.puzzleMatrix);
                Index++;
                Temp.index = Index;
                Temp.parent = node.index;
                Temp.fn = fn + Temp.heuristicCost;
                listMoves.Add(Temp);

                //sau khi thay đổi ma trận thì copy lại ma trận cha cho MaTran để xét trường hợp tiếp theo
                Temp = new Node();
                Temp.puzzleMatrix = new int[n, n];
                Array.Copy(node.puzzleMatrix, Temp.puzzleMatrix, node.puzzleMatrix.Length);
            }
            //xét hàng ngang bắt đầu từ hàng thứ cuối cùng - 1 trở xuống
            if (h < n - 1 && h >= 0)
            {
                // thay đổi hướng đi của ma trận
                Temp.puzzleMatrix[h, c] = Temp.puzzleMatrix[h + 1, c];
                Temp.puzzleMatrix[h + 1, c] = 0;

                //cập nhật lại thông số của node
                Temp.heuristicCost = heuristicFunction == true ? CountMisplacedPieces(Temp.puzzleMatrix) : CalculateTotalManhattanDistance(Temp.puzzleMatrix);
                Index++;
                Temp.index = Index;
                Temp.parent = node.index;
                Temp.fn = fn + Temp.heuristicCost;
                listMoves.Add(Temp);

                //sau khi thay đổi ma trận thì copy lại ma trận cha cho MaTran để xét trường hợp tiếp theo
                Temp = new Node();
                Temp.puzzleMatrix = new int[n, n];
                Array.Copy(node.puzzleMatrix, Temp.puzzleMatrix, node.puzzleMatrix.Length);
            }
            //Xét cột dọc bắt đầu từ cột thứ 2 trở đi
            if (c > 0 && c <= n - 1)
            {
                // thay đổi hướng đi của ma trận
                Temp.puzzleMatrix[h, c] = Temp.puzzleMatrix[h, c - 1];
                Temp.puzzleMatrix[h, c - 1] = 0;

                //cập nhật lại thông số của node
                Temp.heuristicCost = heuristicFunction == true ? CountMisplacedPieces(Temp.puzzleMatrix) : CalculateTotalManhattanDistance(Temp.puzzleMatrix);
                Index++;
                Temp.index = Index;
                Temp.parent = node.index;
                Temp.fn = fn + Temp.heuristicCost;
                listMoves.Add(Temp);

                //sau khi thay đổi ma trận thì copy lại ma trận cha cho MaTran để xét trường hợp tiếp theo
                Temp = new Node();
                Temp.puzzleMatrix = new int[n, n];
                Array.Copy(node.puzzleMatrix, Temp.puzzleMatrix, node.puzzleMatrix.Length);
            }
            //Xét cột dọc bắt đầu từ cột cuối cùng -1 trở xuống
            if (c < n - 1 && c >= 0)
            {
                // thay đổi hướng đi của ma trận
                Temp.puzzleMatrix[h, c] = Temp.puzzleMatrix[h, c + 1];
                Temp.puzzleMatrix[h, c + 1] = 0;

                //cập nhật lại thông số của node
                Temp.heuristicCost = heuristicFunction == true ? CountMisplacedPieces(Temp.puzzleMatrix) : CalculateTotalManhattanDistance(Temp.puzzleMatrix);
                Index++;
                Temp.index = Index;
                Temp.parent = node.index;
                Temp.fn = fn + Temp.heuristicCost;
                listMoves.Add(Temp);

                //đến đây đã xết hết hướng đi nên không cần copy lại ma trận
            }
            #endregion

            return listMoves;
        }

        /// <summary>
        /// Chọn vị trí có chi phí tốt nhất trong Open
        /// </summary>
        /// <param name="Open">Tập Open</param>
        /// <returns>Vị trí tốt nhất</returns>
        int GetBestPositionInOpen(List<Node> Open)
        {
            if (Open.Count != 0)
            {
                Node min = new Node();
                min = Open[0];
                int vt = 0;

                for (int i = 1; i < Open.Count; i++)
                    if (min.heuristicCost > Open[i].heuristicCost)
                    {
                        min = Open[i];
                        vt = i;
                    }
                    else
                    {
                        if (min.heuristicCost == Open[i].heuristicCost)
                        {
                            if (min.fn > Open[i].fn)
                            {
                                min = Open[i];
                                vt = i;
                            }
                        }
                    }
                return vt;
            }

            return 0;
        }

        /// <summary>
        /// So sánh chi phí của hai node
        /// </summary>
        /// <param name="node">Node cần so sánh</param>
        /// <param name="listNode">Tập Open hoặc Close</param>
        /// <returns>trả về true nếu tốt hơn và cập nhật lại cha và chi phí cho node, ngược lại không làm gì và trả về false </returns>
        bool IsBetterCost(Node node, List<Node> listNode)
        {
            for (int i = 0; i < listNode.Count; i++)
                if (AreMatricesEqual(node.puzzleMatrix, listNode[i].puzzleMatrix))
                {
                    if (node.fn < listNode[i].fn)
                    {
                        //vì 2 ma trận bằng nhau lên số mảnh sai vi trị là như nhau lên ta không cần cập nhật
                        listNode[i].parent = node.parent;// cập nhật lại cha của hướng đi
                        listNode[i].fn = node.fn;// cập nhật lại chi phí đường đi

                        return true;
                    }
                    else return false;
                }

            return false;
        }

        /// <summary>
        /// Lấy ra node bị trùng trong tập Close
        /// </summary>
        /// <param name="node">node trùng</param>
        /// <param name="listNode">tập Close</param>
        /// <returns>Trả về node bị trùng</returns>
        Node GetDuplicateNodeFromClose(Node node, List<Node> listNode)
        {
            Node DuplicateNode = new Node();
            for (int i = 0; i < listNode.Count; i++)
                if (AreMatricesEqual(node.puzzleMatrix, listNode[i].puzzleMatrix))
                {
                    DuplicateNode = listNode[i];
                    break;
                }
            return DuplicateNode;
        }

        /// <summary>
        /// So sánh node này có trùng với 1 node trong danh sách các node khác không
        /// </summary>
        /// <param name="node">node cần so sánh</param>
        /// <param name="listNode">dánh sách các node cần so sánh</param>
        /// <returns>Trả về true nếu trùng, ngược lại trả về false </returns>
        bool IsDuplicateNode(Node node, List<Node> listNode)
        {
            for (int i = 0; i < listNode.Count; i++)
                if (AreMatricesEqual(listNode[i].puzzleMatrix, node.puzzleMatrix))
                    return true;

            return false;
        }

        /// <summary>
        /// So sánh hai ma trận có các phần tử bằng nhau hay không
        /// </summary>
        /// <param name="a">Ma trận 1</param>
        /// <param name="b">Ma trận 2</param>
        /// <returns>Trả về true nếu bằng nhau ngược lại trả về false</returns>
        bool AreMatricesEqual(int[,] a, int[,] b)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                    if (a[i, j] != b[i, j])
                        return false;
            }

            return true;
        }

        /// <summary>
        /// trả về số mảnh nằm sai vị trí
        /// </summary>
        /// <param name="matrix">Ma trận</param>
        /// <returns>Trả về số miếng sai vị trí</returns>
        public int CountMisplacedPieces(int[,] matrix)
        {
 
            int misplacedPieces = 0;
            int[,] targetMatrix = new int[,]
            {
                {1,2,3},
                {4,5,6},
                {7,8,0},
            };
            int n = matrix.GetLength(0);
            if (n == 4)
            {
                targetMatrix = new int[,]
                {
                    {1,2,3,4},
                    {5,6,7,8},
                    {9,10,11,12},
                    {13,14,15,0},

                };
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {

                    if (targetMatrix[i, j] != matrix[i, j] && matrix[i, j] != 0)
                    {
                        misplacedPieces++;
                    }
                }
            }

            return misplacedPieces;
        }

        // hàm Heuristic tính tổng khoảng cách sai vị trí của từng miếng
        public int CalculateTotalManhattanDistance(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int totalDistance = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int value = matrix[i, j];
                    if (value != 0)
                    {
                        // Tính toán vị trí đích của miếng
                        int targetRow = (value - 1) / n;
                        int targetCol = (value - 1) % n;

                        // Tính khoảng cách sai vị trí
                        totalDistance += Math.Abs(i - targetRow) + Math.Abs(j - targetCol);
                        
                    }
                }
            }
            return totalDistance;
        }

        // sinh một ma trận ngẫu nhiên để làm node bắt đầu
        public int[,] RandomMatrix(int size)
        {

            int[,] Matrix = new int[size, size];
            int k = 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (k <= size * size - 1)
                        Matrix[i, j] = k;
                    else Matrix[i, j] = 0;
                    k++;
                }
            }


            //tập Close lưu lại các hướng đã đi để đảm bảo sinh ra hướng đi mới không trùng lặp
            List<int[,]> Close = new List<int[,]>();

            int n = Matrix.GetLength(0);

            int[,] Temp = new int[n, n];
            Array.Copy(Matrix, Temp, Matrix.Length);
            Close.Add(Temp);
            int h = n - 1, c = n - 1;

            Random rd = new Random();

            int m = rd.Next(50, 200);//lấy số lần lặp sinh hướng đi
            int t = rd.Next(1, 5);// t=[1...4] tương ứng với 4 hướng đi

            //số lần lặp được lấy random từ đó số lượng hướng đi sẽ thay đổi theo
            for (int r = 0; r < m; r++)
            {
                // vì t được lấy random nên hướng đi sẽ ngẫu nhiên, có thể lên, xuống, trái, phải tùy vào biến t

                //đi lên trên với t =1
                if (h > 0 && h <= n - 1 && t == 1)
                {
                    Matrix[h, c] = Matrix[h - 1, c];
                    Matrix[h - 1, c] = 0;

                    if (!IsMatrixInCloseList(Matrix, Close))
                    {
                        h--;
                        Temp = new int[n, n];
                        Array.Copy(Matrix, Temp, Matrix.Length);
                        Close.Add(Temp);
                    }
                    else
                    {
                        Matrix[h - 1, c] = Matrix[h, c];
                        Matrix[h, c] = 0;
                    }

                }

                t = rd.Next(1, 5);

                //đi sang trái với t=2
                if (c > 0 && c <= n - 1 && t == 2)
                {
                    Matrix[h, c] = Matrix[h, c - 1];
                    Matrix[h, c - 1] = 0;

                    if (!IsMatrixInCloseList(Matrix, Close))
                    {
                        c--;
                        Temp = new int[n, n];
                        Array.Copy(Matrix, Temp, Matrix.Length);
                        Close.Add(Temp);
                    }
                    else
                    {
                        Matrix[h, c - 1] = Matrix[h, c];
                        Matrix[h, c] = 0;
                    }
                }

                t = rd.Next(1, 5);

                //đi xuống giưới với t=3
                if (h < n - 1 && h >= 0 && t == 3)
                {
                    Matrix[h, c] = Matrix[h + 1, c];
                    Matrix[h + 1, c] = 0;

                    if (!IsMatrixInCloseList(Matrix, Close))
                    {
                        h++;
                        Temp = new int[n, n];
                        Array.Copy(Matrix, Temp, Matrix.Length);
                        Close.Add(Temp);
                    }
                    else
                    {
                        Matrix[h + 1, c] = Matrix[h, c];
                        Matrix[h, c] = 0;
                    }

                }

                t = rd.Next(1, 5);

                //đi sang phải với t = 4
                if (c < n - 1 && c >= 0 && t == 4)
                {
                    Matrix[h, c] = Matrix[h, c + 1];
                    Matrix[h, c + 1] = 0;

                    if (!IsMatrixInCloseList(Matrix, Close))
                    {
                        c++;
                        Temp = new int[n, n];
                        Array.Copy(Matrix, Temp, Matrix.Length);
                        Close.Add(Temp);
                    }
                    else
                    {
                        Matrix[h, c + 1] = Matrix[h, c];
                        Matrix[h, c] = 0;
                    }
                }

            }

            // trả về hướng đi cuối dùng trong danh sách hướng đi
            return Close[Close.Count - 1];
        }

        //So sánh nếu ma trận a đã có trang danh sách Close  thì trả về true ngược lại trả về false
        bool IsMatrixInCloseList(int[,] a, List<int[,]> Close)
        {
            for (int i = 0; i < Close.Count; i++)
            {
                if (AreMatricesEqual(a, Close[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
