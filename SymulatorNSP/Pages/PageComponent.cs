using Microsoft.AspNetCore.Components;
using SymulatorNSP.GUI.Core.ViewModels;
using System.ComponentModel;

namespace SymulatorNSP.Pages
{
    public partial class PageComponent: ComponentBase
    {
        //public BaseViewModel ViewModel { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
        //    ViewModel.PropertyChanged += async (sender, e) =>
        //    {
        //        await InvokeAsync(() =>
        //        {
        //            StateHasChanged();
        //        });
        //    };
        //    await base.OnInitializedAsync();
        //}

        //public void Dispose()
        //{
        //    ViewModel.PropertyChanged -= OnPropertyChangedHandler;
        //}

        protected async void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }
    }
}
