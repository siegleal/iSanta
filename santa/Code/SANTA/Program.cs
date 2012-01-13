using System;
using System.Windows.Forms;

namespace SANTA
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			IO.Log.InitLog();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			new Controller.Master();
		}
	}
}
