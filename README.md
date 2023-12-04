# todolist

Publish applicaion in Android platform
1. APK: dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormats=apk

Publish application in IOS platform 
1. Follow the tutorial in https://learn.microsoft.com/en-us/dotnet/maui/ios/deployment/publish-app-store?view=net-maui-8.0&tabs=vs
2. Create an app Id and provisioning profile
3. download provisioning profiles in Visual Studio
4. Counter check for duplicate version number set background color of app icon
5. Connect the visual studio to macbook air for distrubution
6. Remember to have a App-Specific password for apple Id
7. Troubleshooting: clean and build solution

Troubleshooing in IOS
1. solution->properties->iOS->Bundle Signing, Scheme: Manual Provision / Signing identity: whatever (not automatic) / Provisoning profile: whatever (not automatic)
2. After building the archive, Distribute->App Store-> Signing identity: whatever (not automatic) / Provisoning profile: whatever (not automatic), select the valid one that matched with apple account
3. delete the cache profile in AppData\Local\Xamarin\iOS\Provisioning then redownload the profile in Tools->Options->Xamarin->Apple Accounts->View Details
4. use transporter to upload .app while having errors "altool" exited with code 1
