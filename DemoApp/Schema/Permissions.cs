// Generated by SchemaGen 12.06.2023 07:00:32

using System;
using WisejLib;

namespace DemoApp.Schema
{
	[Tablename("Permissions")]
	public class Permissions : DbEntity
	{
		public int UserId { get => GetProperty<int>(); set => SetProperty(value); }
		public int PermissionId { get => GetProperty<int>(); set => SetProperty(value); }
		public bool REdit { get => GetProperty<bool>(); set => SetProperty(value); }
		public bool RDelete { get => GetProperty<bool>(); set => SetProperty(value); }
	}
}
