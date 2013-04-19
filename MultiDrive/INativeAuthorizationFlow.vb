'Imports System
Imports DotNetOpenAuth.OAuth2

''' <summary>
''' An authorization flow is the process of obtaining an AuthorizationCode 
''' when provided with an IAuthorizationState.
''' </summary>
Friend Interface INativeAuthorizationFlow
    ''' <summary>
    ''' Retrieves the authorization of the user for the given AuthorizationState.
    ''' </summary>
    ''' <param name="client">The client used for authentication.</param>
    ''' <param name="authorizationState">The state requested.</param>
    ''' <returns>The authorization code, or null if the user cancelled the request.</returns>
    ''' <exception cref="NotSupportedException">Thrown if this flow is not supported.</exception>
    Function RetrieveAuthorization(client As UserAgentClient, authorizationState As IAuthorizationState) As String
End Interface