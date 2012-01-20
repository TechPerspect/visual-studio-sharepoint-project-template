using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  $safeprojectname$
{
    public class GetEventHandlerAssemblyDetail
    {
        public static String GetAssemblyDetail()
        {
            return typeof(GetEventHandlerAssemblyDetail).Assembly.ToString();
        }
    }
}
