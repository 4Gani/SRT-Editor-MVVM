\# SRT Editor



A lightweight WPF desktop application for editing SRT subtitle files, built with C# and the MVVM pattern.



!\[.NET](https://img.shields.io/badge/.NET-10.0-purple)

!\[WPF](https://img.shields.io/badge/UI-WPF-blue)

!\[Pattern](https://img.shields.io/badge/Pattern-MVVM-green)



\## Features



\- \*\*Rebuild\*\* — Repairs malformed SRT files by correcting line numbers, time formats, and structure

\- \*\*Time Shifting\*\* — Shifts all or a selected range of timestamps forward or backward by a fixed offset

\- \*\*Time Correcting\*\* — Proportionally adjusts all timestamps to match a target end time

\- \*\*Improving Readability\*\* — Cleans up subtitle text formatting

\- \*\*SRT Viewer\*\* — View and edit the raw SRT file content with live refresh after changes

\- \*\*Drag \& Drop\*\* — Drop an SRT file directly onto the input field

\- \*\*Overwrite Mode\*\* — Option to save changes directly to the original file



\## Tech Stack



\- \*\*Language:\*\* C# / .NET 10

\- \*\*UI Framework:\*\* WPF

\- \*\*Pattern:\*\* MVVM

\- \*\*DI Container:\*\* Microsoft.Extensions.DependencyInjection

\- \*\*Behaviors:\*\* Microsoft.Xaml.Behaviors.Wpf



\## Project Structure



Components/       # Views and ViewModels



Editor/         # Feature tabs (Rebuild, TimeShifting, etc.)



MessageBox/     # Custom message box



Viewer/         # SRT file viewer



Services/         # Business logic and data access



ToolKit/        # Low-level SRT parsing utilities



Infrastructure/   # Base classes (BindableBase, RelayCommand, etc.)



Model/            # Data model (Srt)



Validation/       # Input validation rules



Resources/        # Styles and resource dictionaries



Assets/           # Icons and images



\## Screenshot



!\[SRT Editor](screenshot.png)



\## Build \& Run



1\. Install \[.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

2\. Clone the repository

3\. Open `SRT Editor (MVVM).sln` in Visual Studio 2022

4\. Build and run



\## Version History



See \[About.txt](SRT%20Editor%20(MVVM)/Docs/About.txt) for the full changelog.



\## Author



Created by 4Gani — started 2013, modernized 2026

