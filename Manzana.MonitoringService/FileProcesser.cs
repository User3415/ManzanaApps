using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Manzana.Domain.Entities;
using System.Text.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;

namespace Manzana.MonitoringService
{
    public class FileProcesser : BackgroundService
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly FileSystemWatcher _watcher;
        private readonly ServiceSettings _serviceSettings;
        private readonly string _completeFilePath;
        private readonly string _newFilePath;
        private readonly string _garbageFilePath;

        public FileProcesser(IOptions<ServiceSettings> serviceSettings)
        {
            _watcher = new FileSystemWatcher();
            _serviceSettings = serviceSettings.Value;
            _completeFilePath = Path.GetFullPath(_serviceSettings.CompleteFilePath, Environment.CurrentDirectory);
            _newFilePath = Path.GetFullPath(_serviceSettings.NewFilePath, Environment.CurrentDirectory);
            _garbageFilePath = Path.GetFullPath(_serviceSettings.GarbageFilePath, Environment.CurrentDirectory);

            Directory.CreateDirectory(_newFilePath);
            Directory.CreateDirectory(_garbageFilePath);
            Directory.CreateDirectory(_completeFilePath);
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Info($"File watcher service started\t {DateTime.Now:G}");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info($"File watcher service stopped\t {DateTime.Now:G}");
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                _watcher.Path = _newFilePath;
                _watcher.NotifyFilter = NotifyFilters.LastAccess |
                                NotifyFilters.LastWrite |
                                NotifyFilters.FileName |
                                NotifyFilters.DirectoryName;
                _watcher.Filter = "*.*";
                _watcher.Created += new FileSystemEventHandler(OnCreated);
                _watcher.EnableRaisingEvents = true;

            }, stoppingToken);

        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            _logger.Info($"Created new file\t {DateTime.Now:G}");

            Cheque cheque = null;

            if (File.Exists(e.FullPath))
            {
                if (Path.GetExtension(e.FullPath) == ".txt")
                {
                    try
                    {
                        cheque = JsonSerializer.Deserialize<Cheque>(File.ReadAllText(e.FullPath));
                        _logger.Info("File converted successfully");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        File.Copy(e.FullPath, _garbageFilePath + "\\" + Path.GetFileName(e.FullPath), true);
                    }
                    if (cheque != null)
                    {
                        SendToWcf(cheque, e.FullPath);
                    }
                }
                else
                {
                    _logger.Error($"Incorrect file type {Path.GetFileName(e.FullPath)}");
                    File.Copy(e.FullPath, _garbageFilePath + "\\" + Path.GetFileName(e.FullPath), true);
                }

                File.Delete(e.FullPath);
            }
            else
            {
                _logger.Error($"File by path {e.FullPath} not found");
            }
        }

        private void SendToWcf(Cheque cheque, string fileFullPath)
        {
            var serviceUrl = $"{_serviceSettings.WcfServiceUrl}/SaveCheque";
            using var client = new HttpClient();
            var chequeJson = JsonSerializer.Serialize(cheque);
            var buffer = Encoding.UTF8.GetBytes(chequeJson);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = client.PostAsync(serviceUrl, byteContent).Result;
                _logger.Info($"File sent successfully\t {DateTime.Now:G}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.Info($"The file was successfully processed by the WCF service\t {DateTime.Now:G}");
                    File.Copy(fileFullPath, _completeFilePath + "\\" + Path.GetFileName(fileFullPath), true);
                }
                else
                {
                    _logger.Info(response.Content.ReadAsStringAsync().Result);

                }   
            }
            catch(Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}
