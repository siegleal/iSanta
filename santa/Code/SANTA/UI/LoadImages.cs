using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SANTA.UI
{
    /// <summary>
    /// A form to handle input of target IDs to load.
    /// </summary>
    public partial class LoadImages : Form
    {
        private DialogResult _result;

        private List<Int32> _targetIDs;

        private List<String> _errorIDs;

        /// <summary>
        /// The button that was clicked.
        /// </summary>
        public DialogResult Result
        {
            get { return _result; }
        }

        /// <summary>
        /// The target IDs that could be parsed into integers
        /// </summary>
        public List<Int32> TargetIDs
        {
            get { return _targetIDs; }
        }

        /// <summary>
        /// The target IDs that could not be parsed into integers
        /// </summary>
        public List<String> ErrorIDs
        {
            get { return _errorIDs; }
        }

        /// <summary>
        /// The unmodified value of the text field
        /// </summary>
        public String RawIDs
        {
            get { return text_TargetIDs.Text; }
        }

        /// <summary>
        /// Construct a <code>LoadImages</code> form
        /// </summary>
        public LoadImages()
        {
            InitializeComponent();

            _result = DialogResult.Cancel;
            _targetIDs = new List<Int32>();
            _errorIDs = new List<String>();
        }

        /// <summary>
        /// Construct a <code>LoadImages</code> form with the text field populated with the
        /// indicated IDs.
        /// </summary>
        /// <param name="IDs">The target ID string to put in the text field.</param>
        public LoadImages(String IDs) : this()
        {
            text_TargetIDs.Text = IDs;
        }

        /// <summary>
        /// When OK is clicked, parse the text field into integers and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OK_Click(object sender, EventArgs e)
        {
            _result = DialogResult.OK;

            String[] targetIDs = text_TargetIDs.Text.Split(new char[] { ' ' });
            foreach (String id in targetIDs)
            {
                try
                {
                    _targetIDs.Add(Convert.ToInt32(id));
                }
                catch (Exception)
                {
                    _errorIDs.Add(id);
                }
            }

            Close();
        }

        /// <summary>
        /// When cancel is clicked, close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            _result = DialogResult.Cancel;

            Close();
        }
    }
}
