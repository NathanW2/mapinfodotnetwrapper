using System;
using System.Windows.Forms;
using Wrapper.Example.Forms;

namespace Wrapper.Example
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MapForm());
		}
	}
}