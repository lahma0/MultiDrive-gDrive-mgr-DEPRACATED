multidrive
==========

"MultiDrive" is a utility that allows you to manage multiple Google Drive accounts simultaneously. Features uploading, downloading, and drive-to-drive transfers. It is a project I started on, and never got around to releasing. I thought instead of leaving sitting on my hard drive, I would share it with others in case someone else needs this type of functionality.

The following libraries are used in this project:

Google.DotNET.API - specifically "google-api-dotnet-client-1.2.4737-beta.binary"

ObjectListView - http://objectlistview.sourceforge.net/cs/index.html

Mvolo.ShellIcons - http://mvolo.com/iconhandler-20-file-icons-in-your-aspnet-applications/

NOTE: As I have found no secure way of storing my Google Client Secret within an open source application, you will have to provide your own. If you do not know how to obtain a Google Client ID/Secret, I will shortly post instructions below.

Instructions on how to obtain a Google API key, Client Id, and Client Secret:
1. Go to "https://code.google.com/apis/console/"
2. Agree to any 'terms of service' presented on that page
3. Click "Create project" if you are presented with that option, otherwise click on "Services" in the navigation bar on the left
4. Turn on "Drive API" and "Drive SDK," accepting any terms of service along the way
5. Now click on "API Access"
6. Click "Create an OAuth 2.0 Client ID"
7. Choose a name and click next
8. Choose "Installed Application" as the Application Type and choose "Other" as the Installed Application Type
9. After clicking Next, you should now be looking at your "Client ID" and "Client Secret"

You DO NOT need to do this for every Google Drive account. Only one account of your choice.
