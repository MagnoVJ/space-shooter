#include <iostream>
#include <stdio.h>

using std::cout;
using std::endl;

int main(){ 

	char file[] = "C:\\Users\\Magno\\AppData\\LocalLow\\MagnoVJ\\Space Shooter\\scoreInfo.dat";

	if(remove(file) != 0)
		cout << "Erro: " << endl;
	else
		cout << "OK" << endl;

}