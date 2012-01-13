#pragma once

#using "System.Drawing.dll"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Drawing;

namespace ImageRecognition
{
	public ref class EllipseFitting {
	public:
		static List<Point>^ ProcessImage(String^ imagePath, int shotsFired, int pixelsPerRadius, CvRect ROI);
	};
}