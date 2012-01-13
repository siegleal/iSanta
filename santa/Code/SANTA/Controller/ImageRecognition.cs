using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ImageRecognition;

namespace SANTA.Controller
{
	/// <remarks>
	/// A class that performs image recognition on an image.
	/// </remarks>
	class ImageRecognition
	{
		/// <summary>
		/// Performs image recognition on an image returning a list of points where possible bullet holes are located.
		/// </summary>
		/// <param name="filename">The filename of the image.</param>
		/// <param name="shotsFired">The number of bullet holes to look for.</param>
		/// <param name="pixelsPerRadius">The approximate number of pixels per radius of a bullet hole.</param>
		/// <param name="ROIx">The x-coordinate of the top left corner of the region of interest.</param>
		/// <param name="ROIy">The y-coordinate of the top left corner of the region of interest.</param>
		/// <param name="ROIheight">The height in pixels of the region of interest.</param>
		/// <param name="ROIwidth">The width in pixels of the region of interest.</param>
		/// <returns>A list of pixel coordinates of possible bullet holes</returns>
		public static List<Point> ProcessImage(String filename, int shotsFired,
				int pixelsPerRadius, int ROIx, int ROIy, int ROIheight, int ROIwidth) {

			return ImageRecController.ProcessImage(filename, shotsFired, pixelsPerRadius, ROIx, ROIy, ROIheight, ROIwidth);
		}

		/// <summary>
		/// Tests the image recognition module.
		/// </summary>
		public static void TestImageRecognition() {
			// TODO:  Add ROI parameters
			List<Point> points = ProcessImage("C:\\Documents and Settings\\weavermn\\My Documents\\2008-2009\\Senior Project\\SVN\\Supplementary\\Sample Items\\Standard(9).jpg", 9, 9, 0, 0, 0, 0);
			foreach (Point point in points) {
				System.Console.WriteLine(point);
			}
		}
	}
}
