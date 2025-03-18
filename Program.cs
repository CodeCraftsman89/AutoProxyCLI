using Microsoft.Win32;
using System;
using System.Diagnostics;

class ProxySettings
{
    static void Main()
    {
        string msiName = "cert_install_v2.msi"; // Укажи имя исполняемого файла
        RunMsiInstaller(msiName);
        SetProxy("10.0.50.52", 3128, 1); // Включаем прокси с указанным адресом и портом
        // SetProxy("", 0, 0); // Отключить прокси
    }

    public static void SetProxy(string proxyAddress, int port, int proxyEnable)
    {
        const string registryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
        try
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey, true))
            {
                if (key != null)
                {
                    key.SetValue("ProxyEnable", proxyEnable);
                    key.SetValue("ProxyServer", $"{proxyAddress}:{port}");
                }
            }
            Console.WriteLine("Proxy settings updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating proxy settings: " + ex.Message);
        }
    }
    static void RunMsiInstaller(string msiName)
    {
        string msiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, msiName);

        if (File.Exists(msiPath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "msiexec.exe",
                Arguments = $"/i \"{msiPath}\" /qb", // /qn - тихая установка без окон
                UseShellExecute = false
            });

            Console.WriteLine($"Запущена установка: {msiPath}");
        }
        else
        {
            Console.WriteLine($"Файл не найден: {msiPath}");
        }
    }


}
