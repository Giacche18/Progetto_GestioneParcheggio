  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parcheggio.Model
{
    public class Anagrafica
    {
        private string _nome;
        private string _cognome;
        private string _documento;
        private string _numDocumento;
        private Veicolo _veicolo;
        private Tempo _tempo;

        public Anagrafica(string nome, string cognome, string documento, string num_documento, Veicolo veicolo, Tempo tempo)
        {
            _nome = nome;
            _cognome = cognome;
            _documento = documento;
            _numDocumento = num_documento;
            _veicolo = veicolo;
            _tempo = tempo;
        }

        public string Nome { get => _nome; set => _nome = value; }
        public string Cognome { get => _cognome; set => _cognome = value; }
        public string Documento { get => _documento; set => _documento = value; }
        public string Num_documento { get => _numDocumento; set => _numDocumento = value; }
        internal Veicolo Veicolo { get => _veicolo; set => _veicolo = value; }
        internal Tempo Tempo { get => _tempo; set => _tempo = value; }

        public override string ToString()
        {
            return $"{_cognome};{_nome};{_documento};{_numDocumento};{_veicolo.ToString()}{_tempo.ToString()}";
        }
    }
}
