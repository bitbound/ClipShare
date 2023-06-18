using ClipShare.Client.Components;
using ClipShare.Client.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace ClipShare.Client.Services;

public interface IToastService
{
    event EventHandler OnToastsChanged;

    List<Toast> Toasts { get; }
    void ShowToast(
        string message, 
        TimeSpan expiration,
        string classString = "", 
        string styleOverrides = "");

    void ShowToast(
       string message,
       string classString = "",
       string styleOverrides = "");
}

public class ToastService : IToastService
{
    private readonly ConcurrentDictionary<string, Toast> _toastCache = new();

    public event EventHandler? OnToastsChanged;

    public List<Toast> Toasts => _toastCache.Values.ToList();

    private Timer? _clearToastsTimer;

    public void ShowToast(string message,
        TimeSpan expiration,
        string classString = "",
        string styleOverrides = "")
    {

        if (string.IsNullOrWhiteSpace(classString))
        {
            classString = "bg-success text-white";
        };

        var toastModel = new Toast(Guid.NewGuid().ToString(), message, classString, expiration, styleOverrides);
        _toastCache.AddOrUpdate(toastModel.Guid, toastModel, (k, v) => toastModel);
        OnToastsChanged?.Invoke(this, EventArgs.Empty);

        _clearToastsTimer?.Dispose();
        _clearToastsTimer = new Timer(_toastCache.Values.Max(x => x.Expiration.TotalMilliseconds) + 5000)
        {
            AutoReset = false
        };
        _clearToastsTimer.Elapsed += (s, e) =>
        {
            _toastCache.Clear();
            OnToastsChanged?.Invoke(this, EventArgs.Empty);
        };
        _clearToastsTimer.Start();
    }

    public void ShowToast(string message, string classString = "", string styleOverrides = "")
    {
        ShowToast(message, TimeSpan.FromSeconds(3), classString, styleOverrides);
    }
}
