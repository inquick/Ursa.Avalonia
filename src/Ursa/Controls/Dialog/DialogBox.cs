using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Ursa.Controls;

public static class DialogBox
{
    public static async Task<TResult?> ShowModalAsync<TView, TViewModel, TResult>(TViewModel vm) 
        where TView : Control, new()
    {
        var window = new DialogWindow()
        {
            Content = new TView(),
            DataContext = vm,
        };
        var lifetime = Application.Current?.ApplicationLifetime;
        if (lifetime is IClassicDesktopStyleApplicationLifetime classLifetime)
        {
            var main = classLifetime.MainWindow;
            if (main is null)
            {
                window.Show();
                return default(TResult);
            }
            else
            {
                var result = await window.ShowDialog<TResult>(main);
                return result;
            }
        }
        else
        {
            return default(TResult);
        }
    }
    
    public static async Task<TResult> ShowModalAsync<TView, TViewModel, TResult>(Window owner, TViewModel vm) where 
        TView: Control, new()
    {
        var window = new DialogWindow
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm
        };
        return await window.ShowDialog<TResult>(owner);
    }
    

    public static Task<TResult> ShowOverlayModalAsync<TView, TViewModel, TResult>(TViewModel vm, string hostId)
        where TView : Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView(){ DataContext = vm },
            DataContext = vm,
        };
        var host = OverlayDialogManager.GetOverlayDialogHost(hostId);
        host?.AddModalDialog(t);
        return t.ShowAsync<TResult>();
    }

    public static Task<TResult> ShowOverlayModalAsync<TView, TViewModel, TResult>(TViewModel vm, string hostId,
        DialogOptions options)
        where TView : Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
            ExtendToClientArea = options.ExtendToClientArea,
            Title = options.Title,
        };
        var host = OverlayDialogManager.GetOverlayDialogHost(hostId);
        host?.AddModalDialog(t);
        return t.ShowAsync<TResult>();
    }

    public static void ShowOverlay<TView, TViewModel>(TViewModel vm, string hostId)
        where TView: Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
        };
        var host = OverlayDialogManager.GetOverlayDialogHost(hostId);
        host?.AddDialog(t);
    }

    public static void ShowOverlay<TView, TViewModel>(TViewModel vm, string hostId, DialogOptions options)
        where TView: Control, new()
    {
        var t = new DialogControl()
        {
            Content = new TView() { DataContext = vm },
            DataContext = vm,
            ExtendToClientArea = options.ExtendToClientArea,
            Title = options.Title,
        };
        var host = OverlayDialogManager.GetOverlayDialogHost(hostId);
        host?.AddModalDialog(t);
    }
}