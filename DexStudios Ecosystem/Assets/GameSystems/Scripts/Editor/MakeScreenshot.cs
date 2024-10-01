using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MakeScreenshot : EditorWindow
{
	private const String ScreenShotFolder = "/ScreenShots/";

	[MenuItem("Window/Screenshot/1X")]
	public static void Make1X()
	{
		MakeScreen(1);
	}

	[MenuItem("Window/Screenshot/2X")]
	public static void Make2X()
	{
		MakeScreen(2);
	}

	[MenuItem("Window/Screenshot/3X")]
	public static void Make3X()
	{
		MakeScreen(3);
	}

	[MenuItem("Window/Screenshot/4X")]
	public static void Make4X()
	{
		MakeScreen(4);
	}

	[MenuItem("Window/Screenshot/5X")]
	public static void Make5X()
	{
		MakeScreen(5);
	}

	[MenuItem("Window/Screenshot/Open Folder")]
	public static void OpenFolder()
	{
		Open(Application.dataPath + ScreenShotFolder);
	}

	private static void MakeScreen(int factor)
	{
		if (!Directory.Exists(Application.dataPath + ScreenShotFolder))
		{
			Directory.CreateDirectory(Application.dataPath + ScreenShotFolder);
		}

		ScreenCapture.CaptureScreenshot(
			"Assets" + ScreenShotFolder + "Screenshot_" + factor + "X_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") +
			".png", factor);
	}


	public static bool IsInMacOS => SystemInfo.operatingSystem.IndexOf("Mac OS") != -1;

	public static bool IsInWinOS => SystemInfo.operatingSystem.IndexOf("Windows", StringComparison.Ordinal) != -1;


	private static void OpenInMac(string path)
	{
		var openInsidesOfFolder = false;

		// try mac
		var macPath = path.Replace("\\", "/");

		if (Directory.Exists(macPath))
		{
			openInsidesOfFolder = true;
		}

		if (!macPath.StartsWith("\""))
		{
			macPath = "\"" + macPath;
		}

		if (!macPath.EndsWith("\""))
		{
			macPath = macPath + "\"";
		}

		string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;

		try
		{
			System.Diagnostics.Process.Start("open", arguments);
		}
		catch (System.ComponentModel.Win32Exception e)
		{
			e.HelpLink = "";
		}
	}

	private  static void OpenInWin(string path)
	{
		bool openInsidesOfFolder = false;

		string winPath = path.Replace("/", "\\");

		if (Directory.Exists(winPath))
		{
			openInsidesOfFolder = true;
		}

		try
		{
			System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
		}
		catch (System.ComponentModel.Win32Exception e)
		{
			e.HelpLink = "";
		}
	}

	private static void Open(string path)
	{
		if (IsInWinOS)
		{
			OpenInWin(path);
		}
		else if (IsInMacOS)
		{
			OpenInMac(path);
		}
		else
		{
			Debug.LogWarning("Could not open folder: " + path);
		}
	}
}