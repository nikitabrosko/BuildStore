using System;

namespace Application.Common.Exceptions
{
    public class ItemExistsException : Exception
    {
        public ItemExistsException()
            : base()
        {

        }

        public ItemExistsException(string message)
            : base(message)
        {

        }

        public ItemExistsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public ItemExistsException(string name, object id)
            : base($"Entity \"{name}\" is exists in db with id {id}!")
        {

        }
    }
}