using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SANTA.UI
{
    /// <summary>
    /// Image box for measuring the scaling (converting physical lengths into
    /// pixels) and determining the region of interest for the image recognition.
    /// </summary>
    public class MeasureBox : ZPImageBox
    {
        private const float POINT_SIZE = 8F;

        private GrabbedPoint _grabbedPoint;

        private Point _reference, _topLeft, _bottomRight;

        /// <summary>
        /// The current region of interest (green box)
        /// </summary>
        public Datatype.ROI RegionOfInterest
        {
            get
            {
                Datatype.ROI roi;
                roi.topLeft = _topLeft;
                roi.bottomRight = _bottomRight;
                return roi;
            }
            set
            {
                _topLeft = value.topLeft;
                _bottomRight = value.bottomRight;
            }
        }

        private Datatype.Scale _scaling;

        /// <summary>
        /// The current scale indicator (L-shaped thing)
        /// </summary>
        public Datatype.Scale Scaling
        {
            get { return _scaling; }
            set { _scaling = value; }
        }

        /// <summary>
        /// Construct a new MeasureBox
        /// </summary>
        public MeasureBox()
        {
            ResetRules();

            SetEventHandlers();
        }

        private void SetEventHandlers()
        {
            this.Paint += new PaintEventHandler(DrawRule);

            this.MouseDown += new MouseEventHandler(MeasureBox_MouseDown);
            this.MouseUp   += new MouseEventHandler(MeasureBox_MouseUp);
            this.MouseMove += new MouseEventHandler(MeasureBox_MouseMove);
        }

        /// <summary>
        /// Should the vertical and horizontal scaling components be kept
        /// perpendicular
        /// </summary>
        public bool PerpendicularScale { get; set; }

        public void ResetRules()
        {
            _scaling.vertical = new Point(100, 100);
            _scaling.middle = new Point(100, 200);
            _scaling.horizontal = new Point(200, 200);
        }

        private void DrawRule(object sender, PaintEventArgs e)
        {
            if (_image != null && !_multipleFiles)
            {
                float linesize = 2.0F / _zoom,
                    dotsize = POINT_SIZE / _zoom;

                // Vertical scale line
                e.Graphics.DrawLine(new Pen(Color.Red, linesize),
                    _scaling.vertical, _scaling.middle);
                // Horizontal scale line
                e.Graphics.DrawLine(new Pen(Color.Blue, linesize),
                    _scaling.horizontal, _scaling.middle);

                // Circle at end of horizontal scale line
                e.Graphics.FillEllipse(new SolidBrush(Color.Blue),
                    _scaling.horizontal.X - dotsize,
                    _scaling.horizontal.Y - dotsize, 2 * dotsize, 2 * dotsize);
                // Circle connecting horizontal and vertical
                e.Graphics.FillEllipse(new SolidBrush(Color.Yellow),
                    _scaling.middle.X - dotsize, _scaling.middle.Y - dotsize,
                    2 * dotsize, 2 * dotsize);
                // Circle at end of vertical scale line
                e.Graphics.FillEllipse(new SolidBrush(Color.Red),
                    _scaling.vertical.X - dotsize, _scaling.vertical.Y - dotsize,
                    2 * dotsize, 2 * dotsize);

                // Draw region of interest
                e.Graphics.DrawLines(new Pen(Color.DarkGreen, linesize),
                    new Point[] {
                        _topLeft,
                        new Point(_bottomRight.X, _topLeft.Y),
                        _bottomRight,
                        new Point(_topLeft.X, _bottomRight.Y), _topLeft });

                // Top left
                e.Graphics.FillEllipse(new SolidBrush(Color.DarkGreen),
                    _topLeft.X - dotsize, _topLeft.Y - dotsize, 2 * dotsize,
                    2 * dotsize);
                // Bottom left
                e.Graphics.FillEllipse(new SolidBrush(Color.DarkGreen),
                    _topLeft.X - dotsize, _bottomRight.Y - dotsize, 2 * dotsize,
                    2 * dotsize);
                // Bottom right
                e.Graphics.FillEllipse(new SolidBrush(Color.DarkGreen),
                    _bottomRight.X - dotsize, _bottomRight.Y - dotsize, 2 * dotsize,
                    2 * dotsize);
                // Top right
                e.Graphics.FillEllipse(new SolidBrush(Color.DarkGreen),
                    _bottomRight.X - dotsize, _topLeft.Y - dotsize, 2 * dotsize,
                    2 * dotsize);
            }
        }

        private void MeasureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_mode == Mode.Manipulate) {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        Point down = Box2ImageCoord(e.Location);
                        if (Near(down, _scaling.vertical, POINT_SIZE / _zoom))
                        {
                            _grabbedPoint = GrabbedPoint.Vertical;
                        }
                        else if (Near(down, _scaling.middle, POINT_SIZE / _zoom))
                        {
                            _grabbedPoint = GrabbedPoint.Middle;
                        }
                        else if (Near(down, _scaling.horizontal, POINT_SIZE / _zoom))
                        {
                            _grabbedPoint = GrabbedPoint.Horizontal;
                        }
                        else if (Near(down, _topLeft, POINT_SIZE / _zoom))
                        {
                            _grabbedPoint = GrabbedPoint.ROI_TopLeft;
                        }
                        else if (Near(down, new Point(_bottomRight.X, _topLeft.Y), POINT_SIZE / _zoom))
                        {
                            _grabbedPoint = GrabbedPoint.ROI_TopRight;
                        }
                        else if (Near(down, new Point(_topLeft.X, _bottomRight.Y), POINT_SIZE / _zoom))
                        {
                            _grabbedPoint = GrabbedPoint.ROI_BottomLeft;
                        }
                        else if (Near(down, _bottomRight, POINT_SIZE / _zoom))
                        {
                            _grabbedPoint = GrabbedPoint.ROI_BottomRight;
                        }
                        break;
                    case MouseButtons.Right:
                        _grabbedPoint = GrabbedPoint.AllScale;

                        _reference = Box2ImageCoord(e.Location);
                        break;
                }
            }
        }

        private void MeasureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _grabbedPoint = GrabbedPoint.None;
        }

        private void MeasureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_grabbedPoint != GrabbedPoint.None)
            {
                Point down = Box2ImageCoord(e.Location);
                switch (_grabbedPoint)
                {
                    case GrabbedPoint.AllScale:
                        // Move all of the scaling points
                        Point newReference = Box2ImageCoord(e.Location);
                        Point diff = new Point(newReference.X - _reference.X,
                            newReference.Y - _reference.Y);
                        _reference = newReference;

                        _scaling.vertical.X += diff.X;
                        _scaling.vertical.Y += diff.Y;

                        _scaling.horizontal.X += diff.X;
                        _scaling.horizontal.Y += diff.Y;

                        _scaling.middle.X += diff.X;
                        _scaling.middle.Y += diff.Y;
                        break;
                    case GrabbedPoint.Vertical:
                        if (PerpendicularScale)
                        {
                            double length = Math.Sqrt((double)( // (x1 - x2)^2 + (y1 - y2)^2
                                (_scaling.horizontal.X - _scaling.middle.X) * (_scaling.horizontal.X - _scaling.middle.X) + 
                                (_scaling.horizontal.Y - _scaling.middle.Y) * (_scaling.horizontal.Y - _scaling.middle.Y)));
                            double theta = Math.Atan2((double)(_scaling.vertical.Y - _scaling.middle.Y),
                                (double)(_scaling.vertical.X - _scaling.middle.X));

                            _scaling.horizontal.X = _scaling.middle.X + (int)Math.Round(length * Math.Cos(theta + 0.5 * Math.PI));
                            _scaling.horizontal.Y = _scaling.middle.Y + (int)Math.Round(length * Math.Sin(theta + 0.5 * Math.PI));
                        }
                        _scaling.vertical = down;
                        break;
                    case GrabbedPoint.Middle:
                        _scaling.middle = down;
                        break;
                    case GrabbedPoint.Horizontal:
                        if (PerpendicularScale)
                        {
                            double length = Math.Sqrt((double)( // (x1 - x2)^2 + (y1 - y2)^2
                                (_scaling.vertical.X - _scaling.middle.X) * (_scaling.vertical.X - _scaling.middle.X) + 
                                (_scaling.vertical.Y - _scaling.middle.Y) * (_scaling.vertical.Y - _scaling.middle.Y)));
                            double theta = Math.Atan2((double)(_scaling.horizontal.Y - _scaling.middle.Y),
                                (double)(_scaling.horizontal.X - _scaling.middle.X));

                            _scaling.vertical.X = _scaling.middle.X + (int)Math.Round(length * Math.Cos(theta - 0.5 * Math.PI));
                            _scaling.vertical.Y = _scaling.middle.Y + (int)Math.Round(length * Math.Sin(theta - 0.5 * Math.PI));
                        }
                        _scaling.horizontal = down;
                        break;
                    case GrabbedPoint.ROI_TopLeft:
                        // Keep the point on the image
                        if (down.X < 0)
                            _topLeft.X = 0;
                        else if (down.X >= _bottomRight.X)
                            _topLeft.X = _bottomRight.X - 1;
                        else
                            _topLeft.X = down.X;

                        // And not on the opposite side of the diagonal point
                        if (down.Y < 0)
                            _topLeft.Y = 0;
                        else if (down.Y >= _bottomRight.Y)
                            _topLeft.Y = _bottomRight.Y - 1;
                        else
                            _topLeft.Y = down.Y;
                        break;
                    case GrabbedPoint.ROI_TopRight:
                        // Keep the point on the image
                        if (down.X >= _image.Width)
                            _bottomRight.X = _image.Width - 1;
                        else if (down.X <= _topLeft.X)
                            _bottomRight.X = _topLeft.X + 1;
                        else
                            _bottomRight.X = down.X;

                        // And not on the opposite side of the diagonal point
                        if (down.Y < 0)
                            _topLeft.Y = 0;
                        else if (down.Y >= _bottomRight.Y)
                            _topLeft.Y = _bottomRight.Y - 1;
                        else
                            _topLeft.Y = down.Y;
                        break;
                    case GrabbedPoint.ROI_BottomLeft:
                        // Keep the point on the image
                        if (down.X < 0)
                            _topLeft.X = 0;
                        else if (down.X >= _bottomRight.X)
                            _topLeft.X = _bottomRight.X - 1;
                        else
                            _topLeft.X = down.X;

                        // And not on the opposite side of the diagonal point
                        if (down.Y >= _image.Height)
                            _bottomRight.Y = _image.Height - 1;
                        else if (down.Y <= _topLeft.Y)
                            _bottomRight.Y = _topLeft.Y + 1;
                        else
                            _bottomRight.Y = down.Y;
                        break;
                    case GrabbedPoint.ROI_BottomRight:
                        // Keep the point on the image
                        if (down.X >= _image.Width)
                            _bottomRight.X = _image.Width - 1;
                        else if (down.X <= _topLeft.X)
                            _bottomRight.X = _topLeft.X + 1;
                        else
                            _bottomRight.X = down.X;

                        // And not on the opposite side of the diagonal point
                        if (down.Y >= _image.Height)
                            _bottomRight.Y = _image.Height - 1;
                        else if (down.Y <= _topLeft.Y)
                            _bottomRight.Y = _topLeft.Y + 1;
                        else
                            _bottomRight.Y = down.Y;
                        break;
                }

                Refresh();
            }
        }

        private enum GrabbedPoint : short { None, Vertical, Middle, Horizontal, AllScale, ROI_TopLeft, ROI_TopRight, ROI_BottomLeft, ROI_BottomRight };
    }
}
