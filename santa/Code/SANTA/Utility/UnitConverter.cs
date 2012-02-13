using System;
using System.Collections.Generic;
using System.Text;
using SANTA.IO;
using System.Drawing;
using SANTA.Controller;
using System.Drawing.Drawing2D;

namespace SANTA.Utility
{
    class UnitConverter
    {
        /// <summary>
        /// Given Pixel Data, convert the data to inches, generate new statists based on them, and generate the report based on the new data.
        /// </summary>
        /// <param name="data">Data points in pixels.</param>
        /// <param name="stats">Statistics in pixels.</param>
        /// <param name="configReader"></param>
        public static void GenerateReport(Datatype.ImageData data, Datatype.Stats stats, ConfigReader configReader)
        {
            IList<Datatype.DoublePoint> newPoints = convertToInches(data, stats);
            newPoints = verticallyFlip(newPoints);
            newPoints = translatePoints(newPoints);

            //To test the GenerateReport Class, add points from a known set of statistics into the 
            //following field.  Then any report generated will be from this set.
            /*newPoints = new List<Datatype.DoublePoint>();
            newPoints.Add(new Datatype.DoublePoint(-6, 1.5));
            newPoints.Add(new Datatype.DoublePoint(0, -1.4));
            newPoints.Add(new Datatype.DoublePoint(-0.3, 1.6));
            newPoints.Add(new Datatype.DoublePoint(-3.6, -4.2));
            newPoints.Add(new Datatype.DoublePoint(1.1, -1.5));
            newPoints.Add(new Datatype.DoublePoint(-3.4, 0.2));
            newPoints.Add(new Datatype.DoublePoint(0.7, -2.4));
            newPoints.Add(new Datatype.DoublePoint(-3.8, 3.6));
            newPoints.Add(new Datatype.DoublePoint(-0.5, -3.2));
            newPoints.Add(new Datatype.DoublePoint(2.7, -2.4));


            newPoints = translatePoints(newPoints);*/

            Datatype.Stats newStats = Stats.GenerateStatistics(newPoints);

            try
            {
                ExcelReportGenerator.GenerateReport(newPoints, data, newStats, configReader);
            }
            catch (System.InvalidOperationException)
            {
                //Do nothing for now.
            }
        }

        //Translate so center is at the origin.
        private static IList<Datatype.DoublePoint> translatePoints(IList<Datatype.DoublePoint> pointList)
        {
            Datatype.DoublePoint center = Stats.ComputeCenter(pointList);
            Datatype.DoublePoint temp = new Datatype.DoublePoint();
            for (int i = 0; i < pointList.Count; i++ )
            {
                temp.X = pointList[i].X - center.X;
                temp.Y = pointList[i].Y - center.Y;
                pointList[i] = temp;
            }
            return pointList;
        }

        /// <summary>
        /// Given a set of datapoints, convert them to inches and return the points.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static IList<Datatype.DoublePoint> convertToInches(Datatype.ImageData data, Datatype.Stats stats)
        {
            Point oldX = data.scale.horizontal;
            Point oldY = data.scale.vertical;
            Point oldOrigin = data.scale.middle;

            //Change so oldX and oldY are in reference to origin of (0, 0)
            oldX = pointSubtract(oldX, oldOrigin);
            oldY = pointSubtract(oldY, oldOrigin);

            //Declare new points.  Data.scale.___length is the scale and origin of (0,0)
            Datatype.DoublePoint inchesX = new Datatype.DoublePoint(data.scale.horizontalLength, 0);
            Datatype.DoublePoint inchesY = new Datatype.DoublePoint(0, data.scale.verticalLength);
            Datatype.DoublePoint inchesOrigin = new Datatype.DoublePoint(0, 0);

            inchesX = pointSubtract(inchesX, inchesOrigin);
            inchesY = pointSubtract(inchesY, inchesOrigin);

            //Convert by using transformation matrix A:
            //A * oldPoint = newPoint
            //A = [ a  b
            //      c  d ]
            
            //Assuming oldX, oldY, newX, and newY are all measured from an origin of 0:
            double a = (double)inchesX.X / (double)oldX.X;
            double b = (double)inchesX.Y / (double)oldX.X;
            double c = (double)inchesY.X / ((double)oldY.Y - ((double)oldY.X / (double)oldX.X) * (double)oldX.Y);
            double d = (double)inchesY.Y / ((double)oldY.Y - ((double)oldY.X / (double)oldX.X) * (double)oldX.Y);

            //Convert each point by multiplying by matrix A:
            // [a b  *  [oldX  = [newX
            //  c d]     oldY]    newY]
            Datatype.DoublePoint doublePoint = new Datatype.DoublePoint();
            IList<Datatype.DoublePoint> newPoints = new List<Datatype.DoublePoint>();

            for (int point = 0; point < data.points.Count; point++)
            {
                doublePoint.X = a * data.points[point].X + c * data.points[point].Y;
                doublePoint.Y = b * data.points[point].X + d * data.points[point].Y;
                newPoints.Add(doublePoint);
            }

            return newPoints;
        }

        /// <summary>
        /// Given a set of datapoints, vertically flip them.
        /// </summary>
        /// <param name="newData"></param>
        /// <returns></returns>
        private static IList<Datatype.DoublePoint> verticallyFlip(IList<Datatype.DoublePoint> dataPoints)
        {
            Datatype.DoublePoint newPoint = new Datatype.DoublePoint();
            for (int point = 0; point < dataPoints.Count; point++)
            {
                newPoint.X = dataPoints[point].X;
                newPoint.Y = dataPoints[point].Y;

                dataPoints[point] = newPoint;
            }
            return dataPoints;
        }

        private static Point pointSubtract (Point minuend, Point subtrahend) {
            return new Point(minuend.X - subtrahend.X, minuend.Y - subtrahend.Y);
        }

        private static Datatype.DoublePoint pointSubtract(Datatype.DoublePoint minuend, Datatype.DoublePoint subtrahend)
        {
            return new Datatype.DoublePoint(minuend.X - subtrahend.X, minuend.Y - subtrahend.Y);
        }
    }
}
