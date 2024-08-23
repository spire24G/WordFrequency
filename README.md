# WordFrequency
Create a frequency dictionary for a given text file

## Prerequisites
You need to have .Net8 installed. You can find a microsoft link here to download the .Net8 SDK :
https://dotnet.microsoft.com/fr-fr/download/dotnet/8.0

## How to install and Run

You can either build or build+run the application

To build the console application, you can execute the command line with a CMD in the folder WordFrequencyApp
```cs
dotnet build WordFrequency.sln
```

To build and run the console application, you can execute the command line with a CMD in the folder WordFrequencyApp
```cs
dotnet run InputFilePath OutputFilePath
```
where InputFilePath is the path of the file that you want to read and OutputFilePath is the path of the file that you want to write

You can use this example :
```cs
dotnet run ..\ressources\input\hello.txt ..\ressources\result.txt
```

## How to use 

The path for input and output should be valid.

For the input path, the file must exist otherwise the application will stop.

For the output path, the directory must exist otherwise the application will stop.
