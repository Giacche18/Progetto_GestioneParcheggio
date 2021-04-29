using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Parcheggio.Model;
using Parcheggio.Exception;

namespace Parcheggio.Controller
{
    public class ControllerCheckIn 
    {
        /* creo una lista di oggetti di tipo Anagrafica */
        private List<Anagrafica> riferimentoAnagrafica =  new List<Anagrafica>();

        /* metodo per passare dalla form1 (ovvero la form principale) alla form2 (ovvero la form per fare il checkin dei clienti) */
        public void ChangeViewCheckIn()
        {
            formCheckIn Check_in = new formCheckIn(this);
            Check_in.ShowDialog();
        }

        /* metodo per acquisire i dati in input dalle textbox/combobox della form e 
         * creare i miei oggetti di tipo Anagrafica/Veicolo/Tempo da inserire nella lista */
        public void AddDati(string cognome, string nome, string documento, string num_documento, string tipoVeicolo, string targa, string ora)
        {
            try
            {
                /* controllo che tutti i campi siano stati rimepiti prima di procedere con la costruzione degli oggetti e il salvataggio in memoria. 
                 * Se anche solo una textbox risulta essere vuota viene avvisato l'utente con un messaggio di errore e i campi vengono svuotati, pronti 
                 * per il nuovo inserimento                                                                                                             */
                if (cognome == "" ||
                    nome == "" ||
                    documento == "" ||
                    num_documento == "" ||
                    tipoVeicolo == "" ||
                    targa == "")
                {
                    throw new TextBoxNotFilledException();
                }
                else
                {
                    /* controllo che il nome del cliente contenga solo lettere e che inizi con una lettera maiuscola */
                    if (!Regex.Match(nome, "^[A-Z][a-zA-Z]*$").Success)
                    {
                        MessageBox.Show("Il nome del cliente può contenere solo lettere e deve iniziare con una lettera maiuscola!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    /* controllo che il cognome del cliente contenga solo lettere e che inizi con una lettera maiuscola */
                    else if (!Regex.Match(cognome, "^[A-Z][a-zA-Z]*$").Success)
                    {
                        MessageBox.Show("Il cognome del cliente può contenere solo lettere e deve iniziare con una lettera maiuscola!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        /* creo un nuovo oggetto di tipo Veicolo, che è formato dal tipo e dalla targa del veicolo del cliente */
                        Veicolo v = new Veicolo(tipoVeicolo, targa);
                        /* creo un nuovo oggetto di tipo Tempo, che è formato dall'ora in cui il cliente è entrato nel parcheggio */
                        Tempo t = new Tempo(ora);

                        /* creo un nuovo oggetto di tipo Anagrafica, che è formato da tutte le informazioni personali del cliente 
                         * più gli oggetti Tempo e Veicolo precedentemente costruiti */
                        Anagrafica objAnagrafica = new Anagrafica(nome, cognome, documento, num_documento, v, t);

                        /* riempio la mia lista con oggetti di tipo Anagrafica*/
                        this.riferimentoAnagrafica.Add(objAnagrafica);

                        /* invovo il metodo WriteOnFile passandogli l'oggetto di tipo Anagrafica precedentemente creato per scrivere
                         * le proprietà di cui è composto (ovvero tutte le informazioni relative all'utente) su un data-file. 
                         * Questo per ovviare al problema della perdita dei dati dovuta alla erronea chiusura del programma          */
                        this.WriteOnFile(objAnagrafica);
                    }
                }
            }
            catch (TextBoxNotFilledException e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /* metodo a cui viene passato in input un oggetto di tipo Anagrafica 
         * che viene istanziato nel metodo AddDati
         * e che scrive su file le proprietà di cui è composto per creare un data-file */
        public void WriteOnFile(Anagrafica obj)
        {
            /* controllo all'interno della cartella se è presente il file "clienti.csv" o "clienti1.csv", su cui dobbiabo andare a scrivere le informazioni del cliente */
            if (File.Exists("clienti.csv"))
            {
                /* creo una variabile di tipo "StreamWriter" che mi servirà per poter aprire il file in modalità scrittura, passandogli 
                 * il percorso alla cartella dove è presente il suddetto file e dicendogli che non deve mai sovrascriverne il contenuto */
                StreamWriter streamwriter = new StreamWriter("clienti.csv", true);

                /* inizializzo strData con la stringa che contiene tutte le informazioni relative al cliente e che viene elaborata dal metodo "ToString()" */
                string strData = $"{obj.ToString()}";

                /* scrivo la stringa su file */
                streamwriter.WriteLine(strData);
                /* chiudo il file per rendere le modifiche permanenti */
                streamwriter.Close();
            }
            else
            {
                /* creo una variabile di tipo "StreamWriter" che mi servirà per poter aprire il file in modalità scrittura, passandogli 
                 * il percorso alla cartella dove è presente il suddetto file e dicendogli che non deve mai sovrascriverne il contenuto */
                StreamWriter streamwriter = new StreamWriter("clienti1.csv", true);

                /* inizializzo strData con la stringa che contiene tutte le informazioni relative al cliente e che viene elaborata dal metodo "ToString()" */
                string strData = $"{obj.ToString()}";

                /* scrivo la stringa su file */
                streamwriter.WriteLine(strData);
                /* chiudo il file per rendere le modifiche permanenti */
                streamwriter.Close();
            }        
        }

        /* metodo che prende in input una stringa che rappresenta il numero del documento d'identità
         * del cliente da ricercare in memoria.
         * Se la ricerca va a buon fine tutti i dati relativi a quel cliente vengono rimossi perchè uscito dal parcheggio.
         * Altrimenti viene comunicato che il cliente non è presente in memoria                                             */
        public void RemoveData(string num_documento)
        {
            /* vado a ricercare all'interno della mia lista l'oggetto che presenta la proprietà "Num_documento" uguale alla stringa 
             * che viene passata come argomento del metodo. 
             * Se avviene il riscontro allora item viene inilizzato all'oggetto ricercato                                           */
            var item = this.riferimentoAnagrafica.Find(x => x.Num_documento == num_documento);
            
            /* controllo il valore di item e se quest'ultimo rappresenta un oggetto lo rimuovo dalla memoria, altrimenti viene avvisato l'utente 
             * che il cliente relativo a quel numero documento non è presente in memoria                                                        */
            if(this.riferimentoAnagrafica.Remove(item))
            {
                MessageBox.Show("Cliente rimosso con successo!", "", MessageBoxButtons.OK);
            }          
        }

        /* metodo per passare dalla form1 (ovvero la form principale) alla form3 (form per fare l'update dei clienti da file in caso di chiusura erronea del programma) */
        public void ChangeViewUpdateClient()
        {
            formUpdateClient View_Client = new formUpdateClient(this);
            View_Client.ShowDialog();
        }

        /* metodo che mi apre il data-file in modalità lettura e legge una alla volta tutte le righe di cui è composto.
         * La riga (acqusita come stringa) viene splittata e le singole informazioni vengono salvate all'interno di un array di appoggio. 
         * Dopo di che con queste informazioni viene creato l'oggetto di tipo Anagrafica (che fa riferimento ad un cliente) che viene salvato in memoria 
         * Tutto ciò è stato implementato per ovviare alla perdita di informazioni dovuta alla erronea chiusura del programma                            */
        public void UpdateClient(ListBox listbox)
        {
            /* controllo all'interno della cartella se è presente il file "clienti.csv" o "clienti1.csv", su cui dobbiabo andare a scrivere le informazioni del cliente */
            if (File.Exists("clienti.csv"))
            {
                /* apro il data-file in modalità lettura */
                StreamReader streamreader = new StreamReader("clienti.csv");

                string strData;

                /* leggo una per volta tutte le righe di cui è composto il data-file */
                for (; (strData = streamreader.ReadLine()) != null;)
                {
                    /* salto le righe vuote */
                    if (strData != "")
                    {
                        /* splitto il contenuto della riga nel momento in cui incontro ";" e salvo le singole informazioni all'interno di un array di appoggio */
                        string[] tokens = strData.Split(';');

                        /* creo un nuovo oggetto di tipo Veicolo, che è formato dal tipo e dalla targa del veicolo del cliente */
                        Veicolo v = new Veicolo(tokens[4], tokens[5]);

                        /* creo un nuovo oggetto di tipo Tempo, che è formato dall'ora in cui il cliente è entrato nel parcheggio */
                        Tempo t = new Tempo(tokens[6]);


                        /* creo un nuovo oggetto di tipo Anagrafica, che è formato da tutte le informazioni personali del cliente 
                         * più gli oggetti Tempo e Veicolo precedentemente costruiti */
                        Anagrafica objAnagrafica = new Anagrafica(tokens[0], tokens[1], tokens[2], tokens[3], v, t);

                        /* riempio la mia lista con oggetti di tipo Anagrafica*/
                        this.riferimentoAnagrafica.Add(objAnagrafica);

                        /* stampo le informazioni all'interno di una listbox inserita all'interno della formUpdateClient in modo che l'utente, se vuole, possa fare un recup di quello che era stato salvato
                         * in memoria prima della erronea chiusura del programma                                                                                                                                   */
                        listbox.Items.Add(string.Join(", ", tokens));
                    }
                }

                /* chiudo il file per rendere le modifiche permanenti */
                streamreader.Close();
            }
            else
            {
                StreamReader streamreader = new StreamReader("clienti1.csv");

                string strData;

                for (; (strData = streamreader.ReadLine()) != null;)
                {
                    if (strData != "")
                    {
                        string[] tokens = strData.Split(';');

                        Veicolo v = new Veicolo(tokens[4], tokens[5]);

                        Tempo t = new Tempo(tokens[6]);

                        Anagrafica objAnagrafica = new Anagrafica(tokens[0], tokens[1], tokens[2], tokens[3], v, t);
                        this.riferimentoAnagrafica.Add(objAnagrafica);

                        listbox.Items.Add(string.Join(", ", tokens));
                    }
                }

                streamreader.Close();
            }
        }
    }
}
