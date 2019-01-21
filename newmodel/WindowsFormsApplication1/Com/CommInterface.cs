using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Runtime.InteropServices;

using System.Diagnostics;
namespace HG.Communication
{

	interface CommInterface
	{   
    
		bool Open();
		bool Close();
		bool IsOpen();

		void Write(byte[] buffer);
		void Read(out byte[] buffer);
 
	}
}
