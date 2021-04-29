using System;

namespace Parcheggio.Exception
{
    public  class ClientNotFoundException : SystemException
    {
        public ClientNotFoundException() : base("Cliente non trovato!") { }
    }
}
