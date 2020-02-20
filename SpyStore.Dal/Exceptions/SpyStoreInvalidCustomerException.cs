using System;
using System.Collections.Generic;
using System.Text;

namespace SpyStore.Dal.Exceptions
{
    class SpyStoreInvalidCustomerException: SpyStoreException
    {
        public SpyStoreInvalidCustomerException()
        {}
        public SpyStoreInvalidCustomerException(string message) :base(message) 
        {}
        public SpyStoreInvalidCustomerException(string message, Exception innerException) : base(message, innerException)
        { }

    }
}
