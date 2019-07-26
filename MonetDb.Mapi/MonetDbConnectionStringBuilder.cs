﻿namespace MonetDb.Mapi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Provides a simple way to create and manage the contents of connection strings used by the <see cref="MonetDbConnection"/> class.
    /// </summary>
    public class MonetDbConnectionStringBuilder : DbConnectionStringBuilder
    {
        private static Dictionary<PropertyInfo, object> defaultValues;

        private string database;
        private string host;
        private string username;
        private string password;
        private int port;
        private int poolMinimum;
        private int poolMaximum;


        static MonetDbConnectionStringBuilder()
        {
            defaultValues = typeof(MonetDbConnectionStringBuilder).GetProperties()
                .Where(pr => pr.GetCustomAttribute<ObsoleteAttribute>() == null && pr.GetCustomAttribute<DefaultValueAttribute>() != null)
                .ToDictionary(pr => pr, pr => pr.GetCustomAttribute<DefaultValueAttribute>().Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonetDbConnectionStringBuilder"/> class
        /// </summary>
        public MonetDbConnectionStringBuilder()
        {
            this.Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonetDbConnectionStringBuilder"/> class, optionally using ODBC rules for quoting values.
        /// </summary>
        /// <param name="useOdbcRules">true to use {} to delimit fields; false to use quotation marks.</param>
        public MonetDbConnectionStringBuilder(bool useOdbcRules) : base(useOdbcRules)
        {
            this.Init();
        }

        /// <summary>
        /// The username to connect with
        /// </summary>
        [Category("Connection")]
        [Description("The username to connect with")]
        [DisplayName("Username")]
        public string Username
        {
            get => this.username;
            set
            {
                this.username = value;
                this.SetValue(nameof(Username), value);
            }
        }

        /// <summary>
        /// The password to connect with
        /// </summary>
        [Category("Connection")]
        [Description("The password to connect with")]
        [DisplayName("Password")]
        [PasswordPropertyText(true)]
        public string Password
        {
            get => this.password;
            set
            {
                this.password = value;
                this.SetValue(nameof(Password), value);
            }
        }

        /// <summary>
        /// The MonetDb database to connect to
        /// </summary>
        [Category("Connection")]
        [Description("The MonetDb database to connect to")]
        [DisplayName("Database")]
        public string Database
        {
            get => this.database;
            set
            {
                this.database = value;
                this.SetValue(nameof(Database), value);
            }
        }

        /// <summary>
        /// The hostname or IP address of the MonetDb server to connect to
        /// </summary>
        [Category("Connection")]
        [DefaultValue("localhost")]
        [Description("The hostname or IP address of the MonetDb server to connect to")]
        [DisplayName("Host")]
        public string Host
        {
            get => this.host;
            set
            {
                this.host = value;
                this.SetValue(nameof(Host), value);
            }
        }

        /// <summary>
        /// The TCP port of the MonetDb server
        /// </summary>
        [Category("Connection")]
        [DefaultValue(50000)]
        [Description("The TCP port of the MonetDb server")]
        [DisplayName("Port")]
        public int Port
        {
            get => this.port;
            set
            {
                this.port = value;
                this.SetValue(nameof(Port), value);
            }
        }

        /// <summary>
        /// The minimum connection pool size
        /// </summary>
        [Category("Pooling")]
        [DefaultValue(100)]
        [Description("The minimum connection pool size.")]
        [DisplayName("Minimum Pool Size")]
        public int PoolMinimum
        {
            get => this.poolMinimum;
            set
            {
                this.poolMinimum = value;
                this.SetValue(nameof(PoolMinimum), value);
            }
        }

        /// <summary>
        /// The maximum connection pool size
        /// </summary>
        [Category("Pooling")]
        [DefaultValue(0)]
        [Description("The maximum connection pool size")]
        [DisplayName("Maximum Pool Size")]
        public int PoolMaximum
        {
            get => this.poolMaximum;
            set
            {
                this.poolMaximum = value;
                this.SetValue(nameof(PoolMaximum), value);
            }
        }

        private void Init()
        {
            foreach (var prDef in defaultValues)
            {
                prDef.Key.SetValue(this, prDef.Value);
            }
        }

        private void SetValue(string propertyName, object value)
        {
            propertyName = propertyName.ToUpper();
            if (value == null)
            {
                base.Remove(propertyName);
            }
            else
            {
                base[propertyName] = value;
            }
        }
    }
}