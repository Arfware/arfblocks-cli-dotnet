using System;

namespace TodoApp.Domain.Entities.Base
{
	public class BaseEntity : CoreEntity
	{
		public bool IsDeleted { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
