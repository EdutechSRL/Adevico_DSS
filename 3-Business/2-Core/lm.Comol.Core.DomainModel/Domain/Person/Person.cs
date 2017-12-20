

using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class Person : DomainObject<int>, iPerson
	{
        public virtual String TaxCode { get; set; }
        public virtual long IdDefaultProvider { get; set; }
        public virtual int AuthenticationTypeID { get; set; }
        public virtual int TypeID { get; set; }
        public virtual String Login { get; set; }
        public virtual String Name { get; set; }
        public virtual String Password { get; set; }

        public virtual String Surname { get; set; }
        public virtual String FirstLetter { get; set; }

        public virtual String Mail { get; set; }
        public virtual Boolean isDisabled { get; set; }
        public virtual DateTime? LastAccessOn { get; set; }
        public virtual DateTime? PreviousAccess { get; set; }
        
        public virtual Boolean AcceptPolicy { get; set; }
        public virtual DateTime? AcceptPolicyOn { get; set; }
        public virtual String FotoPath { get; set; }
		public virtual string SurnameAndName {
            get { return Surname + " " + Name; }
            set { }
		}
        public virtual string NameAndSurname
        {
            get { return Name + " " + Surname; }
            set { }
        }
        public virtual DateTime CreatedOn { get; set; }

        public virtual int LanguageID { get; set; }


        public virtual String Sector { get; set; }
        public virtual String Job { get; set; }

        public virtual PersonInfo PersonInfo { get; set; }

		public Person()
		{
            PersonInfo = new PersonInfo();
		}
	}


    [Serializable(), CLSCompliant(true)]
	public class expPerson : DomainObject<int>
	{
        public virtual String TaxCode { get; set; }
        public virtual long IdDefaultProvider { get; set; }
        public virtual int AuthenticationTypeID { get; set; }
        public virtual int TypeID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Surname { get; set; }
        public virtual String FirstLetter { get; set; }

        public virtual String Mail { get; set; }
        public virtual Boolean isDisabled { get; set; }
        public virtual DateTime? LastAccessOn { get; set; }
        public virtual DateTime? PreviousAccess { get; set; }
        public virtual DateTime? AcceptPolicyOn { get; set; }
		public virtual string SurnameAndName {
            get { return Surname + " " + Name; }
            set { }
		}

        public virtual DateTime CreatedOn { get; set; }

        public virtual int LanguageID { get; set; }

        public expPerson()
		{
		}
	}


    [Serializable(), CLSCompliant(true)]
    public class litePerson : DomainObject<int>
    {
        public virtual String TaxCode { get; set; }
        public virtual int TypeID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Surname { get; set; }
        public virtual String FirstLetter { get; set; }

        public virtual String Mail { get; set; }
        public virtual Boolean isDisabled { get; set; }
        public virtual DateTime? LastAccessOn { get; set; }
        public virtual DateTime? PreviousAccess { get; set; }
        public virtual String PhotoPath { get; set; }
        public virtual String OfficeHours { get; set; }

        
        public virtual string SurnameAndName
        {
            get { return Surname + " " + Name; }
            set { }
        }

        public virtual int LanguageID { get; set; }

        public litePerson()
        {
        }


        public virtual String Sector { get; set; }
        public virtual String Job { get; set; }
    }

    [Serializable(), CLSCompliant(true)]
    public class ExtendedPerson : Person
    {
        public virtual PersonInfo PersonInfo { get; set; }

        public ExtendedPerson() : base()
        {
            PersonInfo = new PersonInfo();
        }

    }
}