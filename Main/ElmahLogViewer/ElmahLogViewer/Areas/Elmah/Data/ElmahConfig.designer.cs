﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElmahLogViewer.Areas.Elmah.Data
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Elmah")]
	public partial class ElmahConfigDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertELMAH_Error(ELMAH_Error instance);
    partial void UpdateELMAH_Error(ELMAH_Error instance);
    partial void DeleteELMAH_Error(ELMAH_Error instance);
    partial void InsertELMAH_Server(ELMAH_Server instance);
    partial void UpdateELMAH_Server(ELMAH_Server instance);
    partial void DeleteELMAH_Server(ELMAH_Server instance);
    #endregion
		
		public ElmahConfigDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["elmah-sqlserver"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ElmahConfigDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ElmahConfigDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ElmahConfigDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ElmahConfigDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ELMAH_Error> ELMAH_Errors
		{
			get
			{
				return this.GetTable<ELMAH_Error>();
			}
		}
		
		public System.Data.Linq.Table<ELMAH_Server> ELMAH_Servers
		{
			get
			{
				return this.GetTable<ELMAH_Server>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ELMAH_Error")]
	public partial class ELMAH_Error : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ErrorId;
		
		private string _Application;
		
		private string _Host;
		
		private string _Type;
		
		private string _Source;
		
		private string _Message;
		
		private string _User;
		
		private int _StatusCode;
		
		private System.DateTime _TimeUtc;
		
		private int _Sequence;
		
		private string _AllXml;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnErrorIdChanging(System.Guid value);
    partial void OnErrorIdChanged();
    partial void OnApplicationChanging(string value);
    partial void OnApplicationChanged();
    partial void OnHostChanging(string value);
    partial void OnHostChanged();
    partial void OnTypeChanging(string value);
    partial void OnTypeChanged();
    partial void OnSourceChanging(string value);
    partial void OnSourceChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    partial void OnUserChanging(string value);
    partial void OnUserChanged();
    partial void OnStatusCodeChanging(int value);
    partial void OnStatusCodeChanged();
    partial void OnTimeUtcChanging(System.DateTime value);
    partial void OnTimeUtcChanged();
    partial void OnSequenceChanging(int value);
    partial void OnSequenceChanged();
    partial void OnAllXmlChanging(string value);
    partial void OnAllXmlChanged();
    #endregion
		
		public ELMAH_Error()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorId", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ErrorId
		{
			get
			{
				return this._ErrorId;
			}
			set
			{
				if ((this._ErrorId != value))
				{
					this.OnErrorIdChanging(value);
					this.SendPropertyChanging();
					this._ErrorId = value;
					this.SendPropertyChanged("ErrorId");
					this.OnErrorIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Application", DbType="NVarChar(60) NOT NULL", CanBeNull=false)]
		public string Application
		{
			get
			{
				return this._Application;
			}
			set
			{
				if ((this._Application != value))
				{
					this.OnApplicationChanging(value);
					this.SendPropertyChanging();
					this._Application = value;
					this.SendPropertyChanged("Application");
					this.OnApplicationChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Host", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Host
		{
			get
			{
				return this._Host;
			}
			set
			{
				if ((this._Host != value))
				{
					this.OnHostChanging(value);
					this.SendPropertyChanging();
					this._Host = value;
					this.SendPropertyChanged("Host");
					this.OnHostChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Type", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if ((this._Type != value))
				{
					this.OnTypeChanging(value);
					this.SendPropertyChanging();
					this._Type = value;
					this.SendPropertyChanged("Type");
					this.OnTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Source", DbType="NVarChar(60) NOT NULL", CanBeNull=false)]
		public string Source
		{
			get
			{
				return this._Source;
			}
			set
			{
				if ((this._Source != value))
				{
					this.OnSourceChanging(value);
					this.SendPropertyChanging();
					this._Source = value;
					this.SendPropertyChanged("Source");
					this.OnSourceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Message", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[User]", Storage="_User", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string User
		{
			get
			{
				return this._User;
			}
			set
			{
				if ((this._User != value))
				{
					this.OnUserChanging(value);
					this.SendPropertyChanging();
					this._User = value;
					this.SendPropertyChanged("User");
					this.OnUserChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StatusCode", DbType="Int NOT NULL")]
		public int StatusCode
		{
			get
			{
				return this._StatusCode;
			}
			set
			{
				if ((this._StatusCode != value))
				{
					this.OnStatusCodeChanging(value);
					this.SendPropertyChanging();
					this._StatusCode = value;
					this.SendPropertyChanged("StatusCode");
					this.OnStatusCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TimeUtc", DbType="DateTime NOT NULL")]
		public System.DateTime TimeUtc
		{
			get
			{
				return this._TimeUtc;
			}
			set
			{
				if ((this._TimeUtc != value))
				{
					this.OnTimeUtcChanging(value);
					this.SendPropertyChanging();
					this._TimeUtc = value;
					this.SendPropertyChanged("TimeUtc");
					this.OnTimeUtcChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Sequence", AutoSync=AutoSync.Always, DbType="Int NOT NULL IDENTITY", IsDbGenerated=true)]
		public int Sequence
		{
			get
			{
				return this._Sequence;
			}
			set
			{
				if ((this._Sequence != value))
				{
					this.OnSequenceChanging(value);
					this.SendPropertyChanging();
					this._Sequence = value;
					this.SendPropertyChanged("Sequence");
					this.OnSequenceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AllXml", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string AllXml
		{
			get
			{
				return this._AllXml;
			}
			set
			{
				if ((this._AllXml != value))
				{
					this.OnAllXmlChanging(value);
					this.SendPropertyChanging();
					this._AllXml = value;
					this.SendPropertyChanged("AllXml");
					this.OnAllXmlChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ELMAH_Servers")]
	public partial class ELMAH_Server : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ServerId;
		
		private string _ConnectionString;
		
		private string _Name;
		
		private string _Environment;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnServerIdChanging(System.Guid value);
    partial void OnServerIdChanged();
    partial void OnConnectionStringChanging(string value);
    partial void OnConnectionStringChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnEnvironmentChanging(string value);
    partial void OnEnvironmentChanged();
    #endregion
		
		public ELMAH_Server()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServerId", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ServerId
		{
			get
			{
				return this._ServerId;
			}
			set
			{
				if ((this._ServerId != value))
				{
					this.OnServerIdChanging(value);
					this.SendPropertyChanging();
					this._ServerId = value;
					this.SendPropertyChanged("ServerId");
					this.OnServerIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ConnectionString", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
		public string ConnectionString
		{
			get
			{
				return this._ConnectionString;
			}
			set
			{
				if ((this._ConnectionString != value))
				{
					this.OnConnectionStringChanging(value);
					this.SendPropertyChanging();
					this._ConnectionString = value;
					this.SendPropertyChanged("ConnectionString");
					this.OnConnectionStringChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Environment", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string Environment
		{
			get
			{
				return this._Environment;
			}
			set
			{
				if ((this._Environment != value))
				{
					this.OnEnvironmentChanging(value);
					this.SendPropertyChanging();
					this._Environment = value;
					this.SendPropertyChanged("Environment");
					this.OnEnvironmentChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591