using CefSharp;
using System;

namespace BacteriaNetworks
{
	public class Program
    {
		[STAThread]
	    public static void Main()
        {
            Cef.EnableHighDPISupport();
			new MainForm().ShowDialog();
            Cef.Shutdown();
        }
    }
}