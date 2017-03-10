using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestR
{
    /// <summary>
    /// Custom TestR Exception instead of the generic Exception
    /// </summary>
    public class TestRException : Exception
    {
        /// <summary>
        /// Custom TestR Exception instead of the generic Exception
        /// </summary>
        /// <param name="message"></param>
        public TestRException(string message) : base(message)
        {
            
        }
    }
}
