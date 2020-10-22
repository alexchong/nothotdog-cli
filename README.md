# 🌭 nothotdog-cli 
Image analysis app created with .NET Core and Azure [Computer Vision](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/) API to determine if an uploaded picture is a hotdog or not hotdog (inspired by Pied Piper's [Not Hotdog](https://www.youtube.com/watch?v=ACmydtFDTGs))

---
## Installation
###  Requirements
- [.NET Core SDK](https://dotnet.microsoft.com/download) latest or 3.1 version to publish app
- [Visual Studio](https://visualstudio.microsoft.com/) to build the app *(optional)*

Run the following command in a command-line interface (CLI) in the working directory containing `NotHotdog.sln`
```
dotnet publish
```
---
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