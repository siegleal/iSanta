#include "stats.h"
#include <stdio.h>
#include <math.h>


double Mean(double points[], int size){
    int i;
    double xbar=0;
for(i=0;i<size;i++){
	xbar=xbar+points[i];
	}
xbar=xbar/size;
return xbar;
}

//This gets you the mean and standard deviation. Nothing special here.
double StandardDeviation(double points[], int size){
int i;
float xbar=0;
double ans;
xbar=Mean(points, size);
double num=0;
for(i=0;i<size;i++){
	num=num+pow((points[i]-xbar),2);
	}	
double standardDeviation= sqrt(num/size);
ans=standardDeviation;
return ans;
}

//This tells you how corrolated your x and y are. A straight line, at any non0 angle, give you a corrolation of 1. 
//If this is nonzero, it means the points vary more in one direction than in another. This number should be close to 0.
double PearsonProductMomentCorrelation(double x[], double y[], int size)
{
	//Base case
    if( size<=1 )
    {
        return 0;
    }
    double ans;
    double xmean;
    double ymean;
    xmean = Mean(x, size);
	ymean=Mean(y, size);
    int i;
   double s;
    double xv;
    double yv;
    double t1;
    double t2;
    s = 0;
    xv = 0;
    yv = 0;
    for(i=0; i<size; i++)
    {
        t1 = abs(x[i]-xmean);
        t2 = abs(y[i]-ymean);
        xv = xv+sqrt(t1);
        yv = yv+sqrt(t2);
        s = s+t1*t2;
    }
	
        ans = s/sqrt(xv)*sqrt(yv);
    
    return ans;
}

double MaximumSpread(double x[], double y[], int size){
int i;
int ans=0;
if(i<=1) return 0;
for(i=0;i<size;i++){
double distance=distance2d(x[i], y[i]);
if(distance>ans){
ans=distance;
}
}
return ans;

}

double distance2d(double x, double y){
double ans= sqrt(pow(x,2) + pow(y,2));
return ans;
}

double* FindClusters (double x[], double y[], int size){
//This algorithm http://www.ncbi.nlm.nih.gov/pmc/articles/PMC2655859/
    return NULL;
}


