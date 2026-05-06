# EPMS.Client

This project has been updated to include a premium login page, MudBlazor integration, and authentication service connected to the provided backend API.

## Setup and Running Instructions

1.  **Prerequisites:**
    *   .NET 8.0 SDK
    *   Node.js (for potential client-side build tools, though not strictly required for this Blazor WASM project)

2.  **Restore NuGet Packages:**
    Navigate to the `EPMS.Client` directory and run:
    ```bash
    dotnet restore
    ```

3.  **Configure API Base URL:**
    The API base URL is configured in `wwwroot/appsettings.json` and `wwwroot/appsettings.Development.json`. By default, it's set to `https://localhost:7001`.
    
    **`wwwroot/appsettings.json`:**
    ```json
    {
      "ApiBaseUrl": "https://localhost:7001"
    }
    ```
    
    **`wwwroot/appsettings.Development.json`:**
    ```json
    {
      "ApiBaseUrl": "https://localhost:7001"
    }
    ```
    
    Update these files with the correct URL of your backend API.

4.  **Run the Client Application:**
    Navigate to the `EPMS.Client` directory and run:
    ```bash
    dotnet run
    ```
    This will start the Blazor WebAssembly application, typically accessible at `https://localhost:7001` (or another port specified in `launchSettings.json`).

## Key Changes and Additions

*   **MudBlazor Integration:**
    *   MudBlazor NuGet package added.
    *   Custom `EPMSTheme.cs` created with a white, blue (`#0066CC`), and red (`#E63946`) color scheme and Inter font.
    *   `Program.cs` updated to add MudBlazor services.
    *   `App.razor` updated to include `MudThemeProvider`, `MudDialogProvider`, and `MudSnackbarProvider`.
    *   `_Imports.razor` updated with MudBlazor namespaces.
    *   `wwwroot/index.html` updated to include MudBlazor CSS and JS references, and Inter font import.
    *   `Layout/MainLayout.razor` has been refactored to use MudBlazor components for navigation and layout.
    *   `Layout/NavMenu.razor` has been updated to use MudBlazor navigation components.

*   **Authentication Integration:**
    *   `Blazor.LocalStorage` NuGet package added for client-side token storage.
    *   `Microsoft.AspNetCore.Components.Authorization` and `Microsoft.AspNetCore.Components.WebAssembly.Authentication` NuGet packages added.
    *   **DTOs and Enums:** `LoginRequest.cs`, `AuthResponse.cs`, `TokenResponse.cs`, `UserDto.cs`, `SuccessResponse.cs`, and `ErrorType.cs` have been copied to `EPMS.Client/EPMS.Shared/DTOs/Auth`, `EPMS.Client/EPMS.Shared/DTOs/Common`, and `EPMS.Client/EPMS.Shared/Enums` respectively, mirroring the backend contract.
    *   **`AuthApiClient.cs`:** A typed HTTP client for interacting with the `/api/auth/login` endpoint.
    *   **`TokenStorage.cs`:** A service for securely storing and retrieving access and refresh tokens using `Blazor.LocalStorage`.
    *   **`JwtAuthenticationStateProvider.cs`:** A custom `AuthenticationStateProvider` that parses JWTs, exposes claims, and notifies the authentication state.
    *   **`AuthorizationMessageHandler.cs`:** A `DelegatingHandler` that automatically attaches the Bearer token to outgoing HTTP requests and handles unauthorized responses by redirecting to the login page.
    *   `Program.cs` updated to register these services, configure `HttpClient` with `AuthorizationMessageHandler`, and set up `AuthenticationStateProvider`.
    *   `App.razor` updated to use `CascadingAuthenticationState` and `AuthorizeRouteView` for authentication flow.
    *   **`Login.razor`:** A new, polished login page using MudBlazor components, including form validation and error handling.
    *   **`Dashboard.razor`:** A placeholder dashboard page that users are redirected to upon successful login.
    *   **`Logout.razor`:** A page to handle user logout, clearing tokens and redirecting to the login page.
    *   **`RedirectToLogin.razor`:** A component to redirect unauthenticated users to the login page.

## Running the Backend API

Ensure your backend ASP.NET Core API is running and accessible at the `ApiBaseUrl` specified in `appsettings.json` (default: `https://localhost:7001`). The client will attempt to connect to this endpoint for authentication. 
