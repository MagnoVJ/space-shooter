#include <iostream>

using std::cout;
using std::cin;
using std::endl;

void aspectRatioCalculator(float, float, float);

int main(){

	float width;
	float height;
	float newWidth;

	cout << endl;

	cout << "Entre com o width atual: ";
	cin >> width;
	cout << "Entre com o height atual: ";
	cin >> height;
	cout << "Entre com o novo width: ";
	cin >> newWidth;

	cout << endl;

	aspectRatioCalculator(width, height, newWidth);

}

void aspectRatioCalculator(float width, float height, float newWidth){

	cout << "Formula: original height / original width * new width = new height"
		<< endl << endl;

	cout << "original width = " << width << endl
		<< "original height = " << height << endl
		<< "new width = " << newWidth << endl
		<< "new height =  " << height / width * newWidth;


	cout << endl;

}