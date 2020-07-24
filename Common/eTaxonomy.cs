using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliquely
{
	[Flags]
	public enum eTaxonomy
	{
		Archaea = 1,
		Bacteria = 2,
		Eukaryota = 4,
		All = Archaea | Bacteria | Eukaryota
	}
}
