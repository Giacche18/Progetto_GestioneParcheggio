using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Parcheggio.Model;
using Parcheggio.Exception;

namespace Parcheggio.Controller
{
    public class ControllerCheckOut
    {
        private string tempoIngresso;
        private string veicolo;

        private string path_clienti;
        private string path_clienti1;

        public string TempoIngresso { get => tempoIngresso; set => tempoIngresso = value; }
        public string Veicolo { get => veicolo; set => veicolo = value; }
        public string Path_clienti { get => path_clienti; set => path_clienti = value; }
        public string Path_clienti1 { get => path_clienti1; set => path_clienti1 = value; }


        /* metodo per passare dalla form1 (ovvero la form principale) alla form4 (ovvero la form per fare il checkout del cliente) */
        public void ChangeView(ControllerCheckIn controllerCheckIn)
        {
            formCheckOut Check_Out = new formCheckOut(controllerCheckIn);
            Check_Out.ShowDialog();
        }

        /* metodo che mi permette di conoscere il percorso della cartella su cui si trovano i file dati "clienti" e "clienti1" e salvarli all'interno di variabili.
         * In modo da essere sicuro che su qualsiasi computer mi trovo i file saranno sempre trovati dal programma e non saranno generati errori oppure eccezioni. */
        public void GetFullPath()
        {
            this.Path_clienti = Path.GetFullPath("clienti.csv");
            this.Path_clienti1 = Path.GetFullPath("clienti1.csv");
        }

        /* metodo che prende in input una stringa (rappresentante un documento) da ricercare all'interno della memoria. 
         * Se abbiamo un riscontro i campi textbox vengono riempiti automaticamente con le informazioni relative a quel cliente
         * e la riga su file che contiene le informazioni relative al suddetto cliente verrà eliminata                          */
        public void SearchAndRemoveClient(string valoreControllo, TextBox textBoxCognome, TextBox textBoxNome, TextBox textBoxVeicolo, TextBox textBoxTarga, Label labelOraIngresso)
        {
            /* controllo all'interno della cartella se è presente il file "clienti.csv" o "clienti1.csv" */
            if (File.Exists(this.Path_clienti))
            {
                try
                {
                    /* creo una variabile di tipo "StreamReader" che mi servirà per poter aprire il file in modalità lettura, passandogli 
                     * il percorso del file                                                                                                   */
                    StreamReader streamreader = new StreamReader(this.Path_clienti);
                    /* creo una variabile di tipo "StreamWriter" che mi servirà per poter aprire il file in modalità scrittura, passandogli 
                     * il percorso alla cartella dove è presente il suddetto file e dicendogli che non deve mai sovrascriverne il contenuto */
                    StreamWriter streamwriter = new StreamWriter(this.Path_clienti1, true);

                    /* creo una variabile di tipo stringa che mi conterrà la riga che viene letta su file */
                    string strData;

                    /* ciclo for per scorrere e leggere tutte le righe del file */
                    for (; (strData = streamreader.ReadLine()) != null;)
                    {
                        /* salto le righe vuote*/
                        if (strData != "")
                        {
                            /* splitto il contenuto di una riga ogni volta che viene letto un ";" e salvo le singole informazioni all'interno delle celle di un array */
                            string[] tokens = strData.Split(';');

                            /* tokens[3] conterrà ad ogni iterazione il numero documento di un cliente. Questo valore viene confrontato con la stringa che viene passata come
                             * argomento del metodo. Se il confronto va a buon fine costruisco l'oggetto di tipo Anagrafica con le informazioni relative al suddetto documento
                             * e sempre con le stesse informazioni vado a riempire automaticamente le textbox presenti all'interno della form per il checkout.
                             * Oltre a tutto questo vado ad inizializzare le due proprietà della classe "Veicolo" "TempoIngresso" che mi serviranno successivamente nel metodo
                             * "CalculatePrice" */
                            if (valoreControllo == tokens[3])
                            {
                                Veicolo v = new Veicolo(tokens[4], tokens[5]);
                                Tempo t = new Tempo(tokens[6]);

                                Anagrafica objAnagrafica = new Anagrafica(tokens[0], tokens[1], tokens[2], tokens[3], v, t);

                                textBoxCognome.Text = objAnagrafica.Cognome;
                                textBoxNome.Text = objAnagrafica.Nome;
                                textBoxVeicolo.Text = objAnagrafica.Veicolo.TipoVeicolo;
                                textBoxTarga.Text = objAnagrafica.Veicolo.Targa;
                                labelOraIngresso.Text = objAnagrafica.Tempo.Tempo_ingresso;
                                this.Veicolo = objAnagrafica.Veicolo.TipoVeicolo;
                                this.TempoIngresso = objAnagrafica.Tempo.Tempo_ingresso;

                                /* elimino da data-file la stringa con le inforamzioni del cliente che ho ricercato tramite numero documento */
                                strData = "valoreControllo";
                            }
                        }

                        /* scrivo tutte le righe del vecchio file in un nuovo file, eccetto la riga che contiene il numero documento che ha reso vero il suddetto riscontro
                         * (quest'ultima cosa è possibile grazie al controllo sul valore di "strData" che viene inizializzato a "valoreControllo" solo se la ricerca del cliente va a buon fine). 
                         * Questo perchè uno stesso file non può essere aperto contemporaneamente in modalità scrittura o lettura e quindi per gestrire l'eliminazione 
                         * di un cliente da data-file leggo su "clienti.csv" e scrivo su "clienti1.csv". Nel ramo "else" vengono eseguite le stesse operazioni 
                         * ma agendo all'opposto, quindi scrivo su "clienti.csv" e leggo su "clienti1.csv"                                                                                       */
                        if (strData != "valoreControllo")
                        {
                            streamwriter.WriteLine(strData);
                        }
                    }

                    /* chiudo il file per rendere le modifiche permanenti */
                    streamreader.Close();
                    /* chiudo il file per rendere le modifiche permanenti */
                    streamwriter.Close();

                    /* "count" e "count1" vengono inizializzati con il numero di righe presenti all'interno dei data-file "clienti.csv" e "clienti1.csv (0 viceversa)*/
                    int count = File.ReadAllLines(this.Path_clienti).Length;
                    int count1 = File.ReadAllLines(this.Path_clienti1).Length;

                    /* vado a controllare (prima dell'eliminazione di uno dei due data-file) se presentano lo stesso numero di righe. 
                     * Questo mi serve peer verifivare se il numero documento inserito nella textbox all'interno della form checkout è stato trovato in memoria oppure no.
                     * Nel caso in cui non è stato trovato vuol dire che il valore è errato e quindi la cosa viene fatta presente all'utente tramite un messaggio di errore, 
                     * altrimenti si prosegue con l'esecuzione del programma che prevede: il rimepimento automatico dei campi (per una rapida verifica del cliente e del mezzo),
                     * eliminazione del cliente da data-file/memoria e il caloclo del prezzo per uscire dalla struttura */
                    if (count == count1)
                    {
                        throw new ClientNotFoundException();
                    }  
                }
                catch(ClientNotFoundException b)
                {
                    Console.WriteLine(b);
                    MessageBox.Show(b.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                /* elimino "clienti.csv" perchè "clienti1.csv" è il data-file aggiornato. Nel ramo "else" viene eseguita la stessa operazione ma agendo all'opposto,
                 * elimino "clienti1.csv" perchè "clienti.csv" è il data-file aggioranto                                                                            */
                File.Delete(this.Path_clienti);
            }
            else
            {
                try
                {
                    StreamReader streamreader = new StreamReader(this.Path_clienti1);
                    StreamWriter streamwriter = new StreamWriter(this.Path_clienti, true);

                    string strData;

                    for (; (strData = streamreader.ReadLine()) != null;)
                    {
                        if (strData != "")
                        {

                            string[] tokens = strData.Split(';');

                            if (valoreControllo == tokens[3])
                            {
                                Veicolo v = new Veicolo(tokens[4], tokens[5]);
                                Tempo t = new Tempo(tokens[6]);

                                Anagrafica objAnagrafica = new Anagrafica(tokens[0], tokens[1], tokens[2], tokens[3], v, t);

                                textBoxCognome.Text = objAnagrafica.Cognome;
                                textBoxNome.Text = objAnagrafica.Nome;
                                textBoxVeicolo.Text = objAnagrafica.Veicolo.TipoVeicolo;
                                textBoxTarga.Text = objAnagrafica.Veicolo.Targa;
                                labelOraIngresso.Text = objAnagrafica.Tempo.Tempo_ingresso;
                                this.Veicolo = objAnagrafica.Veicolo.TipoVeicolo;
                                this.TempoIngresso = objAnagrafica.Tempo.Tempo_ingresso;
                                strData = "valoreControllo";
                            }
                        }
                        if (strData != "valoreControllo")
                        {
                            streamwriter.WriteLine(strData);
                        }
                    }

                    streamreader.Close();
                    streamwriter.Close();

                    int count = File.ReadAllLines(this.Path_clienti).Length;
                    int count1 = File.ReadAllLines(this.Path_clienti1).Length;

                    if (count == count1)
                    {
                        throw new ClientNotFoundException();
                    }

                    
                }
                catch(ClientNotFoundException b)
                {
                    Console.WriteLine(b);
                    MessageBox.Show(b.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                File.Delete(this.Path_clienti1);
            }                
        }

        /* metodo che prende in ingresso una label (contente l'ora in cui un cliente esce dal parcheggio). 
         * Quest'ultima viene convertita in minuti e sottratta all'ora di ingresso (anch'essa convertita in minuti 
         * e relativa allo stesso cliente) in modo da calcolare il tempo in cui
         * il cliente ha lasciato il proprio veicolo all'interno della struttura. 
         * Nel calcolo del prezzo viene applicata una sovrattassa in base al tipo di veicolo, precisamente:
         * - camion         -> 50% del prezzo relativo alla permanenza nel parcheggio;
         * - macchina       -> 25% del prezzo relativo alla permanenza nel parcheggio;
         * - moto o scooter -> 10% del prezzo relativo alla permanenza nel parcheggio;                                  */
        public void CalculatePrice(Label labelOraUscita)
        {
            /* inizializzo la stringa "oraUscita" alla stringa contenuta nella label della form*/
            string oraUscita = labelOraUscita.Text;
            /* splitto il contenuto della stringa all'interno delle celle di un array quando incontro i ":" */
            string[] ore1 = oraUscita.Split(':');
            /* vado a convertire la stringa, che rappresenta l'ora del giorno in cui il cliente è uscito dal parcheggio, da ore in minuti */
            int minuti1 = (ore1.Length == 2) ? int.Parse(ore1[0]) * 60 + int.Parse(ore1[1]) : int.Parse(ore1[0]) * 60;

            /* splitto il contenuto della stringa (ottenuta precedentemente dal metodo "SearchAndRemoveClient") all'interno delle celle di un array quando incontro i ":" */
            string[] ore2 = this.tempoIngresso.Split(':');
            /* vado a convertire la stringa, che rappresenta l'orario in cui il cliente è entrato nel parcheggio, da ore in minuti */
            int minuti2 = (ore2.Length == 2) ? int.Parse(ore2[0]) * 60 + int.Parse(ore2[1]) : int.Parse(ore2[0]) * 60;

            /* creo una nuova variabile di tipo stringa*/
            string tempo_medio;
            /* creo una nuova variabile di tipo intero che mi conterrà il valore che mi rappresenta il tempo in minuti in cui il 
             * cliente ha lasciato la macchina all'interno del parcheggio (dato dalla differenza tra "oraUscita in minuti" e "tempoIngresso" in minuti) */
            int differenza_minuti = minuti1 - minuti2;

            /* converto il valore calcolato precedentemente da minuti in ore e poi lo trasformo da int a string, per poterlo salvare all'interno della variabile */
            tempo_medio = (differenza_minuti / 60).ToString() + ":" + (differenza_minuti % 60).ToString();

            /* creo una nuova variabile che sarà inizializzata al prezzo che il cliente dovrà pagare per uscire dal parcheggio */
            double prezzo = 0;

                /* serie di if/elseif per controllare quanto tempo il cliente ha lasciato il proprio veicolo all'interno del parcheggio e che tipo di veicolo aveva. 
                 * Tutto ciò per calcolare e comunicare al cliente il prezzo che deve pagare per poter uscire dal parcheggio                                        */
            if(differenza_minuti < 45)
            {
                if(this.Veicolo == "Scooter" || this.Veicolo == "Moto")
                {
                    prezzo = 0.50 + (0.50 * 0.1);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Macchina")
                {
                    prezzo = 0.50 + (0.50 * 0.25);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Camion")
                {
                    prezzo = 0.50 + (0.50 * 0.5);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
            }
            else if(differenza_minuti == 60)
            {
                if(this.Veicolo == "Scooter" || this.Veicolo == "Moto")
                {
                    prezzo = 1.50 + (1.50 * 0.1);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Macchina")
                {
                    prezzo = 1.50 + (1.50 * 0.25);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Camion")
                {
                    prezzo = 1.50 + (1.50 * 0.5);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
            }
            else if(differenza_minuti > 60 && differenza_minuti < 120)
            {
                if(this.Veicolo == "Scooter" || this.Veicolo == "Moto")
                {
                    prezzo = 1 + (1 * 0.1);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Macchina")
                {
                    prezzo = 1 + (1 * 0.25);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Camion")
                {
                    prezzo = 1 + (1 * 0.5);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
            }
            else if(differenza_minuti == 120)
            {
                if(this.Veicolo == "Scooter" || this.Veicolo == "Moto")
                {
                    prezzo = 1.20 + (1.20 * 0.1);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Macchina")
                {
                    prezzo = 1.20 + (1.20 * 0.25);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Camion")
                {
                    prezzo = 1.20 + (1.20 * 0.5);
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
            }
            else
            {
                int j;

                /* metodo for per calcolare il prezzo che il cliente deve pagare nel caso in cui abbia lasciato il veicolo all'interno del parcheggio per più di due ore */
                for(j = differenza_minuti; j >= 0; j--)
                {
                    if(j % 60 == 1)
                    {
                        prezzo += 1;
                    }
                }

                /* serie di if/elseif per andare ad aggiungere al suddetto prezzo la sovrattassa in base al tipo di veicolo del cliente */
                if(this.Veicolo == "Scooter" || this.Veicolo == "Moto")
                {
                    prezzo += prezzo * 0.1;
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Macchina")
                {
                    prezzo += prezzo * 0.25;
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
                else if(this.Veicolo == "Camion")
                {
                    prezzo += prezzo * 0.5;
                    MessageBox.Show("Prezzo sosta di " + prezzo + "€", "Prezzo", MessageBoxButtons.OK);
                }
            }
        }
    }
}
