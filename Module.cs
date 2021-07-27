using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using System.IO;

namespace ConsoleToTextNonAuto
{
	// Token: 0x02000002 RID: 2
	public class CTTModule : ETGModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002230 File Offset: 0x00000430
		public override void Start()
		{
			if (File.Exists(CTTModule.defaultLog))
			{
				File.Delete(CTTModule.defaultLog);
				CTTModule.Log("Deleting Old C.T.T File from when previously used.", CTTModule.TEXT_COLOR);
			}
			ETGModConsole.Commands.AddUnit("print_debug_console", new Action<string[]>(this.PrintDebugLog));
			ETGModConsole.Commands.AddUnit("nulls_only_print_debug_console", new Action<string[]>(this.PrintDebugLogNullOnly));
			CTTModule.Log(CTTModule.MOD_NAME + " v" + CTTModule.VERSION + " started successfully. Use command: 'print_debug_console' To print the entire debug log into a text file.", CTTModule.TEXT_COLOR);
			CTTModule.Log("Use command 'nulls_only_print_debug_console' to print only NullReferenceExceptions into the log.", CTTModule.TEXT_COLOR);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000022CC File Offset: 0x000004CC
		public void PrintDebugLog(string[] args)
		{
			if (File.Exists(CTTModule.defaultLog))
			{
				File.Delete(CTTModule.defaultLog);
				CTTModule.Log("Deleting Old C.T.T File from when previously used.", CTTModule.TEXT_COLOR);
			}
			CTTModule.Log("Creating New C.T.T file.", CTTModule.TEXT_COLOR);
			CTTModule.Log("C.T.T file should be located in." + CTTModule.defaultLog, CTTModule.TEXT_COLOR);
			CTTModule.DebugPrinter.ClearDebugLog();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000232C File Offset: 0x0000052C
		public void PrintDebugLogNullOnly(string[] args)
		{
			if (File.Exists(CTTModule.defaultLog))
			{
				File.Delete(CTTModule.defaultLog);
				CTTModule.Log("Deleting Old C.T.T File from when previously used.", CTTModule.TEXT_COLOR);
			}
			CTTModule.Log("Creating New C.T.T (Nulls Only) file.", CTTModule.TEXT_COLOR);
			CTTModule.Log("C.T.T file should be located in." + CTTModule.defaultLog, CTTModule.TEXT_COLOR);
			CTTModule.DebugPrinter.ClearDebugLogNullOnly();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000238C File Offset: 0x0000058C
		public static void LogHook(Action<string, bool> orig, string text, bool debuglog = false)
		{
			try
			{
				using (StreamWriter streamWriter = File.AppendText(CTTModule.defaultLog))
				{
					streamWriter.WriteLine(text);
				}
			}
			catch
			{
				CTTModule.Log("Something broke so bad that the F2 console can't print it. God pray for you.", CTTModule.TEXT_COLOR);
			}
			orig(text, debuglog);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023F0 File Offset: 0x000005F0
		public static void LogHookU(Action<string> orig, string str)
		{
			orig(str);
			try
			{
				using (StreamWriter streamWriter = File.AppendText(CTTModule.defaultLog))
				{
					streamWriter.WriteLine(str);
				}
			}
			catch
			{
				CTTModule.Log("Something broke so bad that the F3 console can't print it. God pray for you.", CTTModule.TEXT_COLOR);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002050 File Offset: 0x00000250
		public static void Log(string text, string color = "#FFFFFF")
		{
			ETGModConsole.Log(string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				text,
				"</color>"
			}), false);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002083 File Offset: 0x00000283
		public override void Exit()
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002083 File Offset: 0x00000283
		public override void Init()
		{
		}

		// Token: 0x04000001 RID: 1
		public static readonly string MOD_NAME = "Console To Text Mod (Code By NotABot!)";

		// Token: 0x04000002 RID: 2
		public static readonly string VERSION = "1.2";

		// Token: 0x04000003 RID: 3
		public static readonly string TEXT_COLOR = "#71e300";

		// Token: 0x04000004 RID: 4
		private static string defaultLog = Path.Combine(ETGMod.ResourcesDirectory, "ConsoleToText.txt");

		// Token: 0x02000003 RID: 3
		public class DebugPrinter : ETGModDebugLogMenu
		{
			// Token: 0x0600000B RID: 11 RVA: 0x00002454 File Offset: 0x00000654
			public static void ClearDebugLog()
			{
				foreach (ETGModDebugLogMenu.LoggedText loggedText in ETGModDebugLogMenu._AllLoggedText)
				{
					string logMessage = loggedText.LogMessage;
					string stacktace = loggedText.Stacktace;
					using (StreamWriter streamWriter = File.AppendText(CTTModule.defaultLog))
					{
						streamWriter.WriteLine(logMessage.ToString());
						if (loggedText.LogType == LogType.Exception)
						{
							streamWriter.WriteLine("   " + stacktace.ToString());
						}
					}
				}
			}

			// Token: 0x0600000C RID: 12 RVA: 0x00002504 File Offset: 0x00000704
			public static void ClearDebugLogNullOnly()
			{
				foreach (ETGModDebugLogMenu.LoggedText loggedText in ETGModDebugLogMenu._AllLoggedText)
				{
					if (loggedText.LogType == LogType.Exception)
					{
						using (StreamWriter streamWriter = File.AppendText(CTTModule.defaultLog))
						{
							string logMessage = loggedText.LogMessage;
							string stacktace = loggedText.Stacktace;
							streamWriter.WriteLine(logMessage.ToString());
							if (loggedText.LogType == LogType.Exception)
							{
								streamWriter.WriteLine("   " + stacktace.ToString());
							}
						}
					}
				}
			}
		}
	}
}

