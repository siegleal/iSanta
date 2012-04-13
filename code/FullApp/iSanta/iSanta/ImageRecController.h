#pragma once

//#using "System.Drawing.dll"

//using namespace System;
//using namespace System::Collections::Generic;
//using namespace System::Drawing;
#include "EllipseFitting.h"

class ImageRecController {
	public:
		static Point* ProcessImage(char* imagePath, int shotsFired, int pixelsPerRadius,
			int ROIx, int ROIy, int ROIheight, int ROIwidth);
};
