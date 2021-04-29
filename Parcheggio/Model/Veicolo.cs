using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcheggio.Model
{
    public class Veicolo
    {
        private string _tipoVeicolo;
        private string _targa;

        public Veicolo(string tipoVeicolo, string targa)
        {
            _tipoVeicolo = tipoVeicolo;
            _targa = targa;
        }

        public string TipoVeicolo { get => _tipoVeicolo; set => _tipoVeicolo = value; }
        public string Targa { get => _targa; set => _targa = value; }

        public override string ToString()
        {
            return $"{_tipoVeicolo};{_targa};";
        }
    }
}
