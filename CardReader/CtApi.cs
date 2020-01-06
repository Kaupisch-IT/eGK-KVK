using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CardReader
{
	/// <summary>
	/// Stellt Kartenterminal-(Card-Terminal)-Funktionen bereit, die benötigt werden, um auf einfache Weise das Handling und die Kommunikation mit Chipkarten realisieren zu können. 
	/// Die CT-API Software sollte vom Kartenterminal-Hersteller für die Systemumgebungen bereitgestellt werden, die für den Einsatz des betreffenden Kartenterminals relevant sind. 
	/// </summary>
	public sealed class CtApi : IDisposable
	{
		private readonly IntPtr libraryHandle; // Handle auf die DLL-Datei mit der herstellerspezifischen CT-API-Implementierung

		/// <summary> Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded. </summary>
		[DllImport("kernel32.dll",CharSet = CharSet.Ansi,BestFitMapping = false,ThrowOnUnmappableChar = true)]
		private static extern IntPtr LoadLibrary(string dllToLoad);

		/// <summary> Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL). </summary>
		[DllImport("kernel32.dll",CharSet = CharSet.Ansi,BestFitMapping = false,ThrowOnUnmappableChar = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule,string procedureName);

		/// <summary> Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. When the reference count reaches zero, the module is unloaded from the address space of the calling process and the handle is no longer valid. </summary>
		[DllImport("kernel32.dll")]
		private static extern bool FreeLibrary(IntPtr hModule);


		/// <summary>Mit der Funktion CT_init wird die zur Kommunikation benötigte Host-Schnittstelle ausgewählt, an welcher das CardTerminal angeschlossen ist, wobei automatisch die Defaultwerte für die Kommunikation eingestellt werden.</summary>
		/// <param name="ctn">Logische Card-Terminal Number</param>
		/// <param name="pn">Port Number der physikalischen	Schnittstelle</param>
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate sbyte CT_init(ushort ctn,ushort pn);

		/// <summary>Die Funktion CT_data dient dem Senden von Chipkarten- bzw.CardTerminal-Kommandos und liefert die Antwort auf das Kommando an das aufrufende Programm zurück.</summary>
		/// <param name="ctn">Logische Card Terminal Number</param>
		/// <param name="dad">Destination Address</param>
		/// <param name="sad">Source Address</param>
		/// <param name="lenc">Länge des Kommandos (Command) in Byte</param>
		/// <param name="command">Chipkarten bzw. CT-Kommando</param>
		/// <param name="ulenr">Übermittlung der max. Puffergröße des response Feldes an die Funktion und Rückgabe der tatsächlichen Länge der Antwort in Byte</param>
		/// <param name="response">Antwort auf das Kommando</param>
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate sbyte CT_data(ushort ctn,ref byte dad,ref byte sad,ushort lenc,ref byte command,ref ushort ulenr,ref byte response);

		/// <summary>Die Funktion CT_close bildet das Äquivalent zur Funktion CT_init. Sie beendet die Kommunikation zum jeweiligen CardTerminal,welches mit CT_init einer logischen Card-Terminal Number zugewiesen wurde. Die Funktion muss vor Ende des Programms aufgerufen werden, um ggf. belegte Ressourcen freizugeben.</summary>
		/// <param name="ctn">Logische Card-Terminal Number</param>
		/// <returns></returns>
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate sbyte CT_close(ushort ctn);


		/// <summary>
		/// Initiieren der Host/CT-Verbindung
		/// </summary>
		public CT_init Init { get; private set; }

		/// <summary>
		/// Senden eines Kommandosan ein CardTerminal bzw.an eine Chipkarte undRückgabe der Antwort
		/// </summary>
		public CT_data Data { get; private set; }

		/// <summary>
		/// Beenden der Host/CT-Verbindung
		/// </summary>
		public CT_close Close { get; private set; }


		/// <summary>
		/// Lädt die Kartenterminal-Hersteller-spezifische Implementierung der CT-API
		/// </summary>
		/// <param name="path">der Pfad zur DLL-Datei mit der herstellerspezifischen CT-API-Implementierung</param>
		public CtApi(string path)
		{
			this.libraryHandle = CtApi.LoadLibrary(path);
			if (this.libraryHandle==IntPtr.Zero)
				throw new FileLoadException(null,path);

			IntPtr ctInitFunctionHandle = CtApi.GetProcAddress(this.libraryHandle,"CT_init");
			if (ctInitFunctionHandle == IntPtr.Zero)
				throw new InvalidOperationException("GetProcAddress CT_init");
			this.Init = Marshal.GetDelegateForFunctionPointer<CT_init>(ctInitFunctionHandle);

			IntPtr ctDataFunctionHandle = CtApi.GetProcAddress(this.libraryHandle,"CT_data");
			if (ctDataFunctionHandle == IntPtr.Zero)
				throw new InvalidOperationException("GetProcAddress CT_data");
			this.Data = Marshal.GetDelegateForFunctionPointer<CT_data>(ctDataFunctionHandle);

			IntPtr ctCloseFunctionHandle = CtApi.GetProcAddress(this.libraryHandle,"CT_close");
			if (ctCloseFunctionHandle == IntPtr.Zero)
				throw new InvalidOperationException("GetProcAddress CT_close");
			this.Close = Marshal.GetDelegateForFunctionPointer<CT_close>(ctCloseFunctionHandle);
		}


		/// <summary>
		/// Gibt verwendete Ressourcen wieder frei
		/// </summary>
		public void Dispose()
		{
			CtApi.FreeLibrary(this.libraryHandle);
		}
	}
}
