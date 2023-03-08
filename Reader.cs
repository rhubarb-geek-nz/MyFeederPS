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
 * $Id: Reader.cs 8813 2023-03-07 20:44:43Z rogerb $
 */

using System;
using System.ComponentModel;

namespace MyFeederPS
{
    public class Reader : IDisposable
    {
        readonly internal IntPtr hContext;
        private bool bContext;

        public Reader()
        {
            int rc = WinSCard.SCardEstablishContext(WinSCard.SCARD_SCOPE_USER, IntPtr.Zero, IntPtr.Zero, out hContext);
            if (rc != 0) throw new Win32Exception(rc);
            bContext = true;
        }

        public void Dispose()
        {
            if (bContext)
            {
                bContext = false;
                WinSCard.SCardReleaseContext(hContext);
            }
        }

        ~Reader()
        {
            Dispose();
        }

        public string[] GetList()
        {
            UInt32 count = 0;
            int rc = WinSCard.SCardListReaders(hContext, null, null, ref count);
            if (rc != 0) throw new Win32Exception(rc);
            char[] mszReaders = new char[count];
            rc = WinSCard.SCardListReaders(hContext, null, mszReaders, ref count);
            if (rc != 0) throw new Win32Exception(rc);
            UInt32 i = 0, num = 0;

            while (i < count)
            {
                if (mszReaders[i] == 0) break;
                num++;
                while (mszReaders[i++] != 0) ;
            }

            string[] result = new string[num];
            num = 0;
            i = 0;
            while (i < count)
            {
                if (mszReaders[i] == 0) break;
                UInt32 off = i;
                while (mszReaders[i++] != 0) ;
                result[num++] = new string(mszReaders, (int)off, (int)(i - off - 1));
            }

            return result;
        }

        internal IntPtr activeProtocol = IntPtr.Zero, hCard = IntPtr.Zero;
        WinSCard.SCARD_IO_REQUEST ioSend = new WinSCard.SCARD_IO_REQUEST();

        public void Connect(string name)
        {
            int rc = WinSCard.SCardConnect(hContext, name, WinSCard.SCARD_SHARE_EXCLUSIVE, 3, ref hCard, ref activeProtocol);
            if (rc != 0) throw new Win32Exception(rc);
            ioSend.dwProtocol = (int)activeProtocol;
            ioSend.cbPciLength = 8;
        }

        public void Disconnect()
        {
            WinSCard.SCardDisconnect(hCard, WinSCard.SCARD_LEAVE_CARD);
        }

        public byte[] Transceive(byte[] apdu)
        {
            byte[] result = null;
            byte[] resp = new byte[256];
            uint respLen = (uint)resp.Length;

            int rc = WinSCard.SCardTransmit(
                    hCard,
                    ref ioSend,
                    apdu,
                    (UInt32)apdu.Length,
                    IntPtr.Zero,
                    resp,
                    ref respLen);

            if (rc != 0) throw new Win32Exception(rc);

            if ((respLen == 2) && (resp[0] == 0x61))
            {
                apdu = new byte[] { 0x00, 0xC0, 0x00, 0x00, resp[1] };

                respLen = (uint)resp.Length;

                rc = WinSCard.SCardTransmit(
                        hCard,
                        ref ioSend,
                        apdu,
                        (UInt32)apdu.Length,
                        IntPtr.Zero,
                        resp,
                        ref respLen);

                if (rc != 0) throw new Win32Exception(rc);
            }

            result = new byte[respLen];

            System.Array.Copy(resp, 0, result, 0, result.Length);

            return result;
        }

        public byte[] GetATR()
        {
            byte[] pbAtr = new byte[64];
            uint pcbAtrLen = (uint)pbAtr.Length;
            uint pdwState = 0, pdwProtocol = 0, pcchReaderLen = 0;
            int ret = WinSCard.SCardStatus(hCard, null, ref pcchReaderLen, out pdwState, out pdwProtocol, pbAtr, ref pcbAtrLen);
            if (ret != 0) throw new Win32Exception(ret);
            byte[] atr = new byte[pcbAtrLen];

            System.Array.Copy(pbAtr, 0, atr, 0, atr.Length);

            return atr;
        }
    }
}
