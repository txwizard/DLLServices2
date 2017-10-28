#if !defined ( __WWCONAIDLIB_PVT_P6C__ )
    #define __WWCONAIDLIB_PVT_P6C__
#endif /* #if !defined ( __WWCONAIDLIB_PVT_P6C__) */

#define __WWCONAID_BUILDING_STDIN_WRAPPERS__
#define __WWCONAID_BUILDING_GETSTDHANDLEFN_NATIVE__

#define UNICODE


#include <StandardHandleState.H>
#include ".\WWConAid_Pvt.H"
#include ".\resource.h"


LPTSTR SHS_STANDARDHANDLESTATE_API	SHS_GetStdHandleFNW
(
    CSHS_STANDARD_HANDLE penmStdHandleID ,								// Specify the desired standard stream.
    CUINT                puintBufferID                   				// Specify the zero based index of the buffer whose address you need.
)
{
    #include ".\SHS_GetStdHandleFN.INL"
}	// SHS_GetStdHandleFNW