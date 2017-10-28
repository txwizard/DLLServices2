using System;

using Microsoft.Win32.SafeHandles;
//	using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace ConsoleStreamsLab
{
	/// <summary>
    /// Utility class to wrap an unmanaged DLL and be responsible for freeing it
    /// </summary>
    /// <remarks>This is a managed wrapper over the native LoadLibrary,
	/// GetProcAddress, and FreeLibrary calls.
    /// </remarks>
    public sealed class UnmanagedLibrary : IDisposable
    {
        #region Safe Handles and Native imports
        // See http://msdn.microsoft.com/msdnmag/issues/05/10/Reliability/ for more about safe handles.
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
			private SafeLibraryHandle ( ) : base ( true ) { }

			protected override bool ReleaseHandle ( )
			{
				return NativeMethods.FreeLibrary ( handle );
			}	// ReleaseHandle
        }	// class SafeLibraryHandle

        static class NativeMethods
        {
			const string KERNEL32 = "kernel32";
            [DllImport(KERNEL32, CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
            public static extern SafeLibraryHandle LoadLibrary(string fileName);

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [DllImport(KERNEL32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport(KERNEL32)]
            public static extern IntPtr GetProcAddress(SafeLibraryHandle hModule, String procname);
        }	// class NativeMethods
        #endregion // Safe Handles and Native imports


        /// <summary>
        /// Constructor to load a DLL and be responsible for freeing it.
        /// </summary>
		/// <param name="fileName">
		/// full path name of DLL to load
		/// </param>
		/// <exception cref="System.IO.FileNotFound">
		/// if fileName cannot be found
		/// </exception>
        /// <remarks>
		/// Throws exceptions on failure. Most common failure would be file-not-found, or
        /// that the file is not a loadable image.
		/// </remarks>
		public UnmanagedLibrary ( string fileName )
        {
			m_hLibrary = NativeMethods.LoadLibrary ( fileName );

			if ( m_hLibrary.IsInvalid )
			{
				Marshal.ThrowExceptionForHR ( Marshal.GetHRForLastWin32Error ( ) );
			}	// if ( m_hLibrary.IsInvalid )
        }	// UnmanagedLibrary


        /// <summary>
        /// Dynamically lookup a function in the DLL via kernel32!GetProcAddress.
        /// </summary>
		/// <param name="functionName">
		/// raw name of the function in the export table.
		/// </param>
        /// <returns>
		/// null if function is not found, else a delegate to the unmanaged function
        /// </returns>
        /// <remarks>
		/// GetProcAddress results are valid as long as the DLL is not yet unloaded. This
        /// is very, very dangerous to use since you need to ensure that the DLL is not unloaded
        /// until after you’re done with any objects implemented by the DLL. For example, if you
        /// get a delegate that then gets an IUnknown implemented by this DLL,
        /// you can not dispose this library until that IUnknown is collected. Else, you may free
        /// the library and then the CLR may call release on that IUnknown and it will crash.
		/// </remarks>
		public TDelegate GetUnmanagedFunction<TDelegate> ( string functionName ) where TDelegate : class
		{
			IntPtr p = NativeMethods.GetProcAddress ( m_hLibrary , functionName );

			// Failure is a common case, especially for adaptive code.

			if ( p == IntPtr.Zero )
			{
				return null;
			}	// if ( p == IntPtr.Zero )

			Delegate function = Marshal.GetDelegateForFunctionPointer ( p , typeof ( TDelegate ) );

			// Ideally, we’d just make the constraint on TDelegate be
			// System.Delegate, but compiler error CS0702 (constrained can’t be System.Delegate)
			// prevents that. So we make the constraint system.object and do the cast from object–>TDelegate.

			object o = function;

			return ( TDelegate ) o;
		}	// TDelegate

        #region IDisposable Members
        /// <summary>
        /// Call FreeLibrary on the unmanaged DLL. All function pointers
        /// handed out from this class become invalid after this.
        /// </summary>
        /// <remarks>
		/// This is very dangerous because it suddenly invalidate
        /// everything retrieved from this DLL. This includes any functions
        /// handed out via GetProcAddress, and potentially any objects returned
        /// from those functions (which may have an implementation in the DLL).
        /// </remarks>
		public void Dispose ( )
		{
			if ( !m_hLibrary.IsClosed )
			{
				m_hLibrary.Close ( );
			}	// if ( !m_hLibrary.IsClosed )
		}	// Dispose method

        // Unmanaged resource. CLR will ensure SafeHandles get freed, without requiring a finalizer on this class.

        SafeLibraryHandle m_hLibrary;
        #endregion	// IDisposable Members
    } // UnmanagedLibrary
}	// partial namespace ConsoleStreamsLab