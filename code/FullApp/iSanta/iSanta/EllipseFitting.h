#pragma once

#include "OpenCV.framework/Headers/opencv2/opencv.hpp"
//#include "OpenCV.framework/Headers/opencv2/core/core_c.h"
//#include "OpenCV.framework/Headers/opencv2/highgui/highgui_c.h"
//#include "OpenCV.framework/Headers/opencv2/core/types_c.h"
#include "OpenCV.framework/Headers/opencv2/legacy/compat.hpp"



typedef struct{
    int x;
    int y;
} Point;

class ScoredPoint {
public:
	float score;
	Point center;
    
	ScoredPoint(float score, Point center) : score(score), center(center) { };
};

typedef struct _pointnode{
    ScoredPoint* Value;
    _pointnode* Next;
} PointNode;

typedef struct{
    PointNode* head;
} PointsLinkedList;


void AddBefore(PointsLinkedList* x,PointNode* y,ScoredPoint z);
void AddLast(PointsLinkedList* list,ScoredPoint sp);


	class EllipseFitting {
        
	public:
		static Point* ProcessImage(char* imagePath, int shotsFired, int pixelsPerRadius, CvRect ROI);
	};
