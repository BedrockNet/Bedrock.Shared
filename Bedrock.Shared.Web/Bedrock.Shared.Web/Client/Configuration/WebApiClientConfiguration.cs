using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net.Http.Headers;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Web.Client.Configuration
{
    public class WebApiClientConfiguration
    {
        #region Fields
        private string _bearerToken;
        private string _baseAddress;

        private ObservableCollection<KeyValuePair<string, string>> _defaultRequestHeaders;
        private ObservableCollection<MediaTypeWithQualityHeaderValue> _acceptHeaders;
		private ObservableCollection<StringWithQualityHeaderValue> _acceptHeaderEncodings;

        private readonly MediaTypeWithQualityHeaderValue[] _defaultAcceptHeaders = new MediaTypeWithQualityHeaderValue[] { new MediaTypeWithQualityHeaderValue(StringHelper.Current.Lookup(StringMediaType.TextJson)) };
        private readonly StringWithQualityHeaderValue[] _defaultAcceptHeaderEncodings = new StringWithQualityHeaderValue[] { new StringWithQualityHeaderValue("gzip"), new StringWithQualityHeaderValue("deflate") };

        private bool _isObserving;
        #endregion

        #region Constuctors
        public WebApiClientConfiguration()
		{
			Initialize();
		}
        #endregion

        #region Public Events
        public event EventHandler<ConfigurationChangedEventArgs> ConfigurationChanged;
        #endregion

        #region Public Methods
        public void Reset()
        {
            BearerToken = null;
            BaseAddress = null;

            DefaultRequestHeaders.Clear();

            AcceptHeaders.Clear();
            _defaultAcceptHeaders.Each(dah => _acceptHeaders.Add(dah));

            AcceptHeaderEncodings.Clear();
            _defaultAcceptHeaderEncodings.Each(dahe => _acceptHeaderEncodings.Add(dahe));
        }
        #endregion

        #region Public Properties
        public static WebApiClientConfiguration Default => new WebApiClientConfiguration();

		public string BearerToken
        {
            get { return _bearerToken; }
            set
            {
                _bearerToken = value;
                OnConfigurationChanged(new ConfigurationChangedEventArgs(nameof(BearerToken)));
            }
        }

		public string BaseAddress
        {
            get { return _baseAddress; }
            set
            {
                _baseAddress = value;
                OnConfigurationChanged(new ConfigurationChangedEventArgs(nameof(BaseAddress)));
            }
        }

        public virtual ObservableCollection<KeyValuePair<string, string>> DefaultRequestHeaders
        {
            get
            {
                if (_defaultRequestHeaders == null)
                {
                    _defaultRequestHeaders = new ObservableCollection<KeyValuePair<string, string>>();
                    _defaultRequestHeaders.CollectionChanged += DefaultRequestHeadersChanged;
                }

                return _defaultRequestHeaders;
            }
            private set { _defaultRequestHeaders = value; }
        }

        public virtual ObservableCollection<MediaTypeWithQualityHeaderValue> AcceptHeaders
		{
			get
			{
				if (_acceptHeaders == null)
				{
					_acceptHeaders = new ObservableCollection<MediaTypeWithQualityHeaderValue>();
                    _defaultAcceptHeaders.Each(dah => _acceptHeaders.Add(dah));
                    _acceptHeaders.CollectionChanged += AcceptHeadersChanged;
                }

				return _acceptHeaders;
			}
            private set { _acceptHeaders = value; }
		}

        public virtual ObservableCollection<StringWithQualityHeaderValue> AcceptHeaderEncodings
		{
			get
			{
				if (_acceptHeaderEncodings == null)
				{
                    _acceptHeaderEncodings = new ObservableCollection<StringWithQualityHeaderValue>();
                    _defaultAcceptHeaderEncodings.Each(dahe => _acceptHeaderEncodings.Add(dahe));
                    _acceptHeaderEncodings.CollectionChanged += AcceptHeaderEncodingsChanged;
                }

				return _acceptHeaderEncodings;
			}
            private set { _acceptHeaderEncodings = value; }
        }
        #endregion

        #region Private Methods
        private void Initialize()
		{
            _isObserving = true;
        }

        private void DefaultRequestHeadersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnConfigurationChanged(new ConfigurationChangedEventArgs(nameof(DefaultRequestHeaders), true, e.OldItems, e.NewItems, e.Action));
        }

        private void AcceptHeadersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnConfigurationChanged(new ConfigurationChangedEventArgs(nameof(AcceptHeaders), true, e.OldItems, e.NewItems, e.Action));
        }

        private void AcceptHeaderEncodingsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnConfigurationChanged(new ConfigurationChangedEventArgs(nameof(AcceptHeaderEncodings), true, e.OldItems, e.NewItems, e.Action));
        }

        private void OnConfigurationChanged(ConfigurationChangedEventArgs e)
        {
            if(_isObserving)
                ConfigurationChanged?.Invoke(this, e);
        }
		#endregion
	}
}
