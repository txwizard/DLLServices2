/*
    ============================================================================

    Function Name:      SHS_ResolveLabelStringID

    Synopsis:           Compute the Win32 string resource ID of one of several
                        strings that describe a standard handle.

    Arguments:          penmStdHandleID = Use a SHS_STANDARD_HANDLE enumeration
                                          member to identify the standard stream
                                          for which a label is required.

                        penmLabelType   = Use a CSHS_HANDLE_LABELS enumeration
                                          member to identify the set of labels
                                          from which to select the label.

    Returns:            If the function succeeds, it returns the umeric ID of
                        the Win32 string resource to load.

    Remarks:            This routine resolves the Win32 string ID that function
                        SHS_ResolveLabelStringID returns to satisfy its
                        request.

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
    ============================================================================
*/


#include <StandardHandleState.H>                                                // Define the two enmerations and their typedefs that comprise the arguments.
#include ".\WWConAid_Pvt.H"                                                     // Bring our other goodies to the party. (Maybe. I may pull this.)
#include ".\resource.h"                                                         // Get the IDs of the string resources stored in the DLL.

#define SHS_SHORT_LABEL_BASE            ( OrdinalToArrayIndexP6C ( IDS_SHORT_LABEL_STDIN ) )
#define SHS_LONG_LABEL_BASE             ( OrdinalToArrayIndexP6C ( IDS_LONG_LABEL_STDIN ) )
#define SHS_ENUMERATION_LABEL_BASE      ( OrdinalToArrayIndexP6C ( IDS_STANDARD_HANDLE_CONSTANT_NAME_STDIN ) )

UINT __stdcall SHS_ResolveLabelStringID
(
    CSHS_STANDARD_HANDLE penmStdHandleID ,                                      // Specify the desired standard stream.
    CSHS_HANDLE_LABELS   penmLabelType                                          // Specify the set from which to select the label.
)
{
    switch ( penmStdHandleID )
    {
        case SHS_INPUT:
        case SHS_OUTPUT:
        case SHS_ERROR:
            switch ( penmLabelType )
            {
                case SHS_HANDLE_SHORT_LABEL:
                    #pragma warning ( suppress: 4307 4308 )
                    return SHS_SHORT_LABEL_BASE + penmStdHandleID;
                case SHS_HANDLE_LONG_LABEL:
					#pragma warning ( suppress: 4307 4308 )
					return SHS_LONG_LABEL_BASE + penmStdHandleID;
                case SHS_HANDLE_CONSTANT_NAME:
					#pragma warning ( suppress: 4307 4308 )
					return SHS_ENUMERATION_LABEL_BASE + penmStdHandleID;
                default:
                    SetLastError ( SHS_ERROR_INVALID_LABEL_GROUP );
                    return SHS_ERROR_INVALID_LABEL_GROUP;
            }   // switch ( penmLabelType )
        default:
            return  SFS_INVALID_LABEL_ID;
    }   // switch ( penmStdHandleID )
}   // UINT __stdcall SHS_ResolveLabelStringID