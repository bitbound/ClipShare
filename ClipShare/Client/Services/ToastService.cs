using ClipShare.Client.Components;
using ClipShare.Client.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace ClipShare.Client.Services
{
    public interface IToastService
    {
        List<Toast> Toasts { get; }

        event EventHandler OnToastsChanged;

        void ShowToast(string message, TimeSpan expiration, string classString = null);
    }

    public class ToastService : IToastService
    {
        public event EventHandler OnToastsChanged;
        public List<Toast> Toasts => ToastCache.Values.ToList();
        private ConcurrentDictionary<string, Toast> ToastCache { get; } = new ConcurrentDictionary<string, Toast>();


        public void ShowToast(string message,
            TimeSpan expiration,
            string classString = null)
        {

            if (string.IsNullOrWhiteSpace(classString))
            {
                classString = "bg-info text-white";
            };

            var toastModel = new Toast(Guid.NewGuid().ToString(), message, classString, expiration);
            ToastCache.AddOrUpdate(toastModel.Guid, toastModel, (k, v) => toastModel);
            OnToastsChanged?.Invoke(this, null);
            var timer = new Timer(expiration.Add(TimeSpan.FromSeconds(1)).TotalMilliseconds);
            timer.AutoReset = false;
            timer.Elapsed += (s, e) =>
            {
                ToastCache.Remove(toastModel.Guid, out _);
                OnToastsChanged?.Invoke(this, null);
            };
            timer.Start();
        }
    }
}
