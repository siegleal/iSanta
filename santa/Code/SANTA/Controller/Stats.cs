using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using SANTA.IO;

namespace SANTA.Controller
{
	/// <remarks>
	/// A class that performs statistical calculations.
	/// </remarks>
	class Stats
	{
		/// <summary>
		/// Computes the number of image pixels per radius of a single bullet hole.
		/// </summary>
		/// <param name="caliberUnit">The units the caliber is measured in.</param>
		/// <param name="scale">The scale information for the image.</param>
		/// <param name="caliberValue">The magnitude of the caliber in <c>caliberUnit</c>s.</param>
		/// <returns>The number of image pixels per radius of a single bullet hole.</returns>
		/// <exception cref="ArgumentException">Thrown if there is a problem with the scale or caliber.</exception>
		public static int PixelsPerRadius(Datatype.CaliberUnit caliberUnit, Datatype.Scale scale,
				double caliberValue) {
			if (scale.horizontalLength == 0) {
				Log.LogError("Horizontal scale length of zero.", "horizontalLength", scale.horizontalLength.ToString());
				throw new ArgumentException("Horizontal scale length of zero.", "horizontalLength");
			} else if (scale.verticalLength == 0) {
				Log.LogError("Vertical scale length of zero.", "verticalLength", scale.verticalLength.ToString());
				throw new ArgumentException("Vertical scale length of zero.", "verticalLength");
			} else if (scale.middle.Equals(scale.horizontal)) {
				Log.LogError("No horizontal distance for scaling.", "middle", scale.middle.ToString(),
					"horizontal", scale.horizontal.ToString());
				throw new ArgumentException("Horizontal and middle scale points overlap.", "scale");
			} else if (scale.middle.Equals(scale.vertical)) {
				Log.LogError("No vertical distance for scaling.", "middle", scale.middle.ToString(),
					"vertical", scale.vertical.ToString());
				throw new ArgumentException("Vertical and middle scale points overlap.", "scale");
			} else if (caliberValue <= 0) {
				Log.LogError("Zero caliber size.", "caliberValue", caliberValue.ToString());
				throw new ArgumentException("Zero caliber size.", "caliberValue");
			} else if (caliberUnit.unitsPerInch <= 0) {
				Log.LogError("Caliber unit of zero units per inch.", "unitID",
					caliberUnit.caliberUnitID.ToString(), "unitName", caliberUnit.unitName,
					"unitsPerInch", caliberUnit.unitsPerInch.ToString());
				throw new ArgumentException("Caliber unit of zero units per inch.", "caliberUnit"); 
			}

			double dx = DistanceBetween(scale.middle, scale.horizontal) / scale.horizontalLength;
			double dy = DistanceBetween(scale.middle, scale.vertical) / scale.verticalLength;
			double avg = (dx + dy) / 2.0;
			// TODO:  Change to PixelsPerDiameter
			return (int)(avg * caliberValue / caliberUnit.unitsPerInch / 2 + 0.5);
		}

        public static Datatype.Stats GenerateStatistics(IList<Datatype.DoublePoint> points)
        {
            Datatype.Stats stats = new Datatype.Stats();
			stats.center = ComputeCenter(points);
			stats.meanRadius = ComputeMeanRadius(points, stats.center);
			stats.sigmaX = CalculateSigmaValue(points, 'x');
			stats.sigmaY = CalculateSigmaValue(points, 'y');
			stats.extremeSpread = ComputeExtremeSpread(points, stats.center);
		    stats.CEPRadius = ComputeCEPRadius(stats.center, points);
		    stats.furthestLeft = findFurthestLeft(points);
		    stats.furthestRight = findFurthestRight(points);
		    stats.highestRound = findHighestPoint(points);
		    stats.lowestRound = findLowestPoint(points);
		    stats.extremeSpreadX = stats.furthestRight - stats.furthestLeft;
		    stats.extremeSpreadY = stats.highestRound - stats.lowestRound;
		    stats.extremePoints = getExtremePoints(points);

			return stats;
        }

		public static Datatype.Stats GenerateStatistics(IList<Point> points) {
			if (points == null) {
                Log.LogError("GenerateStatictics received no points.", "points");
				throw new ArgumentNullException();
			}

            IList<Datatype.DoublePoint> newPoints = new List<Datatype.DoublePoint>();
            Datatype.DoublePoint newDoublePoint = new Datatype.DoublePoint();

            //Convert Points to DoublePoints
            foreach (Point point in points)
            {
                newDoublePoint.X = point.X;
                newDoublePoint.Y = point.Y;

                newPoints.Add(newDoublePoint);
            }

            return Stats.GenerateStatistics(newPoints);
		}

        private static Datatype.DoublePoint[] getExtremePoints(IList<Datatype.DoublePoint> points)
        {
            Datatype.DoublePoint[] returnPoints = new Datatype.DoublePoint[2];
            double distanceBetween = 0; // This is the minimum it could be.

            if (points == null || points.Count < 2)
            {
                return returnPoints;
            }

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i; j < points.Count; j++)
                {
                    if (DistanceBetween(points[i], points[j]) > distanceBetween)
                    {
                        distanceBetween = DistanceBetween(points[i], points[j]);
                        returnPoints[0] = points[i];
                        returnPoints[1] = points[j];
                    }
                }
            }

            return returnPoints;
        }

        private static double ComputeCEPRadius(Datatype.DoublePoint centerPoint, IList<Datatype.DoublePoint> originalPoints)
        {
            int CEPCount;
            double CEPRadius = 0;

            //Create temp array to pull from:
            IList<Datatype.DoublePoint> points = new List<Datatype.DoublePoint>(originalPoints.Count);

            foreach (Datatype.DoublePoint point in originalPoints)
            {
                points.Add(point);
            }

            if (points.Count % 2 == 0)
            {
                CEPCount = points.Count/2;
            }
            else
            {
                CEPCount = (points.Count + 1)/2;
            }

            for (int numberWithin = 0; numberWithin < CEPCount; numberWithin++)
            {
                //Find closest point
                int indexOfClosestPoint = findClosestPoint(centerPoint, points);

                //set CEPRadius to the distance to that point
                CEPRadius = DistanceBetween(centerPoint, points[indexOfClosestPoint]);

                //Remove it
                points.RemoveAt(indexOfClosestPoint);
            }

            return CEPRadius;
        }

        private static int findClosestPoint(Datatype.DoublePoint centerPoint, IList<Datatype.DoublePoint> points)
        {
            // If null, return -1.
            if (points == null)
            {
                return -1;
            }

            int indexToReturn = 0;
            double distanceOfClosestPoint = DistanceBetween(centerPoint, points[0]);
            
            //If there is one item, return it.
            if (points.Count == 1)
            {
                return indexToReturn;
            }

            for (int indexCounter = 1; indexCounter < points.Count; indexCounter++)
            {
                if (DistanceBetween(points[indexCounter], centerPoint) < distanceOfClosestPoint)
                {
                    indexToReturn = indexCounter;
                    distanceOfClosestPoint = DistanceBetween(points[indexCounter], centerPoint);
                }
            }

            return indexToReturn;
        }

        private static double findFurthestLeft(IList<Datatype.DoublePoint> points)
        {
            double furthestLeftValue;

            if (points == null || points.Count == 0)
            {
                return 0;
            }

            furthestLeftValue = points[0].X;

            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X < furthestLeftValue)
                {
                    furthestLeftValue = points[i].X;
                }
            }

            return furthestLeftValue;
        }

        private static double findFurthestRight(IList<Datatype.DoublePoint> points)
        {
            double furthestRightValue;

            if (points == null || points.Count == 0)
            {
                return 0;
            }

            furthestRightValue = points[0].X;

            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].X > furthestRightValue)
                {
                    furthestRightValue = points[i].X;
                }
            }

            return furthestRightValue;
        }

        private static double findHighestPoint(IList<Datatype.DoublePoint> points)
        {
            double highestPointValue;

            if (points == null || points.Count == 0)
            {
                return 0;
            }

            highestPointValue = points[0].Y;

            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y > highestPointValue)
                {
                    highestPointValue = points[i].Y;
                }
            }

            return highestPointValue;
        }

        private static double findLowestPoint(IList<Datatype.DoublePoint> points)
        {
            double lowestPointValue;

            if (points == null || points.Count == 0)
            {
                return 0;
            }

            lowestPointValue = points[0].Y;

            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y < lowestPointValue)
                {
                    lowestPointValue = points[i].Y;
                }
            }

            return lowestPointValue;
        }

        public static Datatype.DoublePoint ComputeCenter(IList<Datatype.DoublePoint> points)
        {
            Datatype.DoublePoint tempPoint = new Datatype.DoublePoint();
            if (points.Count == 0)
            {
                tempPoint.X = 0;
                tempPoint.Y = 0;
                return tempPoint;
            }
            Datatype.DoublePoint center = new Datatype.DoublePoint();
            center.X = points[0].X;
            center.Y = points[0].Y;

			for (int i = 1; i < points.Count; i++) {
				center.X += points[i].X;
				center.Y += points[i].Y;
			}

			center.X /= points.Count;
			center.Y /= points.Count;

			return center;
		}

        protected static double ComputeMeanRadius(IList<Datatype.DoublePoint> points, Datatype.DoublePoint center)
        {
			double meanRadius = 0;
			for (int i = 0; i < points.Count; i++) {
				meanRadius += DistanceBetween(points[i], center);
			}

			meanRadius /= points.Count;
			return meanRadius;
		}

        protected static double CalculateSigmaValue(IList<Datatype.DoublePoint> pts, char variableToCalculateOn)
        {
			double[] element;
			element = new double[pts.Count];
			if (variableToCalculateOn.CompareTo('x') == 0) {
				for (int x = 0; x < pts.Count; x++) {
					element[x] = pts[x].X;
				}
			} else if (variableToCalculateOn.CompareTo('y') == 0) {
				for (int y = 0; y < pts.Count; y++) {
					element[y] = pts[y].Y;
				}
			}

			return StandardDeviation(element);
		}

        protected static double ComputeExtremeSpread(IList<Datatype.DoublePoint> pts, Datatype.DoublePoint center)
        {
            Datatype.DoublePoint[] dXValues = new Datatype.DoublePoint[pts.Count];
			double spread = 0;

			for (int i = 0; i < pts.Count; i++) {
				dXValues[i].X = pts[i].X - center.X;
				dXValues[i].Y = pts[i].Y - center.Y;
			}

			for (int x = 0; x < pts.Count - 1; x++) {
				for (int y = x; y < pts.Count; y++) {
					if (x == 0 && y == 0) {
						spread = DistanceBetween(dXValues[x], dXValues[y]);
					} else {
						spread = Math.Max(spread, DistanceBetween(dXValues[x], dXValues[y]));
					}
				}
			}

			return spread;
		}

        protected static double DistanceBetween(Datatype.DoublePoint point1, Datatype.DoublePoint point2)
        {
            double diffX = point1.X - point2.X;
            double diffY = point1.Y - point2.Y;
            return Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));
        }

        protected static double DistanceBetween(Point point1, Point point2)
        {
            double diffX = point1.X - point2.X;
            double diffY = point1.Y - point2.Y;
            return Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));
        }

		/// <summary>
		/// Method used to calculate the standard deviation of a number.
		/// Algorithm taken from site: http://www.eggheadcafe.com/articles/standard_deviation_dotnet.asp
		/// </summary>
		/// <param name="data">Array of values to calculate for.</param>
		/// <returns>Returns the standard deviation of the array.</returns>
		protected static double StandardDeviation(double[] data) {
			double dataAverage = 0;
			double totalVariance = 0;
			int max = data.Length;

			if (max == 0) {
				return 0;
			}

			dataAverage = Average(data);

			for (int i = 0; i < max; i++) {
				totalVariance += Math.Pow(data[i] - dataAverage, 2);
			}

			return Math.Sqrt(SafeDivide(totalVariance, max));
		}

		protected static double Average(double[] data) {
			double dataTotal = 0;

			for (int i = 0; i < data.Length; i++) {
				dataTotal += data[i];
			}

			return SafeDivide(dataTotal, data.Length);
		}

		protected static double SafeDivide(double value1, double value2) {
			if ((value1 == 0) || (value2 == 0)) {
				return 0;
			}

			return value1 / value2;
		}
	}
}
