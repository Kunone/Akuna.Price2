using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akuna.PriceService;
using System.Timers;
using System.Collections;
using kun.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Collections.Concurrent;

namespace kun.PriceUI
{
    public class PriceUpdateViewModel : NotifyObject, IDisposable
    {
        private readonly IPriceService _priceUpdateService;
        private readonly Dispatcher _dispatcher;
        private readonly Task _bgWorkerTask;
        private readonly Timer _uiUpdateTimer;

        private ConcurrentDictionary<uint, Price> _lastestPrices;
        private bool _isInitialized = false;

        public PriceUpdateViewModel(IPriceService priceService)
        {
            _priceUpdateService = priceService;
            _uiUpdateTimer = new Timer();
            _dispatcher = Dispatcher.CurrentDispatcher;
            _bgWorkerTask = Task.Factory.StartNew(()=> { });

            StartCommand = new DelegatedCommand<object>(StartCommand_Executed, StartCommand_CanExecuted);
            StopCommand = new DelegatedCommand<object>(StopCommand_Executed, StopCommand_CanExecuted);

            PricesUI = new ObservableCollection<Price>();
            _lastestPrices = new ConcurrentDictionary<uint, Price>();
        }

        /// <summary>
        /// used to receive updates
        /// </summary>
        private void _priceUpdateService_NewPricesArrived(IPriceService sender, uint instrumentID, IPrices prices)
        {
            var price = new Price()
            {
                AskPx = prices.AskPx,
                AskQty = prices.AskQty,
                BidPx = prices.BidPx,
                BidQty = prices.BidQty,
                Volume = prices.Volume,
                LastUpdatedDate = DateTime.Now,
                InstrumentID = instrumentID
            };

            _bgWorkerTask.ContinueWith(new Action<Task>((t)=>UpdateToLastPrice(price)), TaskContinuationOptions.PreferFairness);
        }

        /// <summary>
        /// used to update UI with lastest prices
        /// </summary>
        private void _uiUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _bgWorkerTask.ContinueWith(
                new Action<Task>((t)=> {
                    var snapshot = _lastestPrices.Values.ToList();
                    SyncToUI(snapshot);
                })
                );
        }

        private void SyncToUI(IEnumerable<Price> newPrices)
        {
            foreach(var newPrice in newPrices)
            {
                _dispatcher.BeginInvoke(new Action(() =>
                {
                    var index = PricesUI.IndexOf(newPrice);
                    var isExist = index > -1;
                    if (!isExist)
                        PricesUI.Add(newPrice);
                    else
                    {
                        PricesUI[index].AskPx = newPrice.AskPx;
                        PricesUI[index].AskQty = newPrice.AskQty;
                        PricesUI[index].BidPx = newPrice.BidPx;
                        PricesUI[index].BidQty = newPrice.BidQty;
                        PricesUI[index].Volume = newPrice.Volume;
                        PricesUI[index].LastUpdatedDate = newPrice.LastUpdatedDate;
                    }
                }));
            }
        }

        private void UpdateToLastPrice(Price p)
        {
            if (!_lastestPrices.ContainsKey(p.InstrumentID))
                _lastestPrices.TryAdd(p.InstrumentID, p);
            else if (_lastestPrices[p.InstrumentID].LastUpdatedDate < p.LastUpdatedDate)
            {
                _lastestPrices[p.InstrumentID] = p;
            }
        }

        #region config: Timer
        private void StartTimer()
        {
            _uiUpdateTimer.Interval = _RefreshRate;
            _uiUpdateTimer.Elapsed += _uiUpdateTimer_Elapsed;
            _uiUpdateTimer.Start();
        }

        private void StopTimer()
        {
            _uiUpdateTimer.Interval = int.MaxValue;
            _uiUpdateTimer.Elapsed -= _uiUpdateTimer_Elapsed;
            _uiUpdateTimer.Stop();
        }
        #endregion

        #region config: Update Service 
        private void StartUpdateService()
        {
            if (_priceUpdateService.IsStarted) return;
            _priceUpdateService.NewPricesArrived += _priceUpdateService_NewPricesArrived;
            _priceUpdateService.Start();
        }

        private void StopUpdateService()
        {
            if (!_priceUpdateService.IsStarted) return;
            _priceUpdateService.NewPricesArrived -= _priceUpdateService_NewPricesArrived;
            _priceUpdateService.Stop();
        }
        #endregion

        /// <summary>
        /// Start real time:
        /// 1. UI Timer, to render UI
        /// 2. Update Service, to receive RT feed
        /// </summary>
        private void Start()
        {
            if (_isInitialized) return;

            StartTimer();
            StartUpdateService();

            IsInitialized = true;
        }

        /// <summary>
        /// Stop real time:
        /// 1. UI Timer
        /// 2. Update Service
        /// </summary>
        private void Stop()
        {
            StopUpdateService();
            StopTimer();
            IsInitialized = false;
        }

        #region Property

        private double _RefreshRate = 1000; // 150ms (minimal)
        public double RefreshRate
        {
            get { return _RefreshRate; }
            set
            {
                if (value == _RefreshRate) return;
                _uiUpdateTimer.Interval = value;
                _RefreshRate = value;
                NotifyPropertyChanged("RefreshRate");
            }
        }

        public bool IsInitialized
        {
            get { return _isInitialized; }
            private set
            {
                if (value == _isInitialized) return;
                _isInitialized = value;
                NotifyPropertyChanged("IsInitialized");
            }
        }

        public IEnumerable<Price> LastestPrices
        {
            get { return _lastestPrices.Values.ToList(); ; }
        }

        private ObservableCollection<Price> _pricesUI;
        public ObservableCollection<Price> PricesUI
        {
            get { return _pricesUI; }
            private set
            {
                _pricesUI = value;
                NotifyPropertyChanged("PricesUI");
            }
        }

        #endregion

        #region Command

        public DelegatedCommand<object> StartCommand { get; private set; }
        private void StartCommand_Executed(object args)
        {
            Start();
        }
        private bool StartCommand_CanExecuted(object args)
        {
            return !_isInitialized;
        }

        public DelegatedCommand<object> StopCommand { get; private set; }
        private void StopCommand_Executed(object args)
        {
            Stop();
        }
        private bool StopCommand_CanExecuted(object args)
        {
            return _isInitialized;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Stop();
                    PricesUI.Clear();
                    _lastestPrices.Clear();
                    _uiUpdateTimer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PriceUpdateViewModel() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
