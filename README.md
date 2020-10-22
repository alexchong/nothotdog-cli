# 🌭 nothotdog-cli 
Image analysis app created with .NET Core and Azure [Computer Vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/) API to determine if an uploaded picture is a hotdog or not hotdog (inspired by Pied Piper's [Not Hotdog](https://www.youtube.com/watch?v=ACmydtFDTGs))

![Not Hotdog Demonstration](https://github.com/alexchong/nothotdog-cli/blob/main/static/nothotdog.gif)

## Installation
###  Requirements
- [.NET Core SDK](https://dotnet.microsoft.com/download) latest or 3.1 version to publish app
- [Computer Vision API key/endpoint](https://portal.azure.com/#home)
    - Tutorial to [create access to the API resource on Azure](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/quickstart-create-templates-use-the-portal)
- [Visual Studio](https://visualstudio.microsoft.com/) to build the app *(optional)*

Move `azure-cv-apikey.txt` to your `Documents` folder and overwrite each entire line containing `key` and `endpoint` with your own Computer Vision API credentials.
```
key # e.g. 2ab96390c7dbe3439de74d0c9b0b1767
endpoint # e.g. https://user-nothotdog.cognitiveservices.azure.com/
```

Run the following command in a command-line interface (CLI) in the working directory containing `NotHotdog.sln`
```
dotnet publish
```

## Usage
Navigate to the `publish` directory
```
cd NotHotdog\bin\Debug\netcoreapp*\publish
```
Run the application via CLI
```
./NotHotdog
```
> After performing `dotnet publish`, you may also use your operating system GUI to navigate to the directory and run the application 

In the application, paste from the clipboard (or type in) a direct full URL to the image you want to analyze
```
Enter food image full url (e.g. https://www.*.jpg): <URL>
```