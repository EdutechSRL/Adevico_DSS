
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.FileRepository.Domain
{
	[Serializable()]
	public class VersionPermission
	{
		public virtual Boolean Link  { get; set; }

        public virtual Boolean Download { get; set; }
		public virtual Boolean Play  { get; set; }
		public virtual Boolean Edit  { get; set; }
        public virtual Boolean Preview { get; set; }
        public virtual Boolean SetActive { get; set; }
        public VersionPermission()
		{
		}

	}
}