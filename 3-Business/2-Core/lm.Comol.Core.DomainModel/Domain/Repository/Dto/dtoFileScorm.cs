
using System;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class dtoFileScorm : DomainObject<long>
	{
        public long NumberOfPlay { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }

		public dtoFileScorm()
		{
		}

		public dtoFileScorm(CommunityFile oFile, long Number)
		{
			NumberOfPlay = Number;
		    Name = oFile.Name;
			this.Id = oFile.Id;
			Extension = oFile.Extension;
			FullName = oFile.DisplayName;
		}

	}
}