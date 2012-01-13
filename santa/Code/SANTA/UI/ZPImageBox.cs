using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SANTA.UI
{
    /// <summary>
    /// A zoom-able, pan-able image box for displaying and navigating around images.
    /// </summary>
    public class ZPImageBox : ScrollableControl
    {
        /// <summary>
        /// Image to display
        /// </summary>
        protected Image _image;

        /// <summary>
        /// Amount to scale the scroll distance before updating the zoom level.
        /// </summary>
        protected const float SCROLLFACTOR = 0.001F;

        /// <summary>
        /// These specify the range of scaling factors for the image.
        /// </summary>
        protected float _zoom, _maxZoom, _minZoom = 4F;

        /// <summary>
        /// Is the <code>_image</code> being dragged/moved around.
        /// </summary>
        protected bool _dragging = false;

        /// <summary>
        /// <code>_imageFocus</code> is the point in the image's coordinate system that
        /// corresponds to the <code>_boxFocus</code>, which is the point in the <code>ZPImageBox</code>
        /// coordinate system where the user last clicked when moving the image.  
        /// </summary>
        protected Point _imageFocus, _boxFocus;

        /// <summary>
        /// The current method of interaction with the <code>ZPImageBox</code>.
        /// </summary>
        protected Mode _mode;

        /// <summary>
        /// Have multiple files been selected.
        /// </summary>
        protected bool _multipleFiles;

        /// <summary>
        /// Cursor to use when the mouse button is depressed.  It looks like an
        /// opened hand.
        /// </summary>
        protected static Cursor grabCursor = new Cursor(new System.IO.MemoryStream(SANTA.Properties.Resources.grab));

        /// <summary>
        /// Cursor to use when the mouse button is not depressed.  It looks like
        /// a hand grabbing.
        /// </summary>
        protected static Cursor grabbingCursor = new Cursor(new System.IO.MemoryStream(SANTA.Properties.Resources.grabbing));

        /// <summary>
        /// Construct a <code>ZPImageBox</code>
        /// </summary>
        public ZPImageBox()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            _mode = Mode.Navigate;

            SetEventHandlers();

            this.Cursor = grabCursor;
        }

        private void SetEventHandlers()
        {
            this.Paint += new PaintEventHandler(DrawImage);

            this.KeyDown += new KeyEventHandler(ZPImageBox_KeyDown);
            this.KeyUp   += new KeyEventHandler(ZPImageBox_KeyUp);

            this.MouseEnter += new EventHandler(ZPImageBox_MouseEnter);
            this.MouseDown  += new MouseEventHandler(ZPImageBox_MouseDown);
            this.MouseUp    += new MouseEventHandler(ZPImageBox_MouseUp);
            this.MouseMove  += new MouseEventHandler(ZPImageBox_MouseMove);
            this.MouseWheel += new MouseEventHandler(ZPImageBox_MouseWheel);
        }

        /// <summary>
        /// Change the currently displayed image.
        /// </summary>
        /// <param name="image">The new image to display.</param>
        public void SetImage(Bitmap image)
        {
            SetImage(image, false);
        }

        /// <summary>
        /// Change the currently displayed image 
        /// </summary>
        /// <param name="image">The new image to display</param>
        /// <param name="reset">Whether to reset the zoom level and focal point</param>
        public void SetImage(Bitmap image, bool reset)
        {
            _image = image;

            ResetImage(reset);

            Refresh();
        }

        /// <summary>
        /// Change the zoom level so the image is entirely viewable.
        /// </summary>
        /// <param name="moveToBoxCenter">Should the image be moved back to the center of the box</param>
        protected void ResetImage(bool moveToBoxCenter)
        {
            _zoom = Math.Min((float)this.Height / (float)_image.Height, (float)this.Width / (float)_image.Width);
            _maxZoom = _zoom;

            _imageFocus = new Point(_image.Width / 2, _image.Height / 2);

            if (moveToBoxCenter)
            {
                _boxFocus = new Point(this.Width / 2, this.Height / 2);
            }
        }

        /// <summary>
        /// Paint the image to the box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawImage(object sender, PaintEventArgs e)
        {
            if (_multipleFiles)
            {
                e.Graphics.DrawString("Cannot display multiple images at once.", new Font("Arial", 12), new SolidBrush(Color.Black), (float)(this.Width - 212) * 0.5f, (float)this.Height * 0.5f);
            }
            else
            {
                if (this.Width != 0 && _boxFocus.X == 0)
                {
                    _boxFocus = new Point(this.Width / 2, this.Width / 2);
                }

                if (_image == null)
                {
                    OnPaintBackground(e);
                }
                else
                {
                    // Scale and translate the image accordingly.
                    Matrix mx = new Matrix(_zoom, 0, 0, _zoom, 0, 0);
                    float tx = _boxFocus.X / _zoom - _imageFocus.X;
                    float ty = _boxFocus.Y / _zoom - _imageFocus.Y;
                    mx.Translate(tx, ty);
                    e.Graphics.Transform = mx;
                    e.Graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height), 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel);
                }
            }
        }

        /// <summary>
        /// Change if multiple files are selected or not.  Multiple images cannot be displayed if this
        /// is set to true.
        /// </summary>
        /// <param name="multipleFiles">Are multiple files selected</param>
        public void SetMultipleFileMode(bool multipleFiles)
        {
            _multipleFiles = multipleFiles;

            Refresh();
        }

        #region Image Navigation

        /// <summary>
        /// Zoom in or out of the image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZPImageBox_MouseWheel(object sender, MouseEventArgs e)
        {
            _imageFocus = Box2ImageCoord(e.Location);

            _boxFocus.X = e.X;
            _boxFocus.Y = e.Y;

            float oldZoom = _zoom;
            _zoom += SCROLLFACTOR * (float)e.Delta;

            if (_zoom < _maxZoom)
                _zoom = _maxZoom;
            else if (_zoom > _minZoom)
                _zoom = _minZoom;

            Refresh();
        }

        /// <summary>
        /// This needs to grab focus when the mouse enters so that input can be handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZPImageBox_MouseEnter(object sender, EventArgs e)
        {
            Focus();
        }

        /// <summary>
        /// Handle navigation interaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZPImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_mode == Mode.Navigate)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        Point down = Box2ImageCoord(e.Location);

                        _dragging = true;

                        _imageFocus = down;

                        _boxFocus = e.Location;
                        break;
                    case MouseButtons.Middle:
                        ResetImage(true);

                        Refresh();
                        break;
                }
            }

            this.Cursor = grabbingCursor;
        }

        /// <summary>
        /// Disable dragging mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZPImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;

            this.Cursor = grabCursor;
        }

        /// <summary>
        /// Move the image around.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZPImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mode == Mode.Navigate && _dragging)
            {
                _boxFocus = e.Location;

                this.Refresh();
            }
        }

        #endregion

        /// <summary>
        /// If the Control key was pressed, put the image into Manipulate mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZPImageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _mode = Mode.Manipulate;
            }
        }

        /// <summary>
        /// If the Control key was released, put the image into Navigate mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZPImageBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _mode = Mode.Navigate;
            }
        }

        /// <summary>
        /// Convert a point from mouse coordinates to image coordinates.
        /// </summary>
        /// <param name="boxCoord">Coordinate in the box's coordinate system.</param>
        /// <returns>Coordinate in the image's coordinate system.</returns>
        protected Point Box2ImageCoord(Point boxCoord)
        {
            Point ret = new Point();
            ret.X = (int)((boxCoord.X - _boxFocus.X) / _zoom + _imageFocus.X);
            ret.Y = (int)((boxCoord.Y - _boxFocus.Y) / _zoom + _imageFocus.Y);
            return ret;
        }

        /// <summary>
        /// Determine if two points are within a certain distance of each other.
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second Point.</param>
        /// <param name="dist">The </param>
        /// <returns>If <code>a</code> is no more than <code>dist</code> from <code>b</code>.</returns>
        static protected bool Near(Point a, Point b, float dist)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) < dist * dist;
        }

        /// <summary>
        /// The current method of interaction
        /// </summary>
        protected enum Mode : short {
            /// <summary>Move the image itself</summary>
            Navigate,
            /// <summary>Move items on the image</summary>
            Manipulate };
    }
}
