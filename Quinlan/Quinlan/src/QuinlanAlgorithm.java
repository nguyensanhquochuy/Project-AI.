import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.*;
import java.util.stream.Collectors;

public class QuinlanAlgorithm {
    static List<String> attributes = new ArrayList<>();
    public static void main(String[] args) {
        System.out.println("-----------Chương trình cài đăt thuật giải Quinlan trong máy học-----------");
        System.out.println("Bạn muốn đọc dữ liệu từ file nào");
        System.out.println("1. input.xlsx");
        System.out.println("2. input2.xlsx");

        int chose;
        String fileName = "";
        Scanner scanner = new Scanner(System.in);
        do {
            System.out.print("Nhập lựa chọn của bạn (1 hoặc 2): ");
            chose = scanner.nextInt();
            if (chose != 1 && chose != 2) {
                System.out.println("Lựa chọn không hợp lệ. Vui lòng nhập lại:");
            }
        } while (chose != 1 && chose != 2);

        // Kiem tra nguoi dung chon doc du lieu tu file nao
        if (chose == 1) {
            fileName = "input.xlsx";
        } else if (chose == 2) {
            fileName = "input2.xlsx";
        }


        // Buoc 1: Doc du lieu tu file
        List<Map<String, String>> data = readDataFromExcel(fileName);

        // In bang du lieu da doc duoc tu file
        System.out.println("Bảng dữ liệu:");
        printData(data);

        // Buoc 2: Xay dung cay quyet dinh
        String targetAttribute = getTargetAtrribute();
        Node decisionTree = buildDecisionTree(data, targetAttribute);

        // Buoc 3: Ve cay quyet dinh
        System.out.println("\nCây quyết định:");
        printDecisionTree(decisionTree, "");

        // Buoc 4: In ra cac tap luat
        System.out.println("\nCác tập luật:");
        List<String> rules = new ArrayList<>();
        generateRules(decisionTree, "", rules);
        for (String rule : rules) {
            System.out.println(rule);
        }
    }

    // Phuong thuc in bang du lieu
    private static void printData(List<Map<String, String>> data) {
        if (data.isEmpty()) {
            System.out.println("No data to display.");
            return;
        }

        // Determine the maximum length of attribute names for proper column alignment
        int maxAttributeLength = attributes.stream()
                .mapToInt(String::length)
                .max()
                .orElse(0);

        // Print header
        for (String attribute : attributes) {
            System.out.print("| " + padRight(attribute, maxAttributeLength) + " ");
        }
        System.out.println("|");

        // Print separator row
        for (String attribute : attributes) {
            System.out.print("+");
            for (int i = 0; i < maxAttributeLength + 2; i++) {
                System.out.print("-");
            }
        }
        System.out.println("+");

        // Print each record as a row in the table
        for (Map<String, String> record : data) {
            for (String attribute : attributes) {
                System.out.print("| " + padRight(record.get(attribute), maxAttributeLength) + " ");
            }
            System.out.println("|");
        }
    }
    private static String padRight(String s, int length) {
        return String.format("%-" + length + "s", s);
    }

    // Phuong thuc doc du lieu tu file Excel va tra ve danh sach ban ghi
    private static List<Map<String, String>> readDataFromExcel(String fileName) {
        List<Map<String, String>> data = new ArrayList<>();
        try (FileInputStream fis = new FileInputStream(new File(fileName));
             XSSFWorkbook workbook = new XSSFWorkbook(fis)) {

            XSSFSheet sheet = workbook.getSheetAt(0);

            // Doc danh sach thuoc tinh tu hang dau tien cua sheet
            Row headerRow = sheet.getRow(0);
            for (int j = 0; j < headerRow.getLastCellNum(); j++) {
                Cell cell = headerRow.getCell(j);
                if (cell != null) {
                    String columnName = cell.getStringCellValue();
                    attributes.add(columnName);
                }
            }

            // Doc du lieu tu cac hang con lai cua sheet
            for (int i = 1; i <= sheet.getLastRowNum(); i++) {
                Row row = sheet.getRow(i);
                if (row != null) {
                    Map<String, String> record = new HashMap<>();
                    for (int j = 0; j < row.getLastCellNum(); j++) {
                        Cell cell = row.getCell(j);
                        if (cell != null) {
                            String columnName = attributes.get(j);
                            String cellValue = cell.getStringCellValue();
                            record.put(columnName, cellValue);
                        }
                    }
                    data.add(record);
                }
            }

        } catch (IOException e) {
            e.printStackTrace();
        }
        return data;
    }

    // Phuong thuc lay thuoc tinh muc tieu (target) tu danh sach thuoc tinh
    private static String getTargetAtrribute(){
        return attributes.get(attributes.size()-1);
    }

    // Phuong thuc xay dung cay quyet dinh dua tren du lieu va thuoc tinh muc tieu
    private static Node buildDecisionTree(List<Map<String, String>> data, String targetAttribute) {
        Set<String> targetValues = getUniqueValues(data, targetAttribute);

        // Neu tat ca cac mau deu co cung 1 gia tri thuoc tinh muc tieu, tra ve nut la voi gia tri thuoc tinh muc tieu do
        if (targetValues.size() == 1) {
            String targetValue = targetValues.iterator().next();
            Node leafNode = new Node(null);
            leafNode.setTargetValue(targetValue);
            return leafNode;
        }

        // Tim thuoc tinh dan xuat tot nhat đe phan chia du lieu
        String bestAttribute = findBestAttribute(data, targetAttribute);
        Node root = new Node("" + bestAttribute);

        // Phan chia du lieu và xay dung cây con cho tung nhanh
        Map<String, List<Map<String, String>>> partitions = partitionData(data, bestAttribute);
        for (Map.Entry<String, List<Map<String, String>>> entry : partitions.entrySet()) {
            String attributeValue = entry.getKey();
            List<Map<String, String>> partitionedData = entry.getValue();
            Node subTree = buildDecisionTree(partitionedData, targetAttribute);
            root.addChild(attributeValue, subTree);
        }
        return root;
    }

    // Phuong thuc tra ve tap gia tri duy nhat cua 1 thuoc tinh trong du lieu
    private static Set<String> getUniqueValues(List<Map<String, String>> data, String attribute) {
        return data.stream().map(record -> record.get(attribute)).collect(Collectors.toSet());
    }

    // Phuong thuc tim thuoc tinh tot nhat de phan chia du lieu dua tren entropy
    private static String findBestAttribute(List<Map<String, String>> data, String targetAttribute) {
        double minEntropy = Double.MAX_VALUE;
        String bestAttribute = null;

        // Duyet qua cac thuoc tinh dan xuat de tim thuoc tinh tot nhat
        for (String attribute : data.get(0).keySet()) {
            if (attribute.equals(targetAttribute)) continue; // Bo qua thuoc tinh muc tieu
            double entropy = calculateEntropy(data, attribute, targetAttribute);
            if (entropy < minEntropy) {
                minEntropy = entropy;
                bestAttribute = attribute;
            }
        }

        return bestAttribute;
    }

    // Phuong thuc tinh entropy cho 1 thuoc tinh dan xuat
    private static double calculateEntropy(List<Map<String, String>> data, String attribute, String targetAttribute) {
        Set<String> uniqueValues = getUniqueValues(data, attribute);
        double entropy = 0.0;

        for (String value : uniqueValues) {
            List<Map<String, String>> partitionedData = data.stream()
                    .filter(record -> record.get(attribute).equals(value))
                    .collect(Collectors.toList());

            double probability = (double) partitionedData.size() / data.size();
            double targetEntropy = calculateTargetEntropy(partitionedData, targetAttribute);
            entropy += probability * targetEntropy;
        }

        return entropy;
    }

    // Phuong thuc tinh entropy cho thuoc tinh muc tieu
    private static double calculateTargetEntropy(List<Map<String, String>> data, String targetAttribute) {
        Set<String> targetValues = getUniqueValues(data, targetAttribute);
        double entropy = 0.0;

        for (String value : targetValues) {
            List<Map<String, String>> filteredData = data.stream()
                    .filter(record -> record.get(targetAttribute).equals(value))
                    .collect(Collectors.toList());

            double probability = (double) filteredData.size() / data.size();
            entropy -= probability * log2(probability);
        }

        return entropy;
    }

    private static double log2(double x) {
        return Math.log(x) / Math.log(2);
    }

    // Phuong thuc phan chia du lieu dua tren gia tri cua 1 thuoc tinh
    private static Map<String, List<Map<String, String>>> partitionData(List<Map<String, String>> data, String attribute) {
        Map<String, List<Map<String, String>>> partitions = new HashMap<>();
        for (Map<String, String> record : data) {
            String attributeValue = record.get(attribute);
            if (!partitions.containsKey(attributeValue)) {
                partitions.put(attributeValue, new ArrayList<>());
            }
            partitions.get(attributeValue).add(record);
        }
        return partitions;
    }

    // Phuong thuc in cay quyet dinh
    private static void printDecisionTree(Node node, String indent) {
        if (node.isLeaf()) {
            System.out.println(indent + "  :--> " + node.getTargetValue());
        } else {
            System.out.println(indent + " " + node.getAttribute());
            for (Map.Entry<String, Node> entry : node.getChildren().entrySet()) {
                String attributeValue = entry.getKey();
                Node subTree = entry.getValue();
                System.out.println(indent + "  :--> " + attributeValue);
                printDecisionTree(subTree, indent + "    ");
            }
        }
    }

    // Phuong thuc sinh cac tap luat tu cay quyet dinh
    private static void generateRules(Node node, String rule, List<String> rules) {
        if (node.isLeaf()) {
            String generatedRule = rule + " then (" + getTargetAtrribute() + " IS " + node.getTargetValue() + ")";
            rules.add("If " + generatedRule);
        } else {
            for (Map.Entry<String, Node> entry : node.getChildren().entrySet()) {
                String attributeValue = entry.getKey();
                Node subTree = entry.getValue();
                String nextRule = rule.isEmpty() ? "(" + node.getAttribute() + " IS " + attributeValue + ")" : rule + " And (" + node.getAttribute() + " IS " + attributeValue + ")";
                generateRules(subTree, nextRule, rules);
            }
        }
    }

    // Lop Node dai dien cho cac nut trong cay quyet dinh
    private static class Node {
        private String attribute;
        private String targetValue; // chi cho cac nut la
        private Map<String, Node> children;
        public Node(String attribute) {
            this.attribute = attribute;
            this.children = new HashMap<>();
        }
        public String getAttribute() {
            return attribute;
        }
        public String getTargetValue() {
            return targetValue;
        }
        public void setTargetValue(String targetValue) {
            this.targetValue = targetValue;
        }
        public Map<String, Node> getChildren() {
            return children;
        }
        public void addChild(String attributeValue, Node childNode) {
            children.put(attributeValue, childNode);
        }
        public boolean isLeaf() {
            return targetValue != null;
        }
    }
}
