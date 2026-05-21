dotnet publish qb.Desktop -c Release -r win-x64
dotnet publish qb.Desktop -c Release -r linux-x64
dotnet publish qb.Android -c Release

rm -rf build
mkdir build

mv qb.Android/bin/Release/net10.0-android36/publish/ru.izorofficial.qb-Signed.apk build/qb-android.apk
mv qb.Desktop/bin/Release/net10.0/linux-x64/publish build/qb-linux
mv qb.Desktop/bin/Release/net10.0/win-x64/publish build/qb-windows
