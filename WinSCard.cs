/**************************************************************************
 *
 *  Copyright 2013, Roger Brown
 *
 *  This file is part of Roger Brown's Toolkit.
 *
 *  This program is free software: you can redistribute it and/or modify it
 *  under the terms of the GNU Lesser General Public License as published by the
 *  Free Software Foundation, either version 3 of the License, or (at your
 *  option) any later version.
 * 
 *  This program is distributed in the hope that it will be useful, but WITHOUT
 *  ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 *  FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
 *  more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>
 *
 */

/* 
 * $Id: WinSCard.cs 8813 2023-03-07 20:44:43Z rogerb $
 */

using System;
using System.Runtime.InteropServices;

namespace MyFeederPS
{
    public class WinSCard
    {
        public const int
            SCARD_E_TIMEOUT = -2146435062;

        public const UInt32
            SCARD_STATE_UNAWARE = 0,
            SCARD_STATE_IGNORE = 1,
            SCARD_STATE_CHANGED = 2,
            SCARD_STATE_UNKNOWN = 4,
            SCARD_STATE_UNAVAILABLE = 8,
            SCARD_STATE_EMPTY = 0x10,
            SCARD_STATE_PRESENT = 0x20,
            SCARD_STATE_ATRMATCH = 0x40,
            SCARD_STATE_EXCLUSIVE = 0x80,
            SCARD_STATE_INUSE = 0x100,
            SCARD_STATE_MUTE = 0x200,
            SCARD_STATE_UNPOWERED = 0x400;

        public const UInt32
            SCARD_SCOPE_USER = 0,
            SCARD_SCOPE_TERMINAL = 1,
            SCARD_SCOPE_SYSTEM = 2;

        public const UInt32
            SCARD_SHARE_EXCLUSIVE = 1,
            SCARD_SHARE_SHARED = 2,
            SCARD_SHARE_DIRECT = 3;

        public const UInt32
            SCARD_LEAVE_CARD = 0,
            SCARD_RESET_CARD = 1,
            SCARD_UNPOWER_CARD = 2,
            SCARD_EJECT_CARD = 3;

        #region Win32
        // WinSCard APIs to be imported.
        [DllImport("WinSCard.dll")]
        public static extern int SCardEstablishContext(uint dwScope,
        IntPtr notUsed1,
        IntPtr notUsed2,
        out IntPtr phContext);

        [DllImport("WinSCard.dll")]
        public static extern int SCardReleaseContext(IntPtr phContext);

        [DllImport("WinSCard.dll")]
        public static extern int SCardConnect(IntPtr hContext,
        string cReaderName,
        uint dwShareMode,
        uint dwPrefProtocol,
        ref IntPtr phCard,
        ref IntPtr ActiveProtocol);

        [DllImport("WinSCard.dll")]
        public static extern int SCardDisconnect(IntPtr hCard, uint Disposition);

        // [DllImport("WinScard.dll")]
        // static extern int SCardListReaderGroups(IntPtr hContext,
        // ref string cGroups,
        // ref int nStringSize);

        [DllImport("WinSCard.dll", EntryPoint = "SCardListReadersW", CharSet = CharSet.Unicode)]
        public static extern int SCardListReaders(
          IntPtr hContext,
          char[] mszGroups,
          char[] mszReaders,
          ref UInt32 pcchReaders
          );


        // [DllImport("WinSCard.dll")]
        // static extern int SCardFreeMemory(IntPtr hContext,
        // string cResourceToFree);

        [StructLayout(LayoutKind.Sequential)]
        public struct SCARD_IO_REQUEST
        {
            public int dwProtocol;
            public int cbPciLength;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SCARD_READERSTATE
        {
            public string szReader;
            public IntPtr pvUserData;
            public UInt32 dwCurrentState;
            public UInt32 dwEventState;
            public UInt32 cbAtr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public byte[] m_rgbAtr;
        }

        [DllImport("WinSCard.dll", EntryPoint = "SCardGetStatusChangeW", CharSet = CharSet.Unicode)]
        static extern int SCardGetStatusChange(
            IntPtr hContext,
            UInt32 dwTimeout,
            [In,Out] SCARD_READERSTATE[] rgReaderStates,
            UInt32 cReaders);

        [DllImport("WinSCard.dll")]
        public static extern int SCardTransmit(IntPtr hCard,
            [In] ref SCARD_IO_REQUEST pioSendPci,
            byte[] pbSendBuffer,
            UInt32 cbSendLength,
            IntPtr pioRecvPci,
            [Out] byte[] pbRecvBuffer,
            ref UInt32 pcbRecvLength);

        [DllImport("WinSCard.dll", EntryPoint = "SCardStatusW", CharSet=CharSet.Unicode)]
        public static extern int SCardStatus(IntPtr hCard,
            [Out] char[] szReaderName,
            ref UInt32 pcchReaderLen,
            out UInt32 pdwState,
            out UInt32 pdwProtocol,
            [Out] byte[] pbAtr,
            ref UInt32 pcbAtrLen);

        #endregion
    }
}
