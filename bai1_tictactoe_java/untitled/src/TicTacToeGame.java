import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class TicTacToeGame extends JFrame implements ActionListener {
    private JButton[][] buttons; // mang luu tru cac nut tren ban co
    private String currentPlayer; // nguoi choi hien tai
    private String[][] board;// mang luu tru trang thai cua ban co
    private boolean gameOver;// kiem tra trang thai ket thuc cua tro choi
    private JLabel turnLabel; // nhan hien thi luot choi
    private int pointPlayer = 0; // bien luu tru diem so cua nguoi choi
    private int pointComputer = 0; // bien luu tru diem so cua may tinh
    private JButton newGameButton; // nut new
    private JButton point1_bt;// nut hien thi diem so cua nguoi choi
    private JButton point2_bt;// nut hien thi diem so cua may tinh

    public TicTacToeGame() {
        super("Tic Tac Toe");
        setSize(500, 600);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        buttons = new JButton[3][3];
        currentPlayer = "X";
        board = new String[3][3]; // khoi tao trang thai cua ban co
        gameOver = false;

        setLayout(new BorderLayout());

        JPanel boardPanel = new JPanel();
        boardPanel.setLayout(new GridLayout(3, 3));

        // tao cac nut tren ban co
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                buttons[i][j] = new JButton();
                buttons[i][j].setFont(new Font("Arial", Font.BOLD, 100));
                buttons[i][j].addActionListener(this);
                buttons[i][j].setBackground(Color.white);
                boardPanel.add(buttons[i][j]);
                board[i][j] = "";



            }
        }
        // tao nut New Game
        newGameButton = new JButton("New Game");
        newGameButton.setFont(new Font("Arial", Font.BOLD, 18));
        newGameButton.setForeground(Color.blue);
        newGameButton.setBackground(Color.white);
        newGameButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                // khi nhan new game thi khoi dong lai game
                resetGame();
            }
        });

        point1_bt = new JButton("0");
        point1_bt.setFont(new Font("UTM Nokia", Font.BOLD, 25));
        point2_bt = new JButton("0");
        point2_bt.setFont(new Font("UTM Nokia", Font.BOLD, 25));

        JPanel bottomPanel = new JPanel();
        bottomPanel.setLayout(new FlowLayout());
        bottomPanel.add(point1_bt);
        bottomPanel.add(newGameButton);
        bottomPanel.add(point2_bt);

        add(boardPanel, BorderLayout.CENTER);
        add(bottomPanel, BorderLayout.SOUTH);

        turnLabel = new JLabel(currentPlayer+ "'s turn", SwingConstants.CENTER);
        turnLabel.setFont(new Font("Arial", Font.PLAIN, 25));
        add(turnLabel, BorderLayout.NORTH);
    }


    // phuong thuc xu ly su kien khi nguoi choi nhan 1 nut tren ban co
    public void actionPerformed(ActionEvent e) {
        if (gameOver) {
            return;
        }

        JButton button = (JButton) e.getSource(); // lay thong tin nut duoc nhan

        int row = -1, col = -1;

        // Tim vi tri cua nut duoc nhan trong mang buttons
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (buttons[i][j] == button) {
                    row = i;
                    col = j;
                    break;
                }
            }
        }

        // Neu nut duoc nhan tren ban co va o trong
        if (row != -1 && col != -1 && board[row][col].isEmpty()) {
            board[row][col] = currentPlayer; // cap nhat trang thai cua o tren ban co
            button.setForeground(Color.blue);
            button.setText(currentPlayer);
            button.setEnabled(true);

            // Kiem tra xem nguoi choi hien tai da thang hay chua
            if (checkWin(currentPlayer)) {
                showStatus(currentPlayer + " wins!");
                // hien thi hop thoai thong bao ket qua
                JOptionPane.showMessageDialog(this, currentPlayer + " wins!", "Game Over", JOptionPane.INFORMATION_MESSAGE);
                gameOver = true; // ket thuc tro choi
                updateScore(currentPlayer);
            } else if (checkDraw()) {
                showStatus("Draw!");
                JOptionPane.showMessageDialog(this, "Draw!", "Game Over", JOptionPane.INFORMATION_MESSAGE);
                gameOver = true;
                updateScore("Draw");
            } else {
                currentPlayer = (currentPlayer.equals("X")) ? "O" : "X"; // chuyen luot choi
                if (currentPlayer.equals("O")) { // luot choi cua may tinh
                    computerMove();
                }
            }
        }
        // Cap nhat nhan hien thi luot choi
        if (currentPlayer.equals("X")) {
            turnLabel.setText("X's turn");

        } else {
            turnLabel.setText("O's turn");
        }
    }



    // Phuong thuc thuc hien nuoc di cua may tinh
    private void computerMove() {
        int bestScore = Integer.MIN_VALUE;
        int bestRow = -1;
        int bestCol = -1;
        boolean isBoardEmpty = true;

        // Kiem tra xem ban co co trong hay khong
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (!board[i][j].isEmpty()) {
                    isBoardEmpty = false;
                    break;
                }
            }
        }

        if (isBoardEmpty) {
            bestRow = 1;
            bestCol = 1;
        } else {
            // Su dung thuat toan minimax de tim nuoc di tot nhat cho may tinh
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (board[i][j].isEmpty()) {
                        board[i][j] = currentPlayer;
                        int score = minimax(board, 0, false);
                        board[i][j] = "";
                        if (score > bestScore) {
                            bestScore = score;
                            bestRow = i;
                            bestCol = j;
                        }
                    }
                }
            }
        }

        // Thuc hien nuoc di tot nhat cua may tinh
        if (bestRow != -1 && bestCol != -1) {
            board[bestRow][bestCol] = currentPlayer;
            buttons[bestRow][bestCol].setForeground(Color.red);
            buttons[bestRow][bestCol].setText(currentPlayer);
            buttons[bestRow][bestCol].setEnabled(true);

            if (checkWin(currentPlayer)) {
                showStatus(currentPlayer + " wins!");
                JOptionPane.showMessageDialog(this, currentPlayer + " wins!", "Game Over", JOptionPane.INFORMATION_MESSAGE);
                gameOver = true;
                updateScore(currentPlayer);
            } else if (checkDraw()) {
                showStatus("Draw!");
                JOptionPane.showMessageDialog(this, "Draw!", "Game Over", JOptionPane.INFORMATION_MESSAGE);
                gameOver = true;
                updateScore("Draw");
            } else {
                currentPlayer = "X"; // chuyen luot cho nguoi choi X
            }
        }
        if (currentPlayer.equals("O")) {
            turnLabel.setText("O's turn");
        } else {
            turnLabel.setText("X's turn");
        }
    }

    // Phuong thuc thuc hien thuat toan minimax de danh gia nuoc di tot nhat cho may tinh
    private int minimax(String[][] board, int depth, boolean isMaximizing) {
        if (checkWin("X")) {
            return -1; // Neu X thang tra ve -1
        } else if (checkWin("O")) {
            return 1; // Neu O thang tra ve 1
        } else if (checkDraw()) {
            return 0; // Neu hoa tra ve 0
        }
        // Neu dang la luot của nguoi choi muon toi uu hua (Maximizer)
        if (isMaximizing) {
            int bestScore = Integer.MIN_VALUE;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (board[i][j].isEmpty()) {
                        board[i][j] = "O";
                        // Goi de quy toi đo sau tiep theo va danh gia nuoc di của Minimizer (da chuyen luot)
                        int score = minimax(board, depth + 1, false);
                        // Hoan tac nuoc di tam thoi
                        board[i][j] = "";
                        // Lay gia tri lon nhat giua bestScore và score
                        bestScore = Math.max(score, bestScore);
                    }
                }
            }
            // Tra ve diem so tot nhat cho Maximizer
            return bestScore;
        } else {
            int bestScore = Integer.MAX_VALUE;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (board[i][j].isEmpty()) {
                        board[i][j] = "X";
                        // Goi de quy toi đo sau tiep theo va danh gia nuoc di của Maximizer (da chuyen luot)
                        int score = minimax(board, depth + 1, true);
                        board[i][j] = "";
                        // Lay gia tri nho nhat giua bestScore và score
                        bestScore = Math.min(score, bestScore);
                    }
                }
            }
            // Tra ve diem so tot nhat cho Minimizer
            return bestScore;
        }
    }

    // Phuong thuc kiem tra xem co nguoi choi nao thang khong
    private boolean checkWin(String player) {
        for (int i = 0; i < 3; i++) {
            if (board[i][0].equals(player) && board[i][1].equals(player) && board[i][2].equals(player)) {
                return true;
            }
            if (board[0][i].equals(player) && board[1][i].equals(player) && board[2][i].equals(player)) {
                return true;
            }
        }
        if (board[0][0].equals(player) && board[1][1].equals(player) && board[2][2].equals(player)) {
            return true;
        }
        if (board[0][2].equals(player) && board[1][1].equals(player) && board[2][0].equals(player)) {
            return true;
        }
        return false;
    }

    private boolean checkDraw() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j].isEmpty()) {
                    return false;
                }
            }
        }
        return true;
    }


    // Phuong thuc cap nhat diem so va hien thi len giao dien
    // +10 cho nguoi choi thang, neu hoa thi ca hai duoc cong 1
    private void updateScore(String winner) {
        if (winner.equals("X")) {
            pointPlayer += 10;
        } else if (winner.equals("O")) {
            pointComputer += 10;
        } else  {
            // Draw
            pointPlayer += 1;
            pointComputer += 1;
        }

        // Update score on GUI
        point1_bt.setText(String.valueOf(pointPlayer));
        point2_bt.setText(String.valueOf(pointComputer));
    }



    private void showStatus(String status) {
        setTitle("Tic Tac Toe - " + status);
    }
    private void resetGame() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                buttons[i][j].setText("");
                buttons[i][j].setEnabled(true);
                board[i][j] = "";
            }
        }

        gameOver = false;
        currentPlayer = "X";

        // Neu random >=0.5 thi may tinh duoc di truoc
        if (Math.random() >= 0.5) {
            currentPlayer = "O";
            computerMove();
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(new Runnable() {
            public void run() {
                TicTacToeGame game = new TicTacToeGame();
                game.setVisible(true);
                game.setLocationRelativeTo(null);
            }
        });
    }
}
