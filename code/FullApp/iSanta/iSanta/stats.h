
double Mean(double points[], int size);
double StandardDeviation(double points[], int size);
double PearsonProductMomentCorrelation( double x[], double y[], int size);
double MaximumSpread(double x[], double y[], int size);
double distance2d(double x, double y);

//Index i is the cluster, index j[0] is x, index j[1] is y of a point.
double* FindClusters (double x[], double y[], int size);