cd `dirname $0`

rm -rf ./bin/Release
msbuild /t:Build /p:Configuration=Release

# Convert to bundle
cd bin
mkdir "Super Ping Pong.app"
cd "Super Ping Pong.app"
cp -r ../../AppBundle ./Contents
cd Contents/MacOS
cp -r ../../../Release/. ./
mv ./Content ../Resources/Content
cd ../../../
mv "Super Ping Pong.app" Release/"Super Ping Pong.app"
cd Release

# BUTLER
options=("Yes" "No")
select yn in "${options[@]}"
do
    case $yn in
        "Yes")
            mkdir Butler;
            cp -r "Super Ping Pong.app" Butler/"Super Ping Pong.app";
            butler push Butler pjrader1/super-ping-pong:osx;
            rm -rf Butler;
            exit;;
        "No") exit;;
    esac
done
