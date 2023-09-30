#include <iostream>
#include <vector>
#include <algorithm>
#include <ctime>
#include <omp.h>
#include <fstream>

const int INF = 1e9; // Vô cùng

const int MAX_N = 1000; // So dinh toi da
int N; // So dinh cua do thi

int dist[MAX_N][MAX_N];  // ma tran luu tru khoang cach giua các dinh

// Cau truc chung bieu dien 1 ca the trong quan the
struct Individual {
    std::vector<int> chromosome; // Chuoi chua cac dinh (gen)
    int fitness; // Gia tri fitness (chi phi) cua ca the

    Individual() {
        chromosome.resize(N);
        for (int i = 0; i < N; ++i)
            chromosome[i] = i;
        std::random_shuffle(chromosome.begin(), chromosome.end()); // Hoan vi ngau nhien cac gen trong chuoi
        fitness = calculateFitness(); // Tinh finess cua ca the ngay tu khi khoi tao
    }

	// Tinh Fitness cua ca the bang cach tinh tong khoang cach giua cac dinh trong chromosome
    int calculateFitness() {
        int sum = 0;
        for (int i = 0; i < N - 1; ++i)
            sum += dist[chromosome[i]][chromosome[i + 1]];
        // Cong them khoang cach tu diem cuoi cung den diem dau tien 
        sum += dist[chromosome[N - 1]][chromosome[0]]; 
        return sum;
    }
};

// Ham so sanh dung de sap xep cac ca the trong quan the dua tren Fitness
bool cmp(const Individual &a, const Individual &b) {
    return a.fitness < b.fitness;
}

// Lai ghep 2 ca the cha me de tao ra ca the con moi
Individual crossover(const Individual &parent1, const Individual &parent2) {
    int len = parent1.chromosome.size(); // Do dai chuoi gen cua ca the cha me
    int cutPoint1 = rand() % len;// Diem cat 1 - ngau nhien chon 1 vi tri cat trong chuoi gen
    int cutPoint2 = rand() % len;// Diem cat 2 - ngau nhien chon 1 vi tri cat khac trong chuoi gen
    // neu diem cat 2 < diem cat 1 thi dao vi tri lai
    if (cutPoint2 < cutPoint1)
        std::swap(cutPoint1, cutPoint2);

    std::vector<bool> used(len, false); // chua cac gen da su dung trong qua trinh lai ghep
    Individual child;
    for (int i = cutPoint1; i <= cutPoint2; ++i) {
    	// sao chep cac gen trong khoang [cutPoint1, cutPoint2] cua ca the cha vao ca the con
        child.chromosome[i] = parent1.chromosome[i];
        used[parent1.chromosome[i]] = true; // danh dau cac gen da duoc su dung tu ca the cha
    }

    int pos = 0;
    for (int i = 0; i < len; ++i) {
        if (!used[parent2.chromosome[i]]) {
            while (used[child.chromosome[pos]])
                ++pos; // tim vi tri trong cua ca the con
            // sao chep cac gen tu ca the me vao vi tri trong cua ca the con
            child.chromosome[pos] = parent2.chromosome[i];
            used[parent2.chromosome[i]] = true; // danh dau cac gen da duoc su dung tu ca the me
        }
    }
	
	// Tinh gia tri Fitness cua ca the con sau khi lai ghep
    child.fitness = child.calculateFitness();
    return child;
}


// Ham dot bien cho 1 ca the
void mutate(Individual &individual) {
    const double mutationRate = 0.2; // ty le dot bien
    if ((double)rand() / RAND_MAX < mutationRate) {
        int len = individual.chromosome.size(); // do dai cua chuoi gen (chromosome) trong ca the
        int pos1 = rand() % len; // chon ngau nhien vi tri thu 1 de dot bien
        int pos2 = rand() % len; // chon ngau nhien vi tri thu 2 de dot bien
        
        // hoa doi gia tri cua 2 gen tai vi tri 1 va 2 trong chuoi gen, thuc hien qua trinh dot bien
        std::swap(individual.chromosome[pos1], individual.chromosome[pos2]);
        // cap nhat lai gia tri Fitness bang cach goi ham calculateFitness
        individual.fitness = individual.calculateFitness();
    }
}
// Ham in thong ve the he hien tai
//void printGeneration(int generation, const std::vector<Individual> &population) {
//    std::cout << "The he " << generation << "*************************************************************:\n";
//    for (size_t i = 0; i < population.size(); ++i) {
//        const Individual &individual = population[i];
//        std::cout << "Chuoi gen: ";
//        for (int j = 0; j < N; ++j) {
//            std::cout << individual.chromosome[j];
//            if (j != N - 1)
//                std::cout << " -> ";
//        }
//        std::cout << ", Fitness: " << individual.fitness << "\n";
//    }
//    std::cout << "-----------------------------\n";
//}

// In thong tin ve the he hien tai (chi in ra ca the tot nhat trong the he do)
void printGeneration(int generation, const std::vector<Individual> &population) {
    std::cout << "The he " << generation << ":\n";
    const Individual &bestIndividual = *std::min_element(population.begin(), population.end(), cmp);
    std::cout << "Chuoi gen tot nhat: ";
    for (int j = 0; j < N; ++j) {
        std::cout << bestIndividual.chromosome[j];
        if (j != N - 1)
            std::cout << " -> ";
    }
    std::cout << " -> " << bestIndividual.chromosome[0]; 
    std::cout << ", Fitness: " << bestIndividual.fitness << "\n";
    std::cout << "-----------------------------\n";
}


// Thuat giai de giai bai toan TSP
void geneticAlgorithm(int populationSize, int numGenerations) {
    std::vector<Individual> population(populationSize); // tao 1 quan the ban co kich thuoc populationSize
    const double crossoverRate = 0.8; // ty le lai ghep

	// Khoi tao quan the ban dau
    #pragma omp parallel for
    for (int i = 0; i < populationSize; ++i)
        population[i] = Individual();
	
	// Tien hanh tien hoa qua cac the he
    for (int generation = 1; generation <= numGenerations; ++generation) {
    	// Sap xep quan the theo thu tu tang dan cua Fitness
        std::sort(population.begin(), population.end(), cmp);

        int eliteSize = populationSize / 5; // kich thuoc cua nhom ca the (dai dien cho nhung ca the tot nhat)
        
        // Chon cac ca the tu nhom elite de tao lai quan the moi (loai bo nhung ca the yeu)
        #pragma omp parallel for
        for (int i = eliteSize; i < populationSize; ++i)
            population[i] = population[rand() % eliteSize];

		// Lai ghep cac ca the cha me de tao ra cac ca the con moi, cho den so luong ca the dat du populationSize
        while (population.size() < populationSize) {
            int parent1Index = rand() % eliteSize;
            int parent2Index = rand() % eliteSize;
            if ((double)rand() / RAND_MAX < crossoverRate)
                population.push_back(crossover(population[parent1Index], population[parent2Index]));
        }

		// Tien hanh dot bien ngau nhien cho tung ca the trong quan the
        #pragma omp parallel for
        for (int i = 0; i < populationSize; ++i)
            mutate(population[i]);

		// In thong tin ve the he hien tai va ca the tot nhat trong quan the
        printGeneration(generation, population);
    }

	// ket thuc thuat toan, sap xep la quan the theo thu tu tang dan cua gia tri Fitness
    std::sort(population.begin(), population.end(), cmp);
    std::cout << "Ket qua sau di truyen: " << population[0].fitness << "\n";
    std::cout << "Chuoi dinh (gen) toi uu: ";
    for (int i = 0; i < N; ++i) {
        std::cout << population[0].chromosome[i] << " -> ";
    }
    std::cout << population[0].chromosome[0];
    std::cout << "\n";
}


int main() {
    std::srand(std::time(0));

	int populationSize; // Kich thuoc quan the
    int numGenerations; // So the he

    std::cout << "Chuong trinh giai bai toan TSP bang thuat toan di truyen GA:";
    std::cout << "\n Ban muon doc du lieu tu file nao";
    std::cout << "\n 1. input.txt";
    std::cout << "\n 2. input2.txt";

    int chose;
    std:: string fileName="";
    do {
    	std::cout << "\nNhap lua chon cua ban (1 hoac 2): ";
    	std::cin >> chose;
    	if (chose!=1 && chose!=2) std::cout << "Lua chon khong hop le. Vui long nhap lai:";
	} while (chose!=1 && chose!=2);


	// Kiem tra nguoi dung chon doc du lieu tu file nao
    if (chose == 1) {
     	fileName = "input.txt";
    } 
	else if (chose == 2) {   
		fileName = "input2.txt";
    } 
        // Doc du lieu tu file 
    std::ifstream inputFile(fileName.c_str());
    if (!inputFile) {
        std::cerr << "Khong the mo file.\n";
        return 1;
    }

    inputFile >> N;
    if (N <= 0 ) {
        std::cerr << "So dinh không hop le.\n";
        return 1;
    }
    // Doc kich thuoc quan the (so luong ca the) va so the he
	inputFile >> populationSize;
    inputFile >> numGenerations;

    for (int i = 0; i < N; ++i)
        for (int j = 0; j < N; ++j)
            inputFile >> dist[i][j];

    inputFile.close();
    geneticAlgorithm(populationSize, numGenerations );
    return 0;
}

