# todolist

Publish applicaion in Android platform
APK: dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormats=apk

Publish application in IOS platform 
1. Follow the tutorial in https://learn.microsoft.com/en-us/dotnet/maui/ios/deployment/publish-app-store?view=net-maui-8.0&tabs=vs
2. Create an app Id and provisioning profile
3. download provisioning profiles in Visual Studio
4. Counter check for duplicate version number set background color of app icon
5. Connect the visual studio to macbook air for distrubution (remember to have a App-Specific password for apple Id)
6. Trobuleshooting: (Redownload the profile if not found / use transporter to upload .app while having errors "altool" exited with code 1) 
