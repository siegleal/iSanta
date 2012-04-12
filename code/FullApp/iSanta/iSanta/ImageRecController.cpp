//#include "cv.h"
#include "ImageRecController.h"
#include "EllipseFitting.h"
//#include <OpenCV>

//using namespace System;
//using namespace System::Collections::Generic;
//using namespace System::Drawing;
//using namespace ImageRecognition;

List<Point>^ ImageRecController::ProcessImage(String^ imagePath, int shotsFired, int pixelsPerRadius,
											  int ROIx, int ROIy, int ROIheight, int ROIwidth) {
	CvRect ROI;
	ROI.x = ROIx;
	ROI.y = ROIy;
	ROI.height = ROIheight;
	ROI.width = ROIwidth;
	return EllipseFitting::ProcessImage(imagePath, shotsFired, pixelsPerRadius, ROI);
}
