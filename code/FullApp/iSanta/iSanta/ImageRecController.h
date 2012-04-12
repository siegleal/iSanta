#pragma once

//#using "System.Drawing.dll"

//using namespace System;
//using namespace System::Collections::Generic;
//using namespace System::Drawing;

class ImageRecController {
	public:
		static List<Point>^ ProcessImage(String^ imagePath, int shotsFired, int pixelsPerRadius,
			int ROIx, int ROIy, int ROIheight, int ROIwidth);
	}
