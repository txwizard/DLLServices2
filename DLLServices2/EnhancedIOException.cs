/*
    ============================================================================

    Namespace:          WizardWrx

    Class Name:         EnhancedIOException

    File Name:          EnhancedIOException.cs

    Synopsis:           This class provides a richer set of I/O exceptions to
                        assemblies that target version 2.0 of the Microsoft .NET
                        Framework. In particular, they expose the protected
						HResult property, which provides a more robust mechanism
						for precisely identifying the cause of the exception
						that is culture agnostic, because it doesn't depend on
						parsing the message.

    Remarks:            Due to its potentially broad utility, I put this class
						in my root namespace, WizardWrx.

						It appears that the Microsoft engineers belatedly saw
						the value of exposing the HResult, because the newer
						versions of the Base Class Library expose it.

	Dependencies:		Newtonsoft.Json, available as a NuGet package.

	Reference:			"Json.net fails when trying to Deserialize a Class that Inherits from Exception,"
						http://stackoverflow.com/questions/14186000/json-net-fails-when-trying-to-deserialize-a-class-that-inherits-from-exception#new-answer

    Author:             David A. Gray

    License:            Copyright (C) 2016, David A. Gray. 
						All rights reserved.

                        Redistribution and use in source and binary forms, with
                        or without modification, are permitted provided that the
                        following conditions are met:

                        *   Redistributions of source code must retain the above
                            copyright notice, this list of conditions and the
                            following disclaimer.

                        *   Redistributions in binary form must reproduce the
                            above copyright notice, this list of conditions and
                            the following disclaimer in the documentation and/or
                            other materials provided with the distribution.

                        *   Neither the name of David A. Gray, nor the names of
                            his contributors may be used to endorse or promote
                            products derived from this software without specific
                            prior written permission.

                        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
                        CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
                        WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
                        WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
                        PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
                        David A. Gray BE LIABLE FOR ANY DIRECT, INDIRECT,
                        INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
                        (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
                        SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
                        PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
                        ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
                        LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
                        ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
                        IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

    Created:            Saturday, 02 April 2016 and Sunday, 03 April 2016

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Description
    ---------- ------- ------ --------------------------------------------------
    2016/04/03 1.0     DAG    Initial implementation.
    ============================================================================
*/

using System;
using System.IO;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace WizardWrx
{
	/// <summary>
	/// This class is a thin veneer over its base class, System.IO.IOException,
	/// which exists solely to expose its HRESULT property, so that it can be
	/// tested, rather than being hostage to a message. Breaking free of the
	/// message makes this class locale agnostic.
	/// </summary>
	[Serializable ( )]
	public class EnhancedIOException : IOException
	{
		/// <summary>
		/// Though technically optional, I prefer to make the default
		/// constructor explicit.
		/// </summary>
		/// <remarks>
		/// Since the next constructor is the only one that is usable, this one
		/// is hidden.
		/// </remarks>
		private EnhancedIOException ( )
			: base ( )
		{
		}	// private EnhancedIOException constructor (1 of 2)


		/// <summary>
		/// This method is intended for use by the public EnhancedIOException
		/// constructor, which takes an IOException Exception, which must be
		/// serialized, then read into the subclassed object.
		/// </summary>
		/// <param name="pserInfo">
		/// The SerializationInfo object is a collection of key-value pairs that
		/// stores the properties of the parent class, an instance of an
		/// System.IO.IOException Exception.
		/// </param>
		/// <param name="pscxContext">
		/// The StreamingContext is maintained by the serialization engine. You
		/// can safely treat it as a black box.
		/// </param>
		public EnhancedIOException (
			SerializationInfo pserInfo ,
			StreamingContext pscxContext )
			: base ( pserInfo , pscxContext )
		{
		}	// public EnhancedIOException constructor (3 of 3)


		/// <summary>
		/// Since the object is created by importing an IOException that was
		/// serialized to a JSON string, this is the only constructor that needs
		/// to be public.
		/// </summary>
		/// <param name="pserInfo">
		/// The SerializationInfo object is a collection of key-value pairs that
		/// stores the properties of the parent class, an instance of an
		/// System.IO.IOException Exception.
		/// </param>
		/// <param name="pscxContext">
		/// The StreamingContext is maintained by the serialization engine. You
		/// can safely treat it as a black box.
		/// </param>
		public override void GetObjectData (
			SerializationInfo pserInfo ,
			StreamingContext pscxContext )
		{
			base.GetObjectData (
				pserInfo ,
				pscxContext );
		}	// public override void GetObjectData


		/// <summary>
		/// The sole objective of creating this class is to expose the protected
		/// HRESULT property of the base class as a read only property for an
		/// assembly that targets version 2.0 of the Microsoft .NET Framework.
		/// 
		/// Since newer versions expose the HRESULT, they can dispense with this
		/// device.
		/// </summary>
		public int HRESULT
		{
			get
			{
				return base.HResult;
			}	// public UInt32 HRESULT property get method
		}	// public UInt32 HRESULT property

	
		/// <summary>
		/// This method provides a roundabout mechanism to subclass IOException
		/// exceptions.
		/// </summary>
		/// <param name="pexIOBase">
		/// This public constructor take a roundabout path, through a JSON
		/// serialization engine, to subclass the IOException thrown by a
		/// System.IO object.
		/// </param>
		/// <returns>
		/// The return value is the initialized EnhancedIOException.
		/// </returns>
		public static EnhancedIOException SubclassIOException ( IOException pexIOBase )
		{
			return JsonConvert.DeserializeObject<EnhancedIOException> ( JsonConvert.SerializeObject ( pexIOBase ) );
		}	// SubclassIOException
	}	// public class EnhancedIOException
}	// namespace WizardWrx