using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Net;

namespace GameLoopOptimizer
{
    public partial class MainForm : Form
    {
        private string backupPath = @"C:\Backup\RegistryBackup.reg";
        private string servicesBackupPath = @"C:\Backup\ServicesBackup.reg";
        private string systemBackupPath = @"C:\Backup\SystemBackup.reg";
        private string gameBackupPath = @"C:\Backup\GameBackup.reg";
        private string privacyBackupPath = @"C:\Backup\PrivacyBackup.reg";
        private string networkBackupPath = @"C:\Backup\NetworkBackup.reg";

        // GitHub URL'leri (kendi reponu oluşturduktan sonra güncelleyeceksin)
        private string privacyRegUrl = "https://raw.githubusercontent.com/[kullanıcı_adınız]/GameLoopOptimizer/main/PrivacyOptimization.reg";
        private string systemRegUrl = "https://raw.githubusercontent.com/[kullanıcı_adınız]/GameLoopOptimizer/main/SystemOptimization.reg";
        private string servicesRegUrl = "https://raw.githubusercontent.com/[kullanıcı_adınız]/GameLoopOptimizer/main/ServicesOptimization.reg";
        private string gameRegUrl = "https://raw.githubusercontent.com/[kullanıcı_adınız]/GameLoopOptimizer/main/GameOptimization.reg";
        private string networkRegUrl = "https://raw.githubusercontent.com/[kullanıcı_adınız]/GameLoopOptimizer/main/NetworkOptimization.reg";

        private string privacyRegPath = Path.Combine(Path.GetTempPath(), "PrivacyOptimization.reg");
        private string systemRegPath = Path.Combine(Path.GetTempPath(), "SystemOptimization.reg");
        private string servicesRegPath = Path.Combine(Path.GetTempPath(), "ServicesOptimization.reg");
        private string gameRegPath = Path.Combine(Path.GetTempPath(), "GameOptimization.reg");
        private string networkRegPath = Path.Combine(Path.GetTempPath(), "NetworkOptimization.reg");

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "GameLoop Optimizer";
            this.Size = new System.Drawing.Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            var optimizeButton = new Button
            {
                Text = "Optimize Et",
                Location = new System.Drawing.Point(50, 50),
                Size = new System.Drawing.Size(100, 30)
            };
            optimizeButton.Click += OptimizeButton_Click;

            var revertButton = new Button
            {
                Text = "Geri Dön",
                Location = new System.Drawing.Point(200, 50),
                Size = new System.Drawing.Size(100, 30)
            };
            revertButton.Click += RevertButton_Click;

            var statusLabel = new Label
            {
                Text = "Durum: Hazır",
                Location = new System.Drawing.Point(50, 100),
                Size = new System.Drawing.Size(300, 30)
            };
            this.Controls.Add(statusLabel);
            this.Controls.Add(optimizeButton);
            this.Controls.Add(revertButton);

            this.statusLabel = statusLabel;
        }

        private Label statusLabel;

        private void OptimizeButton_Click(object sender, EventArgs e)
        {
            try
            {
                statusLabel.Text = "Durum: Optimizasyon yapılıyor...";

                // Yedekleme klasörünü oluştur
                Directory.CreateDirectory(@"C:\Backup");

                // Kayıt defteri yedeklerini al
                BackupRegistry("HKLM", backupPath);
                BackupRegistry(@"HKLM\SYSTEM\CurrentControlSet\Services", servicesBackupPath);
                BackupRegistry(@"HKLM\SYSTEM\CurrentControlSet\Control", systemBackupPath);
                BackupRegistry(@"HKLM\SOFTWARE\WOW6432Node\Tencent", gameBackupPath);
                BackupRegistry(@"HKLM\SOFTWARE\Microsoft\Windows", privacyBackupPath);
                BackupRegistry(@"HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", networkBackupPath);

                // .reg dosyalarını GitHub'dan indir
                DownloadFile(privacyRegUrl, privacyRegPath);
                DownloadFile(systemRegUrl, systemRegPath);
                DownloadFile(servicesRegUrl, servicesRegPath);
                DownloadFile(gameRegUrl, gameRegPath);
                DownloadFile(networkRegUrl, networkRegPath);

                // .reg dosyalarını uygula
                ApplyRegedit(privacyRegPath);
                ApplyRegedit(systemRegPath);
                ApplyRegedit(servicesRegPath);
                ApplyRegedit(gameRegPath);
                ApplyRegedit(networkRegPath);

                // CMD ile optimizasyonlar
                RunCMD("net stop wuauserv");
                RunCMD("net stop bits");
                RunCMD("net stop diagtrack");
                RunCMD("ipconfig /flushdns");
                RunCMD("netsh winsock reset");
                RunCMD("powercfg -setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
                RunCMD("bcdedit /set disabledynamictick yes");
                RunCMD("wmic process where name=\"AndroidEmulator.exe\" CALL setpriority \"high\"");

                // Zamanlanmış Görevleri Kapat
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\ProgramDataUpdater\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Autochk\\Proxy\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\DiskDiagnostic\\Microsoft-Windows-DiskDiagnosticDataCollector\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Feedback\\Siuf\\DmClient\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Feedback\\Siuf\\DmClientOnScenarioDownload\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Windows Error Reporting\\QueueReporting\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\MareBackup\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\StartupAppTask\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\PcaPatchDbTask\" /Disable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Maps\\MapsUpdateTask\" /Disable");

                statusLabel.Text = "Durum: Optimizasyon tamamlandı!";
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Durum: Hata: {ex.Message}";
            }
        }

        private void RevertButton_Click(object sender, EventArgs e)
        {
            try
            {
                statusLabel.Text = "Durum: Geri dönülüyor...";

                // Yedekleri geri yükle
                if (File.Exists(backupPath))
                {
                    Process.Start("reg", $"import \"{backupPath}\"").WaitForExit();
                }
                if (File.Exists(servicesBackupPath))
                {
                    Process.Start("reg", $"import \"{servicesBackupPath}\"").WaitForExit();
                }
                if (File.Exists(systemBackupPath))
                {
                    Process.Start("reg", $"import \"{systemBackupPath}\"").WaitForExit();
                }
                if (File.Exists(gameBackupPath))
                {
                    Process.Start("reg", $"import \"{gameBackupPath}\"").WaitForExit();
                }
                if (File.Exists(privacyBackupPath))
                {
                    Process.Start("reg", $"import \"{privacyBackupPath}\"").WaitForExit();
                }
                if (File.Exists(networkBackupPath))
                {
                    Process.Start("reg", $"import \"{networkBackupPath}\"").WaitForExit();
                }

                // Hizmetleri yeniden başlat
                RunCMD("net start wuauserv");
                RunCMD("net start bits");
                RunCMD("net start diagtrack");

                // Zamanlanmış Görevleri Geri Aç
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\ProgramDataUpdater\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Autochk\\Proxy\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\DiskDiagnostic\\Microsoft-Windows-DiskDiagnosticDataCollector\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Feedback\\Siuf\\DmClient\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Feedback\\Siuf\\DmClientOnScenarioDownload\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Windows Error Reporting\\QueueReporting\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\MareBackup\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\StartupAppTask\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Application Experience\\PcaPatchDbTask\" /Enable");
                RunCMD("schtasks /Change /TN \"Microsoft\\Windows\\Maps\\MapsUpdateTask\" /Enable");

                statusLabel.Text = "Durum: Geri dönüş tamamlandı!";
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Durum: Hata: {ex.Message}";
            }
        }

        private void RunCMD(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", $"/c {command}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            Process process = Process.Start(processInfo);
            process.WaitForExit();
        }

        private void BackupRegistry(string key, string backupFile)
        {
            if (File.Exists(backupFile))
            {
                File.Delete(backupFile);
            }
            Process.Start("reg", $"export \"{key}\" \"{backupFile}\"").WaitForExit();
        }

        private void DownloadFile(string url, string path)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
        }

        private void ApplyRegedit(string regFilePath)
        {
            if (File.Exists(regFilePath))
            {
                Process.Start("reg", $"import \"{regFilePath}\"").WaitForExit();
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}