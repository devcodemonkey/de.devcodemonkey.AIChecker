using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GpuOverNVML;

public class NvmlWrapper : IDisposable
{
    private const string NvmlLibrary = "nvml.dll";

    // Define nvmlReturn_t as enum for error codes
    private enum nvmlReturn_t
    {
        NVML_SUCCESS = 0,
        NVML_ERROR_UNINITIALIZED = 1,
        NVML_ERROR_INVALID_ARGUMENT = 2,
        NVML_ERROR_NOT_SUPPORTED = 3,
        NVML_ERROR_NO_PERMISSION = 4,
        NVML_ERROR_ALREADY_INITIALIZED = 5,
        NVML_ERROR_NOT_FOUND = 6,
        NVML_ERROR_INSUFFICIENT_SIZE = 7,
        NVML_ERROR_INSUFFICIENT_POWER = 8,
        NVML_ERROR_DRIVER_NOT_LOADED = 9,
        NVML_ERROR_TIMEOUT = 10,
        NVML_ERROR_UNKNOWN = 999
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct nvmlUtilization_t
    {
        public uint gpu;   // GPU utilization in percent
        public uint memory; // Memory utilization in percent
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct nvmlMemory_t
    {
        public ulong total;  // Total memory in bytes
        public ulong free;   // Free memory in bytes
        public ulong used;   // Used memory in bytes
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct nvmlProcessInfo_t
    {
        public uint pid;      // Process ID
        public ulong usedGpuMemory; // Memory used by this process in bytes
    }

    // NVML Function Imports
    [DllImport(NvmlLibrary, CallingConvention = CallingConvention.Cdecl)]
    private static extern nvmlReturn_t nvmlInit();

    [DllImport(NvmlLibrary, CallingConvention = CallingConvention.Cdecl)]
    private static extern nvmlReturn_t nvmlShutdown();

    [DllImport(NvmlLibrary, CallingConvention = CallingConvention.Cdecl)]
    private static extern nvmlReturn_t nvmlDeviceGetHandleByIndex(uint index, out IntPtr device);

    [DllImport(NvmlLibrary, CallingConvention = CallingConvention.Cdecl)]
    private static extern nvmlReturn_t nvmlDeviceGetUtilizationRates(IntPtr device, out nvmlUtilization_t utilization);

    [DllImport(NvmlLibrary, CallingConvention = CallingConvention.Cdecl)]
    private static extern nvmlReturn_t nvmlDeviceGetMemoryInfo(IntPtr device, out nvmlMemory_t memoryInfo);

    [DllImport(NvmlLibrary, CallingConvention = CallingConvention.Cdecl)]
    private static extern nvmlReturn_t nvmlDeviceGetGraphicsRunningProcesses_v2(
        IntPtr device, ref uint infoCount, [Out] nvmlProcessInfo_t[] infos);

    [DllImport(NvmlLibrary, CallingConvention = CallingConvention.Cdecl)]
    private static extern nvmlReturn_t nvmlDeviceGetComputeRunningProcesses_v2(
        IntPtr device, ref uint infoCount, [Out] nvmlProcessInfo_t[] infos);

    // Constructor
    public NvmlWrapper()
    {
        var result = nvmlInit();
        if (result != nvmlReturn_t.NVML_SUCCESS)
        {
            throw new Exception($"Failed to initialize NVML. Error: {result}");
        }
    }

    // Get GPU Utilization
    public uint GetGpuUsage(uint gpuIndex = 0)
    {
        IntPtr deviceHandle;
        var result = nvmlDeviceGetHandleByIndex(gpuIndex, out deviceHandle);

        if (result != nvmlReturn_t.NVML_SUCCESS)
        {
            throw new Exception($"Failed to get handle for GPU {gpuIndex}. Error: {result}");
        }

        nvmlUtilization_t utilization;
        result = nvmlDeviceGetUtilizationRates(deviceHandle, out utilization);

        return utilization.gpu;
    }

    public uint GetMemoryUsage(uint gpuIndex = 0)
    {
        IntPtr deviceHandle;
        var result = nvmlDeviceGetHandleByIndex(gpuIndex, out deviceHandle);

        if (result != nvmlReturn_t.NVML_SUCCESS)
        {
            throw new Exception($"Failed to get utilization rates. Error: {result}");
        }

        nvmlUtilization_t utilization;
        result = nvmlDeviceGetUtilizationRates(deviceHandle, out utilization);

        return utilization.memory;
    }

    // Get GPU Memory Information
    public (double Total, double Free, double Used) GetGpuMemoryInfo(uint gpuIndex = 0)
    {
        IntPtr deviceHandle;
        var result = nvmlDeviceGetHandleByIndex(gpuIndex, out deviceHandle);
        if (result != nvmlReturn_t.NVML_SUCCESS)
        {
            throw new Exception($"Failed to get handle for GPU {gpuIndex}. Error: {result}");
        }

        nvmlMemory_t memoryInfo;
        result = nvmlDeviceGetMemoryInfo(deviceHandle, out memoryInfo);
        if (result != nvmlReturn_t.NVML_SUCCESS)
        {
            throw new Exception($"Failed to get memory info. Error: {result}");
        }

        // Convert bytes to gigabytes
        double totalGB = memoryInfo.total / 1024.0 / 1024.0 / 1024.0;
        double freeGB = memoryInfo.free / 1024.0 / 1024.0 / 1024.0;
        double usedGB = memoryInfo.used / 1024.0 / 1024.0 / 1024.0;

        return (totalGB, freeGB, usedGB);
    }

    // Get GPU Usage Per Process
    public List<(uint ProcessId, string ProcessName, double UsedGpuMemoryGB)> GetGpuUsagePerProcess(uint gpuIndex = 0)
    {
        IntPtr deviceHandle;
        var result = nvmlDeviceGetHandleByIndex(gpuIndex, out deviceHandle);
        if (result != nvmlReturn_t.NVML_SUCCESS)
        {
            throw new Exception($"Failed to get handle for GPU {gpuIndex}. Error: {result}");
        }

        uint infoCount = 32; // Maximum number of processes to query
        var processInfos = new nvmlProcessInfo_t[infoCount];

        // Graphics Processes
        result = nvmlDeviceGetGraphicsRunningProcesses_v2(deviceHandle, ref infoCount, processInfos);
        if (result != nvmlReturn_t.NVML_SUCCESS && result != nvmlReturn_t.NVML_ERROR_INSUFFICIENT_SIZE)
        {
            throw new Exception($"Failed to get graphics running processes. Error: {result}");
        }

        var processUsage = new List<(uint ProcessId, string ProcessName, double UsedGpuMemoryGB)>();

        // Add graphics process information
        for (int i = 0; i < infoCount; i++)
        {
            var pid = processInfos[i].pid;
            string processName = GetProcessNameById(pid);
            double usedGpuMemoryGB = processInfos[i].usedGpuMemory / 1024.0 / 1024.0 / 1024.0;
            processUsage.Add((pid, processName, usedGpuMemoryGB));
        }

        // Compute Processes
        infoCount = 32; // Reset info count
        result = nvmlDeviceGetComputeRunningProcesses_v2(deviceHandle, ref infoCount, processInfos);
        if (result != nvmlReturn_t.NVML_SUCCESS && result != nvmlReturn_t.NVML_ERROR_INSUFFICIENT_SIZE)
        {
            throw new Exception($"Failed to get compute running processes. Error: {result}");
        }

        // Add compute process information
        for (int i = 0; i < infoCount; i++)
        {
            var pid = processInfos[i].pid;
            string processName = GetProcessNameById(pid);
            double usedGpuMemoryGB = processInfos[i].usedGpuMemory / 1024.0 / 1024.0 / 1024.0;
            processUsage.Add((pid, processName, usedGpuMemoryGB));
        }

        return processUsage;
    }

    private string GetProcessNameById(uint pid)
    {
        try
        {
            var process = Process.GetProcessById((int)pid);
            return process.ProcessName;
        }
        catch (Exception ex)
        {
            // Handle the case where the process has already terminated
            return $"Unknown (PID: {pid}, Error: {ex.Message})";
        }
    }


    // Dispose for cleanup
    public void Dispose()
    {
        var result = nvmlShutdown();
        if (result != nvmlReturn_t.NVML_SUCCESS)
        {
            Console.WriteLine($"Failed to shutdown NVML. Error: {result}");
        }
    }
}
