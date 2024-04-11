# todolist

Rember to change the package id and app icon when publish in different platform

Publish applicaion in Android platform
1. APK: dotnet publish -f net8.0-android -c Release -p:AndroidPackageFormats=apk
2. AAB: dotnet publish -f net8.0-android -c Release -p:AndroidKeyStore=true -p:AndroidSigningKeyPass=(CS password) -p:AndroidSigningStorePass=(CS password) (mac use \ for escaping special symbol <)

Publish application in IOS platform 
1. Follow the tutorial in https://learn.microsoft.com/en-us/dotnet/maui/ios/deployment/publish-cli?view=net-maui-8.0
2. Create an app Id and prvisioning profile in Apple Developer
3. Download provisioning profiles and open it in macbook air
4. Set up the PropertyGroup with RuntimeIdentifier, CodesignKey, CodesignProvision and ArchiveOnBuild
5. Ensure that the location of folder is not sync with iCloud
6. IPA: dotnet publish -f net8.0-ios -c Release

Troubleshooting in IOS
1. Getting error of resource fork, Finder information, or similar detritus not allowed
2. Open terminal in macbook air
3. cd (path to directory)todolist/todolist/bin/Release/net8.0-ios/ios-arm64/
4. xattr -lr todolist.app (list out all attributes)
5. xattr -cr todolist.app (clear all attributes)
6. xattr -lr todolist.app (ensure no more documents are found)



Old method
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
2. After building the archive, Distribute->App Store->Signing identity: whatever (not automatic) / Provisoning profile: whatever (not automatic), select the valid one that matched with apple account
3. delete the cache profile in AppData\Local\Xamarin\iOS\Provisioning then redownload the profile in Tools->Options->Xamarin->Apple Accounts->View Details
4. use xcode (Window-> Organizer->Archives->Distibute App) for distribution after sucessful archive or having errors "altool" exited with code 1
