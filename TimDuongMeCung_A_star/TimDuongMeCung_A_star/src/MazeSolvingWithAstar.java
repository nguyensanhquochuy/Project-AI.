import javax.swing.*;
import javax.swing.Timer;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.*;
import java.util.List;

public class MazeSolvingWithAstar {

    // Mảng chứa các hướng di chuyển có thể từ một ô trong ma trận.
    private static final int[][] DIRECTIONS = {{-1, 0}, {1, 0}, {0, -1}, {0, 1}};

    // Lớp Node đại diện cho một ô trong mê cung
    static class Node implements Comparable<Node> {
        private final int row;        // Chỉ số hàng của ô
        private final int col;        // Chỉ số cột của ô
        private final int cost;       // Chi phí để đi đến ô này từ điểm bắt đầu
        private final int heuristic;  // Giá trị heuristic (khoảng cách Manhattan đến điểm đích)
        private final Node parent;    // Node cha
        private final int[][] maze;   // Mê cung

        public Node(int row, int col, int cost, Node parent, int[][] maze) {
            this.row = row;
            this.col = col;
            this.cost = cost;
            this.parent = parent;
            this.maze = maze;
            this.heuristic = calculateManhattanDistance();
        }

        public int getCost() {
            return cost;
        }

        public int getHeuristic() {
            return heuristic;
        }

        public Node getParent() {
            return parent;
        }

        // Tính toán giá trị heuristic theo khoảng cách Manhattan
        private int calculateManhattanDistance() {
            int[] goal = findGoalCell(); // Lấy tọa độ ô đích
            // Tính khoảng cách Manhattan bằng cách tính tổng khoảng cách giữa các chiều hàng và cột
            return Math.abs(row - goal[0]) + Math.abs(col - goal[1]);
        }

        // Tìm vị trí ô đích trong mê cung
        private int[] findGoalCell() {
            int[] goal = new int[2];

            // Duyệt qua tất cả các ô trong mê cung để tìm ô đích (có giá trị 2)
            for (int i = 0; i < maze.length; i++) {
                for (int j = 0; j < maze[0].length; j++) {
                    if (maze[i][j] == 2) {
                        goal[0] = i;
                        goal[1] = j;
                        return goal;
                    }
                }
            }
            return goal;
        }

        // So sánh các node để xác định ưu tiên trong hàng đợi ưu tiên
        @Override
        public int compareTo (Node other) {
            int totalCost = cost + heuristic;
            int otherTotalCost = other.getCost() + other.getHeuristic();

            if (totalCost < otherTotalCost) {
                return -1;
            } else if (totalCost > otherTotalCost) {
                return 1;
            }

            return 0;
        }
    }

    // Giải mê cung bằng thuật toán A*
    public static List<int[]> Solve(int[][] maze, int[] start) {
        PriorityQueue<Node> openList = new PriorityQueue<>(); // Hàng đợi ưu tiên chứa các Node cần kiểm tra
        Set<String> closedList = new HashSet<>();            // Tập hợp chứa các Node đã kiểm tra

        // Tạo Node ban đầu từ ô bắt đầu
        Node initialNode = new Node(start[0], start[1], 0, null, maze);
        openList.add(initialNode);

        // Tiến hành tìm đường đi trong mê cung
        while (!openList.isEmpty()) {
            Node currentNode = openList.poll(); // Lấy Node đầu tiên trong hàng đợi ưu tiên

            // Nếu Node hiện tại là điểm đích, tìm thấy đường đi và trả về đường đi đã xây dựng
            if (maze[currentNode.row][currentNode.col] == 2) {
                return reconstructPath(currentNode);
            }

            closedList.add(currentNode.row + "," + currentNode.col); // Đánh dấu Node hiện tại là đã kiểm tra
            for (int[] direction : DIRECTIONS) {
                int newRow = currentNode.row + direction[0];
                int newCol = currentNode.col + direction[1];

                // Kiểm tra xem ô mới có hợp lệ để di chuyển vào không
                if (isValidCell(newRow, newCol, maze)) {
                    int newCost = currentNode.getCost() + 1;
                    Node newNode = new Node(newRow, newCol, newCost, currentNode, maze);

                    // Nếu ô mới chưa được kiểm tra, thêm vào hàng đợi ưu tiên
                    if (!closedList.contains(newRow + "," + newCol)) {
                        openList.add(newNode);
                    }
                }
            }
        }

        return null; // không tim thấy đường đi
    }

    // Kiểm tra xem ô có hợp lệ để di chuyển vào hay không
    private static boolean isValidCell(int row, int col, int[][] maze) {
        return row >= 0 && row < maze.length && col >= 0 && col < maze[0].length && maze[row][col] != 1;
    }

    // Truy vết đường đi từ nút đích trở về nút xuất phát
    private static List<int[]> reconstructPath(Node node) {
        List<int[]> path = new ArrayList<>();

        // Lặp qua các nút từ nút hiện tại đến nút gốc và thêm tọa độ của từng ô vào danh sách path
        while (node != null) {
            path.add(new int[]{node.row, node.col});
            node = node.getParent();
        }

        Collections.reverse(path);  // Đảo ngược danh sách để có thứ tự từ nút gốc đến đích
        return path;  // Trả về danh sách các tọa độ trên đường đi từ nút đích đến nút gốc.
    }

    // JPanel tùy chỉnh để vẽ mê cung và đường đi
    static class MazePanel extends JPanel {
        private int[][] maze;
        private List<int[]> path;

        public int[][] getMaze() {
            return maze;
        }

        public List<int[]> getPath() {
            return path;
        }

        public void setPath(List<int[]> path) {
            this.path = path;
        }

        public void setMaze(int[][] maze) {
            this.maze = maze;
        }

        public MazePanel(int[][] maze, List<int[]> path) {
            this.maze = maze;
            this.path = path;
        }

        // Tô màu ô trên mê cung
        public void colorCell(int row, int col, int color) {
            maze[row][col] = color;
            repaint();
        }

        // Vẽ mê cung và đường đi trên panel
        @Override
        protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            int numRows = maze.length;
            int numCols = maze[0].length;

            int cellSize = Math.min(getWidth() / numCols, getHeight() / numRows);

            for (int row = 0; row < maze.length; row++) {
                for (int col = 0; col < maze[0].length; col++) {
                    if (row == 0 && col == 0) {
                        g.setColor(Color.BLUE);
                    }
                    else if (maze[row][col] == 1) {
                        g.setColor(Color.BLACK);
                    } else if (maze[row][col] == 2) {
                        g.setColor(Color.GREEN);
                    } else if (maze[row][col] == 3) {
                        g.setColor(Color.BLUE);
                    } else {
                        g.setColor(Color.WHITE);
                    }

                    g.fillRect(col * cellSize, row * cellSize, cellSize, cellSize);
                    g.setColor(Color.BLACK);
                    g.drawRect(col * cellSize, row * cellSize, cellSize, cellSize);
                }
            }
        }

        @Override
        public Dimension getPreferredSize() {
            int numRows = maze.length;
            int numCols = maze[0].length;

            int cellSize = 50;
            int width = numCols * cellSize;
            int height = numRows * cellSize;

            return new Dimension(width, height);
        }

    }

    public static void main(String[] args) {
        // Khởi tạo mê cung ban đầu và điểm bắt đầu
        int[][] maze = {
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1},
                {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1},
                {1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1},
                {1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1},
                {1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1},
                {1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1},
                {1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1},
                {1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1},
                {1, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1},

        };
        int[] start = {0, 0};

        // Tạo một frame GUI để hiển thị mê cung và đường đi
        SwingUtilities.invokeLater(() -> {
            JFrame frame = new JFrame("Tìm đường mê cung");
            frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

            List<int[]> path = Solve(maze, start);
            MazePanel mazePanel = new MazePanel(maze, path);

            // Tạo nút để tạo mê cung ngẫu nhiên
            JButton randomMazeButton = new JButton("Random Maze");
            randomMazeButton.setFont(new Font("Arial", Font.BOLD, 15));
            randomMazeButton.addActionListener(new ActionListener() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    int numRows = maze.length;
                    int numCols = maze[0].length;
                    int[][] newMaze = generateRandomMaze(numRows, numCols);
                    List<int[]> newPath = Solve(newMaze, start);

                    mazePanel.setMaze(newMaze);
                    mazePanel.setPath(newPath);

                    frame.revalidate();
                    frame.repaint();
                }
            });

            // Create the Solve button
            JButton solveButton = new JButton("Solve");
            solveButton.setFont(new Font("Arial", Font.BOLD, 15));
            solveButton.addActionListener(new ActionListener() {
                @Override
                public void actionPerformed(ActionEvent e) {
                    if (mazePanel.getPath() != null) {
                        JOptionPane.showMessageDialog(frame, "Tìm thấy đường đi với "+(mazePanel.getPath().size() -1) + " bước.");
                        randomMazeButton.setEnabled(false);
                        Timer timer = new Timer(300, new ActionListener() {

                            private int step = 0;
                            @Override
                            public void actionPerformed(ActionEvent e) {
                                if (step < mazePanel.getPath().size()) {
                                    int[] cell = mazePanel.getPath().get(step);
                                    mazePanel.colorCell(cell[0], cell[1], 3);
                                    step++;

                                } else {
                                    JOptionPane.showMessageDialog(frame, "Đã đến đích!");
                                    ((Timer) e.getSource()).stop();
                                    randomMazeButton.setEnabled(true);
                                }
                            }
                        });

                        timer.start();
                    }
                    else {
                        JOptionPane.showMessageDialog(frame, "Không tim thấy đường đi đến đích.");
                        randomMazeButton.setEnabled(true);
                    }
                }
            });

            frame.setLayout(new BorderLayout());
            JPanel buttonPanel = new JPanel();
            buttonPanel.add(randomMazeButton);
            buttonPanel.add(solveButton);
            frame.add(buttonPanel, BorderLayout.NORTH);
            frame.add(mazePanel, BorderLayout.CENTER);
            frame.pack();
            frame.setLocationRelativeTo(null);
            frame.setVisible(true);
        });
    }

    // Phương thức tạo mê cung ngẫu nhiên
    public static int[][] generateRandomMaze(int numRows, int numCols) {
        int[][] maze = new int[numRows][numCols];
        for (int i = 0; i < numRows; i++) {
            for (int j = 0; j < numCols; j++) {
                maze[i][j] = Math.random() < 0.3 ? 1 : 0; // 30% là tường
            }
        }
        maze[numRows - 1][numCols - 1] = 2; // Điểm đích
        maze[0][0] = 0; // Điểm bắt đầu
        return maze;
    }
}
