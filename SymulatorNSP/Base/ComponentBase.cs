using Microsoft.AspNetCore.Components;
using SymulatorNSP.GUI.Core.ViewModels;

namespace SymulatorNSP.Base
{
    /// <summary>
    /// ComponentBase with DataContext
    /// </summary>
    /// <see cref="https://dev.to/xakpc/using-blazor-consider-mvvm-59dm"/>
    public class ContextComponentBase : ComponentBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected BaseViewModel DataContext { get; set; }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            if (DataContext == null) return;

            if (!DataContext.Initialized) // do once
            {
                await DataContext.InitializeAsync();
                DataContext.PropertyChanged += (s, e) => StateHasChanged();
            }

            await base.OnInitializedAsync();
        }
    }
}
