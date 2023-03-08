/**************************************************************************
 *
 *  Copyright 2023, Roger Brown
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
 * $Id: MyFeederPS.cs 58 2023-03-07 23:55:28Z rhubarb-geek-nz $
 */

using System.ComponentModel;
using System.Management.Automation;

namespace MyFeederPS
{
    [Cmdlet(VerbsCommon.Open,"MyFeederPS")]
    [OutputType(typeof(Reader))]
    public class OpenMyFeederPS : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            WriteObject(new Reader());
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }

    [Cmdlet(VerbsCommon.Get,"MyFeederPS")]
    [OutputType(typeof(string))]
    public class GetMyFeederPS : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public Reader Reader { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            try
            {
                string[] list = Reader.GetList();
                foreach (string name in list)
                {
                    WriteObject(name);
                }
            }
            catch (Win32Exception)
            {
            }
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }

    [Cmdlet(VerbsCommon.Close, "MyFeederPS")]
    [OutputType(typeof(void))]
    public class CloseMyFeederPS : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public Reader Reader { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            Reader.Dispose();
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }

    [Cmdlet(VerbsCommunications.Disconnect, "MyFeederPS")]
    [OutputType(typeof(void))]
    public class DisconnectMyFeederPS : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public Reader Reader { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            Reader.Disconnect();
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }

    [Cmdlet(VerbsCommunications.Read, "MyFeederPS")]
    [OutputType(typeof(byte[]))]
    public class ReadMyFeederPS : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public Reader Reader { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            WriteObject(Reader.GetATR());
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }

    [Cmdlet(VerbsCommunications.Connect, "MyFeederPS")]
    [OutputType(typeof(void))]
    public class ConnectMyFeederPS : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public Reader Reader { get; set; }
        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            Reader.Connect(Name);
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }

    [Cmdlet(VerbsCommunications.Send, "MyFeederPS")]
    [OutputType(typeof(byte[]))]
    public class SendMyFeederPS : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public Reader Reader { get; set; }
        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public byte [] APDU { get; set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        protected override void ProcessRecord()
        {
            WriteObject(Reader.Transceive(APDU));
        }

        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }
}
