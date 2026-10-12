using App.Models;
using App.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace App.ViewModels
{
    public class MainViewModel : IDisposable
    {
        private readonly ThreadGeneratorService _generator = new();
        private readonly DatabaseService _db = new();

        private CancellationTokenSource? _cts;
        private readonly object _lock = new();

        public ObservableCollection<ThreadData> Items { get; } = new();

        public int ThreadCount { get; set; } = 2;

        private RelayCommand? _startCommand;
        private RelayCommand? _stopCommand;

        public ICommand StartCommand => _startCommand ??= new RelayCommand(Start, () => _cts == null);
        public ICommand StopCommand => _stopCommand ??= new RelayCommand(Stop, () => _cts != null);

        public MainViewModel()
        {
            try
            {
                var initial = _db.LoadLatest(20);
                for (int i = 0; i < initial.Count; i++)
                {
                    Items.Add(initial[i]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DB load failed on startup: {ex}");
            }
        }

        private void Start()
        {
            if (ThreadCount < 2 || ThreadCount > 15)
                return;

            if (_cts != null)
                return;

            _cts = new CancellationTokenSource();

            for (int i = 1; i <= ThreadCount; i++)
            {
                int id = i;
                Task.Run(() => _generator.StartAsync(id, OnGenerated, _cts.Token));
            }

            _startCommand?.RaiseCanExecuteChanged();
            _stopCommand?.RaiseCanExecuteChanged();
        }

        private void Stop()
        {
            try
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;

            }
            finally
            {
                _startCommand?.RaiseCanExecuteChanged();
                _stopCommand?.RaiseCanExecuteChanged();
            }
        }

        private void OnGenerated(ThreadData? data)
        {
            try
            {
                if (data is null)
                {
                    Debug.WriteLine("OnGenerated: data is null");
                    return;
                }

                bool queued;
                try
                {
                    queued = _db.Save(data);
                }
                catch (Exception exSave)
                {
                    Debug.WriteLine($"OnGenerated: Save threw: {exSave}");
                    queued = false;
                }

                if (!queued)
                {
                    Debug.WriteLine("OnGenerated: Save returned false (queue closed or error).");
                }

                Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    lock (_lock)
                    {
                        Items.Insert(0, data);

                        while (Items.Count > 20)
                        {
                            Items.RemoveAt(Items.Count - 1);
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"OnGenerated: unexpected error: {ex}");
            }
        }

        public void Dispose()
        {
            try
            {
                Stop();
            }
            catch { }

            try
            {
                _db.Dispose();
            }
            catch { }
        }
    }
}
