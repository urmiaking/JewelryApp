using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryApp.Client.Security
{
    public class AppAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public AppAuthorizationMessageHandler(AuthenticationStateProvider authStateProvider, IAccessTokenProvider provider, NavigationManager navigationManager) : base(provider, navigationManager)
        {
            ConfigureHandler(new[] { navigationManager.BaseUri }, returnUrl: "/login");
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _navigationManager.NavigateTo($"/login/{(int)response.StatusCode}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    _navigationManager.NavigateTo($"/denied/{(int)response.StatusCode}");
                }

                return response;
            }
            catch (AccessTokenNotAvailableException exception)
            {
                if (_authStateProvider is AppAuthStateProvider appAuthStateProvider)
                    await appAuthStateProvider.LogoutAsync();

                exception.Redirect();
                return new HttpResponseMessage();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
