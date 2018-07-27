using EasyCodeGeneratorApp.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EasyCodeGeneratorApp
{
    public partial class MainWindow : Window
    {
        List<DbTable> _tables;
        DbTable _currentTable;
        Encoding _writerEncoding = Encoding.UTF8;

        string lastProvider;
        string lastConnection;

        const string configName = ".\\Generator.json";
        const string templatePath = ".\\Template";
        const string sPath = ".\\Generator";
        const string sPathModel = sPath + "\\Model\\";
        const string sPathData = sPath + "\\Data\\";
        const string sPathService = sPath + "\\Service\\";

        public MainWindow()
        {
            InitializeComponent();
        }

        string GetSetSelectedItem()
        {
            _currentTable = null;
            var obj = this.tableTreeView.SelectedItem;
            if (obj == null)
                return string.Empty;
            if (obj.GetType().Name != typeof(DbTable).Name)
                return string.Empty;
            var dt = obj as DbTable;
            if (dt == null)
                return string.Empty;
            var gen = new Generator(dt);
            var txt = gen.GenModel();

            this.txtModel.Document.Blocks.Clear();
            this.txtModel.AppendText(txt);

            _currentTable = dt;
            return txt;
        }

        void deleteFilesAndCreateDir(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string content = string.Empty;
                if (File.Exists(configName))
                    content = File.ReadAllText(configName, Encoding.UTF8);
                var configs = new List<GeneratorConfig>();
                if (!string.IsNullOrWhiteSpace(content))
                    configs = JsonConvert.DeserializeObject<List<GeneratorConfig>>(content);
                if (configs == null || configs.Count <= 0)
                    return;
                var first = configs.OrderByDescending(c => c.CreateTime).First();

                var dicProvider = new Dictionary<string, int>();
                dicProvider.Add("mysql", 0);
                dicProvider.Add("sql server", 1);
                dicProvider.Add("oracle", 2);

                this.lastProvider = first.Provider;
                this.lastConnection = first.ConnectionString;

                if (dicProvider.ContainsKey(this.lastProvider))
                    this.cbDbType.SelectedIndex = dicProvider[this.lastProvider];
                else
                    this.cbDbType.SelectedIndex = 0;
                this.txtConnString.Text = this.lastConnection;

                //this.txtConnString1.ItemsSource = configs;
            }
            catch (Exception ex)
            {
                this.lableStatus.Content = ex.Message;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.lastProvider) || string.IsNullOrWhiteSpace(this.lastConnection))
                    return;
                string content = string.Empty;
                if (File.Exists(configName))
                    content = File.ReadAllText(configName, Encoding.UTF8);
                var configs = new List<GeneratorConfig>();
                if (!string.IsNullOrWhiteSpace(content))
                    configs = JsonConvert.DeserializeObject<List<GeneratorConfig>>(content);
                if (configs == null)
                    configs = new List<GeneratorConfig>();
                var cfg = new GeneratorConfig()
                {
                    Provider = lastProvider,
                    ConnectionString = lastConnection,
                    CreateTime = DateTime.Now
                };
                var first = configs.FirstOrDefault(c => c.Provider == cfg.Provider && c.ConnectionString == cfg.ConnectionString);
                if (first == null)
                    configs.Add(cfg);
                else
                    first.CreateTime = cfg.CreateTime;
                var writeTxt = JsonConvert.SerializeObject(configs.OrderByDescending(c => c.CreateTime));
                File.WriteAllText(configName, writeTxt, Encoding.UTF8);

                GC.Collect();
            }
            catch (Exception ex)
            {
                this.lableStatus.Content = ex.Message;
            }
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            var provider = this.cbDbType.SelectionBoxItem;
            if (provider == null || string.IsNullOrEmpty(provider.ToString()))
                return;
            lastProvider = provider.ToString();
            lastConnection = this.txtConnString.Text.Trim();

            this.btnJoin.IsEnabled = false;
            Task.Run(() =>
            {
                try
                {
                    _tables = GenTables.GetTables(this.lastProvider, this.lastConnection);
                }
                catch (Exception ex)
                {
                    this.lableStatus.Dispatcher.Invoke(() =>
                    {
                        this.lableStatus.Content = ex.Message;
                    });
                }
                finally
                {
                    this.btnJoin.Dispatcher.Invoke(() =>
                    {
                        this.btnJoin.IsEnabled = true;
                    });
                    if (_tables != null)
                    {
                        this.tableTreeView.Dispatcher.Invoke(() =>
                        {
                            this.tableTreeView.ItemsSource = _tables;
                        });
                    }
                }
            });
        }

        private void btnGenCode_Click(object sender, RoutedEventArgs e)
        {
            this.GetSetSelectedItem();
        }

        private void btnSingleGen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var txt = this.GetSetSelectedItem();
                var dt = _currentTable;
                if (dt == null || string.IsNullOrWhiteSpace(txt))
                    return;

                deleteFilesAndCreateDir(sPathModel);

                using (TextWriter tw = new StreamWriter(new BufferedStream(new FileStream(sPathModel + dt.TableName + ".cs", FileMode.Create, FileAccess.Write)), _writerEncoding))
                {
                    tw.Write(txt);
                }

                System.Diagnostics.Process.Start(sPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAllGen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_tables == null || _tables.Count <= 0)
                    return;
                deleteFilesAndCreateDir(sPathModel);
                foreach (var dt in _tables)
                {
                    var gen = new Generator(dt);
                    var txt = gen.GenModel();
                    using (TextWriter tw = new StreamWriter(new BufferedStream(new FileStream(sPathModel + dt.TableName + ".cs", FileMode.Create, FileAccess.Write)), _writerEncoding))
                    {
                        tw.Write(txt);
                    }
                }

                System.Diagnostics.Process.Start(sPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadTemplate_Click(object sender, RoutedEventArgs e)
        {
            string content = string.Empty;
            if (File.Exists(Path.Combine(templatePath, "Model.txt")))
                content = File.ReadAllText(Path.Combine(templatePath, "Model.txt"), Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(content))
                return;
            this.txtModel.Document.Blocks.Clear();
            this.txtModel.AppendText(content);
            this.lableStatus.Content = "load template success";
        }

        private void btnSaveTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtModel.Selection.Text))
            {
                File.WriteAllText(Path.Combine(templatePath, "Model.txt"), this.txtModel.Selection.Text, Encoding.UTF8);
                this.lableStatus.Content = "save template success";
            }
        }
    }
}
