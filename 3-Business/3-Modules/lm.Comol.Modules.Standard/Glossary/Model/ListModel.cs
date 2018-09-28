using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Glossary.Domain;
using lm.Comol.Modules.Standard.Glossary.Domain.Dto;

namespace lm.Comol.Modules.Standard.Glossary.Model
{
    public class ListModel
    {
        public GlossaryGroup Group { get; set; }
        public IList<GlossaryItem> Items { get; set; }
        public IList<ListItemDTO> ColumnsItems { get; set; }
        public Char SelectedLetter { get; set; }

        public IList<LetterDto> AllLetters { get; set;}
        //public IDictionary<Char, LetterDto> AllLetters { get; set; }
        
        public IList<Char> UsedLetters { get; set; }
        //public IDictionary<Char, LetterDto> UsedLetters { get; set; }

        /// <summary>
        /// Numero di termini per la lettera specificata
        /// </summary>
        public IDictionary<Char, Int32> LetterRecurrence { get; set; }

        public DefaultView CurrentView { get; set; }
        public Int32 CurrentPage { get; set; }
        public Int32 TotalPage { get; set; }

        public Boolean HasElement { get; set; }

        public ModuleGlossary Permission { get; set; }
    }
}
