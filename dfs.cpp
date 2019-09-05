// compile & run:     g++ main.cpp -o main && ./main < input.txt > out.txt
//
// © Jose Garfias Lopez
// Maze found path
/*
    Maze values:
    -1 => origin
    -2 => destiny
    0 => free
    1 => blocked
*/
#include <cstdio>
#include <iostream>
#include <stack>
#include <vector>

using namespace std;

stack < pair<int,int> > Stack; // pair <int,int> -> pair<char,int>
vector < pair<int,int> > Path; // pair <int,int> -> pair<char,int>

int Filas = 10;
int Columnas = 7;

int Matriz[10][7] = {
    1,1,1,1,1,1,1,
    1,0,1,1,1,1,1,
    1,0,1,0,0,1,1,
    1,0,1,0,1,1,1,
    1,0,1,0,1,1,1,
    1,-1,0,0,0,-2,1,
    1,1,1,0,0,0,1,
    1,1,1,0,0,0,1,
    1,1,0,0,0,0,1,
    1,1,1,1,1,1,1,
};

int FILS[4] = { -1, 0, 1, 0 };
int COLS[4] = { 0, 1, 0, -1};

bool DFS(pair<int,int> P) {
    Matriz[P.first][P.second] = 1; // ya la estamos procesando;
    Stack.push(P); // insertar coordenadas;
    int pathPosition = 0;
    while(!Stack.empty()) {
        pair<int, int> UltimoElemento = Stack.top();
        Stack.pop();
        int cells = 0;
        Path.push_back(UltimoElemento);
        for (int m=0; m<4; m++) {
            int f = UltimoElemento.first + FILS[m];
            int c = UltimoElemento.second + COLS[m];
            if (Matriz[f][c] == 0) {
                Matriz[f][c] = 1;
                pair<int,int> P = make_pair(f,c);
                Stack.push(P);
                cells++;
            } else if (Matriz[f][c] == -2) {
                // encontré la salida.
                pair<int,int> P = make_pair(f,c);
                Path.push_back(P);
                return true;
            }
        }
        if (cells > 1) {
            // encontró mas de una bifurcación
            pathPosition = Path.size() - 1;
            cout<<"Bifurcación en posicion de path: "<<pathPosition<<endl;
            cout<<"["<< UltimoElemento.first << ", "<< UltimoElemento.second<< "]"<<endl;
        }
        if (cells == 0) {
            // el camino no tiene salida.
            vector < pair<int,int> > newPath (Path.size() - 1);
            for (int i=0; i<=pathPosition; i++) {
                newPath[i] = Path[i];
            }
            Path = newPath;
        }

    }
    return false;
}

int Procesar() {
    bool foundEnd = false;
    for (int i=1; i<=Filas; i++) {
        for (int j=1; j<=Columnas; j++) {
            if (Matriz[i][j] == -1) { // origin
                pair<int,int> origin = make_pair(i,j);
                return DFS(origin);
            }
        }
    }
    return false;
}

int main() {
    
    if (Procesar()) {
        cout<<"YES"<<endl;
    } else {
        cout<<"NO"<<endl;
    }

    for (int i=0; i<Path.size(); i++) {
        pair<int,int> p = Path[i];
        cout<<"["<< p.first << ", "<< p.second<< "]"<<endl;
    }

    return (0);
}
