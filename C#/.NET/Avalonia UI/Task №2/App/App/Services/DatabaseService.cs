using App.Models;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Microsoft.Data.Sqlite;

namespace App.Services
{
    public class DatabaseService : IDisposable
    {
        private readonly string _dbPath;
        private readonly string _connectionString;

        private Thread? _dbThread;
        private Dispatcher? _dispatcher;
        private SqliteConnection? _conn;
        private readonly AutoResetEvent _started = new(false);
        private volatile bool _running = true;

        public DatabaseService()
        {
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.db");
            EnsureDatabaseFileExists();
            _connectionString = $"Data Source={_dbPath};Mode=ReadWriteCreate;Cache=Shared;";

            StartDbThreadAndConnection();

            ExecuteOnDbThread(conn =>
            {
                string createSql = "CREATE TABLE IF NOT EXISTS ThreadData (ID INTEGER PRIMARY KEY AUTOINCREMENT, ThreadID INTEGER, Time TEXT, Data TEXT)";
                using var cmd = conn.CreateCommand();
                cmd.CommandText = createSql;
                cmd.ExecuteNonQuery();
                return true;
            });
        }

        private void EnsureDatabaseFileExists()
        {
            if (File.Exists(_dbPath))
                return;

            using var stream = Assembly.GetExecutingAssembly()
                                      .GetManifestResourceStream("App.Resources.data.db");
            if (stream != null)
            {
                using var fs = File.Create(_dbPath);
                stream.CopyTo(fs);
                return;
            }

            try
            {
                using var fs = File.Create(_dbPath);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException(
                    $"Не удалось создать файл базы данных: {_dbPath}. " +
                    "Добавьте 'data.sqlite' в проект (Build Action = Content) и в свойствах файла установите 'Copy to Output Directory' = 'Copy if newer', " +
                    "или встроьте шаблон как ресурс с именем 'App.Resources.data.sqlite'.", ex);
            }
        }

        private void StartDbThreadAndConnection()
        {
            _dbThread = new Thread(() =>
            {
                _dispatcher = Dispatcher.CurrentDispatcher;

                try
                {
                    _conn = new SqliteConnection(_connectionString);
                    _conn.Open();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"DB connection open failed on DB thread: {ex}");
                }
                finally
                {
                    _started.Set();
                }

                Dispatcher.Run();

                try
                {
                    _conn?.Close();
                    _conn?.Dispose();
                    _conn = null;
                }
                catch { }
            })
            {
                IsBackground = true
            };

            _dbThread.SetApartmentState(ApartmentState.STA);
            _dbThread.Start();

            if (!_started.WaitOne(TimeSpan.FromSeconds(5)))
            {
                System.Diagnostics.Debug.WriteLine("DB thread start timeout.");
            }
        }

        public bool Save(ThreadData data)
        {
            if (!_running) return false;
            try
            {
                if (_dispatcher == null)
                {
                    System.Diagnostics.Debug.WriteLine("DB Save: dispatcher is null");
                    return false;
                }

                _dispatcher.BeginInvoke(new Action(() =>
                {
                    if (_conn == null) return;
                    try
                    {
                        const string sql = "INSERT INTO ThreadData (ThreadID, Time, Data) VALUES (@p1, @p2, @p3)";
                        using var cmd = _conn.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@p1", data.ThreadId);
                        cmd.Parameters.AddWithValue("@p2", data.Time.ToString("o"));
                        cmd.Parameters.AddWithValue("@p3", data.Data ?? string.Empty);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"DB write item failed: {ex}");
                    }
                }), DispatcherPriority.Background);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DB enqueue failed: {ex}");
                return false;
            }
        }

        public List<ThreadData> LoadLatest(int limit = 20)
        {
            return ExecuteOnDbThread(conn =>
            {
                var result = new List<ThreadData>();
                string sql = $"SELECT ThreadID, Time, Data FROM ThreadData ORDER BY Time DESC LIMIT {limit}";
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int threadId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    string timeStr = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    DateTime time = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(timeStr))
                    {
                        DateTime.TryParse(timeStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out time);
                    }
                    string data = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);

                    result.Add(new ThreadData
                    {
                        ThreadId = threadId,
                        Time = time,
                        Data = data
                    });
                }
                return result;
            });
        }

        private T ExecuteOnDbThread<T>(Func<SqliteConnection, T> func)
        {
            if (_dispatcher == null)
                throw new InvalidOperationException("DB dispatcher not initialized.");

            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

            _dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (_conn == null)
                        throw new InvalidOperationException("DB connection is not available.");

                    var res = func(_conn);
                    tcs.SetResult(res);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }));

            return tcs.Task.GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _running = false;

            try
            {
                if (_dispatcher != null)
                {
                    _dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                }

                if (_dbThread != null && !_dbThread.Join(TimeSpan.FromSeconds(5)))
                {
                    System.Diagnostics.Debug.WriteLine("DB thread did not stop within timeout.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DB dispose error: {ex}");
            }
            finally
            {
                _dispatcher = null;
            }
        }
    }
}
