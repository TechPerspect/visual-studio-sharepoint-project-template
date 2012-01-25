using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace SampleProject.Operations
{
    public abstract class BaseOperation
    {
        public SPWeb CurrentWeb;

        public BaseOperation(SPWeb objSPWeb)
        {
            CurrentWeb = objSPWeb;
        }
    }
}
