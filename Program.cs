using System;
using System.Diagnostics;
using System.Management;
using SentinelSyncV1;
using System.Threading.Tasks;
using System.Threading.Channels;
using Microsoft.Win32;
using System.Management;
using System.Runtime.InteropServices;
using System.Management;

namespace SentinelSyncV1
{
    class Program
    {
        static string RefreshCpuInfos()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            dynamic firstVal = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(100);
            dynamic val = cpuCounter.NextValue();

            decimal roundVal = Convert.ToDecimal(val);
            roundVal = Math.Round(roundVal, 2);

            return roundVal + " %";
        }

        static void RefreshRamInfos()
        {
            Console.WriteLine("available : " + SentinelSyncV1.SystemInfo.FormatSize(SentinelSyncV1.SystemInfo.GetAvailPhys()));
            Console.WriteLine("used : " + SentinelSyncV1.SystemInfo.FormatSize(SentinelSyncV1.SystemInfo.GetUsedPhys()));
            Console.WriteLine("total : " + SentinelSyncV1.SystemInfo.FormatSize(SentinelSyncV1.SystemInfo.GetTotalPhys()));
        }
        /*
        static string getRamCounter()
        {
            PerformanceCounter ramCounter = new PerformanceCounter();
            ramCounter.CategoryName = "Memory";
            ramCounter.CounterName = "Available MBytes";

            dynamic firstValue = ramCounter.NextValue();
            System.Threading.Thread.Sleep(100);
            dynamic val = ramCounter.NextValue();

            decimal roundVar = Convert.ToDecimal(val);
            roundVar = Math.Round(roundVar, 2);

            return roundVar.ToString();
        }
        */

        static void RefreshTempInfos()
        {
            try
            {
                double temperature = 0;
                string instanceName = "";

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\cimv2", "SELECT * FROM MSAcpi_ThermalZoneTemperature");

                foreach (ManagementObject obj in searcher.Get())
                {
                    temperature = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                    temperature = (temperature - 2732) / 10.0;
                    instanceName = obj["InstanceName"].ToString();
                }

                Console.WriteLine(temperature + "°C");
            }
            catch (ManagementException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }



        static void GetAllSystemInfos()
        {
            var si = new SystemInfo();
            Console.WriteLine(si.GetCpuInfos());
            Console.WriteLine(si.GetOsInfos("os"));
            Console.WriteLine(si.GetOsInfos("arch"));
            Console.WriteLine(si.GetOsInfos("osv"));
            si.GetGpuInfos();
        }
        static void ShowCpuUse()
        {
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    return;
                }
                Console.WriteLine("(ENTER to stop) processor utilisation : " + RefreshCpuInfos());
            }
        }

        static void ShowRamUse()
        {
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    return;
                }
                Console.WriteLine("(ENTER to stop) RAM utilisation : ");
                RefreshRamInfos();
                System.Threading.Thread.Sleep(500);
            }
        }
        static void ShowTemp()
        {
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    return;
                }
                Console.WriteLine("(ENTER to stop) Temperature : ");
                RefreshTempInfos();
                System.Threading.Thread.Sleep(500);
            }
        }

        static void ShowNet()
        {
            var sysInfo = new SystemInfo();
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    return;
                }
                Console.Write("(ENTER to stop) ");
                sysInfo.RefreshNetworkInfos();
                System.Threading.Thread.Sleep(500);
            }
        }

        static void Main(string[] args)
        {
            GetAllSystemInfos();
            int choice = 0;
            var sysInfo = new SystemInfo();

            Console.WriteLine("1 - show processor utilisation");
            Console.WriteLine("2 - show RAM");
            Console.WriteLine("3 - show PC temperature");
            Console.WriteLine("4 - show stockage infos");
            Console.WriteLine("5 - network infos");

            choice = outils.AskNumberBetween("enter a choice : ", 1, 5);
            switch (choice)
            {
                case 1:
                    ShowCpuUse();
                    break;
                case 2:
                    ShowRamUse();
                    break;
                case 3:
                    ShowTemp();
                    break;
                case 4:
                    sysInfo.GetDrivesInfos();
                    break;
                case 5:
                    ShowNet();
                    break;

            }
        }
    }
}
