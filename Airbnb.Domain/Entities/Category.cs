﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Category : BaseEntity<int>
	{
		public string Name { get; set; } = string.Empty;
		public virtual ICollection<PropertyCategory> PropertyCategories { get; set; } = new HashSet<PropertyCategory>();

	}
}
