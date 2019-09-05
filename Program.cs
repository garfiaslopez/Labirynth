using System.Linq;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

/*
    Maze values:
    -1 => origin
    -2 => destiny
     0 => free
     1 => blocked

     dotnet run
 */

namespace labyrinth
{
    public class Pair<T, U> {
        public Pair() {
        }
        public Pair(T first, U second) {
            this.First = first;
            this.Second = second;
        }
        public T First { get; set; }
        public U Second { get; set; }
    };
    class Program
    {
        static StreamWriter file = new StreamWriter(@"Output.txt", false);
        static int[] FILS = new int[4] { -1, 0, 1, 0 };
        static int[] COLS = new int[4] { 0, 1, 0, -1};
        
        static int[,] Matrix = new int[102, 102];
        static char[,] MatrixResult = new char[102, 102];
        static int maxFil = 0;
        static int maxCol = 0;

        static Stack< Pair<int,int> > StackOfCoords = new Stack< Pair<int,int> >();
        static List< Pair<int,int> > Path = new List< Pair<int,int> >();
        static Pair<int,int> origin = new Pair<int, int>(0,0);
        static Pair<int,int> destiny = new Pair<int, int>(0,0);

        static void readLabyrinth() {
            string[] lines = File.ReadAllLines("./Input.txt");
            maxFil = lines.Length;
            int i = 1;
            foreach (string line in lines) {
                string[] numbersInRow = line.Split(',');
                if (maxCol == 0) {
                    maxCol = numbersInRow.Length;
                }
                for (int j = 1; j<= maxCol; j++) {
                    Matrix[i,j] = Int32.Parse(numbersInRow[j - 1]);
                }
                i++;
            }
        }

        static void printLabyrinth() {
            Console.WriteLine("-------------------");
            Console.WriteLine("Filas:   " + maxFil);
            Console.WriteLine("Coumnas: " + maxCol);
            Console.WriteLine("-------------------");
            for (int i=0; i<=maxFil + 1; i++) {
                for (int j=0; j<=maxCol + 1; j++) {
                    Console.Write(Matrix[i,j]);
                    Console.Write(" ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("-------------------");
        }
        static void borderLabyrinth() {
            for (int i=0; i<=maxFil + 1; i++) {
                Matrix[i, 0] = 1;
                Matrix[i, maxCol + 1] = 1;
            }
            for (int i=0; i<=maxCol + 1; i++) {
                Matrix[0,i] = 1;
                Matrix[maxFil + 1, i] = 1;
            }
        }

        static bool DFS(Pair<int,int> origin) {
            Matrix[origin.First, origin.Second] = 1;
            StackOfCoords.Push(origin);
            int pathPosition = 0;
            while(StackOfCoords.Count > 0) {
                Pair<int,int> LastCell = StackOfCoords.Pop();
                int cells = 0;
                Path.Add(LastCell);
                for (int m=0; m<4; m++) { // explorar vecinos
                    int f = LastCell.First + FILS[m];
                    int c = LastCell.Second + COLS[m];
                    if (Matrix[f,c] == 0) { // posible vecino a explorar
                        Matrix[f,c] = 1;
                        Pair<int,int> P = new Pair<int, int>(f,c);
                        StackOfCoords.Push(P);
                        cells++;
                    } else if (Matrix[f,c] == -2) { // encontré la salida.
                        Pair<int,int> P = new Pair<int, int>(f,c);
                        destiny = P;
                        Path.Add(P);
                        return true;
                    }
                }
                if (cells > 1) { // si encontramos una bifurcación
                    pathPosition = Path.Count - 1;
                }
                if (cells == 0) { // si estamos en un camino sin salida
                    List< Pair<int,int> > newPath = new List< Pair<int,int> > (Path.Count - 1);
                    newPath.AddRange(Enumerable.Repeat(new Pair<int,int>(), (Path.Count - 1)));
                    for (int i=0; i<=pathPosition; i++) {
                        newPath[i] = Path[i];
                    }
                    Path = newPath;
                }
            }
            return false;
        }

        static bool foundLabyrinthExit() {
            for (int i=1; i<=maxFil; i++) {
                for (int j=1; j<=maxCol; j++) {
                    if (Matrix[i,j] == -1) {
                        origin = new Pair<int, int>(i,j);
                        return DFS(origin);
                    }
                }
            }
            return false;
        }

        static void printPath() {
            for (int i=0; i<Path.Count; i++) {
                Pair<int,int> p = Path[i];
                file.WriteLine("[" + p.First + ", " + p.Second + "]");
            }
        }

        static void printPathOnLabyrinth() {
            for (int i=1; i<=maxFil; i++) {
                for (int j=1; j<=maxCol; j++) {
                    MatrixResult[i,j] = 'X';
                }
            }
            for (int i=0; i<Path.Count; i++) {
                Pair<int,int> p = Path[i];
                MatrixResult[p.First, p.Second] = 'O';
            }
            MatrixResult[origin.First, origin.Second] = 'I';
            MatrixResult[destiny.First, destiny.Second] = 'F';
            for (int i=1; i<=maxFil; i++) {
                for (int j=1; j<=maxCol; j++) {
                    file.Write(MatrixResult[i,j]);
                    file.Write(" ");
                }
                file.Write("\n");
            }
        }

        static void Main(string[] args) {
            readLabyrinth();
            borderLabyrinth();
            if (foundLabyrinthExit()) {
                file.WriteLine("El camino para salir del laberinto es:");
                printPath();
                // printLabyrinth();
                file.WriteLine("------------MAPA:------------");
                printPathOnLabyrinth();
            } else {
                file.WriteLine("No hay salida del laberinto :(");
            }
            file.Close();
        }
    }
}
