using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.CallForPapers.Advanced.Domain
{
    /// <summary>
    /// Classe Helper per i tag
    /// </summary>
    [Serializable]
    [CLSCompliant(true)]
    public class AdvTagHelper
    {
        /// <summary>
        /// Lista tag
        /// </summary>
        private List<KeyValuePair<int, string>> TagList { get; set; }

        /// <summary>
        /// SE sono presenti TAG
        /// </summary>
        public bool HasValue { get; set; }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="tags">Stringa con i tag, separati da virgola ","</param>
        public AdvTagHelper(string tags)
        {
            TagList = new List<KeyValuePair<int, string>>();

            if (string.IsNullOrWhiteSpace(tags))
            {
                HasValue = false;
                return;
            }

            int index = 0;

            foreach (string tag in tags.Split(',').Distinct().ToList())
            {
                if (!String.IsNullOrWhiteSpace(tag))
                {
                   TagList.Add(new KeyValuePair<int, string>(index, tag));
                }

                index++;
            }

            HasValue = TagList.Any();
        }


        /// <summary>
        /// Dopo l'inizializzazione, dato un TAG recupera il suo indice per il contesto corrente.
        /// </summary>
        /// <param name="tag">Tag di cui identificare l'indice</param>
        /// <returns>L'indice del tag, inteso come identificativo del tag all'interno dell'oggetto corrente.</returns>
        public int GetTagIndex(string tag)
        {
            if (TagList == null || !TagList.Any())
            {
                HasValue = false;
                return  - 1;
            }

            try
            {
                KeyValuePair<int, string> kvp = TagList.FirstOrDefault(k =>
                k.Value == tag);

                return kvp.Key;
            }
            catch (Exception)
            {
                
            }

            return -1;
        }

        /// <summary>
        /// Crea la classe css relativa al tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetTagCssSingleString(string tag)
        {
            return string.Format("tag_{0}", GetTagIndex(tag));
        }


        /// <summary>
        /// Dopo l'inizializzazione, restituisce l'elenco corrente.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetKvpList()
        {
            return TagList;
        }

        /// <summary>
        /// Multistringa del CSS per identificare i campi (più stringhe css)
        /// </summary>
        /// <param name="fieldTags">Stringa con più tag separati da ,</param>
        /// <returns></returns>
        public string GetTagCssMultiString(string fieldTags)
        {

            if (string.IsNullOrWhiteSpace(fieldTags))
                return "";

            IList<string> fieldTagsList = fieldTags.Split(',').Distinct().ToList();

            if (fieldTagsList == null || !fieldTagsList.Any())
            {
                return "";
            }


            string css = "";

            foreach (string value in fieldTagsList)
            {
                css = String.Format("{0} {1}", css, GetTagCssSingleString(value));
            }

            return css;
        }
    }
}
