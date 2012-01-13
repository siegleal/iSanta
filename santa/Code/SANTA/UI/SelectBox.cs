using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SANTA.UI
{
    /// <summary>
    /// An image box for indicating the location of bullet holes.
    /// </summary>
    public class SelectBox : ZPImageBox
    {
        private int _selectedHole = -1;

        private bool _moved = false;

        /// <summary>
        /// The diameter of bullet holes in pixels.
        /// </summary>
        public int HoleDiameter { set; get; }

        private List<Point> _bulletHoles;

        /// <summary>
        /// The list of bullet hole locations.
        /// </summary>
        public List<Point> BulletHoles
        {
            set
            {
                _bulletHoles = value;
                HolesChanged(EventArgs.Empty);
            }
            get { return _bulletHoles; }
        }

        /// <summary>
        /// The current tool to determine how to interact with the holes.
        /// </summary>
        public Tool CurrentTool { set; get; }

        /// <summary>
        /// Construct a <code>SelectBox</code>
        /// </summary>
        public SelectBox()
        {
            BulletHoles = new List<Point>();

            CurrentTool = Tool.AddSelector;

            SetEventHandlers();
        }

        private void SetEventHandlers()
        {
            this.Paint += new PaintEventHandler(DrawSelectors);

            this.MouseDown  += new MouseEventHandler(SelectBox_MouseDown);
            this.MouseUp    += new MouseEventHandler(SelectBox_MouseUp);
            this.MouseClick += new MouseEventHandler(SelectBox_MouseClick);
            this.MouseMove  += new MouseEventHandler(SelectBox_MouseMove);
        }

        private void DrawSelectors(object sender, PaintEventArgs e)
        {
            if (_image != null && BulletHoles != null && !_multipleFiles)
            {
                foreach (Point p in BulletHoles)
                {
                    e.Graphics.DrawEllipse(new Pen(Color.Red, 2.0F), p.X - HoleDiameter / 2, p.Y - HoleDiameter / 2, HoleDiameter, HoleDiameter);
                }
            }
        }

        private void SelectBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _mode == Mode.Manipulate)
            {
                Point down = Box2ImageCoord(e.Location);
                for (int i = 0; i < BulletHoles.Count; i++)
                {
                    if (Near(down, BulletHoles[i], HoleDiameter / 2))
                    {
                        _selectedHole = i;
                        _moved = false;
                        break;
                    }
                }

                if (CurrentTool == Tool.AddSelector && _selectedHole == -1)
                {
                    BulletHoles.Add(Box2ImageCoord(e.Location));
                    HolesChanged(EventArgs.Empty);

                    _selectedHole = BulletHoles.Count - 1;

                    _moved = true;

                    Refresh();
                }
            }
        }

        private void SelectBox_MouseUp(object sender, MouseEventArgs e)
        {
            _selectedHole = -1;
        }

        private void SelectBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _mode == Mode.Manipulate && !_moved)
            {
                Point down = Box2ImageCoord(e.Location);
                switch (CurrentTool)
                {
                    case Tool.AddSelector:
                        BulletHoles.Add(down);
                        HolesChanged(EventArgs.Empty);
                        break;
                    case Tool.RemoveSelector:
                        foreach (Point p in BulletHoles)
                        {
                            if (Near(down, p, HoleDiameter / 2))
                            {
                                BulletHoles.Remove(p);
                                HolesChanged(EventArgs.Empty);
                                break;
                            }
                        }
                        break;
                }
                Refresh();
            }
        }

        /// <summary>
        /// Occurs when the number of bullet holes changes.
        /// </summary>
        public event ChangedEventHandler Changed;

        private void SelectBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedHole != -1)
            {
                _moved = true;
                BulletHoles[_selectedHole] = Box2ImageCoord(e.Location);
                Refresh();
            }
        }

        /// <summary>
        /// Call the events when the hole count changes
        /// </summary>
        /// <param name="e"></param>
        public virtual void HolesChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        /// <summary>
        /// Methods of interacting with the bullet holes.
        /// </summary>
        public enum Tool : short {
            /// <summary>Add hole selector</summary>
            AddSelector,
            /// <summary>Remove hole selector</summary>
            RemoveSelector };
    }

    /// <summary>
    /// Event for when the hole count changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ChangedEventHandler(object sender, EventArgs e);
}
