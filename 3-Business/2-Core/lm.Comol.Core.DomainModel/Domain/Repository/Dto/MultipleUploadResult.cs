
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel
{
	[Serializable(), CLSCompliant(true)]
	public class MultipleUploadResult<T>
	{
		public List<T> UploadedFile {get;set;}
        public List<T> NotuploadedFile { get; set; }

		public MultipleUploadResult()
		{
			UploadedFile = new List<T>();
			NotuploadedFile = new List<T>();
		}
	}
}