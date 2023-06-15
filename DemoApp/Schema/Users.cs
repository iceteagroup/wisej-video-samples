// Generated by SchemaGen 12.06.2023 07:00:32

using System;
using WisejLib;

namespace DemoApp.Schema
{
	[Tablename("Users")]
	public class Users : DbEntity
	{
		public string Loginname { get => GetProperty<string>(); set => SetProperty(value); }
		public string PasswordEnc { get => GetProperty<string>(); set => SetProperty(value); }
		public int Salutation { get => GetProperty<int>(); set => SetProperty(value); }
		public string Firstname { get => GetProperty<string>(); set => SetProperty(value); }
		public string Lastname { get => GetProperty<string>(); set => SetProperty(value); }
		public string JobTitle { get => GetProperty<string>(); set => SetProperty(value); }
		public DateTime? Hired { get => GetProperty<DateTime?>(); set => SetProperty(value); }
		public DateTime? Retired { get => GetProperty<DateTime?>(); set => SetProperty(value); }
		public bool IsAdmin { get => GetProperty<bool>(); set => SetProperty(value); }
		public bool IsActive { get => GetProperty<bool>(); set => SetProperty(value); }
	}
}
