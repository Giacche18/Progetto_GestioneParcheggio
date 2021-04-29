using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcheggio.Model
{
    public class Tempo
    {
        private string _tempoIngresso; 

        public Tempo(string tempoIngresso)
        {
            _tempoIngresso = tempoIngresso;
        }

        public string Tempo_ingresso { get => _tempoIngresso; set => _tempoIngresso = value; }

        public override string ToString()
        {
            return $"{_tempoIngresso};";
        }
    }
}
