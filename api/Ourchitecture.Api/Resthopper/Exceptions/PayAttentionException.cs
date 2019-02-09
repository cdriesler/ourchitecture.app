using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Exceptions
{
    public class PayAttentionException : Exception
    {
        public PayAttentionException(string m) : base(m)
        {
            Console.WriteLine("Don't fuck up, asshole!");
        }
    }
}