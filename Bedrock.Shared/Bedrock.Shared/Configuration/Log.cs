using System.Collections.Generic;

namespace Bedrock.Shared.Configuration
{
	public class Log
	{
		#region Properties
		public ActionMethod ActionMethod { get; set; }

		public Orm Orm { get; set; }
		#endregion
	}

	public class ActionMethod
	{
		#region Public Properties
		public bool IsEnabled { get; set; }

		public bool IsLogParameters { get; set; }

		public bool IsLogParameterValues { get; set; }

		public bool IsLogReturn { get; set; }

		public bool IsLogReturnValue { get; set; }
		#endregion
	}

	public class Orm
	{
        #region Fields
        private List<string> _categoryFilters;
        #endregion

        #region Public Methods
        public bool IsLog { get; set; }

		public bool IsConsole { get; set; }

		public bool IsDebug { get; set; }

		public bool IsFiltered { get; set; }

        public List<string> CategoryFilters
        {
            get
            {
                _categoryFilters = _categoryFilters ?? new List<string>();
                return _categoryFilters;
            }
            set { _categoryFilters = value; }
        }

        public bool IsEnableSensitiveDataLogging { get; set; }
		#endregion
	}
}
