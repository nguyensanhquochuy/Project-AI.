
import java.util.*;

public class RiverCrossingBFS {

    // Mảng chứa các ký hiệu tượng trưng cho những người/động vật ở bờ sông (F - Nông dân, W - Sói, S - Cừu, C - Bắp cải).
    private static final String[] OCCUPANTS = {"F", "W", "S", "C"};

    //Mảng chứa các bước di chuyển có thể (F - Di chuyển nông dân, FW - Di chuyển nông dân và sói, FS - Di chuyển nông
    // dân và cừu, FC - Di chuyển nông dân và bắp cải).
    private static final String[] MOVES = {"F", "FW", "FS", "FC"};

    // Lớp State đại diện cho trạng thái của bài toán tại một thời điểm cụ thể.
    private static class State {
        Set<String> leftBank; // Tập hợp chứa các đối tượng ở bờ trái.
        Set<String> rightBank; // Tập hợp chứa các đối tượng ở bờ phải.
        boolean farmerOnLeft; //  Biểu thị vị trí của nông dân, true nếu nông dân ở bờ trái và false nếu nông dân ở bờ phải
        State parent; // Trạng thái cha của trạng thái hiện tại

        public State(Set<String> leftBank, Set<String> rightBank, boolean farmerOnLeft, State parent) {
            this.leftBank = new HashSet<>(leftBank);
            this.rightBank = new HashSet<>(rightBank);
            this.farmerOnLeft = farmerOnLeft;
            this.parent = parent;
        }

        // Kiểm tra xem trạng thái có hợp lệ không, tức là không có sự kết hợp nguy hiểm nào (sói ăn cừu, cừu ăn bắp cải khi không có nông dân).
        public boolean isValid() {
            return !(hasDangerousCombination(leftBank) || hasDangerousCombination(rightBank));
        }

        // Kiểm tra xem tập hợp các đối tượng có sự kết hợp nguy hiểm nào không.
        private boolean hasDangerousCombination(Set<String> bank) {
            return (bank.contains("W") && bank.contains("S") && !bank.contains("F"))
                    || (bank.contains("S") && bank.contains("C") && !bank.contains("F"));
        }

        // Kiểm tra xem trạng thái hiện tại có phải trạng thái kết thúc (tất cả đối tượng ở bờ phải, không có gì ở bờ trái) hay không.
        public boolean isFinalState() {
            return leftBank.isEmpty() && rightBank.containsAll(Arrays.asList(OCCUPANTS));
        }

        // Phương thức tạo trạng thái mới sau khi thực hiện bước di chuyển
        public State makeMove(String move) {
            Set<String> newLeftBank = new HashSet<>(leftBank);
            Set<String> newRightBank = new HashSet<>(rightBank);

            // Đảo chiều vị trí của người nông dân sau mỗi bước di chuyển
            boolean newFarmerOnLeft = !farmerOnLeft;

            // Duyệt qua từng ký tự trong chuỗi bước di chuyển
            for (char c : move.toCharArray()) {
                String occupant = String.valueOf(c);
                // Nếu người nông dân đang ở bờ trái
                if (farmerOnLeft) {
                    newLeftBank.remove(occupant); // Di chuyển người và vật thể ra khỏi bờ trái
                    newRightBank.add(occupant); // Đặt người và vật thể vào bờ phải
                } else {
                    newRightBank.remove(occupant);
                    newLeftBank.add(occupant);
                }
            }

            return new State(newLeftBank, newRightBank, newFarmerOnLeft, this);
        }

        @Override
        public String toString() {
            StringBuilder left = new StringBuilder();
            for (var o : leftBank) {
                left.append(occupantToString(o));
            }

            StringBuilder right = new StringBuilder();
            for (var o : rightBank) {
                right.append(occupantToString(o));
            }
            // Trả về chuỗi mô tả trạng thái của cả hai bờ
            return "Left: " + left + "\nRight: " + right ;
        }
    }

    // Phương thức giải quyết bài toán người nông dân qua sông
    public static void solveRiverCrossing() {
        // Tạo trạng thái ban đầu với danh sách vật thể ở bờ trái và bờ phải
        Set<String> initialLeftBank = new HashSet<>(Arrays.asList(OCCUPANTS));
        Set<String> initialRightBank = new HashSet<>();
        State initialState = new State(initialLeftBank, initialRightBank, true, null);

        // Tạo hàng đợi để thực hiện tìm kiếm theo chiều rộng
        Queue<State> queue = new LinkedList<>();
        queue.offer(initialState);

        // Thực hiện tìm kiếm theo chiều rộng
        while (!queue.isEmpty()) {
            State currentState = queue.poll(); // Lấy trạng thái hiện tại ra khỏi hàng đợi để kiểm tra

            // Kiểm tra nếu trạng thái hiện tại là trạng thái cuối cùng
            if (currentState.isFinalState()) {
                // In ra lời giải
                printSolution(currentState);
                break;
            }

            // Duyệt qua các bước di chuyển và tạo các trạng thái kế tiếp
            for (String move : MOVES) {
                State nextState = currentState.makeMove(move);
                // Kiểm tra xem trạng thái kế tiếp có hợp lệ không
                if (nextState.isValid()) {
                    // Thêm trạng thái kế tiếp vào hàng đợi để kiểm tra sau
                    queue.offer(nextState);
                }
            }
        }
    }

    // Phương thức in ra lời giải của bài toán người nông dân qua sông
    public static void printSolution(State state) {
        // Tạo danh sách các trạng thái trong chuỗi lời giải
        List<State> solutionPath = new ArrayList<>();
        while (state != null) {
            solutionPath.add(state);
            state = state.parent;
        }
        Collections.reverse(solutionPath); // Đảo ngược danh sách để bắt đầu từ trạng thái ban đầu

        System.out.println("Lời giải:");
        for (int i = 0; i < solutionPath.size(); i++) {
            State step = solutionPath.get(i);
            if (i == 0) {
                System.out.println(step);
            } else {
                System.out.println("Bước " + i + ":");
                System.out.println(step); // In ra trạng thái hiện tại
                String move = findMove(solutionPath.get(i - 1), solutionPath.get(i));
                System.out.println(move); // In ra bước di chuyển
            }
        }
    }

    // Phương thức tìm di chuyển từ trạng thái này đến trạng thái kia
    public static String findMove(State fromState, State toState) {
        // Tạo tập hợp các vật thể đã di chuyển từ bờ trái
        Set<String> movedFromLeft = new HashSet<>(fromState.leftBank);
        movedFromLeft.removeAll(toState.leftBank);

        // Tạo tập hợp các vật thể đã di chuyển từ bờ phải
        Set<String> movedFromRight = new HashSet<>(fromState.rightBank);
        movedFromRight.removeAll(toState.rightBank);

        StringBuilder moveDescription = new StringBuilder();
        for (String occupant : movedFromLeft) {
            moveDescription.append(occupantToString(occupant));
        }
        if (!movedFromLeft.isEmpty() && !movedFromRight.isEmpty()) {
            moveDescription.append("and ");
        }
        for (String occupant : movedFromRight) {
            moveDescription.append(occupantToString(occupant));
        }
        if (!movedFromLeft.isEmpty() || !movedFromRight.isEmpty()) {
            moveDescription.append("move ");
        }
        if (fromState.farmerOnLeft) {
            moveDescription.append("Right");
        } else {
            moveDescription.append("Left");
        }
        return moveDescription.toString();
    }

    // Phương thức chuyển tên vật thể sang chuỗi mô tả
    public static String occupantToString(String occupant) {
        switch (occupant) {
            case "S":
                return "Sheep, ";
            case "C":
                return "Cabbage, ";
            case "F":
                return "Farmer, ";
            case "W":
                return "Wolf ";
            default:
                return "";
        }
    }

    public static void main(String[] args) {
        System.out.println("-------Bài toán người nông dân qua sông sử dụng thuật toán tím kiếm theo chiểu rộng BFS-----");
        solveRiverCrossing();
    }
}
