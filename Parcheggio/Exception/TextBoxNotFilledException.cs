using System;

namespace Parcheggio.Exception
{
    public class TextBoxNotFilledException : SystemException
    {
        public TextBoxNotFilledException() : base("Riempire tutti i campi prima di premere Enter!") { }
    }
}
