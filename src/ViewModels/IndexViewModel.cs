﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorScrollDetection.ViewModels
{
    public class IndexViewModel : ComponentBase
    {
        protected string result;
        private DotNetObjectReference<IndexViewModel> _objRef;
        protected bool IsScrollTrackingEnabled { get; set; } = false;

        [Inject]
        protected IJSRuntime JS { get; set; }

        public void Dispose()
        {
            _objRef?.Dispose();
        }

        [JSInvokable]
        public bool IsAtWindowBottom(double contentScrollTop, double contentHeight, double containerHeight)
        {
            bool retVal = (contentScrollTop + contentHeight) >= containerHeight;
            return retVal;
        }

        [JSInvokable]
        public bool IsNearWindowBottom(double contentScrollTop, double contentHeight, double containerHeight)
        {
            bool retVal = (contentScrollTop + contentHeight) > (containerHeight - 100);
            return retVal;
        }

        public async Task ToggleDotNetTrackScroll()
        {
            _ = await JS.InvokeAsync<string>("blazorExtensions.toggleTrackScroll", _objRef);
        }

        [JSInvokable]
        public bool ToggleTrackScroll() { IsScrollTrackingEnabled = !IsScrollTrackingEnabled; return IsScrollTrackingEnabled; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _objRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("blazorExtensions.toggleTrackScroll", _objRef);
                StateHasChanged();
            }
        }
    }
}