/*
	----------------------------------------------------------------------------
	Function Name:		SHS_GetLabelStringIDW
	File Name:			SHS_GetLabelStringIDW.C
	File Synopsis:		This source file defines the Unicode (wide character) 
						implementation of SHS_GetLabelStringID. Please see 
						StandardHandleState.H for additional information.
	Remarks:			This routine needs access to its own instance handle, in
						m_hinstMe , to pass along to FixedStringBuffers.

    License:            Copyright (C) 2015, David A. Gray. All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:

    *   Redistributions of source code must retain the above copyright notice,
    this list of conditions and the following disclaimer.

    *   Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in the
    documentation and/or other materials provided with the distribution.

    *   Neither the name of David A. Gray nor the names of his contributors may
    be used to endorse or promote products derived from this software without
    specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
    AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
    IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
    ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
    THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
	----------------------------------------------------------------------------
*/

#define UNICODE

#include <StandardHandleState.H>

#include ".\WWConAid_Pvt.H"

LPTSTR SHS_STANDARDHANDLESTATE_API SHS_GetLabelStringW
(
	CSHS_STANDARD_HANDLE penmStdHandleID ,										// Specify the desired standard stream.
	CSHS_HANDLE_LABELS   penmLabelType ,										// Specify the set from which to select the label.
	CUINT                puintBufferID                   						// Specify the zero based index of the buffer whose address you need.
)
{
	UINT uintID = SHS_ResolveLabelStringID ( penmStdHandleID , penmLabelType );

	if ( uintID )
	{	// The first two arguments passed the smell test. FB_LoadString is resplnsible for vetting the third argument.
		return FB_LoadString ( m_hinstMe , uintID , puintBufferID , FB_HIDE_LENGTH );
	}	// TRUE (anticipated outcome) block, if ( uintID )
	else
	{	// One or both of the first two arguments is invalid.
		return NULL;															// SHS_ResolveLabelStringID called SetLastError.
	}	// FALSE (UNanticipated outcome) block, if ( uintID )
}	// LPTSTR SHS_STANDARDHANDLESTATE_API SHS_GetLabelStringW