//#include "highgui.h"
//#include "cv.h"
#include "EllipseFitting.h"




// Threshold value
const int threshold = 80;



// Computes the average distance from the circle of an array of points
float computeError(CvPoint center, int radius, CvPoint2D32f* points, int count)
{
	float error = 0;
	int i;
	for (i = 0; i < count; i++) { // for each of the points
		float x = (float)points[i].x;
		float y = (float)points[i].y;
		// Compute the distance from the center of the circle
		float dist = sqrt((center.x - x) * (center.x - x) + (center.y - y) * (center.y - y));
		// Add the distance from the point to the circle
		error += fabs(dist - radius);
	}
	// Return the average error
	return error / count;
}

void AddBefore(PointsLinkedList* list, PointNode* node, ScoredPoint point){
    
}

void AddLast(PointsLinkedList* list, ScoredPoint point){
    
}

// Begins bullet-hole recognition
Point* EllipseFitting::ProcessImage(char* imagePath, int numShots, int pixelsPerRadius, CvRect ROI)
{
	char* path = imagePath;
	IplImage* src = cvLoadImage(path);
	cvSetImageROI(src, ROI);
	

	// Create dynamic structure and sequence
	CvMemStorage* stor = cvCreateMemStorage(0);
	CvSeq* cont = cvCreateSeq(CV_SEQ_ELTYPE_POINT, sizeof(CvSeq), sizeof(CvPoint), stor);

	// Create a grayscale image of the source image
	IplImage* graysrc = cvCreateImage(cvSize(src->width, src->height), 8, 1);
	cvSetImageROI(graysrc, ROI);
	cvCvtColor(src, graysrc, CV_BGR2GRAY);

	// Smooth the image to remove noise
	cvSmooth(graysrc, graysrc, CV_GAUSSIAN, 7, 7);

	// Apply threshold for edge detection
	IplImage* thresholdImg = cvCreateImage(cvSize(src->width, src->height), src->depth, 1);
	cvSetImageROI(thresholdImg, ROI);
	cvThreshold(graysrc, thresholdImg, threshold, 255, CV_THRESH_BINARY);

	// Erode then dilate by a 3 x 3 rectangular structuring element
	cvErode(thresholdImg, thresholdImg);
	cvDilate(thresholdImg, thresholdImg);

	// Find contours and store them all as a list
	cvFindContours(thresholdImg, stor, &cont, sizeof(CvContour),
		CV_RETR_LIST, CV_CHAIN_APPROX_SIMPLE, cvPoint(0,0));

	PointsLinkedList scoredPoints;

	// This cycle draw the contour and approximate it by an ellipse
	for (; cont; cont = cont->h_next) {
		int count = cont->total; // the number of points in the contour

		// The number of points must be more than or equal to 6 (for cvFitEllipse_32f)
		if (count < 6) {
			continue;
		}

		// Allocate memory for contour point set
		CvPoint* PointArray = (CvPoint*)malloc(count*sizeof(CvPoint));
		CvPoint2D32f* PointArray2D32f = (CvPoint2D32f*)malloc(count*sizeof(CvPoint2D32f));

		// Allocate memory for ellipse data
		CvBox2D32f* box = (CvBox2D32f*)malloc(sizeof(CvBox2D32f));

		// Get the contour point set
		cvCvtSeqToArray(cont, PointArray, CV_WHOLE_SEQ);

		// Convert CvPoint set to CvBox2D32f set
		int i;
		for (i = 0; i < count; i++) {
			PointArray2D32f[i].x = (float)PointArray[i].x;
			PointArray2D32f[i].y = (float)PointArray[i].y;
		}

		// Fit ellipse to current contour
		cvFitEllipse(PointArray2D32f, count, box);

		// Convert ellipse data from float to integer representation
		CvPoint center;
		CvSize size;
		center.x = cvRound(box->center.x);
		center.y = cvRound(box->center.y);
		size.width = cvRound(box->size.width * 0.5);
		size.height = cvRound(box->size.height * 0.5);
		box->angle = -box->angle;

		// Compute the error of the ellipse from the expected bullet hole
		float error = computeError(center, pixelsPerRadius, PointArray2D32f, count);
		Point p;
        p.x = center.x + ROI.x;
        p.y = center.y + ROI.y;
		ScoredPoint scoredPoint = *new ScoredPoint(error, p);

		// Add the ellipse to the list of possible bullet holes
		PointNode* current = scoredPoints.head;
		bool added = false;
		while (current != NULL) {
			if (error <= current->Value->score) {
				AddBefore(&scoredPoints,current, scoredPoint);
				added = true;
				break;
			}
			current = current->Next;
		}

		// Add the scored point to the end if it hasn't been added yet
		if (!added) {
			AddLast(&scoredPoints,scoredPoint);
		}

		// Free memory
		free(PointArray);
		free(PointArray2D32f);
		free(box);
	}

	// Pick the top numShots bullet holes to return
    //find the size of the list
    PointNode* current = scoredPoints.head;
    int listSize = 0;
    while (current != NULL){
        listSize++;
        current = current->Next;
    }
    Point* points = (Point*)malloc(listSize * sizeof(Point));
    
    
    
	current = scoredPoints.head;
	for (int i = 0; i < numShots && current != NULL; i++, current = current->Next) {
		points[i] = (current->Value->center);
	}

	// Free memory
	cvReleaseImage(&graysrc);
	cvReleaseImage(&thresholdImg);
	cvReleaseMemStorage(&stor);

	return points;
}
